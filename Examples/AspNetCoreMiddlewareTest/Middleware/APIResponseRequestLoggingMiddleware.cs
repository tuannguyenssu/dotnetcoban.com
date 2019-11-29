using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMiddlewareTest.Extensions;
using AspNetCoreMiddlewareTest.Models;
using AspNetCoreMiddlewareTest.Services;
using AspNetCoreMiddlewareTest.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace AspNetCoreMiddlewareTest.Middleware
{
    public class ApiResponseRequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiResponseRequestLoggingMiddleware> _logger;
        private readonly IApiLogService _apiLogService;

        private readonly Func<object, Task> _clearCacheHeadersDelegate;

        private readonly bool _enableAPILogging = true;

        public ApiResponseRequestLoggingMiddleware(RequestDelegate next, IApiLogService apiLogService, ILogger<ApiResponseRequestLoggingMiddleware> logger)
        {
            _next = next;
            _apiLogService = apiLogService;
            _logger = logger;
            _clearCacheHeadersDelegate = ClearCacheHeaders;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            try
            {
                var request = httpContext.Request;
                if (IsSwagger(httpContext) || request.Path.StartsWithSegments(new PathString("/test")))
                {
                    await _next(httpContext);
                }
                else
                {
                    Stopwatch stopWatch = Stopwatch.StartNew();
                    var requestTime = DateTime.UtcNow;

                    var formattedRequest = await FormatRequest(request);
                    var originalBodyStream = httpContext.Response.Body;

                    using (var responseBody = new MemoryStream())
                    {
                        httpContext.Response.Body = responseBody;

                        try
                        {
                            var response = httpContext.Response;
                            response.Body = responseBody;
                            await _next.Invoke(httpContext);

                            string responseBodyContent = null;

                            if (httpContext.Response.StatusCode == (int)HttpStatusCode.OK)
                            {
                                responseBodyContent = await FormatResponse(response);
                                await HandleSuccessRequestAsync(httpContext, responseBodyContent, httpContext.Response.StatusCode);
                            }
                            else
                            {
                                await HandleNotSuccessRequestAsync(httpContext, httpContext.Response.StatusCode);
                            }

                            stopWatch.Stop();

                            #region Log Request / Response
                            if (_enableAPILogging)
                            {
                                try
                                {
                                    await responseBody.CopyToAsync(originalBodyStream);

                                    await _apiLogService.Log(new ApiLogItem
                                    {
                                        RequestTime = requestTime,
                                        ResponseMillis = stopWatch.ElapsedMilliseconds,
                                        StatusCode = response.StatusCode,
                                        Method = request.Method,
                                        Path = request.Path,
                                        QueryString = request.QueryString.ToString(),
                                        RequestBody = formattedRequest,
                                        ResponseBody = responseBodyContent,
                                        IPAddress = httpContext.Connection.RemoteIpAddress.ToString()
                                    });
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning("An Inner Middleware exception occurred on SafeLog: " + ex.Message);
                                }
                            }
                            #endregion
                        }
                        catch (System.Exception ex)
                        {

                            _logger.LogWarning("An Inner Middleware exception occurred: " + ex.Message);
                            await HandleExceptionAsync(httpContext, ex);
                        }
                        finally
                        {
                            responseBody.Seek(0, SeekOrigin.Begin);
                            await responseBody.CopyToAsync(originalBodyStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // We can't do anything if the response has already started, just abort.
                if (httpContext.Response.HasStarted)
                {
                    _logger.LogWarning("A Middleware exception occurred, but response has already started!");
                    throw;
                }

                await HandleExceptionAsync(httpContext, ex);
                throw;
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, System.Exception exception)
        {
            _logger.LogError("Api Exception:", exception);

            ApiError apiError = null;
            ApiResponse apiResponse = null;
            int code = 0;

            if (exception is ApiException)
            {
                var ex = exception as ApiException;
                apiError = new ApiError(ResponseMessageEnum.ValidationError.GetDescription(), ex.Errors)
                {
                    ValidationErrors = ex.Errors,
                    ReferenceErrorCode = ex.ReferenceErrorCode,
                    ReferenceDocumentLink = ex.ReferenceDocumentLink
                };
                code = ex.StatusCode;
                httpContext.Response.StatusCode = code;

            }
            else if (exception is UnauthorizedAccessException)
            {
                apiError = new ApiError("Unauthorized Access");
                code = (int)HttpStatusCode.Unauthorized;
                httpContext.Response.StatusCode = code;
            }
            else
            {
#if !DEBUG
                var msg = "An unhandled error occurred.";
                string stack = null;
#else
                var msg = exception.GetBaseException().Message;
                string stack = exception.StackTrace;
#endif

                apiError = new ApiError(msg)
                {
                    Details = stack
                };
                code = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.StatusCode = code;
            }

            httpContext.Response.ContentType = "application/json";

            apiResponse = new ApiResponse(code, ResponseMessageEnum.Exception.GetDescription(), null, apiError);

            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse));
        }

        private Task HandleNotSuccessRequestAsync(HttpContext httpContext, int code)
        {
            ApiError apiError;

            if (code == (int)HttpStatusCode.NotFound)
            {
                apiError = new ApiError(ResponseMessageEnum.NotFound.GetDescription());
            }
            else if (code == (int)HttpStatusCode.NoContent)
            {
                apiError = new ApiError(ResponseMessageEnum.NotContent.GetDescription());
            }
            else if (code == (int)HttpStatusCode.MethodNotAllowed)
            {
                apiError = new ApiError(ResponseMessageEnum.MethodNotAllowed.GetDescription());
            }
            else if (code == (int)HttpStatusCode.Unauthorized)
            {
                apiError = new ApiError(ResponseMessageEnum.UnAuthorized.GetDescription());
            }
            else
            {
                apiError = new ApiError(ResponseMessageEnum.Unknown.GetDescription());
            }

            ApiResponse apiResponse = new ApiResponse(code, apiError);
            httpContext.Response.StatusCode = code;
            httpContext.Response.ContentType = "application/json";
            return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse));
        }

        private JsonSerializerSettings JsonSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
        }

        private string ConvertToJsonString(object rawJSON)
        {
            return JsonConvert.SerializeObject(rawJSON, JsonSettings());
        }

        private Task HandleSuccessRequestAsync(HttpContext httpContext, object body, int code)
        {
            string jsonString = string.Empty;
            var bodyText = !body.ToString().IsValidJson() ? ConvertToJsonString(body) : body.ToString();

            ApiResponse apiResponse = null;

            if (!body.ToString().IsValidJson())
            {
                return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse));
            }
            else
            {
                bodyText = body.ToString();
            }

            //TODO Review the code below as it might not be necessary
            dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);
            Type type = bodyContent?.GetType();

            // Check to see if body is already an ApiResponse Class type
            if (type.Equals(typeof(Newtonsoft.Json.Linq.JObject)))
            {
                apiResponse = JsonConvert.DeserializeObject<ApiResponse>(bodyText);
                if (apiResponse.StatusCode == 0)
                {
                    apiResponse.StatusCode = code;
                }

                if ((apiResponse.Result != null) || (!string.IsNullOrEmpty(apiResponse.Message)))
                {
                    jsonString = JsonConvert.SerializeObject(apiResponse);
                }
                else
                {
                    apiResponse = new ApiResponse(code, ResponseMessageEnum.Success.GetDescription(), bodyContent, null);
                    jsonString = JsonConvert.SerializeObject(apiResponse);
                }
            }
            else
            {
                apiResponse = new ApiResponse(code, ResponseMessageEnum.Success.GetDescription(), bodyContent, null);
                jsonString = JsonConvert.SerializeObject(apiResponse);
            }

            httpContext.Response.ContentType = "application/json";
            return httpContext.Response.WriteAsync(jsonString);
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return $"{request.Method} {request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return plainBodyText;
        }
        private bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger");
        }

        private Task ClearCacheHeaders(object state)
        {
            var response = (HttpResponse)state;

            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);

            return Task.CompletedTask;
        }
    }
}
