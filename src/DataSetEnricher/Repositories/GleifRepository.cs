﻿using CardanoAssignment.Exceptions;
using CardanoAssignment.Models;

namespace CardanoAssignment.Repositories;

public class GleifRepository : IGleifRepository
{
    private readonly HttpClient _gleiClient;

    public GleifRepository(IHttpClientFactory clientFactory)
    {
        _gleiClient = clientFactory.CreateClient("Glei");

    }
    public async Task<GleifRecord?> GetLeiRecordByLeiNumber(string lei)
    {
        var leiRecordByLeiNumber = await _gleiClient.GetFromJsonAsync<GleifRecord>($"api/v1/lei-records?filter[lei]={lei}");
        return leiRecordByLeiNumber ?? throw new LeiRecordNotExistException(lei);
    }
}