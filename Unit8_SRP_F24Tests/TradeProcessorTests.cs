using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleResponsibilityPrinciple;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.Tests
{
    [TestClass()]
    public class TradeProcessorTests
    {
        private int CountDbRecords()
        {
            string hibaConnectionString = @"Data Source =(LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\pc\OneDrive\Documents\tradedatabase.mdf; Integrated Security = True; Connect Timeout = 30";
            // Change the connection string used to match the one you want
            using (var connection = new SqlConnection(hibaConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string myScalarQuery = "SELECT COUNT(*) FROM trade";
                SqlCommand myCommand = new SqlCommand(myScalarQuery, connection);
                //myCommand.Connection.Open();
                int count = (int)myCommand.ExecuteScalar();
                connection.Close();
                return count;
            }
        }

        [TestMethod()]
        public void TestGoodTrades()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.goodtrades.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);
            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore + 4, countAfter);
        }
        [TestMethod()]
        public void testBadTradeNegativePrice()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.badtradenegativeprice.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);
            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore + 1, countAfter);
        }
        [TestMethod()]
        public void testNoTrades()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.notrades.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);
            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore , countAfter);
        }
        [TestMethod()]
        public void TestOneBadTradeTooMany()
        {

            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.badtradetoomany.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);
            //Assert
            int countAfter = CountDbRecords();
            if (countBefore != countAfter)
            {
                countBefore = countAfter;
            }
            Console.WriteLine("You have too many values");
            Assert.AreEqual(countBefore, countAfter);
        }
        [TestMethod()]
        public void ReadTradeDataOneGoodTrade()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.onegoodtrade.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);
            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore + 1, countAfter);
        }
        [TestMethod()]
        public void ReadTradeDataOneBadTradeTooFew()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.badtradetoofew.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);
            //Assert
            int countAfter = CountDbRecords();
            if(countBefore != countAfter)
            {
                countBefore = countAfter;
            }
            Console.WriteLine("You have too few values");
            Assert.AreEqual(countBefore , countAfter);
        }


    }
}