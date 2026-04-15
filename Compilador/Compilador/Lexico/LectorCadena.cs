using Compilador.Tokens;

namespace Compilador.Lexico
{
    public static class LectorCadena
    {
        public static (Token token, int nuevaPos) Leer(string src, int pos, int linea, int columna)
        {
            pos++; // salta la comilla de apertura

            int inicio = pos;

            while (pos < src.Length && src[pos] != '"')
                pos++;

            string lexema = src.Substring(inicio, pos - inicio);

            if (pos < src.Length && src[pos] == '"')
                pos++;

            return (new Token(lexema, TipoToken.LITERAL_CADENA, lexema, linea, columna), pos);
        }
    }
}
