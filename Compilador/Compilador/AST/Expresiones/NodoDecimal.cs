namespace Compilador.AST.Expresiones
{
    public class NodoDecimal : NodoAST
    {
        public double Valor { get; }

        public NodoDecimal(double valor, int linea, int columna)
            : base(linea, columna)
        {
            Valor = valor;
        }
    }
}
