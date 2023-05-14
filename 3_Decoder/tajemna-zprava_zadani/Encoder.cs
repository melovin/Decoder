using System.Text;

namespace Kolo2_Uloha3;

public static class Encoder
{
    public static void Encode()
    {
        while (true)
        {
            string input = Console.ReadLine() ?? string.Empty;

            if (input.Length < 3)
                return;

            Console.WriteLine(EncodeString(input));
        }
    }

    private static string EncodeBytes(IReadOnlyList<byte> bytes)
    {
        List<string> alphabet = new()
        {
            "a", "i", "u", "e", "o",
            "ka", "ki", "ku", "ke", "ko",
            "sa", "shi", "su", "se", "so",
            "ta", "chi", "tsu", "te", "to",
            "na", "ni", "nu", "ne", "no",
            "ha", "hi", "fu", "he", "ho",
            "ma", "mi", "mu", "me", "mo",
            "ya", "yu", "yo", "ra", "ri",
            "ru", "re", "ro", "wa",
            "ga", "gi", "gu", "ge", "go",
            "za", "ji", "zu", "ze", "zo",
            "ba", "bi", "bu", "be", "bo",
            "pa", "pi", "pu", "pe", "po"
        };

        int length = bytes.Count;
        string result = string.Empty;

        byte sum = 0;
        int position = 0;
        while (position < length)
        {
            if (sum > 64) //neøeším, jen pøidává navíc
                result += " ";

            sum += bytes[position]; //neøeším
            result += alphabet[(bytes[position] & 0b_11111100) >> 2]; //na aktuální èíslo provedu AND s danou maskou (chybí poslední 2 bity), na tom náslenì provedu RIGHTSHIFT o 2

            if (position + 1 < length) //pokud mám ještì nìjaké èílo vpravo, tak...
            {
                result += alphabet[((bytes[position] & 0b_00000011) << 4) + ((bytes[position + 1] & 0b11110000) >> 4)]; //na aktuálním èíle provedu AND s danou maskou (poslední 2 bity), na tom provedu LEFTSHIFT o 4 +
                                                                                                                        //èíslo následující a na nìm AND (první 4 bity) a na tom RIGHTSHIFT o 4

                if (position + 2 < length) //pokud mám vpravo ob místo èíslo, tak...
                {
                    result += alphabet[((bytes[position + 1] & 0b_00001111) << 2) + ((bytes[position + 2] & 0b_11000000) >> 6)];//èíslo následující a na nìm AND (poslední 4 bity) a na tom LEFTSHIFT o 2 +
                                                                                                                                //èíslo vpravo ob místo a na nìm AND (první 2 bity) a na tom RIGHTSHIFT o 6
                    result += alphabet[bytes[position + 2] & 0b_00111111];//èíslo vpravo ob místo a na nìm AND (posledních 6 bitù)
                }
                else
                {
                    result += alphabet[(bytes[position + 1] & 0b_00001111) << 2];//èíslo následující a na nìm AND (poslední 4 bity) a na tom LEFTSHIFT o 2
                    result += ".";
                }
            }
            else //jsem na posledním èísle
            {
                result += alphabet[(bytes[position] & 0b_00000011) << 4];//na aktuálním èíle provedu AND s danou maskou (poslední 2 bity), na tom provedu LEFTSHIFT o 4
                result += sum > 128 ? "!" : "?"; //moc neøeším
            }

            position += 3; //posunu se o 3 èísla dál
        }

        return result;
    }

    private static string EncodeString(string input)
    {
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);

        List<byte> resultBytes = new();
        for (int i = 0; i < input.Length - 1; i++)
        {
            byte next = (byte) (inputBytes[i] ^ inputBytes[i + 1]); //XOR
            resultBytes.Add(next);
        }

        resultBytes.Add(inputBytes[input.Length - 1]);

        return EncodeBytes(resultBytes);
    }
}