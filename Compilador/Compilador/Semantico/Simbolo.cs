namespace Compilador.Semantico
{
    public class Simbolo
    {
        public string Nombre { get; }
        public TipoDato Tipo { get; }
        public int Linea { get; }

        public Simbolo(string nombre, TipoDato tipo, int linea)
        {
            Nombre = nombre;
            Tipo = tipo;
            Linea = linea;
        }
    }
}
