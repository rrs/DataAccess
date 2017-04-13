using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rrs.DataAccess.Database.SqlConnection;
using Rrs.DataAccess.Database;

namespace Rrs.Tests.DataAccess.Database
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var db = SqlDatabaseExecutorFactory.New(ConnectionStrings.WindowsAutoConnectionString("localhost", "pointone_dev"));

            db.ExecuteInTransaction(e =>
            {
                //e.Execute()
            });
        }
    }
}
