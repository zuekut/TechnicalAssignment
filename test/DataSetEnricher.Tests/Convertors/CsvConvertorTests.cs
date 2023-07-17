using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CardanoAssignment.Convertors;
using CardanoAssignment.Exceptions;
using CardanoAssignment.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NFluent;
using Xunit;

namespace DataSetEnricher.Tests.Convertors;

public class CsvConvertorTests
{
    private readonly Mock<ILogger<CsvConvertor>> _logger = new();
    private readonly Mock<ICsvReaderFactory> _csvReaderFactoryMock = new();
    private readonly Mock<ICsvWriterFactory> _csvWriterFactoryMock = new();
    private CsvConvertor _csvConvertor;

    public CsvConvertorTests()
    {
        _csvConvertor = new CsvConvertor(_logger.Object, new CsvWriterFactory(), new CsvReaderFactory());
    }

    [Fact]
    public void GivenValidCsvStream_WhenConvertFromCsv_ThenListOfRecordsReturned()
    {
        //Arrange
        var csvMock = CreateCsvStreamMock();
        //Act
        var leiRecords = _csvConvertor.ConvertFromCsv(csvMock);
        //Assert
        Check.That(leiRecords.Count).IsEqualTo(1);
    }
    
    [Fact]
    public void GivenExceptionThrownInCsvReaderFactory_WhenConvertFromCsv_ThenExceptionThrown()
    {
        //Arrange
        var csvMock = CreateCsvStreamMock();
        _csvReaderFactoryMock.Setup(factory => factory.CreateCsvReader(It.IsAny<TextReader>())).Throws<Exception>();
        _csvConvertor = new CsvConvertor(_logger.Object, _csvWriterFactoryMock.Object, _csvReaderFactoryMock.Object);
        //Act and Assert
        Check.ThatCode(() => _csvConvertor.ConvertFromCsv(csvMock)).Throws<CsvConversionException>();
    }
    
    [Fact]
    public void GivenExceptionThrownInCsvWriterFactory_WhenConvertToCsv_ThenExceptionThrown()
    {
        //Arrange
        var leiRecords = CreateLeiRecordsMock();
        _csvWriterFactoryMock.Setup(factory => factory.CreateCsvWriter(It.IsAny<StringWriter>())).Throws<Exception>();
        _csvConvertor = new CsvConvertor(_logger.Object, _csvWriterFactoryMock.Object, _csvReaderFactoryMock.Object);
        //Act and Assert
        Check.ThatCode(() => _csvConvertor.ConvertToCsv(leiRecords)).Throws<CsvConversionException>();
    }
    
    [Fact]
    public void GivenValidLeiRecords_WhenConvertToCsv_ThenGeneratedCsvReturned()
    {
        //Arrange
        var leiRecords = CreateLeiRecordsMock();
        //Act
        var csvOutput = _csvConvertor.ConvertToCsv(leiRecords);
        //Assert
        Check.That(csvOutput).IsNotEmpty();
        Check.That(csvOutput).IsEqualTo("transaction_uti,isin,notional,notional_currency,transaction_type,transaction_datetime,rate,lei,legalName,bic,transaction_cost,1030291281MARKITWIRE0000000000000112874138,EZ9724VTXK48,763000,GBP,Sell,11/25/2020 16:06:22,0.0070956,XKZZ2JZF41MRHTR1V493,,,0\r\n");
    }
    
    private Stream CreateCsvStreamMock()
    {
        var csvContent = "transaction_uti,isin,notional,notional_currency,transaction_type,transaction_datetime,rate,lei\n1030291281MARKITWIRE0000000000000112874138,EZ9724VTXK48,763000.0,GBP,Sell,2020-11-25T15:06:22Z,0.0070956000,XKZZ2JZF41MRHTR1V493";
        var csvBytes = Encoding.UTF8.GetBytes(csvContent);
        return new MemoryStream(csvBytes);
    }
    private List<LeiRecord> CreateLeiRecordsMock()
    {
        return new List<LeiRecord>
        {
            new()
            {
                TransactionUti = "1030291281MARKITWIRE0000000000000112874138",
                Isin = "EZ9724VTXK48",
                Notional = 763000.0,
                NotionalCurrency = "GBP",
                TransactionType = "Sell",
                TransactionDateTime = DateTime.Parse("2020-11-25T15:06:22Z"),
                Rate = 0.0070956000,
                Lei = "XKZZ2JZF41MRHTR1V493"
            }
        };
    }
}