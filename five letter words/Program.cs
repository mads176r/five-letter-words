using System;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

DateTime startTime = DateTime.Now;

string fileLocation = "C:\\Users\\HFGF\\Downloads\\final.txt";

//List<string> stringToRead = new List<string>();
//stringToRead = GetString(fileLocation);

List<int> bitArraysToRead = new List<int>();
bitArraysToRead = GetString(fileLocation);


int finlaNumber = Begin(bitArraysToRead);

Console.WriteLine(finlaNumber);

GetTimeSpan();






int Begin(List<int> bitArraysToRead)
{
    int totalThreads = 5;

    Task<int>[] tasks = new Task<int>[totalThreads];
    int segmentSize = bitArraysToRead.Count / totalThreads;

    for (int i = 0; i < totalThreads; i++)
    {
        int start = i * segmentSize;
        int end = (i == totalThreads - 1) ? bitArraysToRead.Count : start + segmentSize;
        tasks[i] = Task.Run(() => GetNumberOfPossibilities(bitArraysToRead, start, end));
    }

    Task.WaitAll(tasks);
    int result = tasks.Sum(task => task.Result);
    


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
                            //List<int> tempBitList = new List<int>();
                            //tempBitList.Add(bitArraysToRead[first]);
                            //tempBitList.Add(bitArraysToRead[second]);
                            //tempBitList.Add(bitArraysToRead[third]);
                            //tempBitList.Add(bitArraysToRead[fourth]);
                            //tempBitList.Add(bitArraysToRead[fifth]);

                            //string bitTranslation = BitArrayListToString(tempBitList);

                            //Console.WriteLine(bitTranslation);

                            //bitTranslation = "";

                            //GetTimeSpan();

                            result++;
                            Console.WriteLine(result);
                        }
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
