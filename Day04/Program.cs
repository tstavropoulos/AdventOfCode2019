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

        static void Main(string[] args)
        {
            Console.WriteLine("Day 4 - Secure Container");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            int countA = 0;
            int countB = 0;

            for (int i = lowerBound; i <= upperBound; i++)
            {
                string value = i.ToString();
                if (SatisfiesRulesB(value))
                {
                    if (SatisfiesRulesA(value))
                    {
                        countA++;

                        if (SatisfiesRulesC(value))
                        {
                            countB++;
                        }
                    }
                }
            }

            Console.WriteLine($"The number of matching passwords is: {countA}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            Console.WriteLine($"The new number of matching passwords is: {countB}");

            Console.WriteLine();
            Console.ReadKey();
        }

        private static bool SatisfiesRulesA(string value)
        {
            for (int i = 0; i < value.Length - 1; i++)
            {
                if (value[i] == value[i + 1])
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
            int count = 1;
            for (int i = 1; i < value.Length; i++)
            {
                if (value[i] == value[i-1])
                {
                    count++;
                }
                else
                {
                    if (count == 2)
                    {
                        return true;
                    }

                    count = 1;
                }
            }

            return count == 2;
        }
    }
}
