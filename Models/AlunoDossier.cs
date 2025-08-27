namespace SistemasdeTarefas.Models
{
    public class AlunoDossier
    {
            public int StudentDossierId { get; set; }
            public int StudentId { get; set; }
            public int StudentNumber { get; set; }
            public string? Name { get; set; }
            public int CustomerId { get; set; }
            public int StudentStatusId { get; set; }
            public int YearId { get; set; }
            public int Year { get; set; }
            public int? ScholarshipId { get; set; }
            public int? EnrollmentId { get; set; }
            public int? CycleId { get; set; }
            public string? CycleName { get; set; }
            public int? ClassId { get; set; }
            public string? ClassName { get; set; }
            public int? ShiftId { get; set; }
            public string? ShiftName { get; set; }
            public int? CourseId { get; set; }
            public string? CourseName { get; set; }
            public int? GroupId { get; set; }
            public string? GroupName { get; set; }
            public bool Inactive { get; set; }
            public int? StudentTypeId { get; set; }
            public DateTime? RegisterDate { get; set; }
            public DateTime? UpdateDate { get; set; }
            public string? RegisterTime { get; set; }
            public string? UpdateTime { get; set; }
            public int? UserRegisterId { get; set; }
            public int? UserUpdateId { get; set; }
            public DateTime? DeletedDate { get; set; }
            public int? UserDeletedId { get; set; }
            public bool? Deleted { get; set; }
            public string? DeletedTime { get; set; }
        

    }

}
