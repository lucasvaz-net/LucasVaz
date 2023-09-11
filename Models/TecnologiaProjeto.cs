namespace LucasVaz.Models
{
    public class TecnologiaProjeto
    {
        public int? IdTecnologiasProjetos { get; set; }
        public Projeto? Projeto { get; set; }
        public Tecnologia? Tecnologia { get; set; }
    }
}
