

namespace DB.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
