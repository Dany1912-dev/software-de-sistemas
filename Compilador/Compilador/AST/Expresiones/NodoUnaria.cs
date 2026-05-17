namespace Compilador.AST.Expresiones
{
    public class NodoUnaria : NodoAST
    {
        public string Operador { get; }
        public NodoAST Operando { get; }

        public NodoUnaria(string operador, NodoAST operando, int linea, int columna)
            : base(linea, columna)
        {
            Operador = operador;
            Operando = operando;
        }
    }
}
