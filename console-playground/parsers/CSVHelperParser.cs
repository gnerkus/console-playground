using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Main.parsers
{
    public class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    public static class CSVHelperParser
    {
        public static IEnumerable<Foo> SimpleParse(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<Foo>();
                return records.ToList();
            }
        }
    }
}