namespace CardanoAssignment.Enrichments;

public class LeiDecoratorBuilder
{
    private ILeiData _leiData;

    public LeiDecoratorBuilder(ILeiData leiData)
    {
        _leiData = leiData;
    }

    public LeiDecoratorBuilder WithDecorator(Func<ILeiData, ILeiData> decoratorFunc)
    {
        _leiData = decoratorFunc(_leiData);
        return this;
    }

    public ILeiData Build()
    {
        return _leiData;
    }
}