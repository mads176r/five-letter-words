using System;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

DateTime startTime = DateTime.Now;

string fileLocation = "C:\\Users\\HFGF\\Downloads\\final.txt";

//List<string> stringToRead = new List<string>();
//stringToRead = GetString(fileLocation);

List<int> bitArraysToRead = new List<int>();
bitArraysToRead = GetString(fileLocation);

int totalPossibilities = await Begin(bitArraysToRead);

Console.WriteLine(totalPossibilities);

GetTimeSpan();



static async Task<int> Begin(List<int> bitArraysToRead)
{
    const int maxConcurrentTasks = 12; // Limit to 5 concurrent tasks
    var semaphore = new SemaphoreSlim(maxConcurrentTasks);

    var tasks = new List<Task<int>>();

    // Create tasks for different starting points
    for (int i = 0; i < bitArraysToRead.Count; i++)
    {
        await semaphore.WaitAsync();

        var task = Task.Run(async () =>
        {
            try
            {
                //Console.WriteLine("Task started: " + i);
                // Call your function with different start points
                return GetNumberOfPossibilities(bitArraysToRead, i);
            }
            finally
            {
                semaphore.Release();
            }
        });

        tasks.Add(task);
    }

    // Wait for all tasks to complete and collect results
    var results = await Task.WhenAll(tasks);

    // Aggregate results
    int total = results.Sum();
    return total;
}


static int GetNumberOfPossibilities(List<int> bitArraysToRead, int startWord)
{
    int wordAmount = bitArraysToRead.Count;
    int result = 0;

    
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




List<int> GetString(string fileLocatione)
{
    var file = fileLocatione;
    //List<string> validLines = new List<string>();
    List<int> bitArrays = new List<int>();

    using (System.IO.StreamReader sr = new System.IO.StreamReader(file))
    {
        string line;
        
        while ((line = sr.ReadLine()) != null)
        {
            if (line.Length != 5) continue;
            if (line.Distinct().Count() != 5) continue;

            //validLines.Add(line);
            bitArrays.Add(StringToBitArray(line));
            
        }
    }

    Console.WriteLine("Amount of valid words: " + bitArrays.Count());
    return bitArrays;
    
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
