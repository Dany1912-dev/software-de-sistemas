namespace Compilador.Tokens
{
    public enum TipoToken
    {
        // Palabras reservadas
        SI,
        ENTONCES,
        SINO,
        FIN,
        MIENTRAS,

        // Tipos de dato
        ENTERO,
        CARACTER,
        BOLEANO,
        DOBLE,

        // Literales booleanos
        VERDADERO,
        FALSO,

        // E/S
        LEER,
        ESCRIBIR,

        // Identificador
        IDENTIFICADOR,

        // Literales
        LITERAL_ENTERO,
        LITERAL_DECIMAL,
        LITERAL_CADENA,
        LITERAL_BOOLEANO,

        // Operadores aritméticos
        OP_SUMA,
        OP_RESTA,
        OP_MULT,
        OP_DIV,

        // Asignación
        OP_ASIGNACION,

        // Operadores relacionales
        OP_IGUALDAD,
        OP_DIFERENTE,
        OP_MENOR,
        OP_MAYOR,
        OP_MENOR_IGUAL,
        OP_MAYOR_IGUAL,

        // Delimitadores
        PARENTESIS_IZQ,
        PARENTESIS_DER,
        LLAVE_IZQ,
        LLAVE_DER,
        PUNTO_Y_COMA,
        COMA,
        AMPERSAND,

        // Especiales
        TOKEN_ERROR,
        TOKEN_EOF
    }
}
