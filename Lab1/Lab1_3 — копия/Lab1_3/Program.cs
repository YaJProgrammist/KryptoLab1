using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab1_3
{
    class Program
    {
        private const double MAX_ACCEPTED_DEVIATION = 0.00000000001;
        private static Dictionary<char, double> LETTER_FREQ = new Dictionary<char, double> {
            { 'a', 0.0855 }, { 'b', 0.0160 }, { 'c', 0.0316 }, { 'd', 0.0387 }, { 'e', 0.1210 },
            { 'f', 0.0218 }, { 'g', 0.0209 }, { 'h', 0.0496 }, { 'i', 0.0733 }, { 'j', 0.0022 },
            { 'k', 0.0081 }, { 'l', 0.0421 }, { 'm', 0.0253 }, { 'n', 0.0717 }, { 'o', 0.0747 },
            { 'p', 0.0207 }, { 'q', 0.0010 }, { 'r', 0.0633 }, { 's', 0.0673 }, { 't', 0.0894 },
            { 'u', 0.0268 }, { 'v', 0.0106 }, { 'w', 0.0183 }, { 'x', 0.0019 }, { 'y', 0.0171 },
            { 'z', 0.0011 }
        };
        private static Dictionary<string, double> TRIGRAM_FREQ = new Dictionary<string, double> {
            { "THE", 1.87 }, { "AND", 0.78 }, { "ING", 0.69 }, { "HER", 0.42 },
            { "THA", 0.37 }, { "ENT", 0.36 }, { "ERE", 0.33 }, { "ION", 0.33 },
            { "ETH", 0.32 }, { "NTH", 0.32 }, { "HAT", 0.31 }, { "INT", 0.29 },
            { "FOR", 0.28 }, { "ALL", 0.27 }, { "STH", 0.26 }, { "TER", 0.26 },
            { "EST", 0.26 }, { "TIO", 0.26 }, { "HIS", 0.25 }, { "OFT", 0.24 },
            { "HES", 0.24 }, { "ITH", 0.24 }, { "ERS", 0.24 }, { "ATI", 0.24 },
            { "OTH", 0.23 }, { "FTH", 0.23 }, { "DTH", 0.23 }, { "VER", 0.22 },
            { "TTH", 0.22 }, { "THI", 0.22 }, { "REA", 0.21 }, { "SAN", 0.21 },
            { "WIT", 0.21 }, { "ATE", 0.2 }, { "ARE", 0.2 }, { "EAR", 0.19 },
            { "RES", 0.19 }, { "ONT", 0.18 }, { "TIN", 0.18 }, { "ESS", 0.18 },
            { "RTH", 0.18 }, { "WAS", 0.18 }, { "SOF", 0.18 }, { "EAN", 0.17 },
            { "YOU", 0.17 }, { "SIN", 0.17 }, { "STO", 0.17 }, { "IST", 0.17 },
            { "EDT", 0.16 }, { "EOF", 0.16 }, { "EVE", 0.16 }, { "ONE", 0.16 },
            { "AST", 0.16 }, { "ONS", 0.16 }, { "DIN", 0.16 }, { "OME", 0.16 },
            { "CON", 0.16 }, { "ERA", 0.16 }, { "STA", 0.15 }, { "OUR", 0.15 },
            { "NCE", 0.15 }, { "TED", 0.15 }, { "GHT", 0.15 }, { "HEM", 0.15 },
            { "MAN", 0.15 }, { "HEN", 0.15 }, { "NOT", 0.15 }, { "ORE", 0.15 },
            { "OUT", 0.15 }, { "ORT", 0.15 }, { "ESA", 0.15 }, { "ERT", 0.15 },
            { "SHE", 0.14 }, { "ANT", 0.14 }, { "NGT", 0.14 }, { "EDI", 0.14 },
            { "ERI", 0.14 }, { "EIN", 0.14 }, { "NDT", 0.14 }, { "NTO", 0.14 },
            { "ATT", 0.14 }, { "ECO", 0.13 }, { "AVE", 0.13 }, { "MEN", 0.13 },
            { "HIN", 0.13 }, { "HEA", 0.13 }, { "IVE", 0.13 }, { "EDA", 0.13 },
            { "INE", 0.13 }, { "RAN", 0.13 }, { "HEC", 0.13 }, { "TAN", 0.13 },
            { "RIN", 0.13 }, { "ILL", 0.13 }, { "NDE", 0.13 }, { "THO", 0.13 },
            { "HAN", 0.13 }, { "COM", 0.12 }, { "IGH", 0.12 }, { "AIN", 0.12 }
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
                int keyNum = rand.Next(2);

                char nextChar;
                if (keyNum == 0)
                {

                    nextChar = key1[i];
                }
                else
                {
                    nextChar = key2[i];
                }

                while (resultKey.Contains(nextChar))
                {
                    nextChar = (char)('A' + rand.Next(26));
                }

                resultKey += nextChar;
            }

            return resultKey;
        }

        private static string GetMutatedKey(string input)
        {
            StringBuilder inputStringBuilder = new StringBuilder(input);

            for (int i = 0; i < 3; i++)
            {
                int keyInd1 = rand.Next(input.Length);
                int keyInd2 = rand.Next(input.Length);

                char temp = inputStringBuilder[keyInd1];
                inputStringBuilder[keyInd1] = inputStringBuilder[keyInd2];
                inputStringBuilder[keyInd2] = temp;
            }

            return inputStringBuilder.ToString();
        }

        private static string GetBestText(string input)
        {
            List<SolutionData> solutions = new List<SolutionData>();

            for (int i = 0; i < 100000; i++)
            {
                string randKey = GetRandomKey();//"" + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i) + (char)('A' + i);//GetRandomKey();
                double deviation = TextDeviationEstimationFunc(GetTextWithKey(input, randKey));
                if (deviation <= MAX_ACCEPTED_DEVIATION)
                {
                    Console.WriteLine(deviation);
                    return GetTextWithKey(input, randKey);
                }
                solutions.Add(new SolutionData { Key = randKey, Deviation = deviation });
            }
            solutions.Sort((solData1, solData2) => solData1.Deviation < solData2.Deviation ? -1 : solData1.Deviation > solData2.Deviation ? 1 : 0);

            int iterationNum = 0;
            int sameResultCount = 0;
            double prevResult = -1;

            while (iterationNum < 10000)
            {
                List<string> newKeys = new List<string>();
                for (int i = 0; i < 1000; i++)
                {
                    newKeys.Add(MergeKeys(solutions[rand.Next(600)].Key, solutions[rand.Next(600)].Key));
                }
                for (int i = 0; i < 50; i++)
                {
                    //newKeys.Add(MergeKeys(solutions[rand.Next(10)].Key, solutions[rand.Next(10)].Key));
                }
                for (int i = 0; i < 10; i++)
                {
                    newKeys.Add(solutions[i].Key);
                }
                if (sameResultCount >= 5)
                {
                    for (int i = 0; i < 600; i++)
                    {
                        newKeys.Add(GetMutatedKey(solutions[i].Key));
                    }
                }

                solutions.Clear();

                for (int i = 0; i < newKeys.Count; i++)
                {
                    if (solutions.Exists(sol => sol.Key == newKeys[i]))
                    {
                        continue;
                    }

                    double deviation = TextDeviationEstimationFunc(GetTextWithKey(input, newKeys[i]));
                    if (deviation <= MAX_ACCEPTED_DEVIATION)
                    {
                        Console.WriteLine("{0} {1}", iterationNum, deviation);
                        return GetTextWithKey(input, newKeys[i]);
                    }
                    solutions.Add(new SolutionData { Key = newKeys[i], Deviation = deviation });
                }

                solutions.Sort((solData1, solData2) => solData1.Deviation < solData2.Deviation ? -1 : (solData1.Deviation > solData2.Deviation ? 1 : 0));
                Console.WriteLine("==============================");
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(solutions[i].Key);
                }
                
                Console.WriteLine("{0} {1}", iterationNum, solutions[0].Deviation);
                if (prevResult == solutions[0].Deviation)
                {
                    sameResultCount++;
                }
                else
                {
                    sameResultCount = 0;
                    prevResult = solutions[0].Deviation;
                }

                if (solutions[0].Deviation <= MAX_ACCEPTED_DEVIATION)
                {
                    break;
                }

                if (solutions.Count > 1000)
                    solutions.RemoveRange(1000, solutions.Count - 1000);

                iterationNum++;
                Console.WriteLine(GetTextWithKey(input, solutions[0].Key));
            }

            return GetTextWithKey(input, solutions[0].Key);
        }

        private static double TextDeviationEstimationFunc(string text)
        {
            Dictionary<string, double> trigramCounts = new Dictionary<string, double>();

            for (int i = 0; i < text.Length - 2; i++)
            {
                string trigram = text.Substring(i, 3);
                if (TRIGRAM_FREQ.ContainsKey(trigram))
                {
                    if (trigramCounts.ContainsKey(trigram))
                    {
                        trigramCounts[trigram]++;
                    }
                    else
                    {
                        trigramCounts.Add(trigram, 1);
                    }
                }
            }

            double deviation = 0;

            foreach (var pair in TRIGRAM_FREQ)
            {
                if (trigramCounts.ContainsKey(pair.Key))
                {
                    //Console.WriteLine(pair.Key);
                    deviation += Math.Pow(trigramCounts[pair.Key] * 100 / ((double)text.Length - 2) - pair.Value, 2);
                }
                else
                {
                    deviation += Math.Pow(pair.Value, 2);
                }
            }

            return deviation;
        }

        private static double TextDeviationEstimationFunc1(string text)
        {
            int[] letterCounts = new int[26];
            Array.Fill(letterCounts, 0);

            for (int i = 0; i < text.Length; i++)
            {
                int ind = text[i] - 'A';
                if (ind >= 0 && ind < 26)
                {
                    letterCounts[ind]++;
                }
            }

            double deviation = 0;

            for (int i = 0; i < letterCounts.Length; i++)
            {
                //if (letterCounts[i] > 0)
                //{
                    deviation += Math.Pow(letterCounts[i] / (double)text.Length - LETTER_FREQ[(char)('a' + i)], 2);
                //}
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
