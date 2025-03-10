﻿using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IDividasRepository
    {
        IEnumerable<Devedor> GetDevedores(DateTime? dataInicial = null, DateTime? dataFinal = null);
        IEnumerable<LogBloqueio> LogBloqueio(DateTime? dataInicial = null, DateTime? dataFinal = null);
        public void BloquearDevedoresPorMes(DateTime dataInicial, DateTime dataFinal, int numeroMeses);
        public void LogBloqueio( int IsAluno , int IdEntidade ,string TipoBloqueio,string AcaoBloqueio);
        public void BloqueioCartao(int[] numAluno = null, bool emMassa = false);
        public void DesbloqueioCartao(int[] numAluno = null);
    }
}
