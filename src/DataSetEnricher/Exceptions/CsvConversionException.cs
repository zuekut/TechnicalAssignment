namespace CardanoAssignment.Exceptions;

public class CsvConversionException : Exception
{
    public CsvConversionException(string message, Exception innerException) : base(message, innerException)
    {

    }
    public CsvConversionException(string message) : base(message)
    {

    }

    public CsvConversionException()
    {
    }
}