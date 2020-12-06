﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Lab1_3
{
    class Program
    {
        private const double MAX_ACCEPTED_DEVIATION = 0.006;
        private static Dictionary<char, double> LETTER_FREQ = new Dictionary<char, double> {
            { 'a', 0.0855 }, { 'b', 0.0160 }, { 'c', 0.0316 }, { 'd', 0.0387 }, { 'e', 0.1210 },
            { 'f', 0.0218 }, { 'g', 0.0209 }, { 'h', 0.0496 }, { 'i', 0.0733 }, { 'j', 0.0022 },
            { 'k', 0.0081 }, { 'l', 0.0421 }, { 'm', 0.0253 }, { 'n', 0.0717 }, { 'o', 0.0747 },
            { 'p', 0.0207 }, { 'q', 0.0010 }, { 'r', 0.0633 }, { 's', 0.0673 }, { 't', 0.0894 },
            { 'u', 0.0268 }, { 'v', 0.0106 }, { 'w', 0.0183 }, { 'x', 0.0019 }, { 'y', 0.0171 },
            { 'z', 0.0011 }
        };
        private static Random rand = new Random();

        private static string GetRandomKey()
        {
            string key = string.Empty;

            for (int i = 0; i < 26; i++)
            {
                char newChar = (char)('A' + rand.Next(26));
                while (key.Contains(newChar))
                {
                    newChar = (char)('A' + rand.Next(26));
                }
                key += newChar;
            }

            return key;
        }

        private static string GetTextWithKey(string input, string key)
        {
            string outStr = "";

            for (int i = 0; i < input.Length; i++)
            {
                int newSymb = key[input[i] - 'A'];
                outStr += (char)newSymb;
            }

            return outStr;
        }

        private static string MergeKeys(string key1, string key2)
        {
            string resultKey = string.Empty;

            for (int i = 0; i < key1.Length; i++)
            {
                int keyNum = rand.Next(1);

                if (keyNum == 0)
                {
                    resultKey += key1[i];
                }
                else
                {
                    resultKey += key2[i];
                }
            }

            return resultKey;
        }

        private static string GetBestText(string input)
        {
            int[] letterCounts = new int[26];
            Array.Fill(letterCounts, 0);

            foreach (char ch in input)
            {
                letterCounts[ch - 'A']++;
            }

            List<SolutionData> solutions = new List<SolutionData>();

            for (int i = 0; i < 100000; i++)
            {
                string randKey = GetRandomKey();
                double deviation = TextDeviationEstimationFunc(GetTextWithKey(input, randKey));
                if (deviation <= MAX_ACCEPTED_DEVIATION)
                {
                    return GetTextWithKey(input, randKey);
                }
                solutions.Add(new SolutionData { Key = randKey, Deviation = deviation });
            }
            solutions.Sort((solData1, solData2) => solData1.Deviation < solData2.Deviation ? -1 : solData1.Deviation > solData2.Deviation ? 1 : 0);

            int iterationNum = 0;

            while (iterationNum < 10000)
            {
                for (int i = 0; i < 1000; i++)
                {
                    string mergedKey = MergeKeys(solutions[rand.Next(2)].Key, solutions[rand.Next(10)].Key); 
                    double deviation = TextDeviationEstimationFunc(GetTextWithKey(input, mergedKey));
                    if (deviation <= MAX_ACCEPTED_DEVIATION)
                    {
                        return GetTextWithKey(input, mergedKey);
                    }
                    solutions.Add(new SolutionData { Key = mergedKey, Deviation = deviation });
                }

                solutions.Sort((solData1, solData2) => solData1.Deviation < solData2.Deviation ? -1 : solData1.Deviation > solData2.Deviation ? 1 : 0);

                if (solutions[0].Deviation <= MAX_ACCEPTED_DEVIATION)
                {
                    break;
                }

                solutions.RemoveRange(10, solutions.Count - 10);
                Console.WriteLine("{0} {1}", iterationNum, solutions[0].Deviation);
                iterationNum++;
            }

            return GetTextWithKey(input, solutions[0].Key);
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

        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../../Input.txt");
            StreamWriter sw = new StreamWriter("../../../../Out.txt");
            
            string input = sr.ReadLine();
            string output = GetBestText(input);
            Console.WriteLine(output);
            sw.WriteLine(output);

            sr.Close();
            sw.Close();
        }
    }
}
