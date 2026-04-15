namespace Compilador.AST.Sentencias
{
    public class NodoPrograma : NodoAST
    {
        public List<NodoAST> Sentencias { get; }

        public NodoPrograma(List<NodoAST> sentencias)
        {
            Sentencias = sentencias;
        }
    }
}