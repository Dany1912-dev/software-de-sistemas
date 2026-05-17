namespace Compilador.AST.Expresiones
{
    public class NodoBooleano : NodoAST
    {
        public bool Valor { get; }

        public NodoBooleano(bool valor, int linea, int columna)
            : base(linea, columna)
        {
            Valor = valor;
        }
    }
}
