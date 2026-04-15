namespace Compilador.AST.Expresiones
{
    public class NodoUnaria : NodoAST
    {
        public string Operador { get; }
        public NodoAST Operando { get; }

        public NodoUnaria(string operador, NodoAST operando)
        {
            Operador = operador;
            Operando = operando;
        }
    }
}