namespace LucasVaz.Models
{
    public class Experiencia
    {
        public int IdExperiencia { get; set; }
        public string DsExperiencia { get; set; }
        public string DsFuncao { get; set; }
        public string DsLocal { get; set; }
        public DateTime DtIni { get; set; }
        public DateTime DtFim { get; set; }
        public Pessoa Pessoa { get; set; }
        public TipoExperiencia TipoExperiencia { get; set; }
        public List<ExperienciaTecnologia> ExperienciasTecnologias { get; set; }
    }
}
