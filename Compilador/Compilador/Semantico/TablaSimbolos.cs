namespace Compilador.Semantico
{
    public class TablaSimbolos
    {
        private readonly List<Dictionary<string, Simbolo>> _ambitos;

        public TablaSimbolos()
        {
            _ambitos = new List<Dictionary<string, Simbolo>>();
            AbrirAmbito();
        }

        public void AbrirAmbito()
        {
            _ambitos.Add(new Dictionary<string, Simbolo>());
        }

        public void CerrarAmbito()
        {
            if (_ambitos.Count > 1)
                _ambitos.RemoveAt(_ambitos.Count - 1);
        }

        public bool Declarar(string nombre, TipoDato tipo, int linea, out string? error)
        {
            var actual = _ambitos[^1];
            if (actual.ContainsKey(nombre))
            {
                var existente = actual[nombre];
                error = $"Variable '{nombre}' ya declarada en este ambito (declaracion previa en L{existente.Linea})";
                return false;
            }
            actual[nombre] = new Simbolo(nombre, tipo, linea);
            error = null;
            return true;
        }

        public Simbolo? Buscar(string nombre)
        {
            for (int i = _ambitos.Count - 1; i >= 0; i--)
            {
                if (_ambitos[i].TryGetValue(nombre, out var simbolo))
                    return simbolo;
            }
            return null;
        }
    }
}
