using Compilador.Tokens;
using Compilador.AST;
using Compilador.AST.Sentencias;

namespace Compilador.Sintactico
{
    public partial class AnalizadorSintactico
    {
        private List<Token> _tokens;
        private int _pos;
        private Token _actual;
        private bool _hayError;
        private string _mensajeError;

        public AnalizadorSintactico(List<Token> tokens)
        {
            _tokens = tokens;
            _pos = 0;
            _hayError = false;
            _mensajeError = "";

            while (_pos < _tokens.Count && _tokens[_pos].Tipo == TipoToken.TOKEN_ERROR)
                _pos++;

            _actual = TokenEn(_pos);
        }

        private Token TokenEn(int indice)
        {
            if (indice >= 0 && indice < _tokens.Count)
                return _tokens[indice];

            return new Token("", TipoToken.TOKEN_EOF, "", 0, 0);
        }

        private void Avanzar()
        {
            _pos++;
            _actual = TokenEn(_pos);
        }

        private bool Coincide(TipoToken tipo)
        {
            return _actual.Tipo == tipo;
        }

        private void Consumir(TipoToken esperado)
        {
            if (_hayError) return;

            if (_actual.Tipo == esperado)
            {
                Avanzar();
                return;
            }

            LanzarError($"Esperaba '{esperado}' pero encontro '{_actual.Tipo}' (lexema:'{_actual.Lexema}') en L{_actual.Linea}");
        }

        private void LanzarError(string mensaje)
        {
            if (!_hayError)
            {
                _hayError = true;
                _mensajeError = mensaje;
            }
        }

        private bool EsTipo() =>
            Coincide(TipoToken.ENTERO) || Coincide(TipoToken.CARACTER) ||
            Coincide(TipoToken.BOLEANO) || Coincide(TipoToken.DOBLE);

        private bool EsOpRel() =>
            Coincide(TipoToken.OP_IGUALDAD) || Coincide(TipoToken.OP_DIFERENTE) ||
            Coincide(TipoToken.OP_MENOR) || Coincide(TipoToken.OP_MAYOR) ||
            Coincide(TipoToken.OP_MENOR_IGUAL) || Coincide(TipoToken.OP_MAYOR_IGUAL);

        private bool EsInicioExp() =>
            Coincide(TipoToken.IDENTIFICADOR) || Coincide(TipoToken.LITERAL_ENTERO) ||
            Coincide(TipoToken.LITERAL_DECIMAL) || Coincide(TipoToken.PARENTESIS_IZQ);

        private bool EsInicioSent() =>
            EsTipo() || Coincide(TipoToken.IDENTIFICADOR) || Coincide(TipoToken.SI) ||
            Coincide(TipoToken.MIENTRAS) || Coincide(TipoToken.LEER) ||
            Coincide(TipoToken.ESCRIBIR) || EsInicioExp();

        public NodoPrograma? Analizar()
        {
            var sentencias = ListaSentencias();

            if (!_hayError && !Coincide(TipoToken.TOKEN_EOF))
                LanzarError($"Tokens inesperados al final: '{_actual.Lexema}' L{_actual.Linea}");

            if (_hayError)
            {
                Console.WriteLine($"  ERROR SINTACTICO: {_mensajeError}");
                return null;
            }

            Console.WriteLine("  Sintaxis correcta");
            return new NodoPrograma(sentencias);
        }
    }
}