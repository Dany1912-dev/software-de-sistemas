using Compilador.AST;
using Compilador.AST.Expresiones;
using Compilador.AST.Sentencias;

namespace Compilador.Semantico
{
    public class AnalizadorSemantico
    {
        private TablaSimbolos _tabla = null!;
        private List<string> _errores = null!;

        public List<string> Analizar(NodoPrograma programa)
        {
            _tabla = new TablaSimbolos();
            _errores = new List<string>();
            Visitar(programa);
            return _errores;
        }

        private void Error(string mensaje, int linea)
        {
            _errores.Add($"{mensaje} (L{linea})");
        }

        private TipoDato Visitar(NodoAST nodo)
        {
            return nodo switch
            {
                NodoPrograma n      => VisitarPrograma(n),
                NodoDeclaracion n   => VisitarDeclaracion(n),
                NodoAsignacion n    => VisitarAsignacion(n),
                NodoLeer n          => VisitarLeer(n),
                NodoEscribir n      => VisitarEscribir(n),
                NodoSi n            => VisitarSi(n),
                NodoMientras n      => VisitarMientras(n),
                NodoBinaria n       => VisitarBinaria(n),
                NodoUnaria n        => VisitarUnaria(n),
                NodoIdentificador n => VisitarIdentificador(n),
                NodoNumero          => TipoDato.Entero,
                NodoDecimal         => TipoDato.Doble,
                NodoCadena          => TipoDato.Cadena,
                NodoBooleano        => TipoDato.Booleano,
                _                   => TipoDato.Error
            };
        }

        private TipoDato VisitarPrograma(NodoPrograma n)
        {
            foreach (var s in n.Sentencias)
                Visitar(s);
            return TipoDato.Error;
        }

        private TipoDato VisitarDeclaracion(NodoDeclaracion n)
        {
            TipoDato tipo = ConvertirTipo(n.Tipo);
            if (tipo == TipoDato.Error)
            {
                Error($"Tipo desconocido '{n.Tipo}'", n.Linea);
                return TipoDato.Error;
            }

            if (!_tabla.Declarar(n.Nombre, tipo, n.Linea, out var error))
            {
                Error(error!, n.Linea);
                return TipoDato.Error;
            }

            if (n.Valor != null)
            {
                TipoDato tipoValor = Visitar(n.Valor);
                if (tipoValor != TipoDato.Error && !TiposCompatibles(tipo, tipoValor))
                    Error($"No se puede inicializar '{n.Tipo}' con valor de tipo '{tipoValor}'", n.Linea);
            }

            return tipo;
        }

        private TipoDato VisitarAsignacion(NodoAsignacion n)
        {
            var simbolo = _tabla.Buscar(n.Nombre);
            if (simbolo == null)
            {
                Error($"Variable '{n.Nombre}' no declarada", n.Linea);
                Visitar(n.Valor);
                return TipoDato.Error;
            }

            TipoDato tipoValor = Visitar(n.Valor);
            if (tipoValor != TipoDato.Error && !TiposCompatibles(simbolo.Tipo, tipoValor))
                Error($"No se puede asignar '{tipoValor}' a '{n.Nombre}' (tipo '{simbolo.Tipo}')", n.Linea);

            return simbolo.Tipo;
        }

        private TipoDato VisitarLeer(NodoLeer n)
        {
            var simbolo = _tabla.Buscar(n.Nombre);
            if (simbolo == null)
            {
                Error($"Variable '{n.Nombre}' no declarada para leer()", n.Linea);
                return TipoDato.Error;
            }
            return simbolo.Tipo;
        }

        private TipoDato VisitarEscribir(NodoEscribir n)
        {
            foreach (var val in n.Valores)
                Visitar(val);
            return TipoDato.Cadena;
        }

        private TipoDato VisitarSi(NodoSi n)
        {
            TipoDato tipoCond = Visitar(n.Condicion);
            if (tipoCond != TipoDato.Error && tipoCond != TipoDato.Booleano)
                Error($"La condicion del 'si' debe ser booleana, no '{tipoCond}'", n.Linea);

            _tabla.AbrirAmbito();
            foreach (var s in n.Entonces)
                Visitar(s);
            _tabla.CerrarAmbito();

            if (n.Sino != null)
            {
                _tabla.AbrirAmbito();
                foreach (var s in n.Sino)
                    Visitar(s);
                _tabla.CerrarAmbito();
            }

            return TipoDato.Error;
        }

        private TipoDato VisitarMientras(NodoMientras n)
        {
            TipoDato tipoCond = Visitar(n.Condicion);
            if (tipoCond != TipoDato.Error && tipoCond != TipoDato.Booleano)
                Error($"La condicion del 'mientras' debe ser booleana, no '{tipoCond}'", n.Linea);

            _tabla.AbrirAmbito();
            foreach (var s in n.Cuerpo)
                Visitar(s);
            _tabla.CerrarAmbito();

            return TipoDato.Error;
        }

        private TipoDato VisitarBinaria(NodoBinaria n)
        {
            TipoDato izq = Visitar(n.Izquierdo);
            TipoDato der = Visitar(n.Derecho);

            if (izq == TipoDato.Error || der == TipoDato.Error)
                return TipoDato.Error;

            string op = n.Operador;

            if (op == "+" || op == "-" || op == "*" || op == "/")
            {
                if (!EsNumerico(izq))
                    Error($"Operador '{op}': tipo izquierdo '{izq}' no es numerico", n.Linea);
                if (!EsNumerico(der))
                    Error($"Operador '{op}': tipo derecho '{der}' no es numerico", n.Linea);

                return (izq == TipoDato.Doble || der == TipoDato.Doble)
                    ? TipoDato.Doble
                    : TipoDato.Entero;
            }

            if (op == "==" || op == "!=" || op == "<" || op == ">" || op == "<=" || op == ">=")
            {
                if (!TiposComparables(izq, der))
                    Error($"No se pueden comparar tipos '{izq}' y '{der}'", n.Linea);
                return TipoDato.Booleano;
            }

            Error($"Operador desconocido '{op}'", n.Linea);
            return TipoDato.Error;
        }

        private TipoDato VisitarUnaria(NodoUnaria n)
        {
            Error($"Operador unario '{n.Operador}' no soportado", n.Linea);
            Visitar(n.Operando);
            return TipoDato.Error;
        }

        private TipoDato VisitarIdentificador(NodoIdentificador n)
        {
            var simbolo = _tabla.Buscar(n.Nombre);
            if (simbolo == null)
            {
                Error($"Variable '{n.Nombre}' no declarada", n.Linea);
                return TipoDato.Error;
            }
            return simbolo.Tipo;
        }

        private static TipoDato ConvertirTipo(string tipoLexema)
        {
            return tipoLexema switch
            {
                "entero"   => TipoDato.Entero,
                "doble"    => TipoDato.Doble,
                "booleano" => TipoDato.Booleano,
                "caracter" => TipoDato.Caracter,
                _          => TipoDato.Error
            };
        }

        private static bool EsNumerico(TipoDato t) =>
            t == TipoDato.Entero || t == TipoDato.Doble;

        private static bool TiposCompatibles(TipoDato esperado, TipoDato real)
        {
            if (esperado == real) return true;
            if (esperado == TipoDato.Doble && real == TipoDato.Entero) return true;
            return false;
        }

        private static bool TiposComparables(TipoDato a, TipoDato b)
        {
            if (a == b) return true;
            if (EsNumerico(a) && EsNumerico(b)) return true;
            return false;
        }
    }
}
