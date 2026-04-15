namespace Compilador.AST.Expresiones
{
    public class NodoNumero : NodoAST
    {
        public int Valor { get; }

        public NodoNumero(int valor)
        {
            Valor = valor;
        }
    }
}