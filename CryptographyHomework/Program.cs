using CryptographyHomework.Enigma;
using System.Text;

namespace CryptographyHomework;

public class Program
{
    public static void Main(string[] args)
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        var sourceString = File.ReadAllText("SourceString.txt");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Kódolatlan szöveg:");
        Console.ResetColor();
        Console.WriteLine(sourceString);
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Kódolatlan szöveg karakterisztikája:");
        Console.ResetColor();
        sourceString.GetStringCharacterCounts().PrintCharacterCounts();
        Console.WriteLine();

        RunVigenere(sourceString);
        RunEnigma(sourceString);
    }

    private static void RunVigenere(string sourceString)
    {
        var vigenereEncryptionKey = "nagyontitkoskulcs";
        var vigenereEncryption = CreateVigenere(vigenereEncryptionKey);

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Vigenère-táblázat");
        Console.ResetColor();
        Console.WriteLine(vigenereEncryption.GetTableString());
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Kulcs: {vigenereEncryptionKey}");
        Console.ResetColor();
        Console.WriteLine();

        var vigenereEncodedString = vigenereEncryption.Encode(sourceString);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Vigenère által kódolt szöveg:");
        Console.ResetColor();
        Console.WriteLine(vigenereEncodedString);
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Vigenère által kódolt szöveg karakterisztikája:");
        Console.ResetColor();
        vigenereEncodedString.GetStringCharacterCounts().PrintCharacterCounts();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Vigenère által dekódolt szöveg:");
        Console.ResetColor();
        Console.WriteLine(vigenereEncryption.Decode(vigenereEncodedString));
        Console.WriteLine();
    }

    private static void RunEnigma(string sourceString)
    {
        var enigmaMachine = CreateEnigmaM3(
            CharacterHelper.ENGLISH_ALPHABET,
            ["V", "III", "II"],
            'B',
            ['A', 'K', 'K'],
            ['F', 'D', 'V'],
            [['A', 'O'], ['H', 'I'], ['M', 'U'], ['S', 'N'], ['V', 'X'], ['Z', 'Q']],
            CharacterHelper.HUNGARIAN_ACCENTED_CHARACTERS
        );
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Enigma Machine:");
        Console.ResetColor();
        Console.WriteLine(enigmaMachine);

        var enigmaEncodedString = enigmaMachine.Run(sourceString);

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Enigma által kódolt szöveg:");
        Console.ResetColor();
        Console.WriteLine(enigmaEncodedString);
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Enigma által kódolt szöveg karakterisztikája:");
        Console.ResetColor();
        enigmaEncodedString.GetStringCharacterCounts().PrintCharacterCounts();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Enigma által dekódolt szöveg:");
        Console.ResetColor();
        Console.WriteLine(enigmaMachine.Run(enigmaEncodedString));
        Console.WriteLine();
    }

    public static VigenereEncryptor CreateVigenere(string key, bool isHungarian = true)
    {
        return new VigenereEncryptor(isHungarian ? CharacterHelper.HUNGARIAN_ALPHABET : CharacterHelper.ENGLISH_ALPHABET, key);
    }

    public static EnigmaMachine CreateEnigmaM3(
        string alphabet,
        string[] selectedRotors,
        char selectedReflector,
        char[]? selectedRings = null,
        char[]? selectedStarts = null,
        char[][]? plugs = null,
        string? alphabetExtensions = null
    )
    {
        selectedRings ??= ['A', 'A', 'A'];
        selectedStarts ??= ['A', 'A', 'A'];

        if (selectedRotors.Length != 3 || selectedRings.Length != 3 || selectedStarts.Length != 3)
        {
            throw new ArgumentException("Rotors, rotor rings and rotor starts must have 3 elements.");
        }

        List<Rotor> rotors = [];
        for (int i = 0; i < selectedRotors.Length; i++)
        {
            var rotor = Rotor.Create(alphabet, selectedRotors[i], selectedRings[i], selectedStarts[i], alphabetExtensions);
            rotors.Add(rotor);
        }

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
}
