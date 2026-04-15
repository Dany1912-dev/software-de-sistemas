using Compilador.Tokens;

namespace Compilador.Utilidades
{
    public static class PalabrasReservadas
    {
        private static readonly Dictionary<string, TipoToken> _reservadas = new()
        {
            { "si", TipoToken.SI },
            { "entonces", TipoToken.ENTONCES },
            { "sino", TipoToken.SINO },
            { "fin", TipoToken.FIN },
            { "mientras", TipoToken.MIENTRAS },
            { "entero", TipoToken.ENTERO },
            { "caracter", TipoToken.CARACTER },
            { "booleano", TipoToken.BOLEANO },
            { "doble", TipoToken.DOBLE },
            { "verdadero", TipoToken.VERDADERO },
            { "falso", TipoToken.FALSO },
            { "leer", TipoToken.LEER },
            { "escribir", TipoToken.ESCRIBIR }
        };

        public static TipoToken Buscar(string lexema)
        {
            string clave = lexema.ToLower();
            return _reservadas.TryGetValue(clave, out var tipo)
                ? tipo
                : TipoToken.IDENTIFICADOR;
        }

        public static bool EsMalFormada(string lexema)
        {
            string lower = lexema.ToLower();

            foreach (var palabra in _reservadas.Keys)
            {
                if (lower.StartsWith(palabra) && lower != palabra && lower.Length > palabra.Length && (char.IsLetterOrDigit(lower[palabra.Length]) || lower[palabra.Length] == '_'))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool EsReservada(TipoToken tipo)
        {
            return tipo >= TipoToken.SI && tipo <= TipoToken.ESCRIBIR;
        }
    }
}
