using System;
using System.Collections.Generic;
using System.IO;

namespace Lab1_2
{
    class Program
    {
        private static Dictionary<char, double> LETTER_FREQ = new Dictionary<char, double> {
            { 'a', 0.0855 }, { 'b', 0.0160 }, { 'c', 0.0316 }, { 'd', 0.0387 }, { 'e', 0.1210 },
            { 'f', 0.0218 }, { 'g', 0.0209 }, { 'h', 0.0496 }, { 'i', 0.0733 }, { 'j', 0.0022 },
            { 'k', 0.0081 }, { 'l', 0.0421 }, { 'm', 0.0253 }, { 'n', 0.0717 }, { 'o', 0.0747 },
            { 'p', 0.0207 }, { 'q', 0.0010 }, { 'r', 0.0633 }, { 's', 0.0673 }, { 't', 0.0894 },
            { 'u', 0.0268 }, { 'v', 0.0106 }, { 'w', 0.0183 }, { 'x', 0.0019 }, { 'y', 0.0171 },
            { 'z', 0.0011 }
        };

        private static byte[] StrToBytes(string inp)
        {
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < inp.Length; i += 2)
            {
                bytes.Add(Convert.ToByte(inp.Substring(i, 2), 16));
            }

            return bytes.ToArray();
        }

        private static void CalculateKeyLength(byte[] str)
        {
            StreamWriter sw = new StreamWriter("../../../../KeyLength.txt");

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
                sw.WriteLine(counter);
            }

            sw.Close();
        }

        private static List<int> GetPossibleKeysForOffset(byte[] str, int offset, int keyLength)
        {
            int[] array = new int[100];
            Array.Fill(array, 0);

            for (int i = offset; i < str.Length; i += keyLength)
            {
                int ind = str[i];
                if (ind >= 0 && ind < 100)
                    array[ind]++;
            }

            int maxFreqNum = 0;
            for (int i = 1; i < 100; i++)
            {
                if (array[i] > array[maxFreqNum])
                {
                    maxFreqNum = i;
                }
            }

            char mostFreqChar = (char)maxFreqNum;

            List<int> keys = new List<int>();
            keys.Add(mostFreqChar ^ ' ');
            keys.Add(mostFreqChar ^ 'e');
            keys.Add(mostFreqChar ^ 't');
            keys.Add(mostFreqChar ^ 'a');
            keys.Add(mostFreqChar ^ 'o');
            keys.Add(mostFreqChar ^ 'i');

            return keys;
        }

        private static string GetBestText(List<string> texts)
        {
            double bestEstimation = -1;
            int bestTextNum = -1;

            for (int i = 0; i < texts.Count; i++)
            {
                double estimation = TextDeviationEstimationFunc(texts[i]);
                if (estimation < bestEstimation || bestEstimation == -1)
                {
                    bestEstimation = estimation;
                    bestTextNum = i;
                }
            }

            return texts[bestTextNum];
        }

        private static double TextDeviationEstimationFunc(string text)
        {
            int[] letterCounts = new int[64];
            Array.Fill(letterCounts, 0);

            for (int i = 0; i < text.Length; i++)
            {
                int ind = text[i] - 'A';
                if (ind >= 0 && ind < 26)
                {
                    letterCounts[ind]++;
                }
                else if (ind >= 32 && ind < 58)
                {
                    letterCounts[ind - 32]++;
                }
            }

            double deviation = 0;

            for (int i = 0; i < letterCounts.Length; i++)
            {
                if (letterCounts[i] > 0)
                {
                    deviation += Math.Pow(letterCounts[i] / (double)text.Length - LETTER_FREQ[(char)('a' + i)], 2);
                }
            }

            return deviation;
        }

        private static string GetTextWithKey(byte[] input, List<int> key)
        {
            string outStr = "";

            for (int i = 0; i < input.Length; i++)
            {
                int newSymb = input[i] ^ key[i % key.Count];
                outStr += (char)newSymb;
            }

            return outStr;
        }

        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../../Input.txt");
            StreamWriter sw = new StreamWriter("../../../../Out.txt");
            string inpStr = sr.ReadToEnd();
            byte[] input = StrToBytes(inpStr);

            //CalculateKeyLength(str); // turned out it is 3

            int keyLength = 3;
            List<int>[] keys = new List<int>[keyLength];

            for (int offset = 0; offset < keyLength; offset++)
            {
                keys[offset] = GetPossibleKeysForOffset(input, offset, keyLength);
            }

            List<List<int>> possibleKeys = new List<List<int>>();
            possibleKeys.Add(new List<int>());

            for (int i = 0; i < keyLength; i++)
            {
                List<List<int>> newPossibleKeys = new List<List<int>>();

                for (int j = 0; j < possibleKeys.Count; j++)
                {
                    for (int q = 0; q < keys[i].Count; q++)
                    {
                        List<int> newPossibleKey = new List<int>(possibleKeys[j]);
                        newPossibleKey.Add(keys[i][q]);
                        newPossibleKeys.Add(newPossibleKey);
                    }
                }

                possibleKeys = newPossibleKeys;
            }

            List<string> possibleTexts = new List<string>();

            for (int keyNum = 0; keyNum < possibleKeys.Count; keyNum++)
            {
                possibleTexts.Add(GetTextWithKey(input, possibleKeys[keyNum]));
            }

            string resText = GetBestText(possibleTexts);

            sw.WriteLine(resText);
            Console.WriteLine(resText);

            sr.Close();
            sw.Close();
        }
    }
}
