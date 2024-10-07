using Entities;
using System.Collections.Generic;

namespace Repository.ViewModels
{
    public class DocumentAndDocApprovalViewModel
    {
        public DocumentApproval DocumentApproval { get; set; }
        public Document Document { get; set; }
        public IEnumerable<TrackingStatusViewModel> TrackingStatus { get; set; }
    }
}
