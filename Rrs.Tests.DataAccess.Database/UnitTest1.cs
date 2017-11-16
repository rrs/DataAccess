using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rrs.DataAccess.Database.SqlConnection;
using Rrs.DataAccess.Database;
using Rrs.DataAccess.Database.DbCommand;

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


        [TestMethod]
        public void TestMethod2()
        {
            var db = SqlDatabaseExecutorFactory.New(ConnectionStrings.WindowsAutoConnectionString("localhost", "dev"));

            db.Execute(new Test());
        }

        class Test : IDatabaseCommand
        {
            public void Execute(IDbConnectionWrapper c)
            {
                Sql.Dynamic(Command, c).WithParameters(new { data = (byte[])null }).NonQuery();
            }

            private const string Command = @"
insert into Test (data) VALUES (@data)
";
        }

    }
}
