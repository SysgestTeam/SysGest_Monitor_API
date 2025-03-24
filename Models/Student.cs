namespace SistemasdeTarefas.Models
{
    public class Student
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public byte[] UserPhoto { get; set; }
        public string Neighborhood { get; set; }
        public string Address { get; set; }
        public string Municipality { get; set; }
        public string Commune { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MotherEmail { get; set; }
        public string MotherName { get; set; }
        public string MotherPhone { get; set; }
        public string FatherEmail { get; set; }
        public string FatherName { get; set; }
        public string FatherPhone { get; set; }
        public string Class { get; set; }
        public bool IsBlocked { get; set; }
        public string status { get; set; }
    }

}
