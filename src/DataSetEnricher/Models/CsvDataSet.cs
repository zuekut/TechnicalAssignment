using CsvHelper.Configuration;

namespace CardanoAssignment.Models;

public class CsvDataSet
{
    public string TransactionUti { get; set; }
    public string Isin { get; set; }
    public double Notional { get; set; }
    public string NotionalCurrency { get; set; }
    public string TransactionType { get; set; }
    public DateTime TransactionDateTime { get; set; }
    public decimal Rate { get; set; }
    public string Lei { get; set; }
}

public sealed class CsvDataSetMap : ClassMap<CsvDataSet>
{
    public CsvDataSetMap()
    {
        Map(m => m.TransactionUti).Name("transaction_uti");
        Map(m => m.Isin).Name("isin");
        Map(m => m.Notional).Name("notional");
        Map(m => m.NotionalCurrency).Name("notional_currency");
        Map(m => m.TransactionType).Name("transaction_type");
        Map(m => m.TransactionDateTime).Name("transaction_datetime");
        Map(m => m.Rate).Name("rate");
        Map(m => m.Lei).Name("lei");
    }
}