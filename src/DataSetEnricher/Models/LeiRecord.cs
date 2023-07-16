using CsvHelper.Configuration;

namespace CardanoAssignment.Models;

public class LeiRecord
{
    public string TransactionUti { get; set; }
    public string Isin { get; set; }
    public double Notional { get; set; }
    public string NotionalCurrency { get; set; }
    public string TransactionType { get; set; }
    public DateTime TransactionDateTime { get; set; }
    public double Rate { get; set; }
    public string Lei { get; set; }
    public GleifRecord? GleifRecord { get; set; } 
    public Dictionary<string, object> AdditionalEnrichedData { get; set; }
}

public sealed class CsvDataSetMap : ClassMap<LeiRecord>
{
    public CsvDataSetMap()
    {
        //ToDo map gleifrecord legalName and bic
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