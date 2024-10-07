using System;

namespace Repository.ViewModels
{
    public class TrackingStatusViewModel
    {
        public string UserName { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string Action { get; set; }
        public string DocumentNo { get; set; }
    }
}
