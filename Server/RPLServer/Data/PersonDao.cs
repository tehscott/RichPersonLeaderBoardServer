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
                    "END\n" +
                    "IF object_id('Payment', 'U') IS NULL\n" +
                    "BEGIN\n" +
                    "    CREATE TABLE [dbo].[Payment](\n" +
                    "        [PaymentId] [int] NOT NULL PRIMARY KEY IDENTITY,\n" +
                    "    	 [PersonId] [int] NOT NULL FOREIGN KEY REFERENCES Person(PersonId),\n" +
                    "    	 [Amount] [decimal](15,2) NOT NULL,\n" +
                    "        [InsertDate] [dateTime] NOT NULL CONSTRAINT DF_Payment_InsertDate_GETDATE DEFAULT GETDATE(),\n" +
                    "        [UpdateDate] [dateTime] NOT NULL CONSTRAINT DF_Payment_UpdateDate_GETDATE DEFAULT GETDATE()\n" +
                    "    )\n" +
                    "END\n" +
                    "IF object_id('Achievement', 'U') IS NULL\n" +
                    "BEGIN\n" +
                    "    CREATE TABLE [dbo].[Achievement](\n" +
                    "        [AchievementId] [int] NOT NULL PRIMARY KEY,\n" +
                    "        [Name] [varchar](32),\n" +
                    "        [Description] [varchar](512)\n" +
                    "    )\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (1, 'Richest', 'You are the richest person in the world!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (2, 'Richest Full Day', 'You are the richest person in the world for a full day!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (3, 'Richest Full Week', 'You are the richest person in the world for a full week!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (4, 'Richest Full Month', 'You are the richest person in the world for a full month!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (5, 'Richest Full Year', 'You are the richest person in the world for a full year!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (6, '2nd Richest', 'You are the second richest person in the world!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (7, '3rd Richest', 'You are the third richest person in the world!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (8, '5th Richest', 'You are the fifth richest person in the world!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (9, '10th Richest', 'You are the tenth richest person in the world!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (10, '100th Richest', 'You are the one-hundreth richest person in the world!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (11, '$10 spender', 'You gained $10 in one purchase!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (12, '$100 spender', 'You gained $100 in one purchase!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (13, '$1000 spender', 'You gained $1,000 in one purchase!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (14, '5x a day', 'You purchased money 5 times in one day!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (15, '10x a day', 'You purchased money 10 times in one day!')\n" +
                    "    INSERT INTO Achievement (AchievementId, Name, [Description]) values (16, '100x a day', 'You purchased money 100 times in one day!')\n" +
                    "END\n" +
                    "IF object_id('PersonAchievement', 'U') IS NULL\n" +
                    "BEGIN\n" +
                    "    CREATE TABLE [dbo].[PersonAchievement](\n" +
                    "        [PersonId] [int] NOT NULL FOREIGN KEY REFERENCES Person(PersonId),\n" +
                    "        [AchievementId] [int] NOT NULL FOREIGN KEY REFERENCES Achievement(AchievementId),\n" +
                    "    	[InsertDate] [dateTime] NOT NULL CONSTRAINT DF_PersonAchievement_InsertDate_GETDATE DEFAULT GETDATE()\n" +
                    "    )\n" +
                    "END\n" +
                    "");
            }
        }
/*
*/
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
