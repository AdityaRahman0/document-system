namespace Repository.ViewModels
{
    public class ApprovalViewModel
    {
        public int DocumentId { get; set; }
        public string DocumentNo { get; set; }
        public string ProductOrMaterial { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int ApprovalId { get; set; }
        public int ApproverId { get; set; }
        public string NextApprovalRole { get; set; }
    }
}
