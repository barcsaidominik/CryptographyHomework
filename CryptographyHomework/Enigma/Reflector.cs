namespace CryptographyHomework.Enigma;

public class Reflector
{
    private readonly string _alphabet;
    private readonly string _wiring;

    public Reflector(string alphabet, string wiring)
    {
        ArgumentException.ThrowIfNullOrEmpty(alphabet, nameof(alphabet));
        ArgumentException.ThrowIfNullOrEmpty(wiring, nameof(wiring));
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
}
