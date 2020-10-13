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

            int[] array = new int[30];
            char[] letters = new char[30];
            Array.Fill(array, 0);

            for (int i = 0; i < str.Length; i++)
            {
                int ind = str[i] - 'a';
                if (ind >= 0 && ind < 30)
                    array[ind]++;
            }

            for (int i = 0; i < 30; i++)
            {
                letters[i] = (char)('a' + i);
            }

            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    if (array[i] > array[j])
                    {
                        int temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;

                        char temCh = letters[i];
                        letters[i] = letters[j];
                        letters[j] = temCh;
                    }
                }
            }

            int key = letters[0] ^ 'e';

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
