namespace Compilador.AST
{
    public abstract class NodoAST
    {
        public int Linea { get; }
        public int Columna { get; }

        protected NodoAST(int linea, int columna)
        {
            Linea = linea;
            Columna = columna;
        }
    }
}
