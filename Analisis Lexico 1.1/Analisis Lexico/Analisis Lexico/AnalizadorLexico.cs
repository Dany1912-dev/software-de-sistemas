using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analisis_Lexico
{
    public enum Estado
    {
        E1, // Estado inicial
        IDENTIFICADOR,
        NUMERO_ENTERO,
        NUMERO_DECIMAL,
        CADENA,
        OPERADOR_SIMPLE,
        OPERADOR_DOBLE,
        COMENTARIO,
        ERROR,
        ACEPTACION
    }
    public enum TipoToken
    {
        // Palabras reservadas
        SI, ENTONCES, SINO, FIN, MIENTRAS,
        ENTERO, CARCACTER, BOLEANO, DOBLE,
        VERDADERO, FALSO, LEER, ESCRIBIR,

        // Identificadores y literales
        IDENTIFICADOR,
        LITERAL_ENTERO,
        LITERAL_DECIMAL,
        LITERAL_CADENA,
        LITERAL_BOOLEANO,

        // Operadores
        OP_SUMA, OP_RESTA, OP_MULT, OP_DIV,
        OP_ASIGNACION, OP_IGUALDAD, OP_DIFERENTE,
        OP_MENOR, OP_MAYOR, OP_MENOR_IGUAL, OP_MAYOR_IGUAL,

        // Delimitadores
        PARENTESIS_IZQ, PARENTESIS_DER,
        LLAVE_IZQ, LLAVE_DER,
        PUNTO_Y_COMA, COMA, AMPERSAND,

        ERROR,
        EOF
    }
    public class Token
    {
        public string Lexema { get; set; }
        public TipoToken Tipo { get; set; }
        public string Valor { get; set; }
        public int Linea { get; set; }
        public int Columna { get; set; }

        public Token(string lexema, TipoToken tipo, string valor, int linea, int columna)
        {
            Lexema = lexema;
            Tipo = tipo;
            Valor = valor;
            Linea = linea;
            Columna = columna;
        }

        public override string ToString()
        {
            return $"{Tipo}('{Valor}') [Línea: {Linea}, Columna: {Columna}]";
        }
    }
    internal class AnalizadorLexico
    {
        string codigoFuente;
        public static readonly Dictionary<string, TipoToken> _palabrasReservadas = new Dictionary<string, TipoToken>
        {
            {"si", TipoToken.SI},
            {"entonces", TipoToken.ENTONCES},
            {"sino", TipoToken.SINO},
            {"fin", TipoToken.FIN},
            {"mientras", TipoToken.MIENTRAS},
            {"entero", TipoToken.ENTERO},
            {"caracter", TipoToken.CARCACTER},
            {"boleano", TipoToken.BOLEANO},
            {"doble", TipoToken.DOBLE},
            {"verdadero", TipoToken.VERDADERO},
            {"falso", TipoToken.FALSO},
            {"leer", TipoToken.LEER},
            {"escribir", TipoToken.ESCRIBIR}
        };
        public AnalizadorLexico(string codigoFuente)
        {
            this.codigoFuente = codigoFuente;
        }
        public List<Token> AnalizarCodigo(string codigo)
        {
            List<Token> tokens = new List<Token>();
            int pos = 0;
            int linea = 1;
            int columna = 1;

            while (pos < codigo.Length)
            {
                char actual = codigo[pos];

                // Saltar espacios y tabs
                if (char.IsWhiteSpace(actual))
                {
                    if (actual == '\n')
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

                Token token;
                int nuevaPos;

                if (char.IsLetter(actual) || actual == '_')
                {
                    (token, nuevaPos) = LeerIdentificador(codigo, pos, linea, columna);
                }
                else if (char.IsDigit(actual))
                {
                    (token, nuevaPos) = LeerNumero(codigo, pos, linea, columna);
                }
                else if (actual == '"')
                {
                    (token, nuevaPos) = LeerCadena(codigo, pos, linea, columna);
                }
                else
                {
                    (token, nuevaPos) = LeerSimbolo(codigo, pos, linea, columna);
                }

                tokens.Add(token);

                if (token.Tipo == TipoToken.ERROR)
                {
                    Console.WriteLine($"ERROR LÉXICO: {token.Valor}");
                }

                pos = nuevaPos;
                columna += token.Valor.Length;

                // Ajustar por caracteres especiales como comilla<s
                if (actual == '"') columna += 2;
            }

            tokens.Add(new Token("", TipoToken.EOF, "", linea, columna));
            return tokens;
        }

        (Token, int) LeerIdentificador(string codigo, int pos, int linea, int columna)
        {
            int inicio = pos;

            // Leer hasta el siguiente espacio o símbolo
            while (pos < codigo.Length &&
                   (char.IsLetterOrDigit(codigo[pos]) || codigo[pos] == '_'))
            {
                pos++;
            }

            string lexema = codigo.Substring(inicio, pos - inicio);
            string lexemaMinuscula = lexema.ToLower();

            // Detectar palabra reservada mal formada como "intif"
            if (lexemaMinuscula.Contains("entero") && lexemaMinuscula != "entero" ||
                lexemaMinuscula.Contains("si") && lexemaMinuscula != "si")
            {
                // Verificar si es una combinación de palabras reservadas
                foreach (string palabraReservada in _palabrasReservadas.Keys)
                {
                    if (lexemaMinuscula.Contains(palabraReservada) &&
                        lexemaMinuscula != palabraReservada)
                    {
                        return (new Token(lexema, TipoToken.ERROR, $"Palabra reservada mal formada: '{lexema}'", linea, columna), pos);
                    }
                }
            }

            TipoToken tipo;
            switch (lexemaMinuscula)
            {
                case "si": tipo = TipoToken.SI; break;
                case "entonces": tipo = TipoToken.ENTONCES; break;
                case "sino": tipo = TipoToken.SINO; break;
                case "fin": tipo = TipoToken.FIN; break;
                case "mientras": tipo = TipoToken.MIENTRAS; break;
                case "entero": tipo = TipoToken.ENTERO; break;
                case "caracter": tipo = TipoToken.CARCACTER; break;
                case "boleano": tipo = TipoToken.BOLEANO; break;
                case "doble": tipo = TipoToken.DOBLE; break;
                case "verdadero": tipo = TipoToken.VERDADERO; break;
                case "falso": tipo = TipoToken.FALSO; break;
                case "leer": tipo = TipoToken.LEER; break;
                case "escribir": tipo = TipoToken.ESCRIBIR; break;
                default: tipo = TipoToken.IDENTIFICADOR; break;
            }

            return (new Token(lexema, tipo, lexema, linea, columna), pos);
        }

        static (Token, int) LeerNumero(string codigo, int pos, int linea, int columna)
        {
            int inicio = pos;
            bool tienePunto = false;

            while (pos < codigo.Length && (char.IsDigit(codigo[pos]) || codigo[pos] == '.'))
            {
                if (codigo[pos] == '.')
                {
                    if (tienePunto) break; // Solo un punto decimal
                    tienePunto = true;
                }
                pos++;
            }

            string lexema = codigo.Substring(inicio, pos - inicio);
            TipoToken tipo = tienePunto ? TipoToken.LITERAL_DECIMAL : TipoToken.LITERAL_ENTERO;

            return (new Token(lexema, tipo, lexema, linea, columna), pos);
        }

        static (Token, int) LeerCadena(string codigo, int pos, int linea, int columna)
        {
            int inicio = ++pos; // Saltar comilla inicial

            while (pos < codigo.Length && codigo[pos] != '"')
            {
                pos++;
            }

            if (pos >= codigo.Length)
            {
                // Cadena sin cerrar
                string lexema = codigo.Substring(inicio);
                return (new Token(lexema, TipoToken.LITERAL_CADENA, lexema, linea, columna), pos);
            }

            string contenido = codigo.Substring(inicio, pos - inicio);
            pos++; // Saltar comilla final

            return (new Token(contenido, TipoToken.LITERAL_CADENA, contenido, linea, columna), pos);
        }
        // Leer operadores y símbolos
        static (Token, int) LeerSimbolo(string codigo, int pos, int linea, int columna)
        {
            char actual = codigo[pos];

            if (pos + 1 < codigo.Length)
            {
                string dosCaracteres = codigo.Substring(pos, 2);
                switch (dosCaracteres)
                {
                    case "==": return (new Token("==", TipoToken.OP_IGUALDAD, "==", linea, columna), pos + 2);
                    case "!=": return (new Token("!=", TipoToken.OP_DIFERENTE, "!=", linea, columna), pos + 2);
                    case "<=": return (new Token("<=", TipoToken.OP_MENOR_IGUAL, "<=", linea, columna), pos + 2);
                    case ">=": return (new Token(">=", TipoToken.OP_MAYOR_IGUAL, ">=", linea, columna), pos + 2);
                }
            }

            switch (actual)
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
                    return (new Token(actual.ToString(), TipoToken.ERROR, $"Carácter desconocido: '{actual}'", linea, columna), pos + 1);
            }
        }
    }
}