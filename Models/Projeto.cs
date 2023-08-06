namespace LucasVaz.Models
{
    public class Projeto
    {
        public int IdProjeto { get; set; }
        public string DsProjeto { get; set; }
        public string CmProjeto { get; set; }
        public string LkGithub { get; set; }
        public string? LkWeb { get; set; }
        public string? DsLoginTeste { get; set; }
        public string? DsSenhaTeste { get; set; }
        public Pessoa Pessoa { get; set; }
        public TipoProjeto TipoProjeto { get; set; }
        public List<TecnologiaProjeto> TecnologiasProjetos { get; set; }
    }
}
