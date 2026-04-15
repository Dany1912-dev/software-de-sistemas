namespace Compilador.AST.Sentencias
{
    public class NodoLeer : NodoAST
    {
        public string Nombre { get; }

        public NodoLeer(string nombre)
        {
            Nombre = nombre;
        }
    }
}