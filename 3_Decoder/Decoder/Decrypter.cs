using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Decoder
{
    public class Decrypter
    {
        Dictionary<string, int> alphabet = new(){
            { "a", 0 }, { "i", 1 }, { "u", 2 }, 
            { "e", 3 }, { "o", 4 }, { "ka", 5 }, 
            { "ki", 6 }, { "ku", 7 }, { "ke", 8 }, 
            { "ko", 9 },{ "sa", 10 }, { "shi", 11 }, 
            { "su", 12 }, { "se", 13 }, { "so", 14 },
            { "ta", 15 }, { "chi", 16 }, { "tsu", 17 }, 
            { "te", 18 }, { "to", 19 },{ "na", 20 }, 
            { "ni", 21 }, { "nu", 22 }, { "ne", 23 }, 
            { "no", 24 },{ "ha", 25 }, { "hi", 26 }, 
            { "fu", 27 }, { "he", 28 }, { "ho", 29 },
            { "ma", 30 }, { "mi", 31 }, { "mu", 32 }, 
            { "me", 33 }, { "mo", 34 },{ "ya", 35 }, 
            { "yu", 36 }, { "yo", 37 }, { "ra", 38 }, 
            { "ri", 39 },{ "ru", 40 }, { "re", 41 }, 
            { "ro", 42 }, { "wa", 43 },{ "ga", 44 }, 
            { "gi", 45 }, { "gu", 46 }, { "ge", 47 }, 
            { "go", 48 },{ "za", 49 }, { "ji", 50 }, 
            { "zu", 51 }, { "ze", 52 }, { "zo", 53 },
            { "ba", 54 }, { "bi", 55 }, { "bu", 56 }, 
            { "be", 57 }, { "bo", 58 },{ "pa", 59 }, 
            { "pi", 60 }, { "pu", 61 }, { "pe", 62 }, { "po", 63 },

            };
        private int From { get; set; } = 0;
        public string Decrypt(string input) => this.Translate(this.DecodeInput(input));
        
        private List<byte> DecodeInput(string input)
        {
            List<byte> bytes = new();
            List<int> indexesOfAlpha = new();
            while (this.From < input.Length)
            { 
                int index = this.GetIndex(input);
                if ( index == -1)
                {
                    this.From++;
                    continue;
                }
                else
                    indexesOfAlpha.Add(index);
            }
            byte currentByte;
            for (int i = 0; i < indexesOfAlpha.Count ; i+=4)
            {
                currentByte = (byte)((indexesOfAlpha[i] << 2) + ((indexesOfAlpha[i + 1] & 0b_00110000) >> 4));
                bytes.Add(currentByte);

                currentByte = (byte)((indexesOfAlpha[i+1] & 0b_00001111) << 4);
                if(i + 2 < indexesOfAlpha.Count)
                {
                    currentByte += (byte)((indexesOfAlpha[i + 2] & 0b_00111100) >> 2);
                    bytes.Add(currentByte);
                    if (i + 3 < indexesOfAlpha.Count)
                    {
                        currentByte = (byte)(((indexesOfAlpha[i + 2] & 0b_00000011) << 6) + (indexesOfAlpha[i + 3] & 0b_00111111));
                        bytes.Add(currentByte);
                    }
                }
            }
            return bytes;
        }
        private int GetIndex(string input)
        {
            int result = -1;
            int i = 1;
            bool found = false;
            string checkMe;
            while (!found)
            {
                checkMe = input.Substring(this.From, i);
                if(checkMe == " " | checkMe == "." | checkMe == "!" | checkMe == "?")
                    return result;
                if(this.alphabet.ContainsKey(checkMe))
                {
                    result = this.alphabet[checkMe];
                    found = true;
                    this.From += i;
                }
                i++;
            }
            return result;
        }
        private string Translate(List<byte> bytes)
        {
            string result = "";
            byte temp;
            result += (char)bytes[bytes.Count - 1];
            for (int i = bytes.Count - 1; i > 0 ; i--)
            {
                temp = (byte)(bytes[i-1] ^ bytes[i]);
                result += (char)temp;
                bytes[i-1] = temp;
            }
            return String.Join("", result.Reverse());
        }
    }
}
