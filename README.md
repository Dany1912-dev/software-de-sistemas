# Compilador - Analizador Léxico, Sintáctico, Semántico y AST

Compilador desarrollado en C# (.NET 9) que analiza código fuente escrito en un lenguaje de programación en español. Incluye análisis léxico, análisis sintáctico, análisis semántico y generación de un Árbol de Sintaxis Abstracta (AST).

## Qué hace

El programa lee un archivo `Programa.txt`, lo procesa en cuatro fases y muestra los resultados en consola:

1. **Análisis Léxico** — Convierte el código fuente en una lista de tokens (palabras clave, identificadores, operadores, literales, etc.)
2. **Análisis Sintáctico** — Valida que la secuencia de tokens cumpla con las reglas gramaticales del lenguaje
3. **Análisis Semántico** — Verifica reglas de tipos, declaraciones y ámbitos recorriendo el AST
4. **Generación del AST** — Construye un árbol que representa la estructura del programa y lo imprime en preorden, inorden, postorden y formato visual

## Estructura del proyecto

```
Compilador/
├── Program.cs                          # Punto de entrada - orquesta las 4 fases
├── Compilador.csproj
│
├── Tokens/
│   ├── TipoToken.cs                    # Enum con todos los tipos de token
│   └── Token.cs                        # Record inmutable (Lexema, Tipo, Valor, Linea, Columna)
│
├── Lexico/
│   ├── AnalizadorLexico.cs             # Orquestador: recorre el código y delega a los lectores
│   ├── LectorIdentificador.cs          # Lee identificadores y palabras reservadas
│   ├── LectorNumero.cs                 # Lee literales enteros y decimales
│   ├── LectorCadena.cs                 # Lee cadenas entre comillas dobles
│   └── LectorSimbolo.cs               # Lee operadores y delimitadores
│
├── Sintactico/
│   ├── AnalizadorSintactico.cs         # Estado del parser y helpers (Avanzar, Consumir, etc.)
│   ├── Expresiones.cs                  # Factor, Término, Expresión Aritmética y Relacional
│   └── Sentencias.cs                   # Declaración, Asignación, Si, Mientras, Leer, Escribir
│
├── Semantico/
│   ├── AnalizadorSemantico.cs          # Visitante que recorre el AST y aplica reglas semánticas
│   ├── TablaSimbolos.cs                # Tabla de símbolos con ámbitos anidados (pila de scopes)
│   ├── Simbolo.cs                      # Representa una variable: nombre, tipo, línea
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
│   │   ├── NodoBinaria.cs              # Operación con dos operandos (x + y, a > b)
│   │   └── NodoUnaria.cs               # Operación con un operando
│   └── Sentencias/
│       ├── NodoPrograma.cs             # Nodo raíz con la lista de sentencias
│       ├── NodoDeclaracion.cs          # entero x = 5
│       ├── NodoAsignacion.cs           # x = 10
│       ├── NodoSi.cs                   # si/entonces/fin y si/entonces/sino/fin
│       ├── NodoMientras.cs             # mientras (cond) ... fin
│       ├── NodoLeer.cs                 # leer(x)
│       └── NodoEscribir.cs             # escribir("hola" & x)
│
├── Utilidades/
│   ├── PalabrasReservadas.cs           # Diccionario de palabras reservadas y validación
│   ├── Impresora.cs                    # Impresión de tokens y resumen
│   └── ImpresoraAST.cs                 # Impresión del AST (preorden, inorden, postorden, visual)
│
└── Datos/
    └── Programa.txt                    # Archivo de entrada con el código a analizar
```

## El lenguaje soportado

### Palabras reservadas

`si`, `entonces`, `sino`, `fin`, `mientras`, `entero`, `caracter`, `boleano`, `doble`, `verdadero`, `falso`, `leer`, `escribir`

### Tipos de dato

- `entero` — números enteros
- `doble` — números decimales
- `caracter` — caracteres
- `boleano` — verdadero o falso

### Operadores

