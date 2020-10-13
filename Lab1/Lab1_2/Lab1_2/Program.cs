using System;
using System.IO;

namespace Lab1_2
{
    class Program
    {
        private static void CalculateKeyLength(string str)
        {
            StreamWriter sw = new StreamWriter("../../../../TempData.txt");
            

            for (int smech = 1; smech < str.Length; smech++)
            {
                int counter = 0;
                for (int ind = 0; ind < str.Length; ind++)
                {
                    int newInd = (ind + smech) % str.Length;
                    if (str[ind] == str[newInd])
                    {
                        counter++;
                    }
                }

            }
        }

        private static int GetKeyForOffset(string str, int offset, int keyLength)
        {
            int[] array = new int[100];
            char[] letters = new char[100];
            Array.Fill(array, 0);

            for (int i = offset; i < str.Length; i += keyLength)
            {
                int ind = str[i] - '0';
                array[ind]++;
            }

            for (int i = 0; i < 100; i++)
            {
                letters[i] = (char)(i + '0');
                Console.Write(letters[i]);
            }

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
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

            //if (offset == 2 || offset == 5) key = letters[0] ^ 'a';
            return key;
        }

        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../../Try.txt");
            StreamWriter sw = new StreamWriter("../../../../Out.txt");
            string str = sr.ReadToEnd();

            //CalculateKeyLength(str); // turns out ot is 6

            int keyLength = 6;
            int[] keys = new int[keyLength];

            for (int offset = 0; offset < keyLength; offset++)
            {
                keys[offset] = GetKeyForOffset(str, offset, keyLength);
                Console.WriteLine((char)keys[offset]);
            }

            string outStr = "";

            for (int i = 0; i < str.Length; i++)
            {
                outStr += (char)((str[i] ^ keys[i % keyLength]));
            }

            sw.WriteLine(outStr);
            Console.WriteLine(outStr);

            sr.Close();
            sw.Close();
        }
    }
}
