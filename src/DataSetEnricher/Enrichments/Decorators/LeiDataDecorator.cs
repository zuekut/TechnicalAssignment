using CardanoAssignment.Models;

namespace CardanoAssignment.Enrichments.Decorators;

public abstract class LeiDataDecorator : ILeiData
{
    protected ILeiData _decoratedData;

    protected LeiDataDecorator(ILeiData decoratedData)
    {
        _decoratedData = decoratedData;
    }
    public abstract List<LeiRecord> GetData();
}