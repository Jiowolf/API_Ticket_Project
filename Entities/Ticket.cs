using System.Text.Json.Serialization;

namespace API_Ticket_Project.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string status { get; set; }
        [JsonPropertyName("create_at")]
        public DateTime Create_at { get; set; } = DateTime.Now;
        public User User { get; set; }
    }
}
