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
            if (sum > 64) //ne�e��m, jen p�id�v� nav�c
                result += " ";

            sum += bytes[position]; //ne�e��m
            result += alphabet[(bytes[position] & 0b_11111100) >> 2]; //na aktu�ln� ��slo provedu AND s danou maskou (chyb� posledn� 2 bity), na tom n�slen� provedu RIGHTSHIFT o 2

            if (position + 1 < length) //pokud m�m je�t� n�jak� ��lo vpravo, tak...
            {
                result += alphabet[((bytes[position] & 0b_00000011) << 4) + ((bytes[position + 1] & 0b11110000) >> 4)]; //na aktu�ln�m ��le provedu AND s danou maskou (posledn� 2 bity), na tom provedu LEFTSHIFT o 4 +
                                                                                                                        //��slo n�sleduj�c� a na n�m AND (prvn� 4 bity) a na tom RIGHTSHIFT o 4

                if (position + 2 < length) //pokud m�m vpravo ob m�sto ��slo, tak...
                {
                    result += alphabet[((bytes[position + 1] & 0b_00001111) << 2) + ((bytes[position + 2] & 0b_11000000) >> 6)];//��slo n�sleduj�c� a na n�m AND (posledn� 4 bity) a na tom LEFTSHIFT o 2 +
                                                                                                                                //��slo vpravo ob m�sto a na n�m AND (prvn� 2 bity) a na tom RIGHTSHIFT o 6
                    result += alphabet[bytes[position + 2] & 0b_00111111];//��slo vpravo ob m�sto a na n�m AND (posledn�ch 6 bit�)
                }
                else
                {
                    result += alphabet[(bytes[position + 1] & 0b_00001111) << 2];//��slo n�sleduj�c� a na n�m AND (posledn� 4 bity) a na tom LEFTSHIFT o 2
                    result += ".";
                }
            }
            else //jsem na posledn�m ��sle
            {
                result += alphabet[(bytes[position] & 0b_00000011) << 4];//na aktu�ln�m ��le provedu AND s danou maskou (posledn� 2 bity), na tom provedu LEFTSHIFT o 4
                result += sum > 128 ? "!" : "?"; //moc ne�e��m
            }

            position += 3; //posunu se o 3 ��sla d�l
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