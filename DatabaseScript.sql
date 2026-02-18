IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218032204_InitialCreate'
)
BEGIN
    CREATE TABLE [Departments] (
        [DepartmentId] int NOT NULL IDENTITY,
        [DepartmentName] nvarchar(100) NOT NULL,
        [DepartmentCode] nvarchar(20) NOT NULL,
        [ActiveInactive] bit NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Departments] PRIMARY KEY ([DepartmentId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218032204_InitialCreate'
)
BEGIN
    CREATE TABLE [Employees] (
        [EmployeeId] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Email] nvarchar(100) NOT NULL,
        [Salary] decimal(18,2) NOT NULL,
        [DepartmentId] int NOT NULL,
        [JoiningDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Employees] PRIMARY KEY ([EmployeeId]),
        CONSTRAINT [FK_Employees_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([DepartmentId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218032204_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Departments_DepartmentName] ON [Departments] ([DepartmentName]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218032204_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Employees_DepartmentId] ON [Employees] ([DepartmentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218032204_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Employees_Email] ON [Employees] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218032204_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260218032204_InitialCreate', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

-- NullableJoiningDate migration: the EF Up() method was empty so no ALTER was
-- generated automatically. This statement applies the intended schema change
-- so that JoiningDate is nullable, matching the C# model (DateTime?).
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218073433_NullableJoiningDate'
)
BEGIN
    ALTER TABLE [Employees] ALTER COLUMN [JoiningDate] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260218073433_NullableJoiningDate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260218073433_NullableJoiningDate', N'8.0.0');
END;
GO

COMMIT;
GO