namespace Compilador.AST.Expresiones
{
    public class NodoDecimal : NodoAST
    {
        public double Valor { get; }

        public NodoDecimal(double valor)
        {
            Valor = valor;
        }
    }
}