namespace Compilador.AST.Sentencias
{
    public class NodoLeer : NodoAST
    {
        public string Nombre { get; }

        public NodoLeer(string nombre, int linea, int columna)
            : base(linea, columna)
        {
            Nombre = nombre;
        }
    }
}
