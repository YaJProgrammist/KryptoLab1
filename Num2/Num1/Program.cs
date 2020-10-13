using System;
using System.IO;

namespace Num1
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../../../Try.txt");
            StreamWriter sw = new StreamWriter("../../../../../Out.txt");
            string str = sr.ReadToEnd();

            int maxCounter = 0;
            int maxSmech = 0;

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

                sw.WriteLine("{0}; {1}", smech, counter);
                //Console.WriteLine("smech = {0}, counter = {1}", smech, counter);
                if (counter > maxCounter)
                {
                    maxCounter = counter;
                    maxSmech = smech;
                }
            }

            //Console.WriteLine("max smech = {0}, max counter = {1}", maxSmech, maxCounter);
        }
    }
}
