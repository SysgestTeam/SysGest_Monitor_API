using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IFamiliaArtigoRepository
    {
        public Task CreatFamily(FamiliaDTO familia);
        public Task UpdateFamily(Familia familia);
        public Task DeleteFamily(int IdFamilia);
        public Task<IEnumerable<Familia>> ListFamily();
        public Task<IEnumerable<Familia>> FindByIdAFamily(int idFamily);
        public Task CreatArtigo(ArtigoDTO artigo);
        public Task UpdateArtigo(ArtigoDTOUPDATE artigo);
        public Task DeleteArtigo(int idArtigo);
        public Task<IEnumerable<Artigo>> ListArtigo();
        public Task<IEnumerable<Artigo>> FindByIdArtigo(int idArtigo);
    }
}
