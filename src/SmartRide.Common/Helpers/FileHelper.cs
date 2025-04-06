using System.Text;

namespace SmartRide.Common.Helpers;

public static class FileHelper
{
    public static void WriteToCsv<T>(IEnumerable<T> records, string filePath)
    {
        using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
        var properties = typeof(T).GetProperties();

        // Write header
        writer.WriteLine(string.Join(",", properties.Select(p => p.Name)));

        // Write records
        foreach (var record in records)
        {
            var values = properties.Select(p => p.GetValue(record)?.ToString()?.Replace(",", " ") ?? string.Empty);
            writer.WriteLine(string.Join(",", values));
        }
    }

    public static List<T> ReadFromCsv<T>(string filePath) where T : new()
    {
        var records = new List<T>();
        var properties = typeof(T).GetProperties();

        using var reader = new StreamReader(filePath, Encoding.UTF8);
        var header = reader.ReadLine()?.Split(',');

        if (header == null)
            throw new InvalidOperationException("CSV file is empty or invalid.");

        var propertyMap = header
            .Select((name, index) => new { Name = name, Index = index })
            .Where(h => properties.Any(p => p.Name == h.Name))
            .ToDictionary(h => h.Name, h => h.Index);

        while (!reader.EndOfStream)
        {
            var values = reader.ReadLine()?.Split(',');
            if (values == null) continue;

            var record = new T();
            foreach (var property in properties)
            {
                if (propertyMap.TryGetValue(property.Name, out var index) && index < values.Length)
                {
                    var value = values[index];
                    var convertedValue = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(record, convertedValue);
                }
            }

            records.Add(record);
        }

        return records;
    }
}
