using System;

namespace Entities
{
    public class AuditTrail
    {
        public int AuditId { get; set; }
        public int DocumentId { get; set; } 
        public string Action { get; set; } 
        public int ActionBy { get; set; } 
        public DateTime ActionDate { get; set; } = DateTime.Now; 
        public DateTime dtmUpd { get; set; } = DateTime.Now;
        public string usrUpd { get; set; }
    }
}
