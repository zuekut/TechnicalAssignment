using System;
using System.Collections.Generic;
using System.Linq;
using CardanoAssignment.Convertors;
using CardanoAssignment.Enrichments;
using CardanoAssignment.Exceptions;
using CardanoAssignment.Models;
using CardanoAssignment.Processors;
using Microsoft.Extensions.Logging;
using Moq;
using NFluent;
using Xunit;

namespace DataSetEnricher.Tests.Processors;

public class DataSetEnrichmentProcessorTests
{
    private readonly Mock<ILeiDataEnrichmentHandler> _leiDataEnrichmentHandler = new();
    private readonly Mock<ICsvConvertor> _csvConvertor = new();
    private readonly Mock<ILogger<DataSetEnrichmentProcessor>> _logger = new();
    private DataSetEnrichmentProcessor _dataSetEnrichmentProcessor;
    public DataSetEnrichmentProcessorTests()
    {
        _dataSetEnrichmentProcessor = new DataSetEnrichmentProcessor(_logger.Object, _leiDataEnrichmentHandler.Object, _csvConvertor.Object);
    }
    
    [Fact]
    public void GivenEmptyLeiRecordList_WhenProcessDataSet_ThenEmptyCsvStringIsReturned()
    {
        //Arrange
        var csvDataSet = new List<LeiRecord>();

        _csvConvertor.Setup(convertor => convertor.ConvertToCsv(It.IsAny<List<LeiRecord>>())).Returns(string.Empty);
        //Act
        var csvString = string.Empty;
        Check.ThatCode(() =>
        {
            csvString = _dataSetEnrichmentProcessor.ProcessDataSet(csvDataSet);
            return csvString;
        }).DoesNotThrow();

        //Assert
        Check.That(csvString).IsEmpty();
        _csvConvertor.Verify(convertor => convertor.ConvertToCsv(It.IsAny<List<LeiRecord>>()), Times.Once);
    }
    
    [Fact]
    public void GivenLeiRecordWithImproperCsvData_WhenProcessDataSet_ThenCsvExceptionThrown()
    {
        //Arrange
        var csvDataSet = CreateDefaultLeiRecordsMock();

        _csvConvertor.Setup(convertor => convertor.ConvertToCsv(It.IsAny<List<LeiRecord>>())).Throws<CsvConversionException>();
        //Act and Assert
        var csvString = string.Empty;
        Check.ThatCode(() =>
        {
            csvString = _dataSetEnrichmentProcessor.ProcessDataSet(csvDataSet);
            return csvString;
        }).Throws<CsvConversionException>();
    }
    
    [Fact]
    public void GivenAnErrorIsThrownByGleifRepository_WhenProcessDataSet_ThenGleifApiExceptionThrown()
    {
        //Arrange
        var csvDataSet = CreateDefaultLeiRecordsMock();

        _dataSetEnrichmentProcessor = new DataSetEnrichmentProcessor(_logger.Object, _leiDataEnrichmentHandler.Object, _csvConvertor.Object);
        _leiDataEnrichmentHandler.Setup(handler => handler.EnrichData(It.IsAny<List<LeiRecord>>())).Throws<GleifApiException>();
        //Act and Assert
        var csvString = string.Empty;
        Check.ThatCode(() =>
        {
            csvString = _dataSetEnrichmentProcessor.ProcessDataSet(csvDataSet);
            return csvString;
        }).Throws<GleifApiException>();
    }
    [Fact]
    public void GivenAGenericErrorIsThrown_WhenProcessDataSet_ThenGenericExceptionThrown()
    {
        //Arrange
        var csvDataSet = CreateDefaultLeiRecordsMock();

        _dataSetEnrichmentProcessor = new DataSetEnrichmentProcessor(_logger.Object, _leiDataEnrichmentHandler.Object, _csvConvertor.Object);
        _leiDataEnrichmentHandler.Setup(handler => handler.EnrichData(It.IsAny<List<LeiRecord>>())).Throws<Exception>();
        //Act and Assert
        var csvString = string.Empty;
        Check.ThatCode(() =>
        {
            csvString = _dataSetEnrichmentProcessor.ProcessDataSet(csvDataSet);
            return csvString;
        }).Throws<Exception>();
    }
    
    [Fact]
    public void GivenListOfLeiRecords_WhenProcessDataSet_ThenLeiDataEnrichmentHandlerCalledOnceAndEnrichedDataIsReturnedAsCsv()
    {
        //Arrange
        var csvDataSet = CreateDefaultLeiRecordsMock();
        var expectedCsv = "notional,rate,lei,legalName,bic,transaction_costs\n763000.0,0.0070956000,XKZZ2JZF41MRHTR1V493,test,testbic,12345";
        var expectedLei = "XKZZ2JZF41MRHTR1V493";
        _csvConvertor.Setup(convertor => convertor.ConvertToCsv(It.IsAny<List<LeiRecord>>()))
            .Returns<List<LeiRecord>>(data =>
            {
                Check.That(data).Not.IsEmpty();
                var leiRecord = data.FirstOrDefault();
                if (leiRecord.Lei == expectedLei && leiRecord is { Bic: "testbic", LegalName: "test", TransactionCost: 12345})
                {
                    return expectedCsv;

                }
                return string.Empty;
            });
        //Act
        var csvString = string.Empty;
        Check.ThatCode(() =>
        {
            csvString = _dataSetEnrichmentProcessor.ProcessDataSet(csvDataSet);
            return csvString;
        }).DoesNotThrow();

        //Assert
        Check.That(csvString).IsEqualTo(expectedCsv);
        _leiDataEnrichmentHandler.Verify(handler => handler.EnrichData(It.IsAny<List<LeiRecord>>()), Times.Once);
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

        /*
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
        */
        var enrichedLeiRecords = new List<LeiRecord>();
        enrichedLeiRecords.AddRange(csvDataSet);
        enrichedLeiRecords.FirstOrDefault().Bic = "testbic";
        enrichedLeiRecords.FirstOrDefault().LegalName = "test";
        enrichedLeiRecords.FirstOrDefault().TransactionCost = 12345;
        _leiDataEnrichmentHandler.Setup(handler => handler.EnrichData(It.IsAny<List<LeiRecord>>())).Returns(enrichedLeiRecords);
        _csvConvertor.Setup(convertor => convertor.ConvertToCsv(It.IsAny<List<LeiRecord>>())).Returns(string.Empty);
        return csvDataSet;
    }
}