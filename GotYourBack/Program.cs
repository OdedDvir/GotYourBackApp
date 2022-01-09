
using GotYourBackLibrary.DataAccess;
using GotYourBackLibrary.Models;

const string MonitoredFilesListFile = "Monitored.csv";
CSVConnector csvConnector = new CSVConnector();

// Gets current folder
static string GetCurrentFolder()
{
    return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
}

List<MonitoredFileModel> filesList = new List<MonitoredFileModel>();
string fullPath = $"{GetCurrentFolder()}\\{MonitoredFilesListFile}";
int filesCount = 0;
int filesChanged = 0;

// Load List of monitored files
filesList = csvConnector.LoadFileToList(fullPath);
Console.WriteLine($"Loaded list from {fullPath}");

// Check for file changes by comparing file's last modified date to the date in list
foreach (var file in filesList)
{
    filesCount++;
    Console.Write($"File Path: {file.FilePath}, File Date: {file.FileDate}");
    DateTime actualDate = File.GetLastWriteTime(file.FilePath);
    if (file.FileDate != actualDate) // File was modified
    {
        filesChanged++;
        Console.WriteLine($" * CHANGED *");
        Console.WriteLine($"  Actual file date is: {actualDate}");
        Console.WriteLine($"  Date Difference: {file.FileDate - actualDate}");
        // TODO: Create a new backup of th file
        Console.WriteLine($"  Creating Backup...");
        file.FileDate = actualDate;
    }
    else
    {
        Console.WriteLine($" * NOT CHANGED *");
    }
}
if (filesChanged > 0)
{
    Console.WriteLine($"{filesChanged} of {filesCount} file(s) were changed, updating file list...");
    csvConnector.SaveListToFile(filesList, fullPath);
    Console.WriteLine($"File list saved.");
} else
{
    Console.WriteLine($"No files were changed.");
}

//Console.ReadKey();