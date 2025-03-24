namespace CryptographyHomework.Enigma;

public partial class EnigmaMachine
{
    private class Rotor
    {
        private static readonly (string Wiring, char Notch) RotorI = ("EKMFLGDQVZNTOWYHXUSPAIBRCJ", 'Q');
        private static readonly (string Wiring, char Notch) RotorII = ("AJDKSIRUXBLHWTMCQGZNPYFVOE", 'E');
        private static readonly (string Wiring, char Notch) RotorIII = ("BDFHJLCPRTXVZNYEIWGAKMUSQO", 'V');
        private static readonly (string Wiring, char Notch) RotorIV = ("ESOVPZJAYQUIRHXLNFTGKDCMWB", 'J');
        private static readonly (string Wiring, char Notch) RotorV = ("VZBRGITYUPSDNHLXAWMJQOFECK", 'Z');

        private readonly string _alphabet;
        private readonly string _wiring;
        private readonly int _notchIndex;
        private readonly int _ringOffset;
        private readonly int _startIndex;

        private int _currentIndex;
        private int[] _map;
        private int[] _reverseMap;

        private Rotor(string alphabet, string wiring, char notchCharacter, char ringCharacter, char startCharacter)
        {
            ArgumentException.ThrowIfNullOrEmpty(alphabet, nameof(alphabet));
            ArgumentException.ThrowIfNullOrEmpty(wiring, nameof(wiring));

            alphabet = alphabet.ToUpper();
            wiring = wiring.ToUpper();
            notchCharacter = char.ToUpper(notchCharacter);
            ringCharacter = char.ToUpper(ringCharacter);
            startCharacter = char.ToUpper(startCharacter);

            if (alphabet.Length != wiring.Length)
            {
                throw new ArgumentException("Alphabet and wiring must have the same length.");
            }

            foreach (var character in wiring)
            {
                if (!alphabet.Contains(character))
                {
                    throw new ArgumentException("Wiring must contain characters from the alphabet.");
                }
            }

            if (!alphabet.Contains(notchCharacter))
            {
                throw new ArgumentException("Notch must be part of the alphabet.");
            }

            if (!alphabet.Contains(ringCharacter))
            {
                throw new ArgumentException("Ring setting must be part of the alphabet.");
            }

            if (!alphabet.Contains(startCharacter))
            {
                throw new ArgumentException("Start position must be part of the alphabet.");
            }

            _alphabet = alphabet;
            _wiring = wiring;
            _notchIndex = _alphabet.IndexOf(notchCharacter);
            _ringOffset = _alphabet.IndexOf(ringCharacter);
            _startIndex = _alphabet.IndexOf(startCharacter);
            _currentIndex = _startIndex;

            _map = new int[_alphabet.Length];
            _reverseMap = new int[_alphabet.Length];
            for (int characterIndex = 0; characterIndex < _alphabet.Length; characterIndex++)
            {
                int match = _alphabet.IndexOf(_wiring[characterIndex]);
                _map[characterIndex] = (_alphabet.Length + match - characterIndex) % _alphabet.Length;
                _reverseMap[match] = (_alphabet.Length - match + characterIndex) % _alphabet.Length;
            }
        }

        public string Alphabet => _alphabet;
        public char CurrentTop => _alphabet[_currentIndex];

        public char Forward(char character)
        {
            return GetMappedCharacter(character, _map);
        }

        public char Backward(char character)
        {
            return GetMappedCharacter(character, _reverseMap);
        }

        public void Rotate()
        {
            _currentIndex = (_currentIndex + 1) % _alphabet.Length;
        }

        public bool IsNotched => _currentIndex == _notchIndex;

        public void Reset()
        {
            _currentIndex = _startIndex;
        }

        public override string ToString()
        {
            return $"Wiring: {_wiring}, Notch: {_alphabet[_notchIndex]}, Ring: {_alphabet[_ringOffset]}, Start: {_alphabet[_startIndex]}";
        }

        public static Rotor Create(
            string alphabet,
            string selectedRotor,
            char selectedRing,
            char selectedStart,
            string? alphabetExtensions = null
        )
        {
            return selectedRotor switch
            {
                "I" => new Rotor(
                    alphabet + alphabetExtensions,
                    RotorI.Wiring + alphabetExtensions,
                    RotorI.Notch,
                    selectedRing,
                    selectedStart
                ),
                "II" => new Rotor(
                    alphabet + alphabetExtensions,
                    RotorII.Wiring + alphabetExtensions,
                    RotorII.Notch,
                    selectedRing,
                    selectedStart
                ),
                "III" => new Rotor(
                    alphabet + alphabetExtensions,
                    RotorIII.Wiring + alphabetExtensions,
                    RotorIII.Notch,
                    selectedRing,
                    selectedStart
                ),
                "IV" => new Rotor(
                    alphabet + alphabetExtensions,
                    RotorIV.Wiring + alphabetExtensions,
                    RotorIV.Notch,
                    selectedRing,
                    selectedStart
                ),
                "V" => new Rotor(
                    alphabet + alphabetExtensions,
                    RotorV.Wiring + alphabetExtensions,
                    RotorV.Notch,
                    selectedRing,
                    selectedStart
                ),
                _ => throw new ArgumentException("Invalid rotor.", nameof(selectedRotor))
            };
        }

        private char GetMappedCharacter(char character, int[] map)
        {
            var characterIndex = _alphabet.IndexOf(character);
            var index = (characterIndex + _currentIndex - _ringOffset + _alphabet.Length) % _alphabet.Length;
            return _alphabet[(characterIndex + map[index]) % _alphabet.Length];
        }
    }
}
