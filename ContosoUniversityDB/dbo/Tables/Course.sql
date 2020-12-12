CREATE TABLE [dbo].[Course] (
    [CourseID]     INT           IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (50) NULL,
    [Credits]      INT           NOT NULL,
    [DepartmentID] INT           CONSTRAINT [DF__Course__Departme__1CF15040] DEFAULT ((1)) NOT NULL,
    [DateModified] DATETIME2 (7) DEFAULT ('0001-01-01T00:00:00.0000000') NOT NULL,
    [IsDeleted]    BIT           DEFAULT (CONVERT([bit],(0))) NOT NULL,
    CONSTRAINT [PK_dbo.Course] PRIMARY KEY CLUSTERED ([CourseID] ASC),
    CONSTRAINT [FK_dbo.Course_dbo.Department_DepartmentID] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([DepartmentID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_DepartmentID]
    ON [dbo].[Course]([DepartmentID] ASC);

