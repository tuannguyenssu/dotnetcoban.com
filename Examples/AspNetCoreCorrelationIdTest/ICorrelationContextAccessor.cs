namespace AspNetCoreCorrelationIdTest
{
    public interface ICorrelationContextAccessor
    {
        CorrelationContext CorrelationContext { get; set; }
    }
}
