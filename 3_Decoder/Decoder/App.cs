using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decoder
{
    public class App
    {
        public void StartApp()
        {
            while (true)
            {
                Console.WriteLine("Input:");
                string input = Console.ReadLine();
                Decrypter decrypter = new();
                Console.WriteLine("Output:");
                Console.WriteLine(decrypter.Decrypt(input));
                Console.WriteLine("-------");
            }
        }
    }
}
