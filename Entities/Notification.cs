using System;

namespace Entities
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; } 
        public int DocumentId { get; set; }
        public string NotificationMessage { get; set; } 
        public DateTime SentDate { get; set; } = DateTime.Now;
        public DateTime dtmUpd { get; set; } = DateTime.Now;
        public string usrUpd { get; set; }
    }
}
