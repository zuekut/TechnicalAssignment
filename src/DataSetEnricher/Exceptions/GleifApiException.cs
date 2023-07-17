namespace CardanoAssignment.Exceptions;
public class GleifApiException : Exception
{
    public GleifApiException(string message, Exception innerException) : base(message, innerException)
    {

    }
    public GleifApiException(string message) : base(message)
    {

    }

    public GleifApiException()
    {
    }
}
