using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

DateTime startTime = DateTime.Now;

string fileLocation = "C:\\Users\\HFGF\\Downloads\\final.txt";

List<int> bitArraysToRead = GetBitArrays(fileLocation);

int finalNumber = Begin(bitArraysToRead);

Console.WriteLine(finalNumber);

PrintTimeSpan();

int Begin(List<int> bitArraysToRead)
{
    int totalThreads = 5;
    int segmentSize = bitArraysToRead.Count / totalThreads;
    int result = 0;

    Parallel.For(0, totalThreads, i =>
    {
        int start = i * segmentSize;
        int end = (i == totalThreads - 1) ? bitArraysToRead.Count : start + segmentSize;
        result += GetNumberOfPossibilities(bitArraysToRead, start, end);
    });

    return result;
}

int GetNumberOfPossibilities(List<int> bitArraysToRead, int start, int end)
{
    int wordAmount = bitArraysToRead.Count;
    int result = 0;

    for (int first = start; first < end; first++)
    {
        for (int second = first + 1; second < wordAmount; second++)
        {
            if ((bitArraysToRead[first] & bitArraysToRead[second]) != 0) continue;

            for (int third = second + 1; third < wordAmount; third++)
            {
                if (((bitArraysToRead[first] | bitArraysToRead[second]) & bitArraysToRead[third]) != 0) continue;

                for (int fourth = third + 1; fourth < wordAmount; fourth++)
                {
                    if (((bitArraysToRead[first] | bitArraysToRead[second] | bitArraysToRead[third]) & bitArraysToRead[fourth]) != 0) continue;

                    for (int fifth = fourth + 1; fifth < wordAmount; fifth++)
                    {
                        if (((bitArraysToRead[first] | bitArraysToRead[second] | bitArraysToRead[third] | bitArraysToRead[fourth]) & bitArraysToRead[fifth]) == 0)
                        {
                            // Uncomment this section to use the bit list and translation features
                            /*
                            List<int> tempBitList = new List<int>();
                            tempBitList.Add(bitArraysToRead[first]);
                            tempBitList.Add(bitArraysToRead[second]);
                            tempBitList.Add(bitArraysToRead[third]);
                            tempBitList.Add(bitArraysToRead[fourth]);
                            tempBitList.Add(bitArraysToRead[fifth]);

                            string bitTranslation = BitArrayListToString(tempBitList);
                            Console.WriteLine(bitTranslation);

                            bitTranslation = "";
                            PrintTimeSpan();
                            */

                            result++;
                        }
                    }
                }
            }
        }
    }

    return result;
}

List<int> GetBitArrays(string fileLocation)
{
    List<int> bitArrays = new List<int>();

    using (StreamReader sr = new StreamReader(fileLocation))
    {
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            if (line.Length == 5 && line.Distinct().Count() == 5)
            {
                bitArrays.Add(StringToBitArray(line));
            }
        }
    }

    Console.WriteLine("Amount of valid words: " + bitArrays.Count);
    return bitArrays;
}

void PrintTimeSpan()
{
    TimeSpan diff = DateTime.Now - startTime;
    Console.WriteLine(diff);
}

static int StringToBitArray(string input)
{
    int bitArray = 0;
    foreach (char c in input.ToLower())
    {
        if (c >= 'a' && c <= 'z')
        {
            int position = c - 'a';
            bitArray |= (1 << position);
        }
    }
    return bitArray;
}

string BitArrayListToString(List<int> bitArrayList)
{
    List<string> strings = new List<string>();

    foreach (int bitArray in bitArrayList)
    {
        List<char> chars = new List<char>();

        for (int i = 0; i < 26; i++)
        {
            if ((bitArray & (1 << i)) != 0)
            {
                chars.Add((char)('a' + i));
            }
        }

        strings.Add(new string(chars.ToArray()));
    }

    return string.Join(" ", strings);
}
