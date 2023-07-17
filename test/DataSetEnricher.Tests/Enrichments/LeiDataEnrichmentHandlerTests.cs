using System;
using System.Collections.Generic;
using System.Linq;
using CardanoAssignment.Enrichments;
using CardanoAssignment.Exceptions;
using CardanoAssignment.Models;
using CardanoAssignment.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using NFluent;
using Xunit;

namespace DataSetEnricher.Tests.Enrichments;

public class LeiDataEnrichmentHandlerTests
{
    private LeiDataEnrichmentHandler _leiDataEnrichmentHandler;
    private readonly Mock<IGleifRepository> _gleifRepository = new();
    private readonly Mock<ILogger<LeiDataEnrichmentHandler>> _logger = new();

    public LeiDataEnrichmentHandlerTests()
    {
        _leiDataEnrichmentHandler = new LeiDataEnrichmentHandler(_logger.Object, _gleifRepository.Object);
    }

    [Fact]
    public void GivenProperCsvDataSet_WhenEnrichData_ThenListOfEnrichedDataIsReturned()
    {
        //Arrange
        var mockData = CreateDefaultLeiRecordsMock();
        //Act
        var enrichedData = _leiDataEnrichmentHandler.EnrichData(mockData);
        //Assert
        Check.That(enrichedData.Count).IsEqualTo(1);
        var enrichedRecord = enrichedData.FirstOrDefault();
        Check.That(enrichedRecord.Bic).IsEqualTo("testbic");
        Check.That(enrichedRecord.LegalName).IsEqualTo("test");
        Check.That(enrichedRecord.GleifRecord).IsNotNull();
        Check.That(enrichedRecord.TransactionCost).IsEqualTo(-378793.02860000002);
    }
    
    [Fact]
    public void GivenProperCsvDataSetButFailingGleifResponse_WhenEnrichData_ThenExceptionThrown()
    {
        //Arrange
        var mockData = CreateDefaultLeiRecordsMock();
        _gleifRepository.Setup(gleifrepo => gleifrepo.GetLeiRecordByLeiNumber(It.IsAny<string>())).Throws<GleifApiException>();
        //Act and Assert
        Check.ThatCode(() => _leiDataEnrichmentHandler.EnrichData(mockData)).Throws<LeiDataEnrichmentException>();
    }
    
    [Fact]
    public void GivenEmptyCsvDataSet_WhenEnrichData_ThenExceptionThrown()
    {
        //Arrange
        var mockData = new List<LeiRecord>();
        //Act and Assert
        Check.ThatCode(() => _leiDataEnrichmentHandler.EnrichData(mockData)).Throws<LeiDataEnrichmentException>();
    }
    
    [Fact]
    public void GivenNullCsvDataSet_WhenEnrichData_ThenExceptionThrown()
    {
        //Act and Assert
        Check.ThatCode(() => _leiDataEnrichmentHandler.EnrichData(null)).Throws<LeiDataEnrichmentException>();
    }

    private List<LeiRecord> CreateDefaultLeiRecordsMock()
    {
        var csvDataSet = new List<LeiRecord>
        {
            new()
            {
                Lei = "XKZZ2JZF41MRHTR1V493",
                Rate = 0.0070956,
                Notional = 763000
            }
        };

        var mockGleifRecord = new GleifRecord
        {
            Data = new[]
            {
                new Data
                {
                    Attributes = new Attributes
                    {
                        Entity = new Entity
                        {
                            LegalName = new LegalName
                            {
                                Name = "test"
                            },
                            LegalAddress = new LegalAddress
                            {
                                Country = "GB"
                            }
                        },
                        Bic = new[]
                        {
                            "testbic"
                        }
                    }
                }
            }
        };
        _gleifRepository.Setup(gleifRepo => gleifRepo.GetLeiRecordByLeiNumber(csvDataSet.FirstOrDefault().Lei).Result).Returns(mockGleifRecord);
        return csvDataSet;
    }
}