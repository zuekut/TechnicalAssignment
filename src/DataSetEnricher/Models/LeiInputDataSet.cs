using System.Text.Json.Serialization;

namespace CardanoAssignment.Models;

public class LeiInputDataSet
{
    [JsonPropertyName("transaction_uti")]
    public string TransactionUti { get; set; }
    [JsonPropertyName("isin")]
    public string Isin { get; set; }
    [JsonPropertyName("notional")]
    public long Notional { get; set; }
    [JsonPropertyName("notional_currency")]
    public string NotionalCurrency { get; set; }
    [JsonPropertyName("transaction_type")]
    public string TransactionType { get; set; }
    [JsonPropertyName("transaction_datetime")]
    public DateTime TransactionDateTime { get; set; }
    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }
    [JsonPropertyName("Lei")]
    public string Lei { get; set; }
}