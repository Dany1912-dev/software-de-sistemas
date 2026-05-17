namespace Compilador.AST.Expresiones
{
    public class NodoCadena : NodoAST
    {
        public string Valor { get; }

        public NodoCadena(string valor, int linea, int columna)
            : base(linea, columna)
        {
            Valor = valor;
        }
    }
}
