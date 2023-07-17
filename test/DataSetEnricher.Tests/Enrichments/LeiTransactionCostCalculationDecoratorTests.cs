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

public class LeiTransactionCostCalculationDecoratorTests
{
    private readonly Mock<ILeiData> _leiData = new();
    private readonly Mock<IGleifRepository> _gleifRepository = new();
    private readonly LeiTransactionCostCalculationDecorator _leiTransactionCostCalculationDecorator;
    
    public LeiTransactionCostCalculationDecoratorTests()
    {
        _leiTransactionCostCalculationDecorator = new LeiTransactionCostCalculationDecorator(_leiData.Object, _gleifRepository.Object);
    }

    [Fact]
    public void GivenValidDecoratedData_WhenGetData_ThenDataEnrichedWithTransactionCostCalculation()
    {
        //Arrange
        var leiRecordsMock = CreateLeiRecordsMock();
        _leiData.Setup(leiData => leiData.GetData()).Returns(leiRecordsMock);
        //Act
        var enrichedRecords =_leiTransactionCostCalculationDecorator.GetData();

        //Assert
        Check.That(enrichedRecords.Count).IsEqualTo(2);
        var enrichedGbRecord = enrichedRecords.FirstOrDefault();
        Check.That(enrichedGbRecord.TransactionCost).IsEqualTo(-378793.0286);
        var enrichedNlRecord = enrichedRecords.ElementAt(1);
        Check.That(enrichedNlRecord.TransactionCost).IsEqualTo(-11964973.979238754);
    }
    
    private List<LeiRecord> CreateLeiRecordsMock(GleifRecord? gleifRecordGb = null, GleifRecord? gleifRecordNl = null)
         {
             var leiGB = "XKZZ2JZF41MRHTR1V493";
             var leiNL = "BFXS5XCH7N0Y05NIXW11";
             var leiRecords = new List<LeiRecord>
             {
                 new()
                 {
                     Lei = leiGB,
                     Rate = 0.0070956,
                     Notional = 763000
                 },
                 new()
                 {
                     Lei = leiNL,
                     Rate = 0.00289,
                     Notional = 24000000
                 }
             };
     
             GleifRecord? mockGleifRecordGB;
             GleifRecord? mockGleifRecordNL;
             if (gleifRecordGb == null)
             {
                 mockGleifRecordGB = new GleifRecord
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
                 mockGleifRecordGB = gleifRecordGb;
             }

             if (gleifRecordNl == null)
             {
                 mockGleifRecordNL = new GleifRecord
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
                                         Name = "test1"
                                     },
                                     LegalAddress = new LegalAddress
                                     {
                                         Country = "NL"
                                     }
                                 },
                                 Bic = new[]
                                 {
                                     "testbic1"
                                 }
                             }
                         }
                     }
                 };
             }
             else
             {
                 mockGleifRecordNL = gleifRecordNl;
             }

             _gleifRepository.Setup(gleifRepo => gleifRepo.GetLeiRecordByLeiNumber(It.Is<string>(lei => lei == leiGB)).Result).Returns(mockGleifRecordGB);
             _gleifRepository.Setup(gleifRepo => gleifRepo.GetLeiRecordByLeiNumber(It.Is<string>(lei => lei == leiNL)).Result).Returns(mockGleifRecordNL);
             return leiRecords;
         }
}