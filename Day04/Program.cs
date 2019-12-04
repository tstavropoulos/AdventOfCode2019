using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day04
{
    class Program
    {
        private const int lowerBound = 108457;
        private const int upperBound = 562041;

        private static Dictionary<char, int> matchingDigits = new Dictionary<char, int>();

        static void Main(string[] args)
        {
            Console.WriteLine("Day 4 - Secure Container");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            int count = 0;

            for (int i = lowerBound; i <= upperBound; i++)
            {
                string value = i.ToString();
                if (SatisfiesRulesA(value) && SatisfiesRulesB(value))
                {
                    count++;
                }
            }

            Console.WriteLine($"The number of matching passwords is: {count}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();


            count = 0;

            for (int i = lowerBound; i <= upperBound; i++)
            {
                string value = i.ToString();
                if (SatisfiesRulesB(value) && SatisfiesRulesC(value))
                {
                    count++;
                }
            }

            Console.WriteLine($"The new number of matching passwords is: {count}");

            Console.WriteLine();
            Console.ReadKey();
        }


        private static bool SatisfiesRulesA(string value)
        {
            for (int i = 0; i < value.Length - 1; i++)
            {
                if (value[i] == value[i+1])
                {
                    return true;
                }
            }

            return false;
        }

        private static bool SatisfiesRulesB(string value)
        {
            for (int i = 0; i < value.Length - 1; i++)
            {
                if (value[i] > value[i + 1])
                {
                    return false;
                }
            }

            return true;
        }

        private static bool SatisfiesRulesC(string value)
        {
            matchingDigits.Clear();
            for (int i = 0; i < value.Length - 1; i++)
            {
                if (value[i] == value[i + 1])
                {
                    if (matchingDigits.ContainsKey(value[i]))
                    {
                        matchingDigits[value[i]]++;
                    }
                    else
                    {
                        matchingDigits[value[i]] = 1;
                    }
                }
            }

            return matchingDigits.Values.Where(x=>x==1).Count() > 0;
        }
    }
}
