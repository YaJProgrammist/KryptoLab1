using System;
using System.IO;

namespace Num1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[3000];
            char[] letters = new char[30];
            Array.Fill(array, 0);

            StreamReader sr = new StreamReader("../../../../../Try1.txt");
            string str = sr.ReadToEnd();
            Console.WriteLine(str);

            for (int i = 0; i < str.Length; i++)
            {
                int ind = str[i] - 'a';
                if (ind >= 0 && ind < 3000)
                    array[ind]++;
            }

            for (int i = 0; i < 30; i++)
            {
                letters[i] = (char)('a' + i);
                Console.WriteLine("{0} {1}", array[i], (char)('a' + i));
            }

            for (int i = 0; i < 30; i ++)
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

            Console.WriteLine("=================================================");

            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine("{0} {1}", array[i], letters[i]);
            }

            Console.WriteLine(letters[1] ^ 't');
        }
    }
}
