using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticsearchTest
{
    public class DeviceLogRepository
    {
        private readonly ElasticClient _client;
        private readonly string _indexName;

        public DeviceLogRepository(ElasticClient client, string indexName)
        {
            _client = client;
            _indexName = indexName;
        }

        public async Task<bool> SaveAsync(DeviceLog log)
        {
            var response = await _client.IndexAsync(log, i => i.Index(_indexName));
            return response.IsValid;
        }

        public async Task<bool> SaveAsync(List<DeviceLog> logs)
        {
            var response = await _client.IndexManyAsync(logs, _indexName);
            return response.IsValid;
        }

        public async Task<IEnumerable<DeviceLog>> ListAllAsync()
        {
            var result = await _client.SearchAsync<DeviceLog>(s => s.
                Index(_indexName).
                Query(q => q.MatchAll()));

            return result.Documents;
        }

        public async Task<IEnumerable<DeviceLog>> SearchAsync(string query)
        {
            var result = await _client.SearchAsync<DeviceLog>(s => s.
                Index(_indexName).
                Query(q => q.SimpleQueryString(sq => sq.Query(query))));

            return result.Documents;
        }

        public async Task<IEnumerable<DeviceLog>> SearchAsync(DateTime fromDate, DateTime toDate)
        {
            var result = await _client.SearchAsync<DeviceLog>(s => s.
                Index(_indexName).
                Query(q => q.
                    DateRange(r => r.
                        Field(f => f.CreatedTime).
                        GreaterThanOrEquals(fromDate).
                        LessThanOrEquals(toDate))));

             return result.Documents;
        }

        public async Task<IEnumerable<DeviceLog>> SearchAsync(int currentPage = 1, int pageSize = 10, long customerId = 0, string vehiclePlate = null, DateTime fromDate = default, DateTime toDate = default)
        {
            var result = await _client.SearchAsync<DeviceLog>(s => s
                .Index(_indexName)
                .From((currentPage - 1) * pageSize)
                .Size(pageSize)
                .Query(q => q
                    .Bool(b => b
                        .Filter(bf =>
                            {
                                if (customerId <= 0) return bf.MatchAll();
                                var customerQuery = bf.Term(t => t.Field(field => field.CustomerId).Value(customerId));
                                if (vehiclePlate == null) return customerQuery;
                                var vehiclePlateQuery = bf.Term(t => t.Field(field => field.VehiclePlate).Value(vehiclePlate));
                                if (fromDate == default) return customerQuery && vehiclePlateQuery;
                                var dateRangeQuery = bf.DateRange(r => r.Field(field => field.CreatedTime).GreaterThanOrEquals(fromDate));
                                if (toDate == default) return customerQuery && vehiclePlateQuery && dateRangeQuery;
                                dateRangeQuery = bf.DateRange(r => r.Field(field => field.CreatedTime).GreaterThanOrEquals(fromDate).LessThanOrEquals(toDate));
                                return customerQuery && vehiclePlateQuery && dateRangeQuery;
                            }
                        )
                    ))
            );

            return result.Documents;
        }

        public async Task<bool> DeleteAllAsync()
        {
            var response = await _client.DeleteByQueryAsync<DeviceLog>(s => s.Index(_indexName).MatchAll());
            return response.IsValid;
        }
    }
}
