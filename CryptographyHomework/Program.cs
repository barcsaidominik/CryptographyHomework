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
        var vigenereEncryption = new VigenereEncryptor(CharacterHelper.HUNGARIAN_ALPHABET, vigenereEncryptionKey);

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
        var enigmaMachine = EnigmaMachine.CreateEnigmaM3(
            CharacterHelper.ENGLISH_ALPHABET,
            [
                new EnigmaMachine.RotorSettings("V", 'A', 'F'),
                new EnigmaMachine.RotorSettings("III", 'K', 'D'),
                new EnigmaMachine.RotorSettings("II", 'K', 'V'),
            ],
            'B',
            ["AO", "HI", "MU", "SN", "VX", "ZQ"],
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
}
