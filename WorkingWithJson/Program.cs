using System.Text.Json; // JsonSerializer
using System.Text.Json.Serialization; // [JsonInclude]
using WorkingWithJson;

using static System.Console;
using static System.Environment;
using static System.IO.Path;

Book slaughterhouseFive = new(title:
  "Slaughterhouse-Five")
{
    Author = "Kurt Vonnegut",
    PublishDate = new(year: 1991, month: 12, day: 9),
    Pages = 215,
    Created = DateTimeOffset.UtcNow,
};

JsonSerializerOptions options = new()
{
    IncludeFields = true, // includes all fields
    PropertyNameCaseInsensitive = true,
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
};

options.Converters.Add(new DateOnlyConverter());
options.Converters.Add(new DateOnlyNullableConverter());

string filePath = Combine(CurrentDirectory, "book.json");

using (Stream fileStream = File.Create(filePath))
{
    JsonSerializer.Serialize<Book>(
      utf8Json: fileStream, value: slaughterhouseFive, options);
}

WriteLine("Written {0:N0} bytes of JSON to {1}",
  arg0: new FileInfo(filePath).Length,
  arg1: filePath);

WriteLine();

// Display the serialized object graph 
WriteLine(File.ReadAllText(filePath));

public class Book
{
    public Book(string title)
    {
        Title = title;
    }

    // properties

    public string Title { get; set; }
    public string? Author { get; set; }

    // fields

    [JsonInclude] // include this field
    public DateOnly PublishDate;

    [JsonInclude] // include this field
    public DateTimeOffset Created;

    public ushort Pages;
}

