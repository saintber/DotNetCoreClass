CREATE TABLE [dbo].[Department] (
    [DepartmentID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50) NULL,
    [Budget]       MONEY         NOT NULL,
    [StartDate]    DATETIME      NOT NULL,
    [InstructorID] INT           NULL,
    [RowVersion]   ROWVERSION    NOT NULL,
    [DateModified] DATETIME2 (7) DEFAULT ('0001-01-01T00:00:00.0000000') NOT NULL,
    [IsDeleted]    BIT           DEFAULT (CONVERT([bit],(0))) NOT NULL,
    CONSTRAINT [PK_dbo.Department] PRIMARY KEY CLUSTERED ([DepartmentID] ASC),
    CONSTRAINT [FK_dbo.Department_dbo.Instructor_InstructorID] FOREIGN KEY ([InstructorID]) REFERENCES [dbo].[Person] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_InstructorID]
    ON [dbo].[Department]([InstructorID] ASC);

