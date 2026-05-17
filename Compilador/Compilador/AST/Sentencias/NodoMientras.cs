namespace Compilador.AST.Sentencias
{
    public class NodoMientras : NodoAST
    {
        public NodoAST Condicion { get; }
        public List<NodoAST> Cuerpo { get; }

        public NodoMientras(NodoAST condicion, List<NodoAST> cuerpo, int linea, int columna)
            : base(linea, columna)
        {
            Condicion = condicion;
            Cuerpo = cuerpo;
        }
    }
}
