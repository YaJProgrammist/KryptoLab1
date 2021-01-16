using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab1_3
{
    class Program
    {
        private static Random rand = new Random();

        private const double MAX_ACCEPTED_DEVIATION = 0.00001;
        private static Dictionary<string, double> trigramFrequences = new Dictionary<string, double>();
        private static string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private static void GenerateTrigrams()
        {
            StreamReader sr = new StreamReader("../../../../Alice_in_Wonderland-Lewis_Carroll.txt");
            StreamWriter sw = new StreamWriter("../../../../Trigrams.txt");

            string text = sr.ReadLine();
            Dictionary<string, int> trigrammCounts = new Dictionary<string, int>();

            for (int i = 0; i < text.Length - 2; i++)
            {
                string trigramm = text.Substring(i, 3).ToUpper();

                if (trigrammCounts.ContainsKey(trigramm))
                {
                    trigrammCounts[trigramm]++;
                }
                else
                {
                    trigrammCounts.Add(trigramm, 1);
                }
            }

            foreach (var pair in trigrammCounts)
            {
                sw.WriteLine("{0},{1}", pair.Key, pair.Value);
            }

            sr.Close();
            sw.Close();
        }

        private static void FillTrigramsFrequences()
        {
            StreamReader sr = new StreamReader("../../../../Trigrams.txt");

            long sum = 0;
            Dictionary<string, double> trigramFrequencesRaw = new Dictionary<string, double>();

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                if (line.Length == 0)
                {
                    break;
                }

                int spaceInd = line.IndexOf(',');
                string trigram = line.Substring(0, spaceInd);
                long count = long.Parse(line.Substring(spaceInd + 1, line.Length - (spaceInd + 1)));
                sum += count;
                trigramFrequencesRaw.Add(trigram, count);
            }

            foreach (var pair in trigramFrequencesRaw)
            {
                trigramFrequences[pair.Key] = Math.Log10((double)pair.Value / sum);
                //Console.WriteLine("{0} {1}", trigram, Math.Log10(count));
            }

            sr.Close();
        }

        private static string GetRandomKey()
        {
            string key = string.Empty;
            List<char> lettersRemained = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};

            for (int i = 0; i < 26; i++)
            {
                char newChar = lettersRemained[rand.Next(lettersRemained.Count)];
                lettersRemained.Remove(newChar);
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

        private static char CrossChars(char key1, char key2, List<char> listOfCharacters)
        {
            if (!listOfCharacters.Contains(key1) && !listOfCharacters.Contains(key2))
            {
                return listOfCharacters[rand.Next(listOfCharacters.Count)];
            }
            else if (!listOfCharacters.Contains(key1))
            {
                return key2;
            }
            else if (!listOfCharacters.Contains(key2))
            {
                return key1;
            }
            else
            {
                char[] keyChoice = new char[] { key1, key2 };
                return keyChoice[rand.Next(keyChoice.Length)];
            }
        }

        public static string MergeKeys(string key1, string key2)
        {
            List<char> listOfCharacters = new List<char>();
            listOfCharacters.AddRange(ALPHABET);

            StringBuilder result = new StringBuilder();
            char newKey;

            for (int i = 0; i < 26; i++)
            {
                newKey = CrossChars(key1[i], key2[i], listOfCharacters);
                listOfCharacters.Remove(newKey);
                result.Append(newKey);
            }

            return result.ToString();

        }

        private static string GetMutatedKey(string input)
        {
            StringBuilder result = new StringBuilder(input);

            int randPosition1 = rand.Next(input.Length);
            int randPosition2 = rand.Next(input.Length);

            while (randPosition2 == randPosition1)
            {
                randPosition2 = rand.Next(input.Length);
            }

            result[randPosition1] = input[randPosition2];
            result[randPosition2] = input[randPosition1];

            return result.ToString();
        }

        private static string GetBestText(string input)
        {
            List<SolutionData> solutions = new List<SolutionData>();

            for (int i = 0; i < 10000; i++)
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

            solutions.RemoveRange(1000, solutions.Count - 1000);

            int iterationNum = 0;
            //int sameResultCount = 0;
            //double prevResult = -1;

            while (iterationNum < 10000)
            {
                List<string> newKeys = new List<string>();

                for (int i = 0; i < 1000; i++)
                {
                    int rand1 = rand.Next(500);
                    int rand2 = rand.Next(500);
                    while (rand2 == rand1)
                    {
                        rand2 = rand.Next(500);
                    }
                    newKeys.Add(MergeKeys(solutions[rand1].Key, solutions[rand2].Key));
                }

                for (int i = 0; i < 1000; i++)
                {
                    int isMutatingPerc = rand.Next(10);
                    if (isMutatingPerc == 0)
                    {
                        newKeys[i] = GetMutatedKey(newKeys[i]);
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
                Console.WriteLine("{0} {1}", iterationNum, solutions[0].Deviation);

                if (solutions[0].Deviation <= MAX_ACCEPTED_DEVIATION)
                {
                    break;
                }

                if (solutions.Count > 500)
                    solutions.RemoveRange(500, solutions.Count - 500);

                iterationNum++;
                Console.WriteLine(GetTextWithKey(input, solutions[0].Key));
            }

            return GetTextWithKey(input, solutions[0].Key);
        }

        private static double TextDeviationEstimationFunc(string text)
        {
            double deviation = 0;

            for (int i = 0; i < text.Length - 2; i++)
            {
                string trigram = text.Substring(i, 3);
                if (trigramFrequences.ContainsKey(trigram))
                {
                    deviation += trigramFrequences[trigram];
                }
            }

            //Console.WriteLine("dev {0}", deviation / (double)(text.Length - 2));

            return Math.Abs(deviation / (double)(text.Length - 2) + 3.5939192291735513);
        }

        /*private static double TextDeviationEstimationFunc1(string text)
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
        }*/

        private static double CalcIndex()
        {
            StreamReader sr = new StreamReader("../../../../Alice_in_Wonderland-Lewis_Carroll.txt");

            string text = sr.ReadToEnd();
                        
            sr.Close();

            return TextDeviationEstimationFunc(text);
        }

        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../../Input.txt");
            StreamWriter sw = new StreamWriter("../../../../Out.txt");
            
            string input = sr.ReadLine();

            //GenerateTrigrams();
            FillTrigramsFrequences();
            //Console.WriteLine(CalcIndex());

            string output = GetBestText(input);
            Console.WriteLine(output);
            sw.WriteLine(output);

            sr.Close();
            sw.Close();
        }
    }
}
