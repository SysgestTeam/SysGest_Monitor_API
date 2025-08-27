namespace SistemasdeTarefas.Models
{
    public class AlunoDossierLin
    {
        public int StudentDossierLineId { get; set; }
        public int StudentDossierCTId { get; set; }
        public int StudentDossierId { get; set; }
        public bool IsEnrollment { get; set; }
        public bool IsConfirmation { get; set; }
        public bool IsContract { get; set; }
        public bool IsArticle { get; set; }
        public bool IsPenalty { get; set; }
        public int? ContractId { get; set; }
        public string? ContractName { get; set; }
        public int? ArticleId { get; set; }
        public decimal? IssueAmount { get; set; }
        public int Quantity { get; set; }
        public decimal? ExpectedUnitValue { get; set; }
        public decimal? SubTotal { get; set; }
        public bool IsScholarshipHolder { get; set; }
        public int? ScholarshipId { get; set; }
        public bool HasOtherDiscount { get; set; }
        public decimal? DiscountRate { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? DiscountRate2 { get; set; }
        public decimal? DiscountValue2 { get; set; }
        public string? DiscountName { get; set; }
        public DateTime DueDate { get; set; }
        public decimal? AmountPaid { get; set; }
        public bool? Paid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool Canceled { get; set; }
        public bool Inactive { get; set; }
        public bool Invoiced { get; set; }
        public int? StudentInvoiceLineId { get; set; }
        public int? InvoiceLineId { get; set; }
        public string? InvoiceNumber { get; set; }
        public bool AppliedPenalty { get; set; }
        public int? PenaltyConditionId { get; set; }
        public decimal? PenaltyAmount { get; set; }
        public int RefCTIdIfPenalty { get; set; }
        public int StudentId { get; set; }
        public int OriginDocCCId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? TimeCreated { get; set; }
        public string? TimeUpdated { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedByUserId { get; set; }
        public bool? IsDeleted { get; set; }
        public string? TimeDeleted { get; set; }
        public bool IsFromReceipt { get; set; }
        public int? ReceiptId { get; set; }
        public string? Name { get; set; }
        public int StudentNumber { get; set; }
        public string? ClassName { get; set; }
    }

}
