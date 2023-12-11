
using Console_Test.Conbination;
using Console_Test.Generation;
using Console_Test.Model;
using Console_Test.Services.Data;
using Console_Test.Services.Data.DataServices;
using Console_Test.Services.Data.Repository;
using Console_Test.Services.Parsing;
using System.Data.SqlClient;

string filesFolderPath = @"files\";

// Absolute path to the files folder
string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
string absoluteFilesFolderPath = System.IO.Path.Combine(currentDirectory, filesFolderPath);

// Check if the files folder exists, if not, create it
if (!Directory.Exists(absoluteFilesFolderPath))
{
    Directory.CreateDirectory(absoluteFilesFolderPath);

    // Generate files in the folder (Add your file generation logic here)
    IGenerator gen = new TxtGenerator();
    gen.generateFile();
}
IConbinator conbinator = new TxtConbinator();
while (true)
{
    Console.WriteLine("");
    Console.WriteLine("Choose an action:");
    Console.WriteLine("1. Combine all files");
    Console.WriteLine("2. Combine a specific number of files");
    Console.WriteLine("3. Enter data into the database");
    Console.WriteLine("4. Sum and median");
    Console.WriteLine("5. Exit the program");

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
            try
            {
                folderPath = Path.Combine(Directory.GetCurrentDirectory(), "files");
                string[] files = Directory.GetFiles(folderPath);
                Console.WriteLine("List of available files:");
                for (int i = 0; i < files.Length; i = i + 2)
                {
                    Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}\t" + $"{i + 2}. {Path.GetFileName(files[i + 1])}");
                }
                Console.Write("Enter the numbers of files to combine (1,2,3,4...): ");
                string? input = Console.ReadLine();
                if (ParsingService.tryParseFileNumbers(input, files.Length, out int[] selectedFileNumbers))
                {
                    outputPath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "conbine"), $"combined({input}).txt");
                    Console.WriteLine("Delete by fragment:");
                    fragment = Console.ReadLine();
                    deletedLinesCount = String.IsNullOrEmpty(fragment)
                   ? conbinator.combineSpecificNumberOfFiles(folderPath, outputPath, selectedFileNumbers)
                   : conbinator.combineSpecificNumberOfFiles(folderPath, outputPath, selectedFileNumbers, fragment);
                    Console.WriteLine($"Succes. Lines deleted: {deletedLinesCount}");
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter valid numbers.");
                }

                break;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Error. Check the folder conbine");
                break;
            }


        case "3":
            try
            {

                Console.WriteLine("Enter file path(file\\\\file1.txt)");
                string? input = Console.ReadLine();
                List<DataModel> data = ParsingService.ParseFile(Path.Combine(Directory.GetCurrentDirectory(), input));
                DataService service = new DataService();
                service.insertData(data);

                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid data format in file");
                break;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("No such file");
                break;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("No such directory");
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                break;
            }
        case "4":
            try
            {
                DataService dataService = new DataService();
                long sum;
                double median;
                dataService.calculateSumAndMedian(out sum, out median);
                Console.WriteLine($"Sum of integers: {sum}");
                Console.WriteLine($"Median: {median}");
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                break;
            }
        case "5":
            Environment.Exit(0); // Exit the program
            break;
        default:
            Console.WriteLine("Invalid input. Please choose an action again.");
            break;
    }
}
