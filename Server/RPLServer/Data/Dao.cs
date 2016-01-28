﻿using System;
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
    public class Dao : IDao
    {
        private string _connectionString = "Server=tcp:rpldb.database.windows.net,1433;Database=RPLDB;User ID=rpldbAdmin@rpldb;Password=!QAZ2wsx;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public Dao()
        {
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(@"
USE RPLDB;

IF object_id('Person', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Person](
        [PersonId] [int] NOT NULL PRIMARY KEY IDENTITY,
        [Name] [varchar](128) NOT NULL,
        [Wealth] [decimal](15, 2) NOT NULL,
        [Rank] [int] NOT NULL CONSTRAINT DF_Person_Rank DEFAULT ((0)),
        [InsertDate] [dateTime] NOT NULL CONSTRAINT DF_Person_InsertDate_GETDATE DEFAULT GETDATE(),
        [UpdateDate] [dateTime] NOT NULL CONSTRAINT DF_Person_UpdateDate_GETDATE DEFAULT GETDATE()
    )
END

IF object_id('RankType', 'U') IS NULL
BEGIN
	create table RankType(
		[RankTypeId] [int] NOT NULL PRIMARY KEY IDENTITY,
		Name varchar(256)
	)
END

IF object_id('PersonWealth', 'U') IS NULL
BEGIN
    create table [dbo].[PersonWealth](
	[PersonWealthId] [int] NOT NULL PRIMARY KEY IDENTITY,
	[PersonId] [int] NOT NULL FOREIGN KEY REFERENCES Person(PersonId),
	RankTypeId int NOT NULL FOREIGN KEY REFERENCES RankType(RankTypeId),
	Wealth DECIMAL(15,2),
	[rank] int NOT NULL CONSTRAINT [DF_PersonWealth_Rank]  DEFAULT ((0)),
	InsertDate Datetime NOT NULL DEFAULT(GETDATE()),
	UpdateDate Datetime NOT NULL DEFAULT(GETDATE())
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
    CREATE Proc [dbo].GetPersons (
        @offset int = 0,
        @perPage int = 100
    )
    AS
    BEGIN
    	SET NOCOUNT ON;
    	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    
        SELECT [PersonId], [Name], [Wealth], [Rank], [InsertDate], [UpdateDate]
    	FROM Person
    	ORDER BY [Rank] ASC
    	OFFSET @offset ROWS FETCH NEXT @perPage ROWS ONLY
    END
	')
END

IF (object_id('GetPersonAndSurroundingPeople', 'P') IS NULL AND object_id('GetPersonAndSurroundingPeople', 'PC') IS NULL)
BEGIN
    exec('
    CREATE Proc [dbo].GetPersonAndSurroundingPeople (
        @personId int,
        @range int = 5
    )
    AS
    BEGIN
    	SET NOCOUNT ON;
    	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
            
        DECLARE @numberedPerson table(PersonId INT, [RowNumber] INT); 
        INSERT INTO @numberedPerson
        SELECT [PersonId], ROW_NUMBER() OVER (ORDER BY [Rank] ASC) AS [RowNumber]
        FROM Person;
        
        DECLARE @from int;
        SELECT @from = np.RowNumber - @range
        FROM @numberedPerson np
        WHERE np.[PersonId] = @personId
        
        DECLARE @to int;
        SELECT @to = np.RowNumber + @range
        FROM @numberedPerson np
        WHERE np.[PersonId] = @personId
        
        SELECT p.[PersonId], p.[Name], p.[Wealth], p.[Rank], p.[InsertDate], p.[UpdateDate]
        FROM Person p
        JOIN @numberedPerson np ON np.[PersonId] = p.[PersonId]
        WHERE np.RowNumber BETWEEN @from AND @to
        ORDER BY [Rank] ASC
    END
	')
END

IF (object_id('GetPerson', 'P') IS NULL AND object_id('GetPerson', 'PC') IS NULL)
BEGIN
    exec('
    CREATE Proc [dbo].GetPerson (
    	@personId int
    )
    AS
    BEGIN
    	SET NOCOUNT ON;
    	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    
        SELECT [PersonId], [Name], [Wealth], [Rank], [InsertDate], [UpdateDate]
    	FROM Person
		WHERE PersonId = @personId

		EXEC GetPayments @personId
		EXEC GetAchievements @personId
    END
	')
END

IF (object_id('GetPersonByName', 'P') IS NULL AND object_id('GetPersonByName', 'PC') IS NULL)
BEGIN
    exec('
    Create Proc [dbo].GetPersonByName (
    	@name varchar(128)
    )
    AS
    BEGIN
    	SET NOCOUNT ON;
    	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    
        declare @personId int;

        SELECT @personId = [PersonId]
    	FROM Person
		WHERE Name = @name

		EXEC GetPerson @personId
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
    
        SELECT a.[AchievementId] as [AchievementType], a.[Name], a.[Description], pa.[InsertDate]
    	FROM [dbo].[Achievement] a
		JOIN [dbo].[PersonAchievement] pa ON a.[AchievementId] = pa.[AchievementId]
		WHERE pa.personId = @personId
    	ORDER BY [InsertDate] DESC
    END
	')
END

IF (object_id('SetRank', 'P') IS NULL AND object_id('SetRank', 'PC') IS NULL)
BEGIN    
    exec('
    Create Proc [dbo].SetRank(
    	@personId int
    )
    AS
    BEGIN
    	SET NOCOUNT ON;
    	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

        DECLARE @rankings table(PersonId INT, [Rank] INT); 

        INSERT INTO @rankings
		SELECT [PersonId], RANK() OVER (ORDER BY [Wealth] DESC) as [Rank]
    	FROM Person;
		
		UPDATE [Dbo].[Person]
		SET [Person].[Rank] = r.[Rank]
		FROM @rankings r
		WHERE Person.PersonId = @personId
		AND r.PersonId = @personId; 
    END
	')
END

IF (object_id('SetRanks', 'P') IS NULL AND object_id('SetRanks', 'PC') IS NULL)
BEGIN    
    exec('
    Create Proc [dbo].SetRanks
    AS
    BEGIN
    	SET NOCOUNT ON;
    	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

        DECLARE @rankings table(PersonId INT, [Rank] INT); 

        INSERT INTO @rankings
		SELECT [PersonId], RANK() OVER (ORDER BY [Wealth] DESC) as [Rank]
    	FROM Person;
		
		UPDATE [Dbo].[Person]
		SET [Person].[Rank] = r.[Rank]
		FROM @rankings r
		WHERE Person.PersonId = r.PersonId; 
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

        INSERT INTO [dbo].[Person] ([Name], [Wealth]) VALUES (@Name, 0);

		SELECT SCOPE_IDENTITY() as PersonId;
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
		SELECT SCOPE_IDENTITY() as PaymentId;

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
        public List<Person> GetPersons(int offset = 0, int perPage = 100)
        {
            List<Person> persons;
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                var results =
                    connection.Query<Person>("GetPersons", new { @offset = offset, @perPage = perPage }, commandType: CommandType.StoredProcedure);

                persons = results.ToList();
            }

            return persons;
        }

        public Person GetPerson(int personId)
        {
            Person person;
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                var results =
                    connection.QueryMultiple("GetPerson", new { personId }, commandType: CommandType.StoredProcedure);

                person = results.Read<Person>().FirstOrDefault();
                if (person != null)
                {
                    person.Payments = results.Read<Payment>().ToList();
                    person.Achievements = results.Read<Achievement>().ToList();
                }
            }

            return person;
        }
        public Person GetPerson(string name)
        {
            Person person;
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                var results =
                    connection.QueryMultiple("GetPersonByName", new { name }, commandType: CommandType.StoredProcedure);

                person = results.Read<Person>().FirstOrDefault();
                if (person != null)
                {
                    person.Payments = results.Read<Payment>().ToList();
                    person.Achievements = results.Read<Achievement>().ToList();
                }
            }

            return person;
        }

        public Person CreatePerson(string name)
        {
            Person person = new Person { Name = name };
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                var results =
                    connection.Query<int>("CreatePerson", new { name }, commandType: CommandType.StoredProcedure);
                person.PersonId = results.First();
                SetRanks();
            }

            return person;
        }

        public List<Payment> GetPayments(int personId)
        {
            List<Payment> payments;
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                var results =
                    connection.Query<Payment>("GetPayments", new { personId }, commandType: CommandType.StoredProcedure);

                payments = results.ToList();
            }

            return payments;
        }

        public void CreatePayment(int personId, Decimal amount)
        {
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute("CreatePayment", new { personId, amount }, commandType: CommandType.StoredProcedure);
                SetRanks();
            }
        }

        public List<Achievement> GetAchievements(int personId)
        {
            List<Achievement> achievements;
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                var results =
                    connection.Query<Achievement>("GetAchievements", new { personId }, commandType: CommandType.StoredProcedure);

                achievements = results.ToList();
            }

            return achievements;
        }

        public void CreateAchievement(int personId, AchievementType achievementId)
        {
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute("CreateAchievement", new { personId, @achievementId = (int)achievementId }, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Person> GetPersonAndSurroundingPeople(int personId, int range)
        {
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                var results = connection.Query<Person>("GetPersonAndSurroundingPeople", new { personId, range }, commandType: CommandType.StoredProcedure);

                return results.ToList();
            }
        }

        public void SetRank(int personId)
        {
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute("SetRank", new { personId }, commandType: CommandType.StoredProcedure);
            }
        }
        public void SetRanks()
        {
            using (DbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute("SetRanks", new { }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
