using Compilador.Utilidades;
using Compilador.Tokens;

namespace Compilador.Lexico
{
    public static class LectorIdentificador
    {
        public static (Token token, int nuevaPos) Leer(string src, int pos, int linea, int columna)
        {
            int inicio = pos;

            while (pos < src.Length && (char.IsLetterOrDigit(src[pos]) || src[pos] == '_'))
                pos++;

            string lexema = src.Substring(inicio, pos - inicio);
            string lower = lexema.ToLower();

            TipoToken tipo = PalabrasReservadas.Buscar(lower);

            if (tipo == TipoToken.IDENTIFICADOR && PalabrasReservadas.EsMalFormada(lower))
            {
                string mensaje = $"Palabra reservada mal formada: '{lexema}'";
                return (new Token(lexema, TipoToken.TOKEN_ERROR, mensaje, linea, columna), pos);
            }

            return (new Token(lexema, tipo, lexema, linea, columna), pos);
        }
    }
}
