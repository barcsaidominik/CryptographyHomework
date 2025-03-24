namespace CryptographyHomework;

public static class CharacterHelper
{
    public const string ENGLISH_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string HUNGARIAN_ACCENTED_CHARACTERS = "ÁÉÍÓÖŐÚÜŰ";
    public const string HUNGARIAN_ALPHABET = ENGLISH_ALPHABET + HUNGARIAN_ACCENTED_CHARACTERS;

    public static Dictionary<char, int> GetStringCharacterCounts(this string str)
    {
        var charaters = new Dictionary<char, int>();
        foreach (var character in str.Select(char.ToUpper).Where(HUNGARIAN_ALPHABET.Contains))
        {
            charaters.TryAdd(character, 0);
            charaters[character]++;
        }

        return charaters;
    }

    public static void PrintCharacterCounts(this Dictionary<char, int> charaterCounts)
    {
        foreach (var characterCount in charaterCounts.OrderBy(x => x.Value))
        {
            Console.WriteLine($"{characterCount.Key}\t{characterCount.Value,2}");
        }

        Console.WriteLine();
    }
}
