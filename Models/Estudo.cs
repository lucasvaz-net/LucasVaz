namespace LucasVaz.Models
{
    public class Estudo
    {
        public int IdEstudo { get; set; }
        public string DsEstudo { get; set; }
        public string DsLocal { get; set; }
        public string DtEstudo { get; set; }
        public Pessoa Pessoa { get; set; }
        public TipoEstudo TipoEstudo { get; set; }
        public List<EstudoTecnologia> EstudosTecnologias { get; set; }
    }

}
