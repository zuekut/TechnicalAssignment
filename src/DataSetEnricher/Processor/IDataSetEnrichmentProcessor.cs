using CardanoAssignment.Models;

namespace CardanoAssignment.Processor;

public interface IDataSetEnrichmentProcessor
{
    string ProcessDataSet(List<LeiRecord> csvDataSet);
}