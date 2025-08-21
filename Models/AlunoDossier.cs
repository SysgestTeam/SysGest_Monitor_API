namespace SistemasdeTarefas.Models
{
    public class AlunoDossier
    {
        public int IdAlunoDossier { get; set; }
        public int IdAluno { get; set; }
        public int NumAluno { get; set; }
        public string Nome { get; set; } 
        public int IdCustomer { get; set; }
        public int IdStatusAluno { get; set; }
        public int IdAno { get; set; }
        public int Ano { get; set; }
        public int? IdBolsa { get; set; }
        public int? IdMatricula { get; set; }
        public int? IdCiclo { get; set; }
        public string? NomeCiclo { get; set; }
        public int? IdClasse { get; set; }
        public string? NomeClasse { get; set; }
        public int? IdTurno { get; set; }
        public string? NomeTurno { get; set; }
        public int? IdCurso { get; set; }
        public string? NomeCurso { get; set; }
        public int? IdTurma { get; set; }
        public string? NomeTurma { get; set; }
        public bool Inactivo { get; set; }
        public int? IdTipoAluno { get; set; }
        public DateTime? DataRegisto { get; set; }
        public DateTime? DataAlter { get; set; }
        public string? TimeRegist { get; set; }
        public string? TimeAlter { get; set; }
        public int? IdUserRegisto { get; set; }
        public int? IdUserAlter { get; set; }
        public DateTime? DateDeleted { get; set; }
        public int? IdUserDel { get; set; }
        public bool? Deleted { get; set; }
        public string TimeDeleted { get; set; }
    }

}
