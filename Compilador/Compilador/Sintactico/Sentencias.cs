using Compilador.Tokens;
using Compilador.AST;
using Compilador.AST.Expresiones;
using Compilador.AST.Sentencias;

namespace Compilador.Sintactico
{

    public partial class AnalizadorSintactico
    {
        private NodoAST? Declaracion()
        {
            if (_hayError) return null;

            string tipo = _actual.Lexema;
            int linea = _actual.Linea;
            int columna = _actual.Columna;
            Avanzar(); // consume el tipo

            string nombre = _actual.Lexema;
            Consumir(TipoToken.IDENTIFICADOR);

            NodoAST? valor = null;

            if (!_hayError && Coincide(TipoToken.OP_ASIGNACION))
            {
                Consumir(TipoToken.OP_ASIGNACION);
                valor = ExpresionAritmetica();
            }

            return new NodoDeclaracion(tipo, nombre, valor, linea, columna);
        }

        private NodoAST? Asignacion()
        {
            if (_hayError) return null;

            string nombre = _actual.Lexema;
            int linea = _actual.Linea;
            int columna = _actual.Columna;
            Consumir(TipoToken.IDENTIFICADOR);
            Consumir(TipoToken.OP_ASIGNACION);
            var valor = ExpresionAritmetica();

            return new NodoAsignacion(nombre, valor!, linea, columna);
        }

        private NodoAST? Lectura()
        {
            if (_hayError) return null;

            int linea = _actual.Linea;
            int columna = _actual.Columna;
            Consumir(TipoToken.LEER);
            Consumir(TipoToken.PARENTESIS_IZQ);

            string nombre = _actual.Lexema;
            Consumir(TipoToken.IDENTIFICADOR);
            Consumir(TipoToken.PARENTESIS_DER);

            return new NodoLeer(nombre, linea, columna);
        }

        private NodoAST? Escritura()
        {
            if (_hayError) return null;

            int linea = _actual.Linea;
            int columna = _actual.Columna;
            Consumir(TipoToken.ESCRIBIR);
            Consumir(TipoToken.PARENTESIS_IZQ);

            var valores = new List<NodoAST>();

            if (Coincide(TipoToken.IDENTIFICADOR))
            {
                valores.Add(new NodoIdentificador(_actual.Lexema, _actual.Linea, _actual.Columna));
                Avanzar();
            }
            else if (Coincide(TipoToken.LITERAL_CADENA))
            {
                valores.Add(new NodoCadena(_actual.Lexema, _actual.Linea, _actual.Columna));
                Avanzar();
            }
            else
            {
                LanzarError("ESCRIBIR: se esperaba identificador o cadena");
                return null;
            }

            while (!_hayError && Coincide(TipoToken.AMPERSAND))
            {
                Consumir(TipoToken.AMPERSAND);

                if (Coincide(TipoToken.IDENTIFICADOR))
                {
                    valores.Add(new NodoIdentificador(_actual.Lexema, _actual.Linea, _actual.Columna));
                    Avanzar();
                }
                else if (Coincide(TipoToken.LITERAL_CADENA))
                {
                    valores.Add(new NodoCadena(_actual.Lexema, _actual.Linea, _actual.Columna));
                    Avanzar();
                }
                else if (Coincide(TipoToken.LITERAL_ENTERO))
                {
                    valores.Add(new NodoNumero(int.Parse(_actual.Lexema), _actual.Linea, _actual.Columna));
                    Avanzar();
                }
                else if (Coincide(TipoToken.LITERAL_DECIMAL))
                {
                    valores.Add(new NodoDecimal(double.Parse(_actual.Lexema, System.Globalization.CultureInfo.InvariantCulture), _actual.Linea, _actual.Columna));
                    Avanzar();
                }
                else
                {
                    LanzarError("Se esperaba valor despues de '&'");
                    return null;
                }
            }

            Consumir(TipoToken.PARENTESIS_DER);

            return new NodoEscribir(valores, linea, columna);
        }

