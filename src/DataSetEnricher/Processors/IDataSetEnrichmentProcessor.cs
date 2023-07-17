using CardanoAssignment.Models;

namespace CardanoAssignment.Processors;

public interface IDataSetEnrichmentProcessor
{
    string ProcessDataSet(List<LeiRecord> csvDataSet);
}