- Aritméticos: `+`, `-`, `*`, `/`
- Relacionales: `==`, `!=`, `<`, `>`, `<=`, `>=`
- Asignación: `=`
- Concatenación: `&`

### Estructuras de control

```
// Declaración con inicialización opcional
entero x = 10;

// Asignación
x = 5;

// Condicional simple
si (x > 0) entonces
    escribir("positivo");
fin

// Condicional con sino
si (x > y) entonces
    escribir("x es mayor");
sino
    escribir("y es mayor");
fin

// Ciclo
mientras (x > 0)
    x = x - 1;
fin

// Entrada/Salida
leer(x);
escribir("El valor es: " & x);
```

### Comentarios

```
// Esto es un comentario de línea
```

## Cómo funciona cada fase

### Fase 1: Análisis Léxico

El analizador léxico recorre el código fuente carácter por carácter y lo convierte en tokens. Cada token tiene un tipo, un lexema (texto original), la línea y columna donde aparece.

- Salta espacios en blanco y comentarios `//`
- Delega a lectores especializados según el carácter: letras → `LectorIdentificador`, dígitos → `LectorNumero`, comilla → `LectorCadena`, símbolos → `LectorSimbolo`
- Los identificadores se comparan contra el diccionario de palabras reservadas
- Los caracteres no reconocidos se marcan como `TOKEN_ERROR`

### Fase 2: Análisis Sintáctico

Usa un parser de **descenso recursivo** que recorre la lista de tokens y valida la gramática.

- Expresiones con precedencia correcta: Factor → Término → Expresión Aritmética → Expresión Relacional
- Se detiene al primer error y reporta la línea donde ocurre
- Solo se ejecuta si no hay errores léxicos

### Fase 3: Análisis Semántico

El analizador semántico recorre el AST con un patrón **visitante** y aplica reglas de tipo, declaración y ámbito. Reporta todos los errores encontrados de una sola pasada (no se detiene al primero).

- **Tabla de símbolos con ámbitos anidados**: cada bloque `si`/`sino`/`mientras` abre un nuevo ámbito. Las variables declaradas dentro solo existen en ese bloque.
- **Variables declaradas antes de uso**: cualquier identificador en una expresión, asignación o `leer()` debe estar declarado.
- **Detección de duplicados**: declarar dos variables con el mismo nombre en el mismo ámbito es error. Se permite *shadowing* en ámbitos anidados.
- **Verificación de tipos en operadores**: `+`, `-`, `*`, `/` solo sobre `entero`/`doble`; `==`, `!=`, `<`, `>`, `<=`, `>=` solo entre tipos comparables.
- **Compatibilidad de tipos**: en inicializaciones y asignaciones se verifica que el tipo de la expresión coincida con el de la variable. Se permite promoción `entero → doble`.
- **Condiciones booleanas**: la condición de `si` y `mientras` debe ser de tipo `booleano`.

### Fase 4: Árbol de Sintaxis Abstracta (AST)

El parser construye un árbol donde cada nodo representa una construcción del lenguaje. El árbol se imprime en 4 formatos:

- **Preorden** — Operador primero, luego hijos
- **Inorden** — Hijo izquierdo, operador, hijo derecho
- **Postorden** — Hijos primero, operador al final
- **Visual** — Formato de árbol con indentación y conectores

## Ejemplo

Dado este `Programa.txt`:

```
entero x = 10;
entero y = 5;
si (x > y) entonces
    escribir("x es mayor");
sino
    escribir("y es mayor");
fin
```

La salida del árbol visual sería:

```
Programa
├── Declarar entero x
│   └── 10
├── Declarar entero y
│   └── 5
└── Si
    ├── Condicion
    │   └── >
    │       ├── x
    │       └── y
    ├── Entonces
    │   └── Escribir
    │       └── "x es mayor"
    └── Sino
        └── Escribir
            └── "y es mayor"
```

## Requisitos

- .NET 9.0 SDK

## Ejecución

1. Coloca tu código fuente en `Datos/Programa.txt`
2. Ejecuta el proyecto:

```bash
dotnet run
```