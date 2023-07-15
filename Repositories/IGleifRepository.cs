using CardanoAssignment.Models;

namespace CardanoAssignment.Repositories;

public interface IGleifRepository
{
    public Task<LeiRecord> GetLeiRecordByLeiNumber(string lei);
}