namespace MockData;

public class Utils
{
	public static string HigienizarString(string input)
	{
		if (string.IsNullOrEmpty(input))
			return input;

		string[] acentuadas = new string[] { "á", "à", "ã", "â", "ä", "é", "è", "ê", "ë", "í", "ì", "î", "ï", "ó", "ò", "õ", "ô", "ö", "ú", "ù", "û", "ü", "ç",
										 "Á", "À", "Ã", "Â", "Ä", "É", "È", "Ê", "Ë", "Í", "Ì", "Î", "Ï", "Ó", "Ò", "Õ", "Ô", "Ö", "Ú", "Ù", "Û", "Ü", "Ç" };

		string[] semAcento = new string[] { "a", "a", "a", "a", "a", "e", "e", "e", "e", "i", "i", "i", "i", "o", "o", "o", "o", "o", "u", "u", "u", "u", "c",
										"A", "A", "A", "A", "A", "E", "E", "E", "E", "I", "I", "I", "I", "O", "O", "O", "O", "O", "U", "U", "U", "U", "C" };

		for (int i = 0; i < acentuadas.Length; i++)
		{
			input = input.Replace(acentuadas[i], semAcento[i]);
		}

		return input;
	}

	public static int RandomBetween(int v1, int v2)
	{
		return new Random().Next(v1, v2 + 1);
	}

	public static string RandomString(int length, string chars)
	{
		return new string(Enumerable.Repeat(chars, length).Select(s => s[new Random().Next(s.Length)]).ToArray());
	}
}