        private NodoAST? IfSimple()
        {
            int linea = _actual.Linea;
            int columna = _actual.Columna;
            Consumir(TipoToken.SI);
            Consumir(TipoToken.PARENTESIS_IZQ);
            var condicion = ExpresionRelacional();
            Consumir(TipoToken.PARENTESIS_DER);
            Consumir(TipoToken.ENTONCES);
            var entonces = ListaSentencias();
            Consumir(TipoToken.FIN);

            return new NodoSi(condicion!, entonces, linea, columna);
        }

        private NodoAST? IfExtendido()
        {
            int linea = _actual.Linea;
            int columna = _actual.Columna;
            Consumir(TipoToken.SI);
            Consumir(TipoToken.PARENTESIS_IZQ);
            var condicion = ExpresionRelacional();
            Consumir(TipoToken.PARENTESIS_DER);
            Consumir(TipoToken.ENTONCES);
            var entonces = ListaSentencias();
            Consumir(TipoToken.SINO);
            var sino = ListaSentencias();
            Consumir(TipoToken.FIN);

            return new NodoSi(condicion!, entonces, linea, columna, sino);
        }

        private NodoAST? Ciclo()
        {
            int linea = _actual.Linea;
            int columna = _actual.Columna;
            Consumir(TipoToken.MIENTRAS);
            Consumir(TipoToken.PARENTESIS_IZQ);
            var condicion = ExpresionRelacional();
            Consumir(TipoToken.PARENTESIS_DER);
            var cuerpo = ListaSentencias();
            Consumir(TipoToken.FIN);

            return new NodoMientras(condicion!, cuerpo, linea, columna);
        }

        private NodoAST? SentenciaControl()
        {
            if (_hayError) return null;

            if (Coincide(TipoToken.SI))
            {
                bool tieneElse = false;
                int prof = 0;

                for (int i = _pos; i < _tokens.Count; i++)
                {
                    TipoToken t = TokenEn(i).Tipo;

                    if (t == TipoToken.SI) prof++;
                    else if (t == TipoToken.FIN) prof--;
                    else if (t == TipoToken.SINO && prof == 1) { tieneElse = true; break; }

                    if (prof == 0 && i > _pos) break;
                }

                return tieneElse ? IfExtendido() : IfSimple();
            }

            if (Coincide(TipoToken.MIENTRAS))
                return Ciclo();

            LanzarError($"Control invalido: '{_actual.Tipo}' en L{_actual.Linea}");
            return null;
        }

        private NodoAST? Sentencia()
        {
            if (_hayError) return null;

            if (EsTipo()) return Declaracion();

            if (Coincide(TipoToken.IDENTIFICADOR))
            {
                if (TokenEn(_pos + 1).Tipo == TipoToken.OP_ASIGNACION)
                    return Asignacion();
                else
                    return ExpresionRelacional();
            }

            if (Coincide(TipoToken.LEER)) return Lectura();
            if (Coincide(TipoToken.ESCRIBIR)) return Escritura();

            return ExpresionRelacional();
        }

        private List<NodoAST> ListaSentencias()
        {
            var sentencias = new List<NodoAST>();

            while (!_hayError && !Coincide(TipoToken.TOKEN_EOF) &&
                   !Coincide(TipoToken.FIN) && !Coincide(TipoToken.SINO))
            {
                if (EsInicioSent())
                {
                    NodoAST? nodo;

                    if (Coincide(TipoToken.SI) || Coincide(TipoToken.MIENTRAS))
                    {
                        nodo = SentenciaControl();
                        if (nodo != null) sentencias.Add(nodo);
                        continue;
                    }

                    nodo = Sentencia();
                    if (nodo != null) sentencias.Add(nodo);

                    if (!_hayError)
                    {
                        if (Coincide(TipoToken.PUNTO_Y_COMA))
                        {
                            Consumir(TipoToken.PUNTO_Y_COMA);

                            if (Coincide(TipoToken.PUNTO_Y_COMA))
                                LanzarError($"Doble ';' en L{_actual.Linea}");
                        }
                        else
                        {
                            Token anterior = TokenEn(_pos - 1);
                            LanzarError($"Falta ';' en L{anterior.Linea} (encontrado '{_actual.Tipo}' en L{_actual.Linea})");
                        }
                    }
                }
                else
                {
                    LanzarError($"Sentencia invalida: '{_actual.Lexema}' en L{_actual.Linea}");
                }
            }

            return sentencias;
        }
    }
}
