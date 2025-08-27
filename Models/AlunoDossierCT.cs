namespace SistemasdeTarefas.Models
{
    public class AlunoDossierCT
    {
            public int IdStudentDossierCT { get; set; }
            public int IdStudentDossier { get; set; }
            public int? AcademicYearId { get; set; }
            public int ContractId { get; set; }
            public string? ContractName { get; set; }
            public decimal? IssueValue { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public decimal? ExpectedValue { get; set; }
            public bool? IsScholarship { get; set; }
            public int? ScholarshipId { get; set; }
            public bool? HasOtherDiscount { get; set; }
            public decimal? DiscountRate { get; set; }
            public string? DiscountName { get; set; }
            public bool? RemoveDiscount { get; set; }
            public bool? IsPreviousYear { get; set; }
            public DateTime? RegisterDate { get; set; }
            public DateTime? UpdateDate { get; set; }
            public string? RegisterTime { get; set; }
            public string? UpdateTime { get; set; }
            public int? RegisterUserId { get; set; }
            public int? UpdateUserId { get; set; }
            public DateTime? DeletedDate { get; set; }
            public int? DeletedUserId { get; set; }
            public bool Deleted { get; set; }
            public string? DeletedTime { get; set; }
            public bool? IsFine { get; set; }
       

    }


}
