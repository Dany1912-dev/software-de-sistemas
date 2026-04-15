namespace Compilador.AST.Expresiones
{
    public class NodoBooleano : NodoAST
    {
        public bool Valor { get; }

        public NodoBooleano(bool valor)
        {
            Valor = valor;
        }
    }
}