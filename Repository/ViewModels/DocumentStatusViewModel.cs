using System.Collections.Generic;

namespace Repository.ViewModels
{
    public class DocumentStatusViewModel
    {
        public int? ApprovedCount { get; set; }
        public int? RejectedCount { get; set; }
        public int? RevisedCount { get; set; }
        public List<string> Factories { get; set; }
        public string SelectedFactory { get; set; }
        public int? SelectedMonth { get; set; }
        public int? SelectedYear { get; set; }
        public List<DropDown> Years { get; set; }
        public List<DropDown> Months { get; set; }
    }

}
