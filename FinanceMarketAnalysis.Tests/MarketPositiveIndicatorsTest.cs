using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinanceMarketAnalysis.Tests
{
    [TestClass]
    public class MarketPositiveIndicatorsTest
    {
        [TestMethod]
        public void IsPositive_PriceToEarningShouldReturnTrue()
        {
            var positiveIndicator = new PriceToEarningPositiveIndicator(10);
            Assert.IsTrue(positiveIndicator.IsPositive());
        }

        [TestMethod]
        public void IsPositive_PriceToEarningShouldReturnFalse()
        {
            var positiveIndicator = new PriceToEarningPositiveIndicator(20);
            Assert.IsFalse(positiveIndicator.IsPositive());
        }

        [TestMethod]
        public void IsPositive_PriceToBalanceShouldReturnTrue()
        {
            var positiveIndicator = new PriceToBalancePositiveIndicator(1);
            Assert.IsTrue(positiveIndicator.IsPositive());
        }

        [TestMethod]
        public void IsPositive_PriceToBalanceShouldReturnFalse()
        {
            var positiveIndicator = new PriceToBalancePositiveIndicator(2);
            Assert.IsFalse(positiveIndicator.IsPositive());
        }

        [TestMethod]
        public void IsPositive_MarketCapShouldReturnTrue()
        {
            var positiveIndicator = new MarketCapPositiveIndicator(3000000000f);
            Assert.IsTrue(positiveIndicator.IsPositive());
        }

        [TestMethod]
        public void IsPositive_MarketCapShouldReturnFalse()
        {
            var positiveIndicator = new MarketCapPositiveIndicator(1000000000f);
            Assert.IsFalse(positiveIndicator.IsPositive());
        }

        [TestMethod]
        public void IsPositive_LiquidityShouldReturnTrue()
        {
            var positiveIndicator = new LiquidityPositiveIndicator(2);
            Assert.IsTrue(positiveIndicator.IsPositive());
        }

        [TestMethod]
        public void IsPositive_LiquidityShouldReturnFalse()
        {
            var positiveIndicator = new LiquidityPositiveIndicator(1);
            Assert.IsFalse(positiveIndicator.IsPositive());
        }

        [TestMethod]
        public void IsPositive_LongTermNetIncomeShouldReturnTrue()
        {
            var positiveIndicator = new LongTermNetIncomePositiveIndicator(new double[]{ 690, 420, 100, 32, 12 });
            Assert.IsTrue(positiveIndicator.IsPositive());
        }

        [TestMethod]
        public void IsPositive_LongTermNetIncomeShouldReturnFalse()
        {
            var positiveIndicator = new LongTermNetIncomePositiveIndicator(new double[] { 20, 10, -15, -35 });
            Assert.IsFalse(positiveIndicator.IsPositive());
        }

        [TestMethod]
        public void IsPositive_LongTermEarningsPerShareShouldReturnTrue()
        {
            //MSFT
            var fsm2021 = new FinanceStatementModel
            {
                date = "2021-04-10",
                eps  = 8.12f
            };
            var fsm2020 = new FinanceStatementModel
            {
                date = "2020-04-10",
                eps = 5.82f
            };
            var fsm2019 = new FinanceStatementModel
            {
                date = "2021-04-10",
                eps = 5.11f
            };
            var fsm2018 = new FinanceStatementModel
            {
                date = "2018-04-10",
                eps = 2.15f
            };
            var fsm2017 = new FinanceStatementModel
            {
                date = "2017-04-10",
                eps = 2.74f
            };

            var positiveIndicator = new LongTermEarningsPerSharePositiveIndicator(new[] { fsm2021, fsm2020, fsm2019, fsm2018, fsm2017 });
            Assert.IsTrue(positiveIndicator.IsPositive());
        }

        [TestMethod]
        public void IsPositive_LongTermEarningsPerShareShouldReturnFalse()
        {
            //TSLA
            var fsm2021 = new FinanceStatementModel
            {
                date = "2021-04-10",
                eps = 0.74f
            };
            var fsm2020 = new FinanceStatementModel
            {
                date = "2020-04-10",
                eps = -0.98f
            };
            var fsm2019 = new FinanceStatementModel
            {
                date = "2021-04-10",
                eps = -1.14f
            };
            var fsm2018 = new FinanceStatementModel
            {
                date = "2018-04-10",
                eps = -2.37f
            };
            var fsm2017 = new FinanceStatementModel
            {
                date = "2017-04-10",
                eps = -0.94f
            };
            var positiveIndicator = new LongTermEarningsPerSharePositiveIndicator(new[] { fsm2021, fsm2020, fsm2019, fsm2018, fsm2017 });
            Assert.IsFalse(positiveIndicator.IsPositive());
        }

        [TestMethod]
        public void IsPositive_LongTermHistoricalDividendsShouldReturnTrue()
        {
            var hisDividends = new HistoricalDividendsRootobjectModel
            {
                historical = new[]
                {
                    new HistoricalDividendsModel
                    {
                        date = "2021-08-18",
                        dividend = 0.56f
                    },
                    new HistoricalDividendsModel
                    {
                        date = "2021-04-10",
                        dividend = 0.56f
                    },
                    new HistoricalDividendsModel
                    {
                        date = "2020-08-18",
                        dividend = 0.51f
                    },
                    new HistoricalDividendsModel
                    {
                        date = "2020-04-18",
                        dividend = 0.51f
                    },
                    new HistoricalDividendsModel
                    {
                        date = "2019-08-18",
                        dividend = 0.46f
                    },
                    new HistoricalDividendsModel
                    {
                        date = "2018-04-10",
                        dividend = 0.46f
                    },
                    new HistoricalDividendsModel
                    {
                        date = "2017-08-18",
                        dividend = 0.42f
                    },
                    new HistoricalDividendsModel
                    {
                        date = "2016-04-18",
                        dividend = 0.42f
                    },
                    new HistoricalDividendsModel
                    {
                        date = "2015-08-18",
                        dividend = 0.39f
                    },
                    new HistoricalDividendsModel
                    {
                        date = "2014-04-18",
                        dividend = 0.39f
                    },
                    new HistoricalDividendsModel
                    {
                        date = "2013-08-18",
                        dividend = 0.36f
                    },
                    new HistoricalDividendsModel
                    {
                        date = "2012-04-18",
                        dividend = 0.36f
                    },
                    new HistoricalDividendsModel
                    {
                        date = "2011-04-18",
                        dividend = 0.13f
                    },
                }
            };
            var positiveIndicator = new LongTermHistoricalDividendsPositiveIndicator(hisDividends);
            Assert.IsTrue(positiveIndicator.IsPositive());
        }

        [TestMethod]
        public void IsPositive_LongTermHistoricalDividendsShouldReturnFalse()
        {
            var positiveIndicator = new LongTermHistoricalDividendsPositiveIndicator(new());
            Assert.IsFalse(positiveIndicator.IsPositive());
        }
    }
}
