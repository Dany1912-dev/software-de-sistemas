using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analisis_Lexico
{
    internal class Program
    {
        enum Estado
        {
            E1, //Estado inicial
            v2, v3, v4, //Variables
            n2, n3, n4, n5, n6, n7, //Numeros
            c2, c3, //Cadenas ("")
            s2, s3, //Simbolos ( +, -, * , /)
            error, //Estado de error
            aceptacion //Estado de aceptacion
        }
        enum Categoria
        {
            Letra,
            Numero,
            Simbolo,
            Comilla,
            Punto,
            LetraE,
            FDC,
            Desconocido
        }
        static void Main(string[] args)
        {
            int opc = 1;
            while (opc == 1)
            {
                Console.Clear();
                Console.WriteLine("Ingrese una cadena para validar: ");
                string cadena = Console.ReadLine() + "\0";

                Estado estadoActual = Estado.E1;
                string lexemaActual = "";

                foreach (char c in cadena)
                {
                    Categoria categoria = ObtenerCategoria(c, estadoActual);
                    lexemaActual += c;

                    Console.WriteLine($"Carácter: '{c}' | Categoría: {categoria} | Estado Actual: {estadoActual}");

                    // Verificar palabra reservada si estamos en un estado de variable
                    if (Transiciones.TryGetValue((estadoActual, categoria), out Estado siguiente))
                    {
                        estadoActual = siguiente;

                        if ((estadoActual == Estado.v2 || estadoActual == Estado.v4) && PalabrasReservadas.Contains(lexemaActual.Trim('\0')))
                        {
                            estadoActual = Estado.aceptacion;
                            Console.WriteLine($"Palabra reservada detectada: {lexemaActual.Trim('\0')}");
                        }
                    }
                    else
                    {
                        estadoActual = Estado.error;
                        Console.WriteLine($"Transición no definida para el estado {estadoActual} con categoría {categoria}. Cadena inválida.");
                        break;
                    }

                    //Reiniciar al llegar a estado de aceptacion o error
                    if (estadoActual == Estado.aceptacion || estadoActual == Estado.error)
                    {
                        Console.WriteLine(ObtenerCategoria(c, estadoActual) == Categoria.FDC ? "Fin de cadena alcanzado." : "Cadena Invalida, reiniciando análisis para nueva cadena.");
                    }

                    bool Valida = estadoActual == Estado.aceptacion || EstadosDeAceptacion.Contains(estadoActual);

                    Console.WriteLine(Valida ? "Cadena válida" : "Cadena inválida");
                    Console.WriteLine($"Estado Final: {estadoActual}, No se aceptan letras despues de los numeros en las variables");

                }
                Console.ReadKey();
            }
        }

        static Categoria ObtenerCategoria(char c, Estado estadoActual)
        {
            if (c == 'e' || c == 'E')
            {
                if ((estadoActual == Estado.n2) || estadoActual == Estado.n4) return Categoria.LetraE;
            }

            if (char.IsLetter(c)) return Categoria.Letra;   
            if (char.IsDigit(c)) return Categoria.Numero;   
            if (c == '+' || c == '-' || c == '*' || c == '/' || c == '{' || c == '}' || c == '(' || c == ')') return Categoria.Simbolo;
            if (c == '"') return Categoria.Comilla;
            if (c == '.') return Categoria.Punto;
            if (c == '\0') return Categoria.FDC;
            return Categoria.Desconocido; //Cualquier otro simbolo
        }

        static readonly HashSet<string> PalabrasReservadas = new HashSet<string>
        {
            "if", "else", "while", "for", "return", "int", "float",
            "string", "bool", "true", "false", "void", "class"
        };

        static readonly HashSet<Estado> EstadosDeAceptacion = new HashSet<Estado>
        {
            Estado.v2, Estado.v3, Estado.v4, //Variables
            Estado.n2, Estado.n4, Estado.n7, //Numeros
            Estado.c3, //Cadenas
            Estado.s3 //Simbolos
        };

        static readonly Dictionary<(Estado, Categoria), Estado> Transiciones = new Dictionary<(Estado, Categoria), Estado>
        {
            // ----- Estado inicial -----
            { (Estado.E1, Categoria.Letra), Estado.v2 },
            { (Estado.E1, Categoria.Numero), Estado.n2 },
            { (Estado.E1, Categoria.Comilla), Estado.c2 },
            { (Estado.E1, Categoria.Simbolo), Estado.s2 },
            { (Estado.E1, Categoria.FDC), Estado.error },
            { (Estado.E1, Categoria.Punto), Estado.error },
            { (Estado.E1, Categoria.LetraE), Estado.error },

            // ----- Identificadores (Variables) -----
            { (Estado.v2, Categoria.Letra), Estado.v2 },
            { (Estado.v2, Categoria.Numero), Estado.v3 },
            { (Estado.v2, Categoria.FDC), Estado.aceptacion },
            { (Estado.v2, Categoria.Simbolo), Estado.error },
            { (Estado.v2, Categoria.Comilla), Estado.error },
            { (Estado.v2, Categoria.Punto), Estado.error },
            { (Estado.v2, Categoria.LetraE), Estado.v2 },

            { (Estado.v3, Categoria.Numero), Estado.v3 },
            { (Estado.v3, Categoria.Letra), Estado.error },
            { (Estado.v3, Categoria.FDC), Estado.aceptacion },
            { (Estado.v3, Categoria.Simbolo), Estado.error },
            { (Estado.v3, Categoria.Comilla), Estado.error },
            { (Estado.v3, Categoria.Punto), Estado.error },
            { (Estado.v3, Categoria.LetraE), Estado.error },

            { (Estado.v4, Categoria.FDC), Estado.aceptacion},
            { (Estado.v4, Categoria.Letra), Estado.error },
            { (Estado.v4, Categoria.Numero), Estado.error },
            { (Estado.v4, Categoria.Simbolo), Estado.error },
            { (Estado.v4, Categoria.Comilla), Estado.error },
            { (Estado.v4, Categoria.Punto), Estado.error },
            { (Estado.v4, Categoria.LetraE), Estado.error },

            // ----- Números -----
            { (Estado.n2, Categoria.Letra), Estado.error },
            { (Estado.n2, Categoria.Numero), Estado.n2 },
            { (Estado.n2, Categoria.LetraE), Estado.n5 },
            { (Estado.n2, Categoria.Punto), Estado.n3 },
            { (Estado.n2, Categoria.FDC), Estado.aceptacion },
            { (Estado.n2, Categoria.Simbolo), Estado.error },
            { (Estado.n2, Categoria.Comilla), Estado.error },

            { (Estado.n3, Categoria.Letra), Estado.error },
            { (Estado.n3, Categoria.Numero), Estado.n4},
            { (Estado.n3, Categoria.Punto), Estado.error },
            { (Estado.n3, Categoria.FDC), Estado.error },
            { (Estado.n3, Categoria.Simbolo), Estado.error },
            { (Estado.n3, Categoria.Comilla), Estado.error },
            { (Estado.n3, Categoria.LetraE), Estado.error },

            { (Estado.n4, Categoria.Letra), Estado.error },
            { (Estado.n4, Categoria.Numero), Estado.n4 },
            { (Estado.n4, Categoria.Punto), Estado.error },
            { (Estado.n4, Categoria.FDC), Estado.aceptacion },
            { (Estado.n4, Categoria.Simbolo), Estado.error },
            { (Estado.n4, Categoria.Comilla), Estado.error },
            { (Estado.n4, Categoria.LetraE), Estado.n5 },

            { (Estado.n5, Categoria.Letra), Estado.error },
            { (Estado.n5, Categoria.Numero), Estado.n7 },
            { (Estado.n5, Categoria.Punto), Estado.error },
            { (Estado.n5, Categoria.FDC), Estado.error },
            { (Estado.n5, Categoria.Simbolo), Estado.n6 },
            { (Estado.n5, Categoria.Comilla), Estado.error },
            { (Estado.n5, Categoria.LetraE), Estado.error },

            { (Estado.n6, Categoria.Letra), Estado.error },
            { (Estado.n6, Categoria.Numero), Estado.n7 },
            { (Estado.n6, Categoria.Punto), Estado.error },
            { (Estado.n6, Categoria.FDC), Estado.error },
            { (Estado.n6, Categoria.Simbolo), Estado.error },
            { (Estado.n6, Categoria.Comilla), Estado.error },
            { (Estado.n6, Categoria.LetraE), Estado.error },

            { (Estado.n7, Categoria.Letra), Estado.error },
            { (Estado.n7, Categoria.Numero), Estado.n7 },
            { (Estado.n7, Categoria.Punto), Estado.error },
            { (Estado.n7, Categoria.FDC), Estado.aceptacion },
            { (Estado.n7, Categoria.Simbolo), Estado.error },
            { (Estado.n7, Categoria.Comilla), Estado.error },
            { (Estado.n7, Categoria.LetraE), Estado.error },

            // ----- Cadenas (entre comillas) -----
            { (Estado.c2, Categoria.Letra), Estado.c2 },
            { (Estado.c2, Categoria.Numero), Estado.c2 },
            { (Estado.c2, Categoria.Simbolo), Estado.c2 },
            { (Estado.c2, Categoria.Punto), Estado.c2 },
            { (Estado.c2, Categoria.FDC), Estado.error },
            { (Estado.c2, Categoria.LetraE), Estado.c2 },
            { (Estado.c2, Categoria.Comilla), Estado.c3 },
            
            { (Estado.c3, Categoria.FDC), Estado.aceptacion },
            { (Estado.c3, Categoria.Letra), Estado.error },
            { (Estado.c3, Categoria.Numero), Estado.error },
            { (Estado.c3, Categoria.Simbolo), Estado.error },
            { (Estado.c3, Categoria.Comilla), Estado.error },
            { (Estado.c3, Categoria.Punto), Estado.error },
            { (Estado.c3, Categoria.LetraE), Estado.error },

            // ----- Símbolos -----
            { (Estado.s2, Categoria.FDC), Estado.error },
            { (Estado.s2, Categoria.Letra), Estado.error },
            { (Estado.s2, Categoria.Numero), Estado.error },
            { (Estado.s2, Categoria.Simbolo), Estado.s3 },
            { (Estado.s2, Categoria.Comilla), Estado.error },
            { (Estado.s2, Categoria.Punto), Estado.error },
            { (Estado.s2, Categoria.LetraE), Estado.error },

            { (Estado.s3, Categoria.FDC), Estado.aceptacion },
            { (Estado.s3, Categoria.Letra), Estado.error },
            { (Estado.s3, Categoria.Numero), Estado.error },
            { (Estado.s3, Categoria.Simbolo), Estado.error },
            { (Estado.s3, Categoria.Comilla), Estado.error },
            { (Estado.s3, Categoria.Punto), Estado.error },
            { (Estado.s3, Categoria.LetraE), Estado.error },
        };
    }
}
