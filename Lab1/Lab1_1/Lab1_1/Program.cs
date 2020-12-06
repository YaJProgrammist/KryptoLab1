using System;
using System.IO;

namespace Lab1_1
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../../Try.txt");
            StreamWriter sw = new StreamWriter("../../../../Out.txt");
            string str = sr.ReadToEnd();

            int[] array = new int[26];
            Array.Fill(array, 0);

            for (int i = 0; i < str.Length; i++)
            {
                int ind = str[i] - 'a';
                if (ind >= 0 && ind < 26)
                {
                    array[ind]++;
                }
                else if (ind >= 32 && ind < 58)
                {
                    array[ind - 32]++;
                }
            }

            int maxFreqNum = 0;
            for (int i = 1; i < 26; i++)
            {
                if (array[i] > array[maxFreqNum])
                {
                    maxFreqNum = i;
                }
            }

            char mostFreqChar = (char)('a' + maxFreqNum);

            int key = mostFreqChar ^ 'e';

            string outStr = "";

            for (int i = 0; i < str.Length; i++)
            {
                outStr += (char)(str[i] ^ key);
            }

            sw.WriteLine(outStr);
            Console.WriteLine(outStr);

            sr.Close();
            sw.Close();
        }
    }
}
