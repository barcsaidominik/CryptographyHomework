using System.Text;

namespace CryptographyHomework.Enigma;

public class EnigmaMachine
{
    public static readonly (string Wiring, char Notch) RotorI = ("ekmflgdqvzntowyhxuspaibrcj", 'q');
    public static readonly (string Wiring, char Notch) RotorII = ("ajdksiruxblhwtmcqgznpyfvoe", 'e');
    public static readonly (string Wiring, char Notch) RotorIII = ("bdfhjlcprtxvznyeiwgakmusqo", 'v');
    public static readonly (string Wiring, char Notch) RotorIV = ("esovpzjayquirhxlnftgkdcmwb", 'j');
    public static readonly (string Wiring, char Notch) RotorV = ("vzbrgityupsdnhlxawmjqofeck", 'z');

    public static readonly (string Wiring, char Notch) RotorI_Upper = ("EKMFLGDQVZNTOWYHXUSPAIBRCJ", 'Q');
    public static readonly (string Wiring, char Notch) RotorII_Upper = ("AJDKSIRUXBLHWTMCQGZNPYFVOE", 'E');
    public static readonly (string Wiring, char Notch) RotorIII_Upper = ("BDFHJLCPRTXVZNYEIWGAKMUSQO", 'V');
    public static readonly (string Wiring, char Notch) RotorIV_Upper = ("ESOVPZJAYQUIRHXLNFTGKDCMWB", 'J');
    public static readonly (string Wiring, char Notch) RotorV_Upper = ("VZBRGITYUPSDNHLXAWMJQOFECK", 'Z');

    public static readonly string ReflectorAWiring = "ejmzalyxvbwfcrquontspikhgd";
    public static readonly string ReflectorBWiring = "yruhqsldpxngokmiebfzcwvjat";
    public static readonly string ReflectorCWiring = "fvpjiaoyedrzxwgctkuqsbnmhl";

    public static readonly string ReflectorAWiring_Upper = "EJMZALYXVBWFCRQUONTSPIKHGD";
    public static readonly string ReflectorBWiring_Upper = "YRUHQSLDPXNGOKMIEBFZCWVJAT";
    public static readonly string ReflectorCWiring_Upper = "FVPJIAOYEDRZXWGCTKUQSBNMHL";

    private readonly string _alphabet;
    private readonly List<Rotor> _rotors;
    private readonly Reflector _reflector;
    private readonly Plugboard _plugboard;
    private readonly bool _isLower;

    public EnigmaMachine(string alphabet, List<Rotor> rotors, Reflector reflector, Plugboard? plugboard = null, bool isLower = true)
    {
        ArgumentException.ThrowIfNullOrEmpty(alphabet, nameof(alphabet));
        ArgumentNullException.ThrowIfNull(rotors, nameof(rotors));
        if (rotors.Count != 3)
        {
            throw new ArgumentException("Three rotors are required.", nameof(rotors));
        }

        ArgumentNullException.ThrowIfNull(reflector, nameof(reflector));
        if (rotors.Any(x => x.Alphabet != alphabet))
        {
            throw new ArgumentException("All rotors must have the same alphabet as the machine.", nameof(rotors));
        }

        if (reflector.Alphabet != alphabet)
        {
            throw new ArgumentException("Reflector must have the same alphabet as the machine.", nameof(reflector));
        }

        _alphabet = alphabet;
        _rotors = rotors;
        _reflector = reflector;
        _plugboard = plugboard ?? new();
        _isLower = isLower;
    }

    public Plugboard Plugboard => _plugboard;

    public string Run(string input)
    {
        Reset();
        var output = new StringBuilder();
        foreach (var character in input)
        {
            var correctedCaseCharacter = _isLower ? char.ToLower(character) : char.ToUpper(character);
            if (!_alphabet.Contains(correctedCaseCharacter))
            {
                output.Append(character);
                continue;
            }

            var encodedChar = EncodeCharacter(correctedCaseCharacter);
            output.Append(encodedChar);
        }

        return output.ToString();
    }

    public void Reset()
    {
        foreach (var rotor in _rotors)
        {
            rotor.Reset();
        }
    }

    public override string ToString()
    {
        var output = new StringBuilder();
        output.AppendLine("Enigma Machine:");

        output.AppendLine("\tAlphabet:");
        output.AppendLine($"\t\t{_alphabet}");
        output.AppendLine();

        output.AppendLine("\tRotors:");
        foreach (var rotor in _rotors)
        {
            output.AppendLine($"\t\t{rotor}");
        }

        output.AppendLine();

        output.AppendLine("\tReflector:");
        output.AppendLine($"\t\t{_reflector}");
        output.AppendLine();

        output.AppendLine("\tPlugboard:");
        output.AppendLine($"\t\t{_plugboard}");

        return output.ToString();
    }

    public static (string Wiring, char Notch) GetRotor(string input, bool isLower)
    {
        if (isLower)
        {
            return input switch
            {
                "I" => RotorI,
                "II" => RotorII,
                "III" => RotorIII,
                "IV" => RotorIV,
                "V" => RotorV,
                _ => throw new ArgumentException("Invalid rotor.", nameof(input))
            };
        }

        return input switch
        {
            "I" => RotorI_Upper,
            "II" => RotorII_Upper,
            "III" => RotorIII_Upper,
            "IV" => RotorIV_Upper,
            "V" => RotorV_Upper,
            _ => throw new ArgumentException("Invalid rotor.", nameof(input))
        };
    }

    public static string GetReflectorWiring(char input, bool isLower)
    {
        if (isLower)
        {
            return input switch
            {
                'A' => ReflectorAWiring,
                'B' => ReflectorBWiring,
                'C' => ReflectorCWiring,
                _ => throw new ArgumentException("Invalid reflector.", nameof(input))
            };
        }

        return input switch
        {
            'A' => ReflectorAWiring_Upper,
            'B' => ReflectorBWiring_Upper,
            'C' => ReflectorCWiring_Upper,
            _ => throw new ArgumentException("Invalid reflector.", nameof(input))
        };
    }

    private char EncodeCharacter(char character)
    {
        StepRotors();

        character = _plugboard.Swap(character);

        character = _rotors[2].Forward(character);
        character = _rotors[1].Forward(character);
        character = _rotors[0].Forward(character);

        character = _reflector.Reflect(character);

        character = _rotors[0].Backward(character);
        character = _rotors[1].Backward(character);
        character = _rotors[2].Backward(character);

        character = _plugboard.Swap(character);

        return character;
    }

    private void StepRotors()
    {
        if (_rotors[1].IsNotched)
        {
            _rotors[0].Rotate();
            _rotors[1].Rotate();
        }
        else if (_rotors[2].IsNotched)
        {
            _rotors[1].Rotate();
        }

        _rotors[2].Rotate();
    }
}
