using CardanoAssignment.Models;

namespace CardanoAssignment.Repositories;

public interface IGleifRepository
{
    public Task<GleifRecord?> GetLeiRecordByLeiNumber(string lei);
}