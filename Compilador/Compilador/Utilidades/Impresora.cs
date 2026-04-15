using Compilador.Tokens;

namespace Compilador.Utilidades
{
    public static class Impresora
    {
        private static string NombreTipo(TipoToken tipo)
        {
            if (PalabrasReservadas.EsReservada(tipo))
                return $"PR_{tipo}";

            return tipo.ToString();
        }

        public static void Separador()
        {
            Console.WriteLine("=================================");
        }

        public static void ImprimirToken(Token token)
        {
            string nombre = NombreTipo(token.Tipo);

            if (token.Tipo == TipoToken.TOKEN_ERROR)
                Console.WriteLine($"  [ERROR ] {nombre,-20} | '{token.Lexema,-20}' | L:{token.Linea} C:{token.Columna} | {token.Valor}");
            else
                Console.WriteLine($"  [VALIDO] {nombre,-20} | '{token.Lexema,-20}' | L:{token.Linea} C:{token.Columna}");
        }

        public static void ImprimirLista(List<Token> tokens)
        {
            Console.WriteLine($"\n=== LISTA DE TOKENS ({tokens.Count} elementos) ===");

            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                string nombre = NombreTipo(token.Tipo);

                Console.WriteLine("  +--------------------------------+");
                Console.WriteLine($"  | Nodo #{i,-3}                      |");
                Console.WriteLine($"  | Tipo   : {nombre,-22} |");
                Console.WriteLine($"  | Lexema : {token.Lexema,-22} |");
                Console.WriteLine($"  | Linea  : {token.Linea,-22} |");
                Console.WriteLine($"  | Columna: {token.Columna,-22} |");

                if (token.Tipo == TipoToken.TOKEN_ERROR)
                    Console.WriteLine($"  | ERROR  : {token.Valor,-22} |");

                Console.WriteLine("  +--------------------------------+");

                if (i < tokens.Count - 1)
                    Console.WriteLine("           |\n           v");
            }

            Console.WriteLine("  (fin de lista)");
        }

        public static void ImprimirResumen(List<Token> tokens)
        {
            int validos = 0;
            int invalidos = 0;

            foreach (var token in tokens)
            {
                if (token.Tipo == TipoToken.TOKEN_EOF)
                    continue;

                ImprimirToken(token);

                if (token.Tipo == TipoToken.TOKEN_ERROR)
                    invalidos++;
                else
                    validos++;
            }

            Separador();
            Console.WriteLine($"Tokens validos  : {validos}");
            Console.WriteLine($"Tokens invalidos: {invalidos}");
        }
    }
}
