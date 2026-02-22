using System;
using System.Collections.Generic;

namespace Analisis_Lexico
{
    internal class Parser
    {
        private List<Token> _tokens;
        private int _posicion;
        private Token _tokenActual;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _posicion = 0;
            _tokenActual = _tokens[0];
        }
        //Inicia el análisis sintáctico
        public void Analizar()
        {
            try
            {
                Programa();
                Console.WriteLine("Análisis sintáctico COMPLETADO sin errores");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR SINTÁCTICO: {ex.Message}");
            }
        }
        //Avanza al siguiente token en la lista
        private void Avanzar()
        {
            _posicion++;
            if (_posicion < _tokens.Count)
                _tokenActual = _tokens[_posicion];
            else
                _tokenActual = new Token(TipoToken.EOF, "", 0, 0);
        }
        //Consume el token actual si coincide con el tipo esperado, de lo contrario lanza una excepción
        private void Consumir(TipoToken tipoEsperado)
        {
            if (_tokenActual.Tipo == tipoEsperado)
            {
                Avanzar();
            }
            else
            {
                throw new Exception($"Se esperaba {tipoEsperado} pero se encontró {_tokenActual.Tipo} en línea {_tokenActual.Linea}");
            }
        }
        //Verifica si el token actual coincide con el tipo dado
        private bool Coincide(TipoToken tipo)
        {
            return _tokenActual.Tipo == tipo;
        }

        // <programa> ::= <lista_sent>
        private void Programa()
        {
            ListaSentencias();
        }

