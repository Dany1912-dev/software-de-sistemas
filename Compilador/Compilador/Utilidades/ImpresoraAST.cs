using Compilador.AST;
using Compilador.AST.Expresiones;
using Compilador.AST.Sentencias;

namespace Compilador.Utilidades
{

    public static class ImpresoraAST
    {
        public static void ImprimirPreorden(NodoAST? nodo)
        {
            if (nodo == null) return;

            switch (nodo)
            {
                case NodoNumero n:
                    Console.Write($"{n.Valor} ");
                    break;

                case NodoDecimal n:
                    Console.Write($"{n.Valor} ");
                    break;

                case NodoCadena n:
                    Console.Write($"\"{n.Valor}\" ");
                    break;

                case NodoBooleano n:
                    Console.Write($"{(n.Valor ? "verdadero" : "falso")} ");
                    break;

                case NodoIdentificador n:
                    Console.Write($"{n.Nombre} ");
                    break;

                case NodoBinaria n:
                    Console.Write($"({n.Operador} ");
                    ImprimirPreorden(n.Izquierdo);
                    ImprimirPreorden(n.Derecho);
                    Console.Write(") ");
                    break;

                case NodoUnaria n:
                    Console.Write($"({n.Operador} ");
                    ImprimirPreorden(n.Operando);
                    Console.Write(") ");
                    break;

                case NodoDeclaracion n:
                    Console.Write($"(Declarar {n.Tipo} {n.Nombre} ");
                    if (n.Valor != null)
                    {
                        Console.Write("= ");
                        ImprimirPreorden(n.Valor);
                    }
                    Console.Write(") ");
                    break;

                case NodoAsignacion n:
                    Console.Write($"(Asignar {n.Nombre} = ");
                    ImprimirPreorden(n.Valor);
                    Console.Write(") ");
                    break;

                case NodoLeer n:
                    Console.Write($"Leer({n.Nombre}) ");
                    break;

                case NodoEscribir n:
                    Console.Write("Escribir(");
                    foreach (var val in n.Valores)
                        ImprimirPreorden(val);
                    Console.Write(") ");
                    break;

                case NodoSi n:
                    Console.Write("Si(");
                    ImprimirPreorden(n.Condicion);
                    Console.Write(") Entonces ");
                    foreach (var s in n.Entonces)
                        ImprimirPreorden(s);
                    if (n.Sino != null)
                    {
                        Console.Write("Sino ");
                        foreach (var s in n.Sino)
                            ImprimirPreorden(s);
                    }
                    Console.Write("FinSi ");
                    break;

                case NodoMientras n:
                    Console.Write("Mientras(");
                    ImprimirPreorden(n.Condicion);
                    Console.Write(") ");
                    foreach (var s in n.Cuerpo)
                        ImprimirPreorden(s);
                    Console.Write("FinMientras ");
                    break;

                case NodoPrograma n:
                    foreach (var s in n.Sentencias)
                    {
                        ImprimirPreorden(s);
                        Console.WriteLine();
                    }
                    break;
            }
        }

