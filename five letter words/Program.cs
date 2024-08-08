using System;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Collections.Concurrent;

DateTime startTime = DateTime.Now;

string fileLocation = "C:\\Users\\HFGF\\Downloads\\final.txt";

List<int> bitArraysToRead = new List<int>();

(bitArraysToRead, int indexNoLongerUsed) = GetString(fileLocation);

int totalPossibilities = CalculateTotalPossibilities(bitArraysToRead, indexNoLongerUsed);

Console.WriteLine("\nTotal possibilities: " + totalPossibilities);

GetTimeSpan();





static int CalculateTotalPossibilities(List<int> bitArraysToRead, int endIndex)
{
    var results = new ConcurrentBag<int>();

    // Use Parallel.For to parallelize the calls to GetNumberOfPossibilities
    Parallel.For(0, endIndex, startWord =>
    {
        int possibilities = GetNumberOfPossibilities(bitArraysToRead, startWord);
        if (possibilities > 0)
        {
            results.Add(possibilities);
        }
    });

    return results.Sum();
}

static int GetNumberOfPossibilities(List<int> bitArraysToRead, int startWord)
{
    int wordAmount = bitArraysToRead.Count;
    int result = 0;

    Console.WriteLine("start index:" + startWord);

    for (int second = startWord + 1; second < wordAmount; second++)
    {
        if ((bitArraysToRead[startWord] & bitArraysToRead[second]) != 0) continue;

        for (int third = second + 1; third < wordAmount; third++)
        {
            if (((bitArraysToRead[startWord] | bitArraysToRead[second]) & bitArraysToRead[third]) != 0) continue;

            for (int fourth = third + 1; fourth < wordAmount; fourth++)
            {
                if (((bitArraysToRead[startWord] | bitArraysToRead[second] | bitArraysToRead[third]) & bitArraysToRead[fourth]) != 0) continue;

                for (int fifth = fourth + 1; fifth < wordAmount; fifth++)
                {
                    if (((bitArraysToRead[startWord] | bitArraysToRead[second] | bitArraysToRead[third] | bitArraysToRead[fourth]) & bitArraysToRead[fifth]) == 0)
                    {
                        result++;
                        Console.WriteLine(result);
                    }
                }
            }
        }
    }

    return result;
}




(List<int>, int) GetString(string fileLocatione)
{
    var file = fileLocatione;
    List<string> validLines = new List<string>();
    List<int> bitArrays = new List<int>();
    Dictionary<char, int> charFrequency = new Dictionary<char, int>();

    var logFile = File.ReadAllLines(fileLocatione);
    List<string> validWords = new List<string>();
    List<int> validBits = new List<int>();


    foreach (var word in logFile)
    {
        if (word.Length != 5) continue;
        if (word.Distinct().Count() != 5) continue;
        foreach (var c in word)
        {
            if (!validBits.Contains(StringToBitArray(word)))
            {
                validBits.Add(StringToBitArray(word));
                validWords.Add(word);
            }
            
            if (charFrequency.ContainsKey(c))
            {
                charFrequency[c]++;
            }
            else
            {
                charFrequency[c] = 1;
            }
        }
    }

    validLines = validWords.OrderBy(word => word.Sum(c => charFrequency[c])).ToList();

    int indexNoLongerInUse = LeastUsedLetter(charFrequency);

    foreach (var word in validLines)
    {
        bitArrays.Add(StringToBitArray(word));
    }


    Console.WriteLine("Amount of valid words: " + bitArrays.Count());

    return (bitArrays, indexNoLongerInUse);
    
}


void GetTimeSpan()
{
    DateTime endTime = DateTime.Now;

    TimeSpan diff = (endTime - startTime).Duration();

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

    //Console.WriteLine("{0} - {1}", input, bitArray.ToString("B"));

    return bitArray;
}


int LeastUsedLetter(Dictionary<char, int> charFrequency)
{
    var sortedCharFrequency = charFrequency.OrderBy(kv => kv.Value);

    // Step 3: Get the two least used letters and their counts
    var leastUsedLetters = sortedCharFrequency.Take(2).ToArray();
    var firstLeastUsedLetter = leastUsedLetters[0];
    var secondLeastUsedLetter = leastUsedLetters[1];

    // Step 4: Calculate the index at which the two least used letters are no longer used
    int indexNoLongerInUse = firstLeastUsedLetter.Value + secondLeastUsedLetter.Value + 1;


    return indexNoLongerInUse;
}





