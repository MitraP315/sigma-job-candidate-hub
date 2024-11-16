# sigma-job-candidate-hub

#### 1 Clone the project to the local and checout to the dev brench
#### 2. Open the solution in Visual Studio.
#### 3. Set up the database connection in `appsettings.json`.

#### 4. Run the code to create the Candidate  table as mentioned to MSSql after setting the databaseString for selective database
# Candidate Table

The `Candidate` table stores information about candidates, including personal details, contact information, and additional metadata.

## Table Definition

| Column Name           | Data Type         | Constraints               | Description                                                   |
|------------------------|-------------------|---------------------------|---------------------------------------------------------------|
| `Id`                  | `INT`            | Primary Key, Identity     | Unique identifier for each candidate (auto-incremented).      |
| `FirstName`           | `NVARCHAR(50)`   | NOT NULL                  | First name of the candidate (max 50 characters).              |
| `LastName`            | `NVARCHAR(50)`   | NOT NULL                  | Last name of the candidate (max 50 characters).               |
| `PhoneNumber`         | `BIGINT`         |                           | Phone number of the candidate.                                |
| `Email`               | `NVARCHAR(100)`  | NOT NULL, UNIQUE          | Email address of the candidate (must be unique).              |
| `PreferredContactTime`| `NVARCHAR(50)`   |                           | Candidate's preferred time to be contacted (optional).        |
| `LinkedInProfile`     | `NVARCHAR(100)`  |                           | URL to the candidate's LinkedIn profile (optional).           |
| `GitHubProfile`       | `NVARCHAR(100)`  |                           | URL to the candidate's GitHub profile (optional).             |
| `Comment`             | `NVARCHAR(MAX)`  | NOT NULL                  | Additional comments or notes about the candidate.             |
| `AddedOn`             | `DATETIME`       | NOT NULL                  | Timestamp indicating when the record was created.             |
| `ModifiedOn`          | `DATETIME`       |                           | Timestamp indicating when the record was last modified.       |

## Constraints

- **Primary Key**: `Id` ensures each row in the table is uniquely identified.
- **Unique Constraint**: `Email` must be unique across all rows.
- **Not Null Constraints**:
  - `FirstName`
  - `LastName`
  - `Email`
  - `Comment`
  - `AddedOn`

## Notes

- `AddedOn` is required and should store the creation timestamp.
- `ModifiedOn` is optional and can store `NULL` if not modified.

