namespace Compilador.AST.Sentencias
{
    public class NodoEscribir : NodoAST
    {
        public List<NodoAST> Valores { get; }

        public NodoEscribir(List<NodoAST> valores)
        {
            Valores = valores;
        }
    }
}