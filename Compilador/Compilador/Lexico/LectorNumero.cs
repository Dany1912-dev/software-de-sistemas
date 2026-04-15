using Compilador.Tokens;

namespace Compilador.Lexico
{
    public static class LectorNumero
    {
        public static (Token token, int nuevaPos) Leer(string src, int pos, int linea, int columna)
        {
            int inicio = pos;
            bool tienePunto = false;

            while (pos < src.Length && (char.IsDigit(src[pos]) || src[pos] == '.'))
            {
                if (src[pos] == '.')
                {
                    if (tienePunto)
                        break;

                    tienePunto = true;
                }

                pos++;
            }

            string lexema = src.Substring(inicio, pos - inicio);

            TipoToken tipo = tienePunto ? TipoToken.LITERAL_DECIMAL : TipoToken.LITERAL_ENTERO;

            return (new Token(lexema, tipo, lexema, linea, columna), pos);
        }
    }
}
