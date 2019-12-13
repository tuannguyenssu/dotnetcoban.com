using GraphQL.Types;

namespace GraphQLTest.GraphQL
{
    public class OrderStatusesEnum : EnumerationGraphType
    {
        public OrderStatusesEnum()
        {
            Name = "OrderStatus";
            AddValue("CREATED", "Order was created", 2);
            AddValue("PROCESSING", "Order is being processed", 4);
            AddValue("COMPLETED", "Order is completed", 8);
            AddValue("CANCELLED", "Order was cancelled", 16);
            AddValue("CLOSED", "Order was closed", 32);
        }
    }
}