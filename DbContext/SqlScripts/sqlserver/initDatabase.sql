USE [sql-travels];
GO

--Creates a view that gives overview of the database content
CREATE OR ALTER VIEW supusr.vwInfoDb AS SELECT 
-- Countries
(SELECT COUNT(*) FROM supusr.Countries WHERE Seeded = 1) AS NrSeededCountries, 
(SELECT COUNT(*) FROM supusr.Countries WHERE Seeded = 0) AS NrUnseededCountries,

-- Cities
(SELECT COUNT(*) FROM supusr.Cities WHERE Seeded = 1) AS NrSeededCities, 
(SELECT COUNT(*) FROM supusr.Cities WHERE Seeded = 0) AS NrUnseededCities, 

-- Addresses
(SELECT COUNT(*) FROM supusr.Addresses WHERE Seeded = 1) AS NrSeededAddresses, 
(SELECT COUNT(*) FROM supusr.Addresses WHERE Seeded = 0) AS NrUnseededAddresses, 

-- Attractions
(SELECT COUNT(*) FROM supusr.Attractions WHERE Seeded = 1) AS NrSeededAttractions, 
(SELECT COUNT(*) FROM supusr.Attractions WHERE Seeded = 0) AS NrUnseededAttractions, 

-- Reviews
(SELECT COUNT(*) FROM supusr.Reviews WHERE Seeded = 1) AS NrSeededReviews, 
(SELECT COUNT(*) FROM supusr.Reviews WHERE Seeded = 0) AS NrUnseededReviews,

-- Users
(SELECT COUNT(*) FROM supusr.Users WHERE Seeded = 1) AS NrSeededUsers, 
(SELECT COUNT(*) FROM supusr.Users WHERE Seeded = 0) AS NrUnseededUsers;

GO


--Creates the DeleteAll procedure
CREATE OR ALTER PROC supusr.spDeleteAll
    @seededParam BIT = 1
    AS
    BEGIN
    SET NOCOUNT ON;

    DELETE FROM supusr.Reviews WHERE Seeded = @seededParam;
    DELETE FROM supusr.Users WHERE Seeded = @seededParam;
    DELETE FROM supusr.Attractions WHERE Seeded = @seededParam;
    DELETE FROM supusr.Addresses WHERE Seeded = @seededParam;
    DELETE FROM supusr.Cities WHERE Seeded = @seededParam;
    DELETE FROM supusr.Countries WHERE Seeded = @seededParam;

END
GO