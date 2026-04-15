using Compilador.Lexico;
using Compilador.Tokens;
using Compilador.Sintactico;
using Compilador.Utilidades;

Impresora.Separador();
Console.WriteLine("====== ANALIZADOR LEXICO ======");
Impresora.Separador();

string rutaBase = AppDomain.CurrentDomain.BaseDirectory;
string rutaArchivo = Path.Combine(rutaBase, "Datos", "Programa.txt");

if (!File.Exists(rutaArchivo))
{
    Console.WriteLine($"Error: no se encontro {rutaArchivo}");
    Console.ReadKey();
    return;
}

string codigo = File.ReadAllText(rutaArchivo);
Console.WriteLine($"Archivo leido: {codigo.Length} caracteres");

List<Token> tokens = AnalizadorLexico.Analizar(codigo);
Console.WriteLine($"Tokens encontrados: {tokens.Count - 1}");

Impresora.ImprimirLista(tokens);

Impresora.Separador();
Console.WriteLine("====== RESUMEN DE TOKENS ======");
Impresora.Separador();

int invalidos = tokens.Count(t => t.Tipo == TipoToken.TOKEN_ERROR);
Impresora.ImprimirResumen(tokens);

Impresora.Separador();
Console.WriteLine("\n====== ANALISIS SINTACTICO ======");

if (invalidos > 0)
{
    Console.WriteLine($"Omitido: corrija los {invalidos} error(es) lexico(s) primero.");
}
else
{
    var parser = new AnalizadorSintactico(tokens);
    var arbol = parser.Analizar();

    if (arbol != null)
    {
        Impresora.Separador();
        Console.WriteLine("\n====== AST - PREORDEN ======");
        ImpresoraAST.ImprimirPreorden(arbol);

        Impresora.Separador();
        Console.WriteLine("\n====== AST - INORDEN ======");
        ImpresoraAST.ImprimirInorden(arbol);

        Impresora.Separador();
        Console.WriteLine("\n====== AST - POSTORDEN ======");
        ImpresoraAST.ImprimirPostorden(arbol);

        Impresora.Separador();
        Console.WriteLine("\n====== AST - ARBOL VISUAL ======");
        ImpresoraAST.ImprimirArbol(arbol);
    }
}

Console.WriteLine("\nPresiona ENTER para salir...");
Console.ReadLine();