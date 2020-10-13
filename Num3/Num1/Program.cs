using System;
using System.IO;

namespace Num1
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../../../Try1.txt");
            StreamWriter sw = new StreamWriter("../../../../../Out.txt");
            string str = sr.ReadToEnd();

            int maxCounter = 0;
            int maxSmech = 0;

            for (int smech = 0; smech < 4; smech++)
            {
                int step = 4 - smech;
                for (int ind = smech; ind < str.Length; ind+=4)
                {
                    sw.Write(str[ind]);
                }
            }
        }
    }
}
