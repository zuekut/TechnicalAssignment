using System.Globalization;
using CardanoAssignment.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace CardanoAssignment.Convertors;

public class CsvToDataModelConvertor : ICsvToDataModelConvertor
{
    public List<CsvDataSet> ConvertCsv(Stream csvStream)
    {
        using (var reader = new StreamReader(csvStream))
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true, 
            };

            using (var csv = new CsvReader(reader, csvConfig))
            {
                csv.Context.RegisterClassMap<CsvDataSetMap>();
                return csv.GetRecords<CsvDataSet>().ToList();
            }
        }
    }
}