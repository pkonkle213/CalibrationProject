using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace CapstoneTest
{
    public abstract class SqlDaoTestBase
    {
        protected string ConnectionString = @"Server=.\SQLEXPRESS;Database=calibration_database;Trusted_Connection=True;";

        private TransactionScope transaction;

        [TestInitialize]
        public void Initalize()
        {
            transaction = new TransactionScope();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }
    }
}
