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

        alphabet = alphabet.ToUpper();
        key = key.ToUpper();

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
            var wasLower = char.IsLower(character);
            var upperCharacter = char.ToUpper(character);
            if (!_alphabet.Contains(upperCharacter))
            {
                output.Append(character);
                continue;
            }

            var characterIndex = _alphabet.IndexOf(upperCharacter);
            var keyCharacterIndex = _alphabet.IndexOf(_key[keyIndex]);

            var encodedCharacter = _vigenereTable[characterIndex, keyCharacterIndex];
            encodedCharacter = wasLower ? char.ToLower(encodedCharacter) : encodedCharacter;

            output.Append(encodedCharacter);

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
            var wasLower = char.IsLower(character);
            var upperCharacter = char.ToUpper(character);
            if (!_alphabet.Contains(upperCharacter))
            {
                output.Append(character);
                continue;
            }

            var keyCharacterIndex = _alphabet.IndexOf(_key[keyIndex]);
            var characterIndex = _alphabet.IndexOf(upperCharacter);
            var decodedIndex = (characterIndex - keyCharacterIndex + _alphabetLength) % _alphabetLength;

            var decodedCharacter = _alphabet[decodedIndex];
            decodedCharacter = wasLower ? char.ToLower(decodedCharacter) : decodedCharacter;

            output.Append(decodedCharacter);

            keyIndex = (keyIndex + 1) % _key.Length;
        }

        return output.ToString();
    }

    public string GetTableString()
    {
        var output = new StringBuilder();
        AppendTopBorder(output);

        for (var rowIndex = 0; rowIndex < _alphabetLength; rowIndex++)
        {
            AppendRow(output, rowIndex);
            if (rowIndex != _alphabetLength - 1)
            {
                AppendMiddleBorder(output);
            }
        }

        AppendBottomBorder(output);
        return output.ToString();
    }

    private void AppendTopBorder(StringBuilder output)
    {
        output.Append('┌');
        for (var columnIndex = 2; columnIndex < _alphabetLength * 2 + 1; columnIndex++)
        {
            output.Append(columnIndex % 2 != 0 ? '┬' : "───");
        }
        output.Append('┐').AppendLine();
    }

    private void AppendMiddleBorder(StringBuilder output)
    {
        output.Append('├');
        for (var columnIndex = 2; columnIndex < _alphabetLength * 2 + 1; columnIndex++)
        {
            output.Append(columnIndex % 2 != 0 ? '┼' : "───");
        }
        output.Append('┤').AppendLine();
    }

    private void AppendBottomBorder(StringBuilder output)
    {
        output.Append('└');
        for (var columnIndex = 2; columnIndex < _alphabetLength * 2 + 1; columnIndex++)
        {
            output.Append(columnIndex % 2 != 0 ? '┴' : "───");
        }
        output.Append('┘');
    }

    private void AppendRow(StringBuilder output, int rowIndex)
    {
        output.Append("│ ");
        for (var columnIndex = 0; columnIndex < _alphabetLength; columnIndex++)
        {
            if (columnIndex != 0)
            {
                output.Append(" │ ");
            }
            output.Append(_vigenereTable[rowIndex, columnIndex]);
        }
        output.Append(" │").AppendLine();
    }
}
