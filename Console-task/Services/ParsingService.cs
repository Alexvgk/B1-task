using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Test.Services
{
    public class ParsingService
    {

        public bool tryParseFileNumbers(string input, int maxNumber, out int[] numbers)
        {
            // Split the input string into individual parts using commas as separators
            string[] parts = input.Split(',');

            // Initialize the output array to store parsed numbers
            numbers = new int[parts.Length];

            // Iterate through each part of the input
            for (int i = 0; i < parts.Length; i++)
            {
                // Try to parse each part into an integer
                if (int.TryParse(parts[i], out int number) && number >= 1 && number <= maxNumber)
                {
                    numbers[i] = number;
                }
                else
                {
                    return false;
                }
            }

            // Return true if all parts are successfully parsed
            return true;
        }
    }
}
