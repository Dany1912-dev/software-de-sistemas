namespace Compilador.AST.Expresiones
{
    public class NodoIdentificador : NodoAST
    {
        public string Nombre { get; }

        public NodoIdentificador(string nombre)
        {
            Nombre = nombre;
        }
    }
}