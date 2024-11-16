# sigma-job-candidate-hub

1 clone the project to the local and checout to the dev brench
2. Open the solution in Visual Studio.
3. Set up the database connection in `appsettings.json`.

4. run the code to MSSql after setting the databaseString for selective database


CREATE TABLE Candidate (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL, 
    PhoneNumber BIGINT,
    Email NVARCHAR(100) NOT NULL UNIQUE, 
    PreferredContactTime NVARCHAR(50),
    LinkedInProfile NVARCHAR(100),
    GitHubProfile NVARCHAR(100),
    Comment NVARCHAR(MAX) NOT NULL, 
    AddedOn DATETIME NOT NULL, 
    ModifiedOn DATETIME Null)
