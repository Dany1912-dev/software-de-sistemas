using Compilador.Tokens;
namespace Compilador.Lexico
{
    public static class AnalizadorLexico
    {
        public static List<Token> Analizar(string codigo)
        {
            var tokens = new List<Token>();
            int pos = 0;
            int linea = 1;
            int columna = 1;
            
            while (pos < codigo.Length)
            {
                char c = codigo[pos];

                // Espacios en blanco
                if (char.IsWhiteSpace(c))
                {
                    if (c == '\n')
                    {
                        linea++;
                        columna = 1;
                    }
                    else
                    {
                        columna++;
                    }

                    pos++;
                    continue;
                }

                // Comentarios de línea
                if (c == '/' && pos + 1 < codigo.Length && codigo[pos + 1] == '/')
                {
                    while (pos < codigo.Length && codigo[pos] != '\n')
                        pos++;

                    continue;
                }

                Token token;
                int nuevaPos;

                if (char.IsLetter(c) || c == '_')
                {
                    (token, nuevaPos) = LectorIdentificador.Leer(codigo, pos, linea, columna);
                }
                else if (char.IsDigit(c))
                {
                    (token, nuevaPos) = LectorNumero.Leer(codigo, pos, linea, columna);
                }
                else if (c == '"')
                {
                    (token, nuevaPos) = LectorCadena.Leer(codigo, pos, linea, columna);
                }
                else
                {
                    (token, nuevaPos) = LectorSimbolo.Leer(codigo, pos, linea, columna);
                }

                tokens.Add(token);
                columna += (nuevaPos - pos);
                pos = nuevaPos;
            }

            tokens.Add(new Token("EOF", TipoToken.TOKEN_EOF, "", linea, columna));

            return tokens;
        }
    }
}
