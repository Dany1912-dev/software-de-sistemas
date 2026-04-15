using Compilador.Tokens;

namespace Compilador.Lexico
{
    public static class LectorSimbolo
    {
        public static (Token token, int nuevaPos) Leer(string src, int pos, int linea, int columna)
        {
            char c = src[pos];
            string dos = pos + 1 < src.Length ? $"{src[pos]}{src[pos + 1]}" : "";

            // Operadores de dos caracteres
            switch (dos)
            {
                case "==": return (new Token("==", TipoToken.OP_IGUALDAD, "==", linea, columna), pos + 2);
                case "!=": return (new Token("!=", TipoToken.OP_DIFERENTE, "!=", linea, columna), pos + 2);
                case "<=": return (new Token("<=", TipoToken.OP_MENOR_IGUAL, "<=", linea, columna), pos + 2);
                case ">=": return (new Token(">=", TipoToken.OP_MAYOR_IGUAL, ">=", linea, columna), pos + 2);
            }

            // Operadores y delimitadores de un carácter
            switch (c)
            {
                case '+': return (new Token("+", TipoToken.OP_SUMA, "+", linea, columna), pos + 1);
                case '-': return (new Token("-", TipoToken.OP_RESTA, "-", linea, columna), pos + 1);
                case '*': return (new Token("*", TipoToken.OP_MULT, "*", linea, columna), pos + 1);
                case '/': return (new Token("/", TipoToken.OP_DIV, "/", linea, columna), pos + 1);
                case '=': return (new Token("=", TipoToken.OP_ASIGNACION, "=", linea, columna), pos + 1);
                case '<': return (new Token("<", TipoToken.OP_MENOR, "<", linea, columna), pos + 1);
                case '>': return (new Token(">", TipoToken.OP_MAYOR, ">", linea, columna), pos + 1);
                case '(': return (new Token("(", TipoToken.PARENTESIS_IZQ, "(", linea, columna), pos + 1);
                case ')': return (new Token(")", TipoToken.PARENTESIS_DER, ")", linea, columna), pos + 1);
                case '{': return (new Token("{", TipoToken.LLAVE_IZQ, "{", linea, columna), pos + 1);
                case '}': return (new Token("}", TipoToken.LLAVE_DER, "}", linea, columna), pos + 1);
                case ';': return (new Token(";", TipoToken.PUNTO_Y_COMA, ";", linea, columna), pos + 1);
                case ',': return (new Token(",", TipoToken.COMA, ",", linea, columna), pos + 1);
                case '&': return (new Token("&", TipoToken.AMPERSAND, "&", linea, columna), pos + 1);
                default:
                    string msg = $"Caracter Desconocido: '{c}'";
                    return (new Token(c.ToString(), TipoToken.TOKEN_ERROR, msg, linea, columna), pos + 1);
            }
        }
    }
}
