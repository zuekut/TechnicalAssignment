using CardanoAssignment.Models;

namespace CardanoAssignment.Repositories;

public interface IGleifRepository
{
    public Task<LeiMapping> GetLeiRecordByLeiNumber(string lei);
}