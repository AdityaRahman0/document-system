using System.Collections.Generic;

namespace Repository.ViewModels
{
    public class ApprovalPaggingViewModel
    {
        public IEnumerable<ApprovalViewModel> ApprovalDocument { get; set; }
        public BasePagging Pagging { get; set; }
    }
}
