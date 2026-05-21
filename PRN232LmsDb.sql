-- ============================================================================
-- PRN232 LMS System Database Initialization Script
-- Target DBMS: Microsoft SQL Server
-- Database Name: PRN232LmsDb
-- ============================================================================

USE master;
GO

-- 1. Recreate the database if it already exists
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'PRN232LmsDb')
BEGIN
    ALTER DATABASE PRN232LmsDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PRN232LmsDb;
END
GO

CREATE DATABASE PRN232LmsDb;
GO

USE PRN232LmsDb;
GO

-- ============================================================================
-- 2. CREATE TABLES (Schema Definition matching EF Core Migrations)
-- ============================================================================

-- A. Table: Semesters
CREATE TABLE [Semesters] (
    [SemesterId]   INT IDENTITY(1,1) NOT NULL,
    [SemesterName] NVARCHAR(100) NOT NULL,
    [StartDate]    DATETIME2 NOT NULL,
    [EndDate]      DATETIME2 NOT NULL,
    CONSTRAINT [PK_Semesters] PRIMARY KEY CLUSTERED ([SemesterId] ASC)
);
GO

-- B. Table: Subjects
CREATE TABLE [Subjects] (
    [SubjectId]   INT IDENTITY(1,1) NOT NULL,
    [SubjectCode] VARCHAR(20) NOT NULL,
    [SubjectName] NVARCHAR(100) NOT NULL,
    [Credit]      INT NOT NULL,
    CONSTRAINT [PK_Subjects] PRIMARY KEY CLUSTERED ([SubjectId] ASC)
);
GO

-- C. Table: Students
CREATE TABLE [Students] (
    [StudentId]   INT IDENTITY(1,1) NOT NULL,
    [FullName]    NVARCHAR(100) NOT NULL,
    [Email]       VARCHAR(100) NOT NULL,
    [DateOfBirth] DATETIME2 NOT NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([StudentId] ASC)
);
GO

-- D. Table: Courses
CREATE TABLE [Courses] (
    [CourseId]   INT IDENTITY(1,1) NOT NULL,
    [CourseName] NVARCHAR(100) NOT NULL,
    [SemesterId] INT NOT NULL,
    [SubjectId]  INT NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED ([CourseId] ASC),
    CONSTRAINT [FK_Courses_Semesters_SemesterId] FOREIGN KEY ([SemesterId]) 
        REFERENCES [Semesters] ([SemesterId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Courses_Subjects_SubjectId] FOREIGN KEY ([SubjectId]) 
        REFERENCES [Subjects] ([SubjectId]) ON DELETE CASCADE
);
GO

-- E. Table: Enrollments
CREATE TABLE [Enrollments] (
    [EnrollmentId] INT IDENTITY(1,1) NOT NULL,
    [StudentId]    INT NOT NULL,
    [CourseId]     INT NOT NULL,
    [EnrollDate]   DATETIME2 NOT NULL,
    [Status]       VARCHAR(20) NOT NULL,
    CONSTRAINT [PK_Enrollments] PRIMARY KEY CLUSTERED ([EnrollmentId] ASC),
    CONSTRAINT [FK_Enrollments_Courses_CourseId] FOREIGN KEY ([CourseId]) 
        REFERENCES [Courses] ([CourseId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Enrollments_Students_StudentId] FOREIGN KEY ([StudentId]) 
        REFERENCES [Students] ([StudentId]) ON DELETE CASCADE
);
GO

-- ============================================================================
-- 3. CREATE INDEXES (Performance optimization & parity with EF Core keys)
-- ============================================================================
CREATE NONCLUSTERED INDEX [IX_Courses_SemesterId] ON [Courses] ([SemesterId] ASC);
CREATE NONCLUSTERED INDEX [IX_Courses_SubjectId] ON [Courses] ([SubjectId] ASC);
CREATE NONCLUSTERED INDEX [IX_Enrollments_CourseId] ON [Enrollments] ([CourseId] ASC);
CREATE NONCLUSTERED INDEX [IX_Enrollments_StudentId] ON [Enrollments] ([StudentId] ASC);
GO


-- ============================================================================
-- 4. INSERT DATA (EF Core Model Seed Data Parity via CTEs & Generation)
-- ============================================================================

-- A. Seed Semesters (1 to 5)
PRINT 'Seeding [Semesters]...';
SET IDENTITY_INSERT [Semesters] ON;
WITH SeqSem AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM SeqSem WHERE n < 5
)
INSERT INTO [Semesters] ([SemesterId], [SemesterName], [StartDate], [EndDate])
SELECT 
    n,
    CONCAT('Semester ', n),
    DATEADD(month, (n - 1) * 4, '2024-01-01 00:00:00'),
    DATEADD(month, (n - 1) * 4, '2024-04-30 00:00:00')
FROM SeqSem;
SET IDENTITY_INSERT [Semesters] OFF;
GO

-- B. Seed Subjects (1 to 10)
PRINT 'Seeding [Subjects]...';
SET IDENTITY_INSERT [Subjects] ON;
WITH SeqSub AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM SeqSub WHERE n < 10
)
INSERT INTO [Subjects] ([SubjectId], [SubjectCode], [SubjectName], [Credit])
SELECT 
    n,
    CONCAT('SUBJ', RIGHT(CONCAT('000', n), 3)),
    CONCAT('Subject ', n),
    2 + (n % 3)
