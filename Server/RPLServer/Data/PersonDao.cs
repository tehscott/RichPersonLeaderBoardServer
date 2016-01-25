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
                connection.Execute(@"
IF object_id('Person', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Person](
        [PersonId] [int] NOT NULL PRIMARY KEY IDENTITY,
        [Name] [varchar](128) NOT NULL,
        [Wealth] [decimal](15, 2) NOT NULL,
        [InsertDate] [dateTime] NOT NULL CONSTRAINT DF_Person_InsertDate_GETDATE DEFAULT GETDATE(),
        [UpdateDate] [dateTime] NOT NULL CONSTRAINT DF_Person_UpdateDate_GETDATE DEFAULT GETDATE()
    )
END

IF object_id('Payment', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Payment](
        [PaymentId] [int] NOT NULL PRIMARY KEY IDENTITY,
    	 [PersonId] [int] NOT NULL FOREIGN KEY REFERENCES Person(PersonId),
    	 [Amount] [decimal](15,2) NOT NULL,
        [InsertDate] [dateTime] NOT NULL CONSTRAINT DF_Payment_InsertDate_GETDATE DEFAULT GETDATE(),
        [UpdateDate] [dateTime] NOT NULL CONSTRAINT DF_Payment_UpdateDate_GETDATE DEFAULT GETDATE()
    )
END

IF object_id('Achievement', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Achievement](
        [AchievementId] [int] NOT NULL PRIMARY KEY,
        [Name] [varchar](32),
        [Description] [varchar](512)
    )
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (1, 'Richest', 'You are the richest person in the world!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (2, 'Richest Full Day', 'You are the richest person in the world for a full day!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (3, 'Richest Full Week', 'You are the richest person in the world for a full week!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (4, 'Richest Full Month', 'You are the richest person in the world for a full month!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (5, 'Richest Full Year', 'You are the richest person in the world for a full year!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (6, '2nd Richest', 'You are the second richest person in the world!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (7, '3rd Richest', 'You are the third richest person in the world!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (8, '5th Richest', 'You are the fifth richest person in the world!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (9, '10th Richest', 'You are the tenth richest person in the world!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (10, '100th Richest', 'You are the one-hundreth richest person in the world!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (11, '$10 spender', 'You gained $10 in one purchase!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (12, '$100 spender', 'You gained $100 in one purchase!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (13, '$1000 spender', 'You gained $1,000 in one purchase!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (14, '5x a day', 'You purchased money 5 times in one day!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (15, '10x a day', 'You purchased money 10 times in one day!')
    INSERT INTO Achievement (AchievementId, Name, [Description]) values (16, '100x a day', 'You purchased money 100 times in one day!')
END

IF object_id('PersonAchievement', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PersonAchievement](
        [PersonId] [int] NOT NULL FOREIGN KEY REFERENCES Person(PersonId),
        [AchievementId] [int] NOT NULL FOREIGN KEY REFERENCES Achievement(AchievementId),
    	[InsertDate] [dateTime] NOT NULL CONSTRAINT DF_PersonAchievement_InsertDate_GETDATE DEFAULT GETDATE()
    )
END

IF (object_id('GetPersons', 'P') IS NULL AND object_id('GetPersons', 'PC') IS NULL)
BEGIN
    exec('
    Create Proc [dbo].GetPersons (
        @offset int = 0,
        @perPage int = 100
    )
    AS
    BEGIN
    	SET NOCOUNT ON;
    	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    
        SELECT [PersonId], [Name], [Wealth], [InsertDate], [UpdateDate]
    	FROM Person
    	ORDER BY Wealth DESC
    	OFFSET @offset ROWS FETCH NEXT @perPage ROWS ONLY
    END
	')
END

IF (object_id('GetPayments', 'P') IS NULL AND object_id('GetPayments', 'PC') IS NULL)
BEGIN    
    exec('
    Create Proc [dbo].GetPayments(
    	@personId int,
        @offset int = 0,
        @perPage int = 2147483647
    )
    AS
    BEGIN
    	SET NOCOUNT ON;
    	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    
        SELECT [PaymentId], [PersonId], [Amount], [InsertDate], [UpdateDate]
    	FROM Payment
		WHERE personId = @personId
    	ORDER BY [InsertDate] DESC
    	OFFSET @offset ROWS FETCH NEXT @perPage ROWS ONLY
    END
	')
END


IF (object_id('GetAchievements', 'P') IS NULL AND object_id('GetAchievements', 'PC') IS NULL)
BEGIN    
    exec('
	Create Proc [dbo].GetAchievements(
    	@personId int
    )
    AS
    BEGIN
    	SET NOCOUNT ON;
    	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    
        SELECT a.[AchievementId], a.[Name], a.[Description], pa.[InsertDate]
    	FROM [dbo].[Achievement] a
		JOIN [dbo].[PersonAchievement] pa ON a.[AchievementId] = pa.[AchievementId]
		WHERE pa.personId = @personId
    	ORDER BY [InsertDate] DESC
    END
	')
END

IF (object_id('CreatePerson', 'P') IS NULL AND object_id('CreatePerson', 'PC') IS NULL)
BEGIN
    exec('
    Create Proc [dbo].CreatePerson (
	    @Name varchar(32)
    )
    AS
    BEGIN
    	SET NOCOUNT ON;
    	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

        INSERT INTO [dbo].[Person] ([Name], [Wealth]) VALUES (@Name, 0)
    END
	')
END

IF (object_id('CreatePayment', 'P') IS NULL AND object_id('CreatePayment', 'PC') IS NULL)
BEGIN    
    exec('
    Create Proc [dbo].CreatePayment(
    	@personId int,
		@amount decimal(15,2)
    )
    AS
    BEGIN
    	SET NOCOUNT ON;
    	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    
	    INSERT INTO [dbo].[Payment] (PersonId,Amount)
		VALUES (@personId, @amount)		

        UPDATE [dbo].[Person]
        SET Wealth = Wealth + @amount
        WHERE PersonId = @personId
    END
	')
END

IF (object_id('CreateAchievement', 'P') IS NULL AND object_id('CreateAchievement', 'PC') IS NULL)
BEGIN    
    exec('
    Create Proc [dbo].CreateAchievement(
    	@personId int,
		@achievementId int
    )
    AS
    BEGIN
    	SET NOCOUNT ON;
    	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    
	    INSERT INTO [dbo].[PersonAchievement] (PersonId, AchievementId) VALUES (@personId, @achievementId)
    END
	')
END
");
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
                    connection.Query("GetPersons", new { @offset = offset, @perPage = perPage }, commandType: CommandType.StoredProcedure);

                persons = results.Select(o => new Person { Name = o.Name, Wealth = o.Wealth })
                    .ToList();
            }

            return persons;
        }

    }
}
