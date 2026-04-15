namespace Compilador.AST.Expresiones
{
    public class NodoCadena : NodoAST
    {
        public string Valor { get; }

        public NodoCadena(string valor)
        {
            Valor = valor;
        }
    }
}