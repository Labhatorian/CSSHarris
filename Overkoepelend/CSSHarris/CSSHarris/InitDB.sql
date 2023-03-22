 DROP DATABASE IF EXISTS MHeetDB
 GO
 CREATE DATABASE MHeetDB
 GO
 USE MHeetDB
 GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

SET QUOTED_IDENTIFIER ON;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'Identity') IS NULL EXEC(N'CREATE SCHEMA [Identity];');
GO

CREATE TABLE [Identity].[AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [Banned] bit NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Identity].[Chatlogs] (
    [ID] int NOT NULL IDENTITY,
    CONSTRAINT [PK_Chatlogs] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [Identity].[Emails] (
    [idEmail] int NOT NULL IDENTITY,
    [Subject] nvarchar(200) NULL,
    [EmailAddress] nvarchar(max) NULL,
    [Message] nvarchar(600) NULL,
    CONSTRAINT [PK_Emails] PRIMARY KEY ([idEmail])
);
GO

CREATE TABLE [Identity].[Role] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Identity].[User] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(max) NULL,
    [NormalizedUserName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [NormalizedEmail] nvarchar(max) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Identity].[UserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Identity].[UserLogins] (
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(128) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_UserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Identity].[UserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_UserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Identity].[Message] (
    [ID] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [ChatlogID] int NULL,
    CONSTRAINT [PK_Message] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Message_Chatlogs_ChatlogID] FOREIGN KEY ([ChatlogID]) REFERENCES [Identity].[Chatlogs] ([ID])
);
GO

CREATE TABLE [Identity].[Rooms] (
    [ID] nvarchar(450) NOT NULL,
    [Owner] nvarchar(max) NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [ChatlogID] int NOT NULL,
    CONSTRAINT [PK_Rooms] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Rooms_Chatlogs_ChatlogID] FOREIGN KEY ([ChatlogID]) REFERENCES [Identity].[Chatlogs] ([ID]) ON DELETE CASCADE
);
GO

CREATE TABLE [Identity].[RoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RoleClaims_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Identity].[Role] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Identity].[UserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Identity].[Role] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Identity].[ChatUsers] (
    [ChatUserID] nvarchar(450) NOT NULL,
    [UserID] nvarchar(max) NULL,
    [ConnectionId] nvarchar(max) NULL,
    [UserName] nvarchar(max) NOT NULL,
    [RoomId] nvarchar(450) NULL,
    CONSTRAINT [PK_ChatUsers] PRIMARY KEY ([ChatUserID]),
    CONSTRAINT [FK_ChatUsers_Rooms_RoomId] FOREIGN KEY ([RoomId]) REFERENCES [Identity].[Rooms] ([ID])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'Banned', N'ConcurrencyStamp', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[Identity].[AspNetUsers]'))
    SET IDENTITY_INSERT [Identity].[AspNetUsers] ON;
INSERT INTO [Identity].[AspNetUsers] ([Id], [AccessFailedCount], [Banned], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName])
VALUES (N'a18be9c0-aa65-4af8-bd17-00bd9344e575', 0, CAST(0 AS bit), N'76103b16-9f71-4fe0-845b-b6bccbda92e1', N'admin@admin.nl', CAST(1 AS bit), CAST(0 AS bit), NULL, N'admin@admin.nl,', N'admin', N'AQAAAAIAAYagAAAAED6rZ7dN2vy3GkUdjushHQ7ZPARbL8Dx/hmS/7X2aicQazZt9ypZ2KTGzZWYFsMXjw==', NULL, CAST(0 AS bit), N'', CAST(0 AS bit), N'admin');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'Banned', N'ConcurrencyStamp', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[Identity].[AspNetUsers]'))
    SET IDENTITY_INSERT [Identity].[AspNetUsers] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[Identity].[Role]'))
    SET IDENTITY_INSERT [Identity].[Role] ON;
INSERT INTO [Identity].[Role] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
VALUES (N'a18be9c0-aa65-4af8-bd17-00bd9320e575', NULL, N'Admin', N'ADMIN'),
(N'a18be9c0-aa65-4af8-bd17-00ca3234e243', NULL, N'Moderator', N'MODERATOR');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[Identity].[Role]'))
    SET IDENTITY_INSERT [Identity].[Role] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[Identity].[UserRoles]'))
    SET IDENTITY_INSERT [Identity].[UserRoles] ON;
INSERT INTO [Identity].[UserRoles] ([RoleId], [UserId])
VALUES (N'a18be9c0-aa65-4af8-bd17-00bd9320e575', N'a18be9c0-aa65-4af8-bd17-00bd9344e575');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[Identity].[UserRoles]'))
    SET IDENTITY_INSERT [Identity].[UserRoles] OFF;
GO

CREATE INDEX [EmailIndex] ON [Identity].[AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [Identity].[AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_ChatUsers_RoomId] ON [Identity].[ChatUsers] ([RoomId]);
GO

CREATE INDEX [IX_Message_ChatlogID] ON [Identity].[Message] ([ChatlogID]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [Identity].[Role] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_RoleClaims_RoleId] ON [Identity].[RoleClaims] ([RoleId]);
GO

CREATE INDEX [IX_Rooms_ChatlogID] ON [Identity].[Rooms] ([ChatlogID]);
GO

CREATE INDEX [IX_UserClaims_UserId] ON [Identity].[UserClaims] ([UserId]);
GO

CREATE INDEX [IX_UserLogins_UserId] ON [Identity].[UserLogins] ([UserId]);
GO

CREATE INDEX [IX_UserRoles_RoleId] ON [Identity].[UserRoles] ([RoleId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230321100843_MHeet', N'7.0.4');
GO

COMMIT;
GO