        // <lista_sent> ::= <sentencia> ';' <lista_sent> | <sentencia_control> <lista_sent> | <sentencia> ';' | <sentencia_control> | ⋋
        private void ListaSentencias()
        {
            while (_tokenActual.Tipo != TipoToken.EOF)
            {
                if (EsInicioSentencia())
                {
                    if (EsSentenciaControl())
                    {
                        SentenciaControl();
                    }
                    else
                    {
                        Sentencia();
                        if (Coincide(TipoToken.PUNTO_Y_COMA))
                        {
                            Consumir(TipoToken.PUNTO_Y_COMA);

                            if (Coincide(TipoToken.PUNTO_Y_COMA))
                            {
                                throw new Exception($"Doble punto y coma no permitido en línea {_tokenActual.Linea}");
                            }
                        }
                        else
                        {
                            throw new Exception($"Se esperaba ';' después de sentencia en línea {_tokenActual.Linea}");
                        }
                    }
                }
                else if (Coincide(TipoToken.PUNTO_Y_COMA))
                {
                    throw new Exception($"Punto y coma sin sentencia previa en línea {_tokenActual.Linea}");
                }
                else
                {
                    throw new Exception($"Sentencia inválida: {_tokenActual.Tipo} en línea {_tokenActual.Linea}");
                }
            }
        }
        // <sentencia_control> ::= <if_simple> | <if_extendido> | <ciclo>
        private void SentenciaControl()
        {
            if (Coincide(TipoToken.IF))
            {
                int posicionActual = _posicion;
                Token tokenTemp = _tokenActual;

                bool tieneElse = false;
                int profundidad = 0;

                for (int i = posicionActual; i < _tokens.Count; i++)
                {
                    if (_tokens[i].Tipo == TipoToken.IF) profundidad++;
                    else if (_tokens[i].Tipo == TipoToken.END) profundidad--;
                    else if (_tokens[i].Tipo == TipoToken.ELSE && profundidad == 1)
                    {
                        tieneElse = true;
                        break;
                    }

                    if (profundidad == 0 && i > posicionActual) break;
                }

                if (tieneElse)
                    IfExtendido();
                else
                    IfSimple();
            }
            else if (Coincide(TipoToken.WHILE))
            {
                Ciclo();
            }
            else
            {
                throw new Exception($"Sentencia de control inválida: {_tokenActual.Tipo}");
            }
        }
        // <sentencia> ::= <declaracion> | <asignacion> | <exp_relacional> | <lectura> | <escritura>
        private void Sentencia()
        {
            if (EsTipo())
            {
                Declaracion();
            }
            else if (Coincide(TipoToken.IDENTIFICADOR))
            {
                if (_posicion + 1 < _tokens.Count && _tokens[_posicion + 1].Tipo == TipoToken.OP_ASIGNACION)
                {
                    Asignacion();
                }
                else
                {
                    ExpresionRelacional();
                }
            }
            else if (Coincide(TipoToken.LEER))
            {
                Lectura();
            }
            else if (Coincide(TipoToken.ESCRIBIR))
            {
                Escritura();
            }
            else
            {
                ExpresionRelacional();
            }
        }
        // <declaracion> ::= <tipo> identificador [ '=' <factor> ]
        private void Declaracion()
        {
            if (EsTipo())
            {
                Avanzar();
            }
            else
            {
                throw new Exception($"Se esperaba un tipo (int, char, etc.) pero se encontró {_tokenActual.Tipo}");
            }

            Consumir(TipoToken.IDENTIFICADOR);

            if (Coincide(TipoToken.OP_ASIGNACION))
            {
                Consumir(TipoToken.OP_ASIGNACION);
                Factor();
            }
        }
        // <asignacion> ::= identificador '=' <factor>
        private void Asignacion()
        {
            Consumir(TipoToken.IDENTIFICADOR);
            Consumir(TipoToken.OP_ASIGNACION);
            Factor();
        }
        // <if_simple> ::= 'if' '(' <exp_relacional> ')' 'then' <lista_sent> 'end'
        private void IfSimple()
        {
            Consumir(TipoToken.IF);
            Consumir(TipoToken.PARENTESIS_IZQ);
            ExpresionRelacional();
            Consumir(TipoToken.PARENTESIS_DER);
            Consumir(TipoToken.THEN);
            ListaSentencias();
            Consumir(TipoToken.END);
        }
        // <if_extendido> ::= 'if' '(' <exp_relacional> ')' 'then' <lista_sent> 'else' <lista_sent> 'end'
        private void IfExtendido()
        {
            Consumir(TipoToken.IF);
            Consumir(TipoToken.PARENTESIS_IZQ);
            ExpresionRelacional();
            Consumir(TipoToken.PARENTESIS_DER);
            Consumir(TipoToken.THEN);
            ListaSentencias();
            Consumir(TipoToken.ELSE);
            ListaSentencias();
            Consumir(TipoToken.END);
        }
        // <ciclo> ::= 'while' '(' <exp_relacional> ')' <lista_sent> 'end'
        private void Ciclo()
        {
            Consumir(TipoToken.WHILE);
            Consumir(TipoToken.PARENTESIS_IZQ);
            ExpresionRelacional();
            Consumir(TipoToken.PARENTESIS_DER);
            ListaSentencias();
            Consumir(TipoToken.END);
        }
        // <lectura> ::= 'leer' '(' identificador ')'
        private void Lectura()
        {
            Consumir(TipoToken.LEER);
            Consumir(TipoToken.PARENTESIS_IZQ);
            Consumir(TipoToken.IDENTIFICADOR);
            Consumir(TipoToken.PARENTESIS_DER);
        }
        // <escritura> ::= 'escribir' '(' ( identificador | cadena ) { '&' ( identificador | cadena | número ) } ')'
        private void Escritura()
        {
            Consumir(TipoToken.ESCRIBIR);
            Consumir(TipoToken.PARENTESIS_IZQ);

            if (Coincide(TipoToken.IDENTIFICADOR) || Coincide(TipoToken.LITERAL_CADENA))
            {
                Avanzar();
            }
            else
            {
                throw new Exception($"Se esperaba identificador o cadena en escribir, pero se encontró {_tokenActual.Tipo}");
            }

            while (Coincide(TipoToken.AMPERSAND))
            {
                Consumir(TipoToken.AMPERSAND);
                if (Coincide(TipoToken.IDENTIFICADOR) || Coincide(TipoToken.LITERAL_CADENA) ||
                    Coincide(TipoToken.LITERAL_ENTERO) || Coincide(TipoToken.LITERAL_DECIMAL))
                {
                    Avanzar();
                }
                else
                {
                    throw new Exception($"Se esperaba identificador, cadena o número después de &");
                }
            }

            Consumir(TipoToken.PARENTESIS_DER);
        }
        // Métodos auxiliares para verificar inicios de producciones
        private bool EsInicioSentencia()
        {
            return EsTipo() ||
                   Coincide(TipoToken.IDENTIFICADOR) ||
                   Coincide(TipoToken.IF) ||
                   Coincide(TipoToken.WHILE) ||
                   Coincide(TipoToken.LEER) ||
                   Coincide(TipoToken.ESCRIBIR) ||
                   EsInicioExpresionRelacional();
        }
        // Verifica si el token actual es una sentencia de control (if o while)
        private bool EsSentenciaControl()
        {
            return Coincide(TipoToken.IF) || Coincide(TipoToken.WHILE);
        }
        // Verifica si el token actual es un tipo de dato válido
        private bool EsTipo()
        {
            return Coincide(TipoToken.INT) ||
                   Coincide(TipoToken.CHAR) ||
                   Coincide(TipoToken.BOOLEAN) ||
                   Coincide(TipoToken.DOUBLE);
        }
        // Verifica si el token actual puede iniciar una expresión relacional
        private bool EsInicioExpresionRelacional()
        {
            return Coincide(TipoToken.IDENTIFICADOR) ||
                   Coincide(TipoToken.LITERAL_ENTERO) ||
                   Coincide(TipoToken.LITERAL_DECIMAL) ||
                   Coincide(TipoToken.PARENTESIS_IZQ);
        }
        // <exp_relacional> ::= <exp_aritmetica> [ ( '==' | '!=' | '<' | '>' | '<=' | '>=' ) <exp_aritmetica> ]
        private void ExpresionRelacional()
        {
            ExpresionAritmetica();
            if (EsOperadorRelacional())
            {
                Avanzar(); 
                ExpresionAritmetica();
            }
        }
        // <exp_aritmetica> ::= <termino> { ( '+' | '-' ) <termino> }
        private void ExpresionAritmetica()
        {
            Termino();
            while (Coincide(TipoToken.OP_SUMA) || Coincide(TipoToken.OP_RESTA))
            {
                Avanzar();
                Termino();
            }
        }
        // <termino> ::= <factor> { ( '*' | '/' ) <factor> }
        private void Termino()
        {
            Factor();
            while (Coincide(TipoToken.OP_MULT) || Coincide(TipoToken.OP_DIV))
            {
                Avanzar();
                Factor();
            }
        }
        // <factor> ::= identificador | número | cadena | 'true' | 'false' | '(' <exp_aritmetica> ')'
        private void Factor()
        {
            if (Coincide(TipoToken.IDENTIFICADOR) ||
                Coincide(TipoToken.LITERAL_ENTERO) ||
                Coincide(TipoToken.LITERAL_DECIMAL) ||
                Coincide(TipoToken.LITERAL_CADENA) ||
                Coincide(TipoToken.TRUE) ||
                Coincide(TipoToken.FALSE))
            {
                Avanzar();
            }
            else if (Coincide(TipoToken.PARENTESIS_IZQ))
            {
                Consumir(TipoToken.PARENTESIS_IZQ);
                ExpresionAritmetica();
                Consumir(TipoToken.PARENTESIS_DER);
            }
            else
            {
                throw new Exception($"Factor inválido: {_tokenActual.Tipo}");
            }
        }
        // Verifica si el token actual es un operador relacional
        private bool EsOperadorRelacional()
        {
            return Coincide(TipoToken.OP_IGUALDAD) ||
                   Coincide(TipoToken.OP_DIFERENTE) ||
                   Coincide(TipoToken.OP_MENOR) ||
                   Coincide(TipoToken.OP_MAYOR) ||
                   Coincide(TipoToken.OP_MENOR_IGUAL) ||
                   Coincide(TipoToken.OP_MAYOR_IGUAL);
        }
    }
}