namespace Compilador.AST.Sentencias
{
    public class NodoSi : NodoAST
    {
        public NodoAST Condicion { get; }
        public List<NodoAST> Entonces { get; }
        public List<NodoAST>? Sino { get; }

        public NodoSi(NodoAST condicion, List<NodoAST> entonces, int linea, int columna, List<NodoAST>? sino = null)
            : base(linea, columna)
        {
            Condicion = condicion;
            Entonces = entonces;
            Sino = sino;
        }
    }
}
