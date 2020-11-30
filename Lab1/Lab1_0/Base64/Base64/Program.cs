using System;
using System.IO;

namespace Base64
{
    class Program
    {
        static string BASE_64_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

        static string GetBinaryStringFromChar(char ch, int strLength)
        {
            int chInt = BASE_64_ALPHABET.IndexOf(ch);

            string res = string.Empty;

            for (int i = 0; i < strLength; i++)
            {
                res = chInt % 2 + res;
                chInt = chInt / 2;
            }

            return res;
        }

        static char GetCharFromBinaryString(string str)
        {
            int res = 0;
            int factor = 1;

            for (int i = str.Length - 1; i >= 0; i--)
            {
                res += (str[i] - '0') * factor;
                factor *= 2;
            }

            return (char)res;
        }

        static string DecodeBase64(string inpStr)
        {
            string binaryStr = string.Empty;

            for (int i = 0; i < inpStr.Length; i++)
            {
                binaryStr += GetBinaryStringFromChar(inpStr[i], 6);
            }

            string outStr = string.Empty;
            for (int i = 0; i < binaryStr.Length; i += 8)
            {
                if (binaryStr.Length - i < 8)
                {
                    break;
                }
                outStr += GetCharFromBinaryString(binaryStr.Substring(i, 8));
            }

            return outStr;
        }

        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../../In.txt");
            StreamWriter sw = new StreamWriter("../../../../Out.txt");

            while (!sr.EndOfStream)
            {
                string inpStr = sr.ReadLine();
                inpStr = inpStr.TrimEnd('=');
                sw.WriteLine(DecodeBase64(DecodeBase64(inpStr)));
            }

            sr.Close();
            sw.Close();
        }
    }
}
