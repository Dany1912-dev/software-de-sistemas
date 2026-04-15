namespace Compilador.AST.Sentencias
{
    public class NodoAsignacion : NodoAST
    {
        public string Nombre { get; }
        public NodoAST Valor { get; }

        public NodoAsignacion(string nombre, NodoAST valor)
        {
            Nombre = nombre;
            Valor = valor;
        }
    }
}