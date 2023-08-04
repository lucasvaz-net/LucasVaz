namespace LucasVaz.Models
{
    public class Contato
    {
        public int IdContato { get; set; }
        public string DsContato { get; set; }
        public Pessoa Pessoa { get; set; }
        public TipoContato TipoContato { get; set; }
    }
}
