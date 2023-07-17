using System.Net;

namespace CardanoAssignment.Exceptions;

public class CsvConversionException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
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