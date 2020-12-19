using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1_5
{
    public static class KeysMutator
    {
        public static string Key { get; private set; }

        private static Random random = new Random();

        private static List<char> alphabet;

        static KeysMutator()
        {
            alphabet = new List<char> {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };

            Key = GetRandomKey();
        }

        private static string GetRandomKey()
        {
            StringBuilder result = new StringBuilder();
            List<char> lettersRemained = new List<char>();
            lettersRemained.AddRange(alphabet);

            for (int i = 0; i < 26; i++)
            {
                char newChar = lettersRemained[random.Next(lettersRemained.Count)];
                lettersRemained.Remove(newChar);
                result.Append(newChar);
            }

            return result.ToString();
        }

        private static char CrossChars(char key1, char key2, List<char> listOfCharacters)
        {
            if (!listOfCharacters.Contains(key1) && !listOfCharacters.Contains(key2))
            {
                return listOfCharacters[random.Next(listOfCharacters.Count)];
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
                return keyChoice[random.Next(keyChoice.Length)];
            }
        }

        public static string CrossKeys(string key1, string key2)
        {
            List<char> listOfCharacters = new List<char>();
            listOfCharacters.AddRange(alphabet);

            StringBuilder result = new StringBuilder();
            char newKey;

            for (int i = 0; i < Key.Length; i++)
            {
                newKey = CrossChars(key1[i], key2[i], listOfCharacters);
                listOfCharacters.Remove(newKey);
                result.Append(newKey);
            }

            return result.ToString();

        }

        public static string Mutate(string key)
        {
            StringBuilder result = new StringBuilder();

            result.Insert(0, key);

            int randPosition1 = random.Next(key.Length);
            int randPosition2 = random.Next(key.Length);

            result.Insert(randPosition1, key[randPosition1]);
            result.Insert(randPosition2, key[randPosition2]);

            Key = result.ToString();
            return Key;
        }

    }
}
