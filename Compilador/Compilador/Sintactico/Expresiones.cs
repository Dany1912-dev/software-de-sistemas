using Compilador.Tokens;
using Compilador.AST;
using Compilador.AST.Expresiones;

namespace Compilador.Sintactico
{

    public partial class AnalizadorSintactico
    {
        private NodoAST? Factor()
        {
            if (_hayError) return null;

            if (Coincide(TipoToken.LITERAL_ENTERO))
            {
                int valor = int.Parse(_actual.Lexema);
                int linea = _actual.Linea;
                int columna = _actual.Columna;
                Avanzar();
                return new NodoNumero(valor, linea, columna);
            }

            if (Coincide(TipoToken.LITERAL_DECIMAL))
            {
                double valor = double.Parse(_actual.Lexema, System.Globalization.CultureInfo.InvariantCulture);
                int linea = _actual.Linea;
                int columna = _actual.Columna;
                Avanzar();
                return new NodoDecimal(valor, linea, columna);
            }

            if (Coincide(TipoToken.LITERAL_CADENA))
            {
                string valor = _actual.Lexema;
                int linea = _actual.Linea;
                int columna = _actual.Columna;
                Avanzar();
                return new NodoCadena(valor, linea, columna);
            }

            if (Coincide(TipoToken.VERDADERO))
            {
                int linea = _actual.Linea;
                int columna = _actual.Columna;
                Avanzar();
                return new NodoBooleano(true, linea, columna);
            }

            if (Coincide(TipoToken.FALSO))
            {
                int linea = _actual.Linea;
                int columna = _actual.Columna;
                Avanzar();
                return new NodoBooleano(false, linea, columna);
            }

            if (Coincide(TipoToken.IDENTIFICADOR))
            {
                string nombre = _actual.Lexema;
                int linea = _actual.Linea;
                int columna = _actual.Columna;
                Avanzar();
                return new NodoIdentificador(nombre, linea, columna);
            }

            if (Coincide(TipoToken.PARENTESIS_IZQ))
            {
                Consumir(TipoToken.PARENTESIS_IZQ);
                var expr = ExpresionAritmetica();
                Consumir(TipoToken.PARENTESIS_DER);
                return expr;
            }

            LanzarError("Factor invalido");
            return null;
        }

        private NodoAST? Termino()
        {
            if (_hayError) return null;

            var izq = Factor();

            while (!_hayError && (Coincide(TipoToken.OP_MULT) || Coincide(TipoToken.OP_DIV)))
            {
                string op = _actual.Lexema;
                int linea = _actual.Linea;
                int columna = _actual.Columna;
                Avanzar();
                var der = Factor();
                izq = new NodoBinaria(op, izq!, der!, linea, columna);
            }

            return izq;
        }

        private NodoAST? ExpresionAritmetica()
        {
            if (_hayError) return null;

            var izq = Termino();

            while (!_hayError && (Coincide(TipoToken.OP_SUMA) || Coincide(TipoToken.OP_RESTA)))
            {
                string op = _actual.Lexema;
                int linea = _actual.Linea;
                int columna = _actual.Columna;
                Avanzar();
                var der = Termino();
                izq = new NodoBinaria(op, izq!, der!, linea, columna);
            }

            return izq;
        }

        private NodoAST? ExpresionRelacional()
        {
            if (_hayError) return null;

            var izq = ExpresionAritmetica();

            if (!_hayError && EsOpRel())
            {
                string op = _actual.Lexema;
                int linea = _actual.Linea;
                int columna = _actual.Columna;
                Avanzar();
                var der = ExpresionAritmetica();
                izq = new NodoBinaria(op, izq!, der!, linea, columna);
            }

            return izq;
        }
    }
}
