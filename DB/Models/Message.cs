
namespace DB.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}
