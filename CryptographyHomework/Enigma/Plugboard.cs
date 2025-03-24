using System.Text;

namespace CryptographyHomework.Enigma;

public partial class EnigmaMachine
{
    private class Plugboard
    {
        private readonly Dictionary<char, char> _swaps = [];

        public void AddPlug(char from, char to)
        {
            from = char.ToUpper(from);
            to = char.ToUpper(to);

            if (from == to)
            {
                throw new ArgumentException("Cannot swap a character with itself.");
            }

            if (_swaps.ContainsKey(from) || _swaps.ContainsKey(to))
            {
                throw new ArgumentException("Plug already exists.");
            }

            _swaps[from] = to;
            _swaps[to] = from;
        }

        public void RemovePlug(char from, char to)
        {
            from = char.ToUpper(from);
            to = char.ToUpper(to);

            if (_swaps.TryGetValue(from, out var value) && value == to)
            {
                _swaps.Remove(from);
                _swaps.Remove(to);
            }
        }

        public char Swap(char input)
        {
            return _swaps.TryGetValue(input, out char value) ? value : input;
        }

        public override string ToString()
        {
            if (_swaps.Count == 0)
            {
                return "No plugs added.";
            }

            var output = new StringBuilder();
            var processed = new HashSet<char>();
            foreach (var item in _swaps)
            {
                if (processed.Add(item.Key))
                {
                    processed.Add(item.Value);
                    output.Append($"{item.Key}{item.Value} ");
                }
            }

            return output.ToString();
        }
    }
}
