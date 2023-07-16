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
    public string LegalName { get; set; }
    public string Bic { get; set; }
    public double TransactionCost { get; set; }
    public GleifRecord? GleifRecord { get; set; }
}

public sealed class InputDataSetMap : ClassMap<LeiRecord>
{
    public InputDataSetMap()
    {
        Map(m => m.TransactionUti).Name("transaction_uti");
        Map(m => m.Isin).Name("isin");
        Map(m => m.Notional).Name("notional");
        Map(m => m.NotionalCurrency).Name("notional_currency");
        Map(m => m.TransactionType).Name("transaction_type");
        //ToDo Fix DateTime format to be like input_dataset
        Map(m => m.TransactionDateTime).Name("transaction_datetime");
        Map(m => m.Rate).Name("rate");
        Map(m => m.Lei).Name("lei");
        Map(m => m.LegalName).Name("legalName").Optional();
        Map(m => m.Bic).Name("bic").Optional();
        Map(m => m.TransactionCost).Name("transaction_cost").Optional();
    }
}