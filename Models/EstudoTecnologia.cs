namespace LucasVaz.Models
{
    public class EstudoTecnologia
    {
        public int IdEstudoTecnologia { get; set; }
        public Estudo Estudo { get; set; }
        public Tecnologia Tecnologia { get; set; }
    }
}
