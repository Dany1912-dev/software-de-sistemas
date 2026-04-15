namespace Compilador.AST.Expresiones
{
    public class NodoBinaria : NodoAST
    {
        public string Operador { get; }
        public NodoAST Izquierdo { get; }
        public NodoAST Derecho { get; }

        public NodoBinaria(string operador, NodoAST izquierdo, NodoAST derecho)
        {
            Operador = operador;
            Izquierdo = izquierdo;
            Derecho = derecho;
        }
    }
}