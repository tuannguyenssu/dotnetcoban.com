using System;

namespace AspNetCoreCorrelationIdTest
{
    public static class GuardExtensions
    {
        public static TReturn NotNull<TReturn>(this TReturn value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return value;
        }

        public static String NotNullOrEmpty(this String value)
        {
            value.NotNull();
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("String parameter cannot be null or empty and cannot contain only blanks.", nameof(value));
            }
            return value;
        }

        public static Guid NotNullOrEmpty(this Guid value)
        {
            value.NotNull();
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Guid parameter cannot be null or empty and cannot contain only blanks.", nameof(value));
            }
            return value;
        }
    }
}