using System.IO;

namespace Lab1_5
{
    class Program
    {
        private static void CalculateKeyLength(string str)
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

        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../../Input.txt");
            StreamWriter sw = new StreamWriter("../../../../Out.txt");
            string input = sr.ReadToEnd();

            CalculateKeyLength(input); // turned out it is 4

            sr.Close();
            sw.Close();
        }
    }
}