        public static void ImprimirInorden(NodoAST? nodo)
        {
            if (nodo == null) return;

            switch (nodo)
            {
                case NodoNumero n:
                    Console.Write($"{n.Valor} ");
                    break;

                case NodoDecimal n:
                    Console.Write($"{n.Valor} ");
                    break;

                case NodoCadena n:
                    Console.Write($"\"{n.Valor}\" ");
                    break;

                case NodoBooleano n:
                    Console.Write($"{(n.Valor ? "verdadero" : "falso")} ");
                    break;

                case NodoIdentificador n:
                    Console.Write($"{n.Nombre} ");
                    break;

                case NodoBinaria n:
                    Console.Write("(");
                    ImprimirInorden(n.Izquierdo);
                    Console.Write($"{n.Operador} ");
                    ImprimirInorden(n.Derecho);
                    Console.Write(") ");
                    break;

                case NodoUnaria n:
                    Console.Write($"({n.Operador} ");
                    ImprimirInorden(n.Operando);
                    Console.Write(") ");
                    break;

                case NodoDeclaracion n:
                    Console.Write($"({n.Tipo} {n.Nombre} ");
                    if (n.Valor != null)
                    {
                        Console.Write("= ");
                        ImprimirInorden(n.Valor);
                    }
                    Console.Write(") ");
                    break;

                case NodoAsignacion n:
                    Console.Write($"({n.Nombre} = ");
                    ImprimirInorden(n.Valor);
                    Console.Write(") ");
                    break;

                case NodoLeer n:
                    Console.Write($"Leer({n.Nombre}) ");
                    break;

                case NodoEscribir n:
                    Console.Write("Escribir(");
                    foreach (var val in n.Valores)
                        ImprimirInorden(val);
                    Console.Write(") ");
                    break;

                case NodoSi n:
                    Console.Write("Si(");
                    ImprimirInorden(n.Condicion);
                    Console.Write(") Entonces ");
                    foreach (var s in n.Entonces)
                        ImprimirInorden(s);
                    if (n.Sino != null)
                    {
                        Console.Write("Sino ");
                        foreach (var s in n.Sino)
                            ImprimirInorden(s);
                    }
                    Console.Write("FinSi ");
                    break;

                case NodoMientras n:
                    Console.Write("Mientras(");
                    ImprimirInorden(n.Condicion);
                    Console.Write(") ");
                    foreach (var s in n.Cuerpo)
                        ImprimirInorden(s);
                    Console.Write("FinMientras ");
                    break;

                case NodoPrograma n:
                    foreach (var s in n.Sentencias)
                    {
                        ImprimirInorden(s);
                        Console.WriteLine();
                    }
                    break;
            }
        }

        public static void ImprimirPostorden(NodoAST? nodo)
        {
            if (nodo == null) return;

            switch (nodo)
            {
                case NodoNumero n:
                    Console.Write($"{n.Valor} ");
                    break;

                case NodoDecimal n:
                    Console.Write($"{n.Valor} ");
                    break;

                case NodoCadena n:
                    Console.Write($"\"{n.Valor}\" ");
                    break;

                case NodoBooleano n:
                    Console.Write($"{(n.Valor ? "verdadero" : "falso")} ");
                    break;

                case NodoIdentificador n:
                    Console.Write($"{n.Nombre} ");
                    break;

                case NodoBinaria n:
                    ImprimirPostorden(n.Izquierdo);
                    ImprimirPostorden(n.Derecho);
                    Console.Write($"{n.Operador} ");
                    break;

                case NodoUnaria n:
                    ImprimirPostorden(n.Operando);
                    Console.Write($"{n.Operador} ");
                    break;

                case NodoDeclaracion n:
                    if (n.Valor != null)
                        ImprimirPostorden(n.Valor);
                    Console.Write($"{n.Tipo} {n.Nombre} Declarar ");
                    break;

                case NodoAsignacion n:
                    ImprimirPostorden(n.Valor);
                    Console.Write($"{n.Nombre} Asignar ");
                    break;

                case NodoLeer n:
                    Console.Write($"{n.Nombre} Leer ");
                    break;

                case NodoEscribir n:
                    foreach (var val in n.Valores)
                        ImprimirPostorden(val);
                    Console.Write("Escribir ");
                    break;

                case NodoSi n:
                    ImprimirPostorden(n.Condicion);
                    foreach (var s in n.Entonces)
                        ImprimirPostorden(s);
                    if (n.Sino != null)
                    {
                        foreach (var s in n.Sino)
                            ImprimirPostorden(s);
                    }
                    Console.Write("Si ");
                    break;

                case NodoMientras n:
                    ImprimirPostorden(n.Condicion);
                    foreach (var s in n.Cuerpo)
                        ImprimirPostorden(s);
                    Console.Write("Mientras ");
                    break;

                case NodoPrograma n:
                    foreach (var s in n.Sentencias)
                    {
                        ImprimirPostorden(s);
                        Console.WriteLine();
                    }
                    break;
            }
        }

