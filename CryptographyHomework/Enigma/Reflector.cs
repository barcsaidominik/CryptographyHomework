namespace CryptographyHomework.Enigma;

public class Reflector
{
    private static readonly string ReflectorAWiring = "EJMZALYXVBWFCRQUONTSPIKHGD";
    private static readonly string ReflectorBWiring = "YRUHQSLDPXNGOKMIEBFZCWVJAT";
    private static readonly string ReflectorCWiring = "FVPJIAOYEDRZXWGCTKUQSBNMHL";

    private readonly string _alphabet;
    private readonly string _wiring;

    private Reflector(string alphabet, string wiring)
    {
        ArgumentException.ThrowIfNullOrEmpty(alphabet, nameof(alphabet));
        ArgumentException.ThrowIfNullOrEmpty(wiring, nameof(wiring));

        alphabet = alphabet.ToUpper();
        wiring = wiring.ToUpper();

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

        _alphabet = alphabet;
        _wiring = wiring;
    }

    public string Alphabet => _alphabet;

    public char Reflect(char character)
    {
        var index = _alphabet.IndexOf(character);
        return _wiring[index];
    }

    public override string ToString()
    {
        return $"Wiring: {_wiring}";
    }

    public static Reflector Create(
        string alphabet,
        char selectedReflector,
        string? alphabetExtensions = null
    )
    {
        return selectedReflector switch
        {
            'A' => new Reflector(alphabet + alphabetExtensions, ReflectorAWiring + alphabetExtensions),
            'B' => new Reflector(alphabet + alphabetExtensions, ReflectorBWiring + alphabetExtensions),
            'C' => new Reflector(alphabet + alphabetExtensions, ReflectorCWiring + alphabetExtensions),
            _ => throw new ArgumentException("Invalid reflector.", nameof(selectedReflector))
        };
    }

}
