using System.Text;

namespace CryptographyHomework;

public class VigenereEncryptor
{
    private readonly int _alphabetLength;
    private readonly string _alphabet;
    private readonly char[,] _vigenereTable;
    private readonly string _key;

    public VigenereEncryptor(string alphabet, string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(alphabet, nameof(alphabet));
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

        foreach (var character in key)
        {
            if (!alphabet.Contains(character))
            {
                throw new ArgumentException("Key must contain characters from the alphabet.");
            }
        }

        _alphabetLength = alphabet.Length;
        _alphabet = alphabet;
        _vigenereTable = new char[_alphabetLength, _alphabetLength];
        for (var rowIndex = 0; rowIndex < _alphabetLength; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < _alphabetLength; columnIndex++)
            {
                _vigenereTable[rowIndex, columnIndex] = alphabet[(rowIndex + columnIndex) % _alphabetLength];
            }
        }

        _key = key;
    }

    public string Encode(string input)
    {
        var keyIndex = 0;
        var output = new StringBuilder();
        foreach (var character in input)
        {
            var lowerCharacter = char.ToLower(character);
            if (!_alphabet.Contains(lowerCharacter))
            {
                output.Append(character);
                continue;
            }

            var characterIndex = _alphabet.IndexOf(char.ToLower(character));
            var keyCharacterIndex = _alphabet.IndexOf(char.ToLower(_key[keyIndex]));
            output.Append(_vigenereTable[characterIndex, keyCharacterIndex]);
            keyIndex = (keyIndex + 1) % _key.Length;
        }

        return output.ToString();
    }

    public string Decode(string input)
    {
        var keyIndex = 0;
        var output = new StringBuilder();
        foreach (var character in input)
        {
            var lowerCharacter = char.ToLower(character);
            if (!_alphabet.Contains(lowerCharacter))
            {
                output.Append(character);
                continue;
            }

            var keyCharacterIndex = _alphabet.IndexOf(char.ToLower(_key[keyIndex]));
            var characterIndex = _alphabet.IndexOf(lowerCharacter);
            var decodedIndex = (characterIndex - keyCharacterIndex + _alphabetLength) % _alphabetLength;
            output.Append(_alphabet[decodedIndex]);
            keyIndex = (keyIndex + 1) % _key.Length;
        }

        return output.ToString();
    }

    public string GetTableString()
    {
        var output = new StringBuilder();
        output.AppendLine();
        for (var rowIndex = 0; rowIndex < _alphabetLength; rowIndex++)
        {
            output.Append(' ');
            for (var columnIndex = 0; columnIndex < _alphabetLength; columnIndex++)
            {
                if (columnIndex != 0)
                {
                    output.Append(" │ ");
                }

                output.Append(_vigenereTable[rowIndex, columnIndex]);
            }

            if (rowIndex != _alphabetLength - 1)
            {
                output.AppendLine();
                for (var columnIndex = 0; columnIndex < _alphabetLength * 2 - 1; columnIndex++)
                {
                    if (columnIndex % 2 != 0)
                    {
                        output.Append("┼");
                    }
                    else
                    {
                        output.Append("───");
                    }
                }
            }

            output.AppendLine();
        }

        return output.ToString();
    }
}
