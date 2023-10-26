namespace API.Models
{
    public class RequestModel
    {
        public Guid OrderId { get; set; }
        public string ClientName { get; set; }
        public double OrderPrice { get; set; }
    }
}
