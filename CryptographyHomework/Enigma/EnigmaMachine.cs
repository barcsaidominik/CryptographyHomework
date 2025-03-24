using System.Text;

namespace CryptographyHomework.Enigma;

public class EnigmaMachine
{
    private readonly string _alphabet;
    private readonly List<Rotor> _rotors;
    private readonly Reflector _reflector;
    private readonly Plugboard _plugboard;

    public EnigmaMachine(string alphabet, List<Rotor> rotors, Reflector reflector, Plugboard plugboard)
    {
        ArgumentException.ThrowIfNullOrEmpty(alphabet, nameof(alphabet));
        ArgumentNullException.ThrowIfNull(rotors, nameof(rotors));
        if (rotors.Count != 3)
        {
            throw new ArgumentException("Three rotors are required.", nameof(rotors));
        }

        ArgumentNullException.ThrowIfNull(reflector, nameof(reflector));
        ArgumentNullException.ThrowIfNull(plugboard, nameof(plugboard));

        alphabet = alphabet.ToUpper();
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
        _plugboard = plugboard;
    }

    public Plugboard Plugboard => _plugboard;

    public string Run(string input)
    {
        Reset();
        var output = new StringBuilder();
        foreach (var character in input)
        {
            var wasLower = char.IsLower(character);
            var upperCharacter = char.ToUpper(character);
            if (!_alphabet.Contains(upperCharacter))
            {
                output.Append(character);
                continue;
            }

            var encodedCharacter = EncodeCharacter(upperCharacter);
            encodedCharacter = wasLower ? char.ToLower(encodedCharacter) : encodedCharacter;

            output.Append(encodedCharacter);
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