FROM SeqSub;
SET IDENTITY_INSERT [Subjects] OFF;
GO

-- C. Seed Students (1 to 50)
PRINT 'Seeding [Students]...';
SET IDENTITY_INSERT [Students] ON;
WITH SeqStu AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM SeqStu WHERE n < 50
)
INSERT INTO [Students] ([StudentId], [FullName], [Email], [DateOfBirth])
SELECT 
    n,
    CONCAT('Student ', n),
    CONCAT('student', n, '@lms.local'),
    DATEADD(day, n * 7, '2000-01-01 00:00:00')
FROM SeqStu
OPTION (MAXRECURSION 50);
SET IDENTITY_INSERT [Students] OFF;
GO

-- D. Seed Courses (1 to 20)
PRINT 'Seeding [Courses]...';
SET IDENTITY_INSERT [Courses] ON;
WITH SeqCrse AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM SeqCrse WHERE n < 20
)
INSERT INTO [Courses] ([CourseId], [CourseName], [SemesterId], [SubjectId])
SELECT 
    n,
    CONCAT('Course ', n),
    ((n - 1) % 5) + 1,
    ((n - 1) % 10) + 1
FROM SeqCrse;
SET IDENTITY_INSERT [Courses] OFF;
GO

-- E. Seed Enrollments (1 to 500)
PRINT 'Seeding [Enrollments]...';
SET IDENTITY_INSERT [Enrollments] ON;
WITH SeqEnr AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM SeqEnr WHERE n < 500
)
INSERT INTO [Enrollments] ([EnrollmentId], [StudentId], [CourseId], [EnrollDate], [Status])
SELECT 
    n,
    ((n - 1) % 50) + 1,
    ((n - 1) % 20) + 1,
    DATEADD(day, n, '2024-01-15 00:00:00'),
    CASE 
        WHEN n % 3 = 1 THEN 'Completed'
        WHEN n % 3 = 2 THEN 'Dropped'
        ELSE 'Active'
    END
FROM SeqEnr
OPTION (MAXRECURSION 500);
SET IDENTITY_INSERT [Enrollments] OFF;
GO

PRINT 'Database Initialization Completed Successfully!';
GO

-- ============================================================================
-- 5. SEED EF CORE MIGRATIONS HISTORY
-- ============================================================================
PRINT 'Seeding [__EFMigrationsHistory]...';
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] NVARCHAR(150) NOT NULL,
        [ProductVersion] NVARCHAR(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC)
    );
END
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES 
(N'20260518015006_InitialCreate', N'8.0.8'),
(N'20260518083506_AddSubjectCourseRelation', N'8.0.8');
GO

