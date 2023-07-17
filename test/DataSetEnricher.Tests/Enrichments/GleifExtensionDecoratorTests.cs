using System.Collections.Generic;
using System.Linq;
using CardanoAssignment.Enrichments;
using CardanoAssignment.Enrichments.Decorators;
using CardanoAssignment.Models;
using CardanoAssignment.Repositories;
using Moq;
using NFluent;
using Xunit;

namespace DataSetEnricher.Tests.Enrichments;

public class GleifExtensionDecoratorTests
{
    private readonly Mock<ILeiData> _leiData = new();
    private readonly Mock<IGleifRepository> _gleifRepository = new();
    private readonly GleifExtensionDecorator _gleifExtensionDecorator;

    public GleifExtensionDecoratorTests()
    {
        _gleifExtensionDecorator = new GleifExtensionDecorator(_leiData.Object, _gleifRepository.Object);
    }

    [Fact]
    public void GivenValidDecoratedData_WhenGetData_ThenDataEnrichedWithGleifOutput()
    {
        //Arrange
        var leiRecordsMock = CreateLeiRecordsMock();
        _leiData.Setup(leiData => leiData.GetData()).Returns(leiRecordsMock);
        //Act
        var enrichedRecords = _gleifExtensionDecorator.GetData();

        //Assert
        Check.That(enrichedRecords.Count).IsNotZero();
        var enrichedRecord = enrichedRecords.FirstOrDefault();
        Check.That(enrichedRecord.GleifRecord).IsNotNull();
        Check.That(enrichedRecord.Bic).IsEqualTo("testbic");
        Check.That(enrichedRecord.LegalName).IsEqualTo("test");
    }

    [Fact]
    public void GivenEmptyDecoratedData_WhenGetData_ThenEmptyListReturned()
    {
        //Arrange
        var leiRecordsMock = new List<LeiRecord>();
        _leiData.Setup(leiData => leiData.GetData()).Returns(leiRecordsMock);
        //Act
        var enrichedRecords = _gleifExtensionDecorator.GetData();

        //Assert
        Check.That(enrichedRecords.Count).IsZero();
    }

    [Fact]
    public void GivenDecoratedDataButWithEmptyGleifData_WhenGetData_ThenDoesNotThrow()
    {
        //Arrange
        var leiRecordsMock = CreateLeiRecordsMock(new GleifRecord());
        _leiData.Setup(leiData => leiData.GetData()).Returns(leiRecordsMock);
        //Act and Assert
        List<LeiRecord> enrichedRecords = new List<LeiRecord>();
        Check.ThatCode(() =>
        {
            enrichedRecords = _gleifExtensionDecorator.GetData();
            return enrichedRecords;
        }).DoesNotThrow();
        Check.That(enrichedRecords.Count).IsEqualTo(1);
        var enrichedRecord = enrichedRecords.FirstOrDefault();
        Check.That(enrichedRecord.GleifRecord).IsNull();
        Check.That(enrichedRecord.Bic).IsNull();
        Check.That(enrichedRecord.LegalName).IsNull();
    }

    [Fact]
    public void GivenDecoratedDataButWithEmptyLegalName_WhenGetData_ThenDoesNotThrow()
    {
        //Arrange
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
                            LegalAddress = new LegalAddress
                            {
                                Country = "GB"
                            }
                        }
                    }
                }
            }
        };
        var leiRecordsMock = CreateLeiRecordsMock(mockGleifRecord);
        _leiData.Setup(leiData => leiData.GetData()).Returns(leiRecordsMock);
        //Act and Assert
        List<LeiRecord> enrichedRecords = new List<LeiRecord>();
        Check.ThatCode(() =>
        {
            enrichedRecords = _gleifExtensionDecorator.GetData();
            return enrichedRecords;
        }).DoesNotThrow();
        Check.That(enrichedRecords.Count).IsEqualTo(1);
        var enrichedRecord = enrichedRecords.FirstOrDefault();
        Check.That(enrichedRecord.GleifRecord).IsNotNull();
        Check.That(enrichedRecord.Bic).IsEmpty();
        Check.That(enrichedRecord.LegalName).IsEmpty();
    }
    
    private List<LeiRecord> CreateLeiRecordsMock(GleifRecord? gleifRecord = null)
    {
        var leiRecords = new List<LeiRecord>
        {
            new()
            {
                Lei = "XKZZ2JZF41MRHTR1V493",
                Rate = 0.0070956,
                Notional = 763000
            }
        };

        GleifRecord? mockGleifRecord;
        if (gleifRecord == null)
        {
            mockGleifRecord = new GleifRecord
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
        }
        else
        {
            mockGleifRecord = gleifRecord;
        }

        _gleifRepository.Setup(gleifRepo => gleifRepo.GetLeiRecordByLeiNumber(leiRecords.FirstOrDefault().Lei).Result).Returns(mockGleifRecord);
        return leiRecords;
    }
}