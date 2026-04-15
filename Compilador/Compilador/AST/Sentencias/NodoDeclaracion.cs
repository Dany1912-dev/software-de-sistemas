namespace Compilador.AST.Sentencias
{
    public class NodoDeclaracion : NodoAST
    {
        public string Tipo { get; }
        public string Nombre { get; }
        public NodoAST? Valor { get; }

        public NodoDeclaracion(string tipo, string nombre, NodoAST? valor = null)
        {
            Tipo = tipo;
            Nombre = nombre;
            Valor = valor;
        }
    }
}