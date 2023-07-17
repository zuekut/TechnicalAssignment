namespace CardanoAssignment.Exceptions;

public class LeiDataEnrichmentException : Exception
{
    public LeiDataEnrichmentException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public LeiDataEnrichmentException(string message) : base(message)
    {
    }

    public LeiDataEnrichmentException()
    {
    }
}