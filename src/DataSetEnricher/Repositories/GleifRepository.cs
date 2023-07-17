using CardanoAssignment.Exceptions;
using CardanoAssignment.Models;

namespace CardanoAssignment.Repositories;

public class GleifRepository : IGleifRepository
{
    private readonly ILogger<GleifRepository> _logger;
    private readonly HttpClient _gleiClient;

    public GleifRepository(ILogger<GleifRepository> logger, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _gleiClient = clientFactory.CreateClient("Glei");
    }
    public async Task<GleifRecord?> GetLeiRecordByLeiNumber(string lei)
    {
        GleifRecord? leiRecordByLeiNumber;
        try
        {
            leiRecordByLeiNumber = await _gleiClient.GetFromJsonAsync<GleifRecord>($"api/v1/lei-records?filter[lei]={lei}");
        }
        catch (Exception exception)
        {
           _logger.LogDebug($"An error occured fetching lei record with id: {lei} from Gleif endpoint"); 
            throw new GleifApiException($"An error occured fetching lei record from Gleif endpoint: {exception.Message}", exception.InnerException);
        }
        return leiRecordByLeiNumber ?? throw new LeiRecordNotExistException(lei);
    }
}