using System.ComponentModel.DataAnnotations;

namespace Webapi.Models
{
    public class VisitorInfo
    {
        [Key]
        public int Id { get; set; }
        public string VisitorName { get; set; }
        public string ClientIp { get; set; }
        public string Location { get; set; }
        public float Temperature { get; set; }
        public DateTime VisitTime { get; set; }
    }
}
