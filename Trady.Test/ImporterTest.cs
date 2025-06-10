using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;
using Trady.Importer.Csv;

namespace Trady.Test
{
    [TestClass]
    public class ImporterTest
    {
        public ImporterTest()
        {
            // Test culture info
            CultureInfo.CurrentCulture = new CultureInfo("nl-nl");
        }

      

        [TestMethod]
        public void ImportFromCsv()
        {
            var importer = new CsvImporter("fb.csv", CultureInfo.GetCultureInfo("en-US"));
            var candles = importer.ImportAsync("FB").Result;
            Assert.AreEqual(candles.Count, 1342);
            var firstIOhlcvData = candles.First();
            Assert.AreEqual(firstIOhlcvData.DateTime, new DateTime(2012, 5, 18));
        }

        [TestMethod]
        public void ImportFromCsvWithTime()
        {
            var config = new CsvImportConfiguration()
            {
                Culture = "en-US",
                Delimiter = ";",
                DateFormat = "yyyyMMdd HHmmss",
                HasHeaderRecord = false
            };
            var importer = new CsvImporter("EURUSD.csv", config);
            var candles = importer.ImportAsync("EURUSD").Result;
            Assert.AreEqual(744, candles.Count);
            var firstIOhlcvData = candles.First();
            Assert.AreEqual(new DateTime(2000, 5, 30, 17, 27, 00), firstIOhlcvData.DateTime);
        }
    }
}
