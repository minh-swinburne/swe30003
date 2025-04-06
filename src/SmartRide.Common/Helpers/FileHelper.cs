using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace SmartRide.Common.Helpers;

public static class FileHelper
{
    public static void WriteToCsv<T>(IEnumerable<T> records, string filePath)
    {
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
        csv.WriteRecords(records);
    }

    public static List<T> ReadFromCsv<T>(string filePath) where T : class
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
        return csv.GetRecords<T>().ToList();
    }
}
