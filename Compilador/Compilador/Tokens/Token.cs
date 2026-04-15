namespace Compilador.Tokens
{
    public record Token(
        string Lexema,
        TipoToken Tipo,
        string Valor,
        int Linea,
        int Columna
    );
}