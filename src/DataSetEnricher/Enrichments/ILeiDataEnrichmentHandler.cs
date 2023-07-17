using CardanoAssignment.Models;

namespace CardanoAssignment.Enrichments;

public interface ILeiDataEnrichmentHandler
{
    List<LeiRecord> EnrichData(List<LeiRecord> csvDataSet); 
}