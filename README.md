# Compilador — Analizador Léxico, Sintáctico y Semántico

Compilador de front-end desarrollado en **C# (.NET 9)** para un lenguaje de programación con sintaxis en español. Implementa el pipeline de análisis completo hasta la fase semántica, incluyendo la construcción y recorrido del Árbol de Sintaxis Abstracta (AST).

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-13-239120?logo=csharp&logoColor=white)
![Estado](https://img.shields.io/badge/fase_alcanzada-semántica-blue)

---

## Alcance del proyecto

Este compilador cubre las fases de **análisis** del pipeline clásico de compilación. La generación de código intermedio y las fases posteriores quedan fuera del alcance actual.

| Fase | Estado |
|------|--------|
| Análisis Léxico | ✅ Implementado |
| Análisis Sintáctico | ✅ Implementado |
| Generación del AST | ✅ Implementado |
| Análisis Semántico | ✅ Implementado |
| Código Intermedio | ❌ No implementado |
| Optimización | ❌ No implementado |
| Generación de código | ❌ No implementado |

---

## ¿Qué hace?

El compilador lee un archivo `Programa.txt` con código fuente en el lenguaje definido, lo procesa fase por fase y muestra los resultados en consola:

1. **Análisis Léxico** — Tokeniza el código fuente carácter por carácter: palabras clave, identificadores, literales, operadores y delimitadores.
2. **Análisis Sintáctico** — Valida la gramática con un parser de descenso recursivo y construye el AST al mismo tiempo.
3. **Análisis Semántico** — Recorre el AST con el patrón Visitante y aplica reglas de tipos, declaraciones y ámbitos.
4. **Recorridos del AST** — Imprime el árbol en preorden, inorden, postorden y formato visual con conectores.

---

## El lenguaje

Un lenguaje imperativo sencillo con palabras clave en español.

### Palabras reservadas

| Categoría | Palabras |
|-----------|----------|
| Tipos de dato | `entero`, `doble`, `caracter`, `boleano` |
| Control de flujo | `si`, `entonces`, `sino`, `fin`, `mientras` |
| Entrada / Salida | `leer`, `escribir` |
| Literales booleanos | `verdadero`, `falso` |

### Operadores

| Tipo | Operadores |
|------|------------|
| Aritméticos | `+`  `-`  `*`  `/` |
| Relacionales | `==`  `!=`  `<`  `>`  `<=`  `>=` |
| Asignación | `=` |
| Concatenación de cadenas | `&` |

### Sintaxis del lenguaje

```
// Declaración e inicialización
entero x = 10;
doble pi = 3.14;
boleano activo = verdadero;

// Asignación
x = x + 1;

// Condicional simple
si (x > 0) entonces
    escribir("positivo");
fin

// Condicional con sino
si (x > y) entonces
    escribir("x es mayor");
sino
    escribir("y es mayor o igual");
fin

// Ciclo mientras
mientras (x > 0)
    x = x - 1;
fin

// Entrada y salida
leer(x);
escribir("El valor es: " & x);

// Comentario de línea
// Esto es un comentario
```

---

## Arquitectura del proyecto

```
Compilador/
├── Program.cs                          # Punto de entrada — orquesta todas las fases
├── Compilador.csproj
│
├── Tokens/
│   ├── TipoToken.cs                    # Enum con todos los tipos de token posibles
│   └── Token.cs                        # Record inmutable (Lexema, Tipo, Valor, Línea, Columna)
│
├── Lexico/
│   ├── AnalizadorLexico.cs             # Orquestador: recorre el código y delega a los lectores
│   ├── LectorIdentificador.cs          # Lee identificadores y palabras reservadas
│   ├── LectorNumero.cs                 # Lee literales enteros y decimales
│   ├── LectorCadena.cs                 # Lee cadenas entre comillas dobles
│   └── LectorSimbolo.cs               # Lee operadores simples y compuestos
│
├── Sintactico/
│   ├── AnalizadorSintactico.cs         # Estado del parser y métodos auxiliares (Avanzar, Consumir...)
│   ├── Expresiones.cs                  # Factor, Término, Expresión Aritmética y Relacional
│   └── Sentencias.cs                   # Declaración, Asignación, Si, Mientras, Leer, Escribir
│
├── Semantico/
│   ├── AnalizadorSemantico.cs          # Visitante que recorre el AST y aplica reglas semánticas
│   ├── TablaSimbolos.cs                # Tabla de símbolos con ámbitos anidados (pila de scopes)
│   ├── Simbolo.cs                      # Representa una variable: nombre, tipo, línea de declaración
│   └── TipoDato.cs                     # Enum con los tipos del lenguaje (Entero, Doble, etc.)
│
├── AST/
│   ├── NodoAST.cs                      # Clase base abstracta para todos los nodos
│   ├── Expresiones/
│   │   ├── NodoNumero.cs               # Literal entero (5, 10)
│   │   ├── NodoDecimal.cs              # Literal decimal (3.14)
│   │   ├── NodoCadena.cs               # Literal cadena ("hola")
│   │   ├── NodoBooleano.cs             # verdadero / falso
│   │   ├── NodoIdentificador.cs        # Variable (x, suma)
│   │   ├── NodoBinaria.cs              # Operación binaria (x + y, a > b)
│   │   └── NodoUnaria.cs               # Operación unaria (-x)
│   └── Sentencias/
│       ├── NodoPrograma.cs             # Nodo raíz — lista de sentencias del programa
│       ├── NodoDeclaracion.cs          # entero x = 5
│       ├── NodoAsignacion.cs           # x = 10
│       ├── NodoSi.cs                   # si/entonces/fin  y  si/entonces/sino/fin
│       ├── NodoMientras.cs             # mientras (cond) ... fin
│       ├── NodoLeer.cs                 # leer(x)
│       └── NodoEscribir.cs             # escribir("hola" & x)
│
├── Utilidades/
│   ├── PalabrasReservadas.cs           # Diccionario de palabras reservadas y validación
│   ├── Impresora.cs                    # Impresión de tokens y resumen en consola
│   └── ImpresoraAST.cs                 # Recorridos del AST (preorden, inorden, postorden, visual)
│
└── Datos/
    └── Programa.txt                    # Archivo de entrada con el código a analizar
```

---

## Cómo funciona cada fase

### Fase 1 — Análisis Léxico

El `AnalizadorLexico` recorre el código fuente carácter por carácter y produce una secuencia de `Token`. Cada token registra su tipo, lexema original, valor procesado, línea y columna.

- Salta espacios en blanco y comentarios `//`
- Delega la lectura a un lector especializado según el primer carácter:
  - Letra → `LectorIdentificador` (reconoce palabras reservadas automáticamente)
  - Dígito → `LectorNumero` (soporta enteros y decimales)
  - `"` → `LectorCadena`
  - Símbolo → `LectorSimbolo` (operadores simples y compuestos como `<=`, `==`)
- Los caracteres no reconocidos producen un `TOKEN_ERROR` con línea y columna exactas

### Fase 2 — Análisis Sintáctico

Parser de **descenso recursivo** que valida la gramática y construye el AST simultáneamente.

Jerarquía de precedencia (de menor a mayor):

```
Expresión Relacional  →  ExprAritmética  (== | != | < | > | <= | >=)  ExprAritmética
Expresión Aritmética  →  Término  (+ | -)  Término
Término               →  Factor  (* | /)  Factor
Factor                →  número | decimal | cadena | booleano | identificador | ( Expresión )
```

- Solo se ejecuta si no hubo errores léxicos
- Ante el primer error sintáctico reporta la línea y se detiene

### Fase 3 — Análisis Semántico

El `AnalizadorSemantico` implementa el patrón **Visitante** sobre el AST. Recorre el árbol completo y acumula todos los errores antes de reportarlos (no se detiene al primero).

Reglas que verifica:

- **Declaración antes de uso** — cualquier identificador en una expresión, asignación o `leer()` debe existir en la tabla de símbolos.
- **Sin redeclaración en el mismo ámbito** — dos variables con el mismo nombre en el mismo bloque son error. El *shadowing* en ámbitos anidados está permitido.
- **Compatibilidad de tipos** — en inicializaciones y asignaciones el tipo de la expresión debe coincidir con el de la variable. Se permite la promoción implícita `entero → doble`.
- **Operadores aritméticos** — solo sobre `entero` o `doble`.
- **Operadores relacionales** — solo entre tipos comparables.
- **Condiciones booleanas** — la condición de `si` y `mientras` debe resolverse como `boleano`.
- **Ámbitos anidados** — cada bloque `si` / `sino` / `mientras` abre un nuevo scope. Las variables declaradas dentro no son visibles fuera de ese bloque.

### Fase 4 — Árbol de Sintaxis Abstracta (AST)

El AST se construye durante el análisis sintáctico. La `ImpresoraAST` lo recorre en cuatro formatos:

| Recorrido | Descripción |
|-----------|-------------|
| **Preorden** | Nodo raíz → subárbol izquierdo → subárbol derecho |
| **Inorden** | Subárbol izquierdo → nodo raíz → subárbol derecho |
| **Postorden** | Subárbol izquierdo → subárbol derecho → nodo raíz |
| **Visual** | Árbol con indentación y conectores `├──` / `└──` |

---

## Ejemplo

Dado este `Programa.txt`:

```
entero x = 10;
entero y = 5;
doble resultado = 3.14;
si (x > y) entonces
    escribir("x es mayor");
sino
    escribir("y es mayor o igual");
fin
mientras (x > 0)
    x = x - 1;
fin
```

Fragmento de la salida en consola:

```
====== ANALIZADOR LEXICO ======
Tokens encontrados: 40

====== ANALISIS SEMANTICO ======
  Analisis semantico correcto

====== AST - ARBOL VISUAL ======
Programa
├── Declarar entero x
│   └── 10
├── Declarar entero y
│   └── 5
├── Declarar doble resultado
│   └── 3.14
├── Si
│   ├── Condicion
│   │   └── >
│   │       ├── x
│   │       └── y
│   ├── Entonces
│   │   └── Escribir
│   │       └── "x es mayor"
│   └── Sino
│       └── Escribir
│           └── "y es mayor o igual"
└── Mientras
    ├── Condicion
    │   └── >
    │       ├── x
    │       └── 0
    └── Cuerpo
        └── Asignar x
            └── -
                ├── x
                └── 1
```

---

## Requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## Cómo ejecutar

1. Escribe el programa a analizar en `Compilador/Compilador/Datos/Programa.txt`
2. Desde la raíz del repositorio:

```bash
dotnet run --project Compilador/Compilador
```

O abre `Compilador/Compilador.slnx` en Visual Studio / Rider y ejecuta con **F5**.

---

## Tecnologías y decisiones de diseño

| Aspecto | Decisión |
|---------|----------|
| Lenguaje | C# 13 con .NET 9 |
| Estrategia de parsing | Descenso recursivo manual |
| Recorrido del AST | Patrón Visitante |
| Tabla de símbolos | Pila de scopes para ámbitos anidados |
| Reporte de errores | Semántico: acumula todos los errores; Sintáctico: falla rápido |
| Tokens | Records inmutables con información de posición |
