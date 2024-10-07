using System;

namespace Entities
{
    public class DocumentApproval
    {
        public int ApprovalId { get; set; }
        public int DocumentId { get; set; } 
        public int ApproverId { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime? ApprovalDate { get; set; } 
        public DateTime dtmUpd { get; set; } = DateTime.Now;
        public string usrUpd { get; set; }
        public string NextApprovalRole { get; set; }
    }
}
