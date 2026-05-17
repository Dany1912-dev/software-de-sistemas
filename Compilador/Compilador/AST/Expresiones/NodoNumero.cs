namespace Compilador.AST.Expresiones
{
    public class NodoNumero : NodoAST
    {
        public int Valor { get; }

        public NodoNumero(int valor, int linea, int columna)
            : base(linea, columna)
        {
            Valor = valor;
        }
    }
}