        public static void ImprimirArbol(NodoAST? nodo, string indent = "", bool esUltimo = true)
        {
            if (nodo == null) return;

            string conector = esUltimo ? "└── " : "├── ";
            string nuevaIndent = indent + (esUltimo ? "    " : "│   ");

            switch (nodo)
            {
                case NodoNumero n:
                    Console.WriteLine($"{indent}{conector}{n.Valor}");
                    break;

                case NodoDecimal n:
                    Console.WriteLine($"{indent}{conector}{n.Valor}");
                    break;

                case NodoCadena n:
                    Console.WriteLine($"{indent}{conector}\"{n.Valor}\"");
                    break;

                case NodoBooleano n:
                    Console.WriteLine($"{indent}{conector}{(n.Valor ? "verdadero" : "falso")}");
                    break;

                case NodoIdentificador n:
                    Console.WriteLine($"{indent}{conector}{n.Nombre}");
                    break;

                case NodoBinaria n:
                    Console.WriteLine($"{indent}{conector}{n.Operador}");
                    ImprimirArbol(n.Izquierdo, nuevaIndent, false);
                    ImprimirArbol(n.Derecho, nuevaIndent, true);
                    break;

                case NodoUnaria n:
                    Console.WriteLine($"{indent}{conector}{n.Operador}");
                    ImprimirArbol(n.Operando, nuevaIndent, true);
                    break;

                case NodoDeclaracion n:
                    Console.WriteLine($"{indent}{conector}Declarar {n.Tipo} {n.Nombre}");
                    if (n.Valor != null)
                        ImprimirArbol(n.Valor, nuevaIndent, true);
                    break;

                case NodoAsignacion n:
                    Console.WriteLine($"{indent}{conector}Asignar {n.Nombre}");
                    ImprimirArbol(n.Valor, nuevaIndent, true);
                    break;

                case NodoLeer n:
                    Console.WriteLine($"{indent}{conector}Leer({n.Nombre})");
                    break;

                case NodoEscribir n:
                    Console.WriteLine($"{indent}{conector}Escribir");
                    for (int i = 0; i < n.Valores.Count; i++)
                        ImprimirArbol(n.Valores[i], nuevaIndent, i == n.Valores.Count - 1);
                    break;

                case NodoSi n:
                    Console.WriteLine($"{indent}{conector}Si");

                    Console.WriteLine($"{nuevaIndent}├── Condicion");
                    ImprimirArbol(n.Condicion, nuevaIndent + "│   ", true);

                    if (n.Sino != null)
                    {
                        Console.WriteLine($"{nuevaIndent}├── Entonces");
                        for (int i = 0; i < n.Entonces.Count; i++)
                            ImprimirArbol(n.Entonces[i], nuevaIndent + "│   ", i == n.Entonces.Count - 1);

                        Console.WriteLine($"{nuevaIndent}└── Sino");
                        for (int i = 0; i < n.Sino.Count; i++)
                            ImprimirArbol(n.Sino[i], nuevaIndent + "    ", i == n.Sino.Count - 1);
                    }
                    else
                    {
                        Console.WriteLine($"{nuevaIndent}└── Entonces");
                        for (int i = 0; i < n.Entonces.Count; i++)
                            ImprimirArbol(n.Entonces[i], nuevaIndent + "    ", i == n.Entonces.Count - 1);
                    }
                    break;

                case NodoMientras n:
                    Console.WriteLine($"{indent}{conector}Mientras");

                    Console.WriteLine($"{nuevaIndent}├── Condicion");
                    ImprimirArbol(n.Condicion, nuevaIndent + "│   ", true);

                    Console.WriteLine($"{nuevaIndent}└── Cuerpo");
                    for (int i = 0; i < n.Cuerpo.Count; i++)
                        ImprimirArbol(n.Cuerpo[i], nuevaIndent + "    ", i == n.Cuerpo.Count - 1);
                    break;

                case NodoPrograma n:
                    Console.WriteLine($"{indent}Programa");
                    for (int i = 0; i < n.Sentencias.Count; i++)
                        ImprimirArbol(n.Sentencias[i], indent, i == n.Sentencias.Count - 1);
                    break;
            }
        }
    }
}