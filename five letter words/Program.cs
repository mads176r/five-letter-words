using System;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Collections.Concurrent;
using five_letter_word_library;

DateTime startTime = DateTime.Now;

string fileLocation = "C:\\Users\\HFGF\\Downloads\\final.txt";

GetProduct GetProduct = new GetProduct();

List<int> bitArraysToRead = new List<int>();



(bitArraysToRead, int indexNoLongerUsed) = GetProduct.GetString(fileLocation);

int totalPossibilities = CalculatePossibilities.CalculateTotalPossibilities(bitArraysToRead, indexNoLongerUsed);

Console.WriteLine("\nTotal possibilities: " + totalPossibilities);

GetTimeSpan();






void GetTimeSpan()
{
    DateTime endTime = DateTime.Now;

    TimeSpan diff = (endTime - startTime).Duration();

    Console.WriteLine(diff);
}
