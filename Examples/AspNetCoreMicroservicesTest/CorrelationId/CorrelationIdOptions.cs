namespace AspNetCoreMicroservicesTest.CorrelationId
{
    public class CorrelationIdOptions
    {
        private const string DefaultHeader = "X-Correlation-ID";
        public string Header { get; set; } = DefaultHeader;
        public bool IncludeInResponse { get; set; } = true;
        public bool UpdateTraceIdentifier { get; set; } = false;
    }
}
