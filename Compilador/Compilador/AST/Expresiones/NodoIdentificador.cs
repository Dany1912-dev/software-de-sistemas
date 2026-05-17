namespace Compilador.AST.Expresiones
{
    public class NodoIdentificador : NodoAST
    {
        public string Nombre { get; }

        public NodoIdentificador(string nombre, int linea, int columna)
            : base(linea, columna)
        {
            Nombre = nombre;
        }
    }
}
