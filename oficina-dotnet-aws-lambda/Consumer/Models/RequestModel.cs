using Amazon.DynamoDBv2.DataModel;

namespace Consumer.Models
{
    [DynamoDBTable("orders")]
    public class RequestModel
    {
        [DynamoDBHashKey]
        public Guid OrderId { get; set; }
        public string ClientName { get; set; }
        public double OrderPrice { get; set; }
        public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;
    }
}
