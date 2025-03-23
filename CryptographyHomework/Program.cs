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
            ["V", "III", "II"],
            'B',
            ['a', 'k', 'k'],
            ['f', 'd', 'v'],
            [['a', 'o'], ['h', 'i'], ['m', 'u'], ['s', 'n'], ['v', 'x'], ['z', 'q']]
        );
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
        string[] selectedRotors,
        char selectedReflector,
        char[]? selectedRotorRings = null,
        char[]? selectedRotorStarts = null,
        char[][]? plugboardSettings = null,
        bool isLower = true,
        bool isHungarian = true
    )
    {
        selectedRotorRings ??= Enumerable.Repeat(isLower ? 'a' : 'A', 3).ToArray();
        selectedRotorStarts ??= Enumerable.Repeat(isLower ? 'a' : 'A', 3).ToArray();

        if (selectedRotors.Length != 3 || selectedRotorRings.Length != 3 || selectedRotorStarts.Length != 3)
        {
            throw new ArgumentException("Rotors, rotor rings and rotor starts must have 3 elements.");
        }

        var alphabet = isLower
            ? isHungarian ? CharacterHelper.HUNGARIAN_ALPHABET : CharacterHelper.ENGLISH_ALPHABET
            : isHungarian ? CharacterHelper.HUNGARIAN_ALPHABET_UPPER : CharacterHelper.ENGLISH_ALPHABET_UPPER;

        List<Rotor> rotors = [];
        for (int i = 0; i < selectedRotors.Length; i++)
        {
            var (wiring, notch) = EnigmaMachine.GetRotor(selectedRotors[i], isLower);
            if (isHungarian)
            {
                wiring += isLower ? CharacterHelper.HUNGARIAN_ACCENTED_CHARACTERS : CharacterHelper.HUNGARIAN_ACCENTED_CHARACTERS_UPPER;
            }

            rotors.Add(new(alphabet, wiring, notch, selectedRotorRings[i], selectedRotorStarts[i]));
        }

        var reflectorWiring = EnigmaMachine.GetReflectorWiring(selectedReflector, isLower);
        if (isHungarian)
        {
            if (isLower)
            {
                reflectorWiring += CharacterHelper.HUNGARIAN_ACCENTED_CHARACTERS;
            }
            else
            {
                reflectorWiring += CharacterHelper.HUNGARIAN_ACCENTED_CHARACTERS_UPPER;
            }
        }

        var reflector = new Reflector(alphabet, reflectorWiring);

        Plugboard? plugboard = null;
        if (plugboardSettings is { Length: > 0 })
        {
            plugboard = new();
            foreach (var pair in plugboardSettings)
            {
                plugboard.AddPlug(pair[0], pair[1]);
            }
        }

        return new EnigmaMachine(alphabet, rotors, reflector, plugboard, isLower);
    }
}
