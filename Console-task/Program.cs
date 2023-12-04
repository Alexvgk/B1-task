
using Console_Test.Conbination;
using Console_Test.Services;


IConbinator conbinator = new TxtConbinator();
ParsingService parsingService = new ParsingService();
while (true)
{
    Console.WriteLine("Choose an action:");
    Console.WriteLine("1. Combine all files");
    Console.WriteLine("2. Combine a specific number of files");
    Console.WriteLine("3. Exit the program");

    // Read user's choice
    string ? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "files");
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "combinedFile.txt");
            Console.WriteLine("Delete by fragment:");
            string? fragment = Console.ReadLine();
            int deletedLinesCount;
            deletedLinesCount = String.IsNullOrEmpty(fragment)
                ? conbinator.combineAllFiles(folderPath, outputPath)
                : conbinator.combineAllFiles(folderPath, outputPath, fragment);
            Console.WriteLine($"Succes. Lines deleted: {deletedLinesCount}");
            break;
        case "2":
            folderPath = Path.Combine(Directory.GetCurrentDirectory(), "files");
            string[] files = Directory.GetFiles(folderPath);
            Console.WriteLine("List of available files:");
            for (int i = 0; i < files.Length; i = i + 2)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}\t" + $"{i + 2}. {Path.GetFileName(files[i+1])}");
            }
            Console.Write("Enter the numbers of files to combine (1,2,3,4...): ");
            string ? input = Console.ReadLine();
            if (parsingService.tryParseFileNumbers(input, files.Length, out int[] selectedFileNumbers))
            {
                outputPath = Path.Combine(Directory.GetCurrentDirectory(), $"combined({input}).txt");
                Console.WriteLine("Delete by fragment:");
                fragment = Console.ReadLine();
                deletedLinesCount = String.IsNullOrEmpty(fragment)
               ? conbinator.combineSpecificNumberOfFiles(folderPath, outputPath,selectedFileNumbers)
               : conbinator.combineSpecificNumberOfFiles(folderPath, outputPath, selectedFileNumbers, fragment);
                Console.WriteLine($"Succes. Lines deleted: {deletedLinesCount}");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter valid numbers.");
            }

            break;
        case "3":
            Environment.Exit(0); // Exit the program
            break;
        default:
            Console.WriteLine("Invalid input. Please choose an action again.");
            break;
    }
}
