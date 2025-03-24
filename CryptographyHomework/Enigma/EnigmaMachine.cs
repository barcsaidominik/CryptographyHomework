using System.Text;

namespace CryptographyHomework.Enigma;

public partial class EnigmaMachine
{
    public record RotorSettings(string SelectedRotor, char? SelectedRing = null, char? SelectedStart = null);

    private readonly string _alphabet;
    private readonly List<Rotor> _rotors;
    private readonly Reflector _reflector;
    private readonly Plugboard _plugboard;

    private EnigmaMachine(string alphabet, List<Rotor> rotors, Reflector reflector, Plugboard plugboard)
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
        output.AppendLine("Alphabet:");
        output.AppendLine($"\t{_alphabet}");

        output.AppendLine("Rotors:");
        foreach (var rotor in _rotors)
        {
            output.AppendLine($"\t{rotor}");
        }

        output.AppendLine("Reflector:");
        output.AppendLine($"\t{_reflector}");

        output.AppendLine("Plugboard:");
        output.AppendLine($"\t{_plugboard}");

        return output.ToString();
    }

    public static EnigmaMachine CreateEnigmaM3(
        string alphabet,
        RotorSettings[] rotorSettings,
        char selectedReflector,
        string[]? plugs = null,
        string? alphabetExtensions = null
    )
    {
        var rotors = rotorSettings?
            .Select(x => Rotor.Create(alphabet, x.SelectedRotor, x.SelectedRing ?? 'A', x.SelectedStart ?? 'A', alphabetExtensions))
            .ToList()
            ?? [];

        var reflector = Reflector.Create(alphabet, selectedReflector, alphabetExtensions);
        Plugboard plugboard = new();
        if (plugs is { Length: > 0 })
        {
            foreach (var pair in plugs)
            {
                plugboard.AddPlug(pair[0], pair[1]);
            }
        }

        return new EnigmaMachine(alphabet + alphabetExtensions, rotors, reflector, plugboard);
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
