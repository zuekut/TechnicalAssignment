namespace CardanoAssignment.Exceptions;

public class LeiRecordNotExistException : GleifApiException
{
    public LeiRecordNotExistException(string lei) : base($"Record with provided lei - {lei} does not exist.")
    {

    }
}