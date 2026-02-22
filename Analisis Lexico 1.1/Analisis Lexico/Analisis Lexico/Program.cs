using System;
using System.Collections.Generic;
using System.IO;

namespace Analisis_Lexico
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== COMPILADOR COMPLETO ===");

            string archivo = "C:/Users/leyva/OneDrive/Desktop/Programa.txt";
            if (!File.Exists(archivo))
            {
                Console.WriteLine($"Error: El archivo {archivo} no existe");
                Console.WriteLine("Presiona cualquier tecla para salir...");
                Console.ReadKey();
                return;
            }

            try
            {
                string codigo = File.ReadAllText(archivo);
                Console.WriteLine("Código fuente leído correctamente");

                Console.WriteLine("\n=== ANÁLISIS LÉXICO ===");
                AnalizadorLexico lexer = new AnalizadorLexico(codigo);
                var tokens = lexer.AnalizarCodigo(codigo);

                int tokensNormales = 0;
                int tokensError = 0;

                foreach (var token in tokens)
                {
                    if (token.Tipo == TipoToken.ERROR)
                    {
                        Console.WriteLine($"{token}");
                        tokensError++;
                    }
                    else if (token.Tipo != TipoToken.EOF)
                    {
                        Console.WriteLine($"{token}");
                        tokensNormales++;
                    }
                }

                Console.WriteLine($"\nResumen léxico: {tokensNormales} tokens válidos, {tokensError} errores");

                if (tokensError == 0)
                {
                    Console.WriteLine("\n=== ANÁLISIS SINTÁCTICO ===");
                    var parser = new Parser(tokens);
                    parser.Analizar();
                }
                else
                {
                    Console.WriteLine("\nNo se realiza análisis sintáctico por errores léxicos");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
            }

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}