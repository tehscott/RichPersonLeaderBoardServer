using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain;

namespace Data
{
    public class PersonDao
    {
        private string _connectionString = "Server=tcp:rpldb.database.windows.net,1433;Database=RPLDB;User ID=rpldbAdmin@rpldb;Password=!QAZ2wsx;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public PersonDao()
        {
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(
                    "IF object_id('Person', 'U') IS NULL\n" +
                    "BEGIN\n" +
                    "    CREATE TABLE [dbo].[Person](\n" +
                    "        [PersonId] [int] NOT NULL PRIMARY KEY IDENTITY,\n" +
                    "        [Name] [varchar](128) NOT NULL,\n" +
                    "        [Wealth] [decimal](15, 2) NOT NULL,\n" +
                    "        [InsertDate] [dateTime] NOT NULL CONSTRAINT DF_Person_InsertDate_GETDATE DEFAULT GETDATE(),\n" +
                    "        [UpdateDate] [dateTime] NOT NULL CONSTRAINT DF_Person_UpdateDate_GETDATE DEFAULT GETDATE()\n" +
                    "    )\n" +
                    "END\n");
            }
        }

        public List<Person> GetPersons(int offset, int perPage)
        {
            List<Person> persons;
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                var results =
                    connection.Query(
                        "SELECT * FROM Person ORDER BY Wealth DESC OFFSET @offset ROWS FETCH NEXT @perPage ROWS ONLY",
                        new { @offset = offset, @perPage = perPage },
                        commandType: CommandType.Text);

                persons = results.Select(o => new Person { Name = o.Name, Wealth = o.Wealth })
                    .ToList();
            }

            return persons;
        }

    }
}
