--

CREATE TABLE Articles
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SiteId int NOT NULL,
	Culture nvarchar(20) NOT NULL,
	Title nvarchar(1000) NOT NULL,
	UrlSlug nvarchar(1000) NOT NULL,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateDeleted datetime2(7) NULL,
	CONSTRAINT UQ_Articles_Title UNIQUE (Title),
	CONSTRAINT UQ_Articles_UrlSlug UNIQUE (UrlSlug)
);

CREATE INDEX IX_Articles_UrlSlug
ON Articles (UrlSlug);

-- IMPORTANT TODO
-- FileRevisions table needs another link table; take ArticleId out of here. A list of FileRevisions can be
-- attached to multiple Articles, one per culture/siteid. It also needs a RevisionReasonCulture so that you can instantly see
-- what language the RevisionReason is written in and machine translate it?

CREATE TABLE FileRevisions
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ArticleId int NOT NULL,
	[FileName] nvarchar(max) NOT NULL,
	FileSizeBytes bigint NOT NULL,
	MimeType nvarchar(100) NOT NULL,
	Is2dImage bit NOT NULL,
	IsVideo bit NOT NULL,
	IsAudio bit NOT NULL,
	RevisionReason nvarchar(1000) NOT NULL,
  	CreatedByAspNetUserId nvarchar(450) NOT NULL,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateDeleted datetime2(7) NULL
);

CREATE INDEX IX_FileRevisions_ArticleId
ON FileRevisions (ArticleId);

CREATE TABLE ArticleRevisions
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ArticleId int NOT NULL,
	[Text] nvarchar(max) NOT NULL,
	RevisionReason nvarchar(1000) NOT NULL,
  	CreatedByAspNetUserId nvarchar(450) NOT NULL,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateDeleted datetime2(7) NULL
);

CREATE INDEX IX_ArticleRevisions_ArticleId
ON ArticleRevisions (ArticleId);

CREATE TABLE ArticleCultureLinks 
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SiteId int NOT NULL,
	ArticleId int NOT NULL,
	ArticleCultureLinkGroupId int NOT NULL,
	DateDeleted datetime2(7) NULL
);

CREATE TABLE DownloadUrls
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SiteId int NOT NULL,
	[Filename] nvarchar(1000) NOT NULL,
	Filesize int NOT NULL,
	HashSha256 varbinary(64) NOT NULL,
	DownloadUrls nvarchar(MAX) NULL,
	NeedsOcr bit NULL,
	IsComplete bit NULL,
	CreatedByAspNetUserId nvarchar(450) NOT NULL,
	DateCreated datetime2(7) NOT NULL,
	DateModified datetime2(7) NOT NULL,
	DateDeleted datetime2(7) NULL
);

CREATE TABLE ArticleTalkSubjects
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SiteId int NOT NULL,
	ArticleId int NOT NULL,
	[Subject] nvarchar(300) NOT NULL,
	UrlSlug nvarchar(300) NOT NULL,
	HasBeenEdited bit NOT NULL DEFAULT 0,
	CreatedByAspNetUserId nvarchar(450) NOT NULL,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateModified datetime2(7) NULL,
	DateDeleted datetime2(7) NULL
);

CREATE TABLE ArticleTalkSubjectPosts
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ArticleTalkSubjectId int NOT NULL,
	ParentTalkSubjectPostId int NULL,
	[Text] nvarchar(2000) NOT NULL,
	HasBeenEdited bit NOT NULL DEFAULT 0,
	CreatedByAspNetUserId nvarchar(450) NOT NULL,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateModified datetime2(7) NULL,
	DateDeleted datetime2(7) NULL
);




-- UPDATE CRLF SET col1 = REPLACE(col1, '@', CHAR(13))

-- 128.140.38.19
-- mguiJLbasW3AifV4tCxN

CREATE TABLE AspNetRoles
(
	Id nvarchar(450) NOT NULL PRIMARY KEY,
	ConcurrencyStamp nvarchar(max) NULL,
	[Name] nvarchar(256) NULL,
	NormalizedName nvarchar(256) NULL
);

CREATE TABLE AspNetRoleClaims
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ClaimType nvarchar(max) NULL,
	ClaimValue nvarchar(max) NULL,
	RoleId nvarchar(450) NOT NULL
);

CREATE TABLE AspNetUsers
(
	Id nvarchar(450) NOT NULL PRIMARY KEY,
	AccessFailedCount int NOT NULL,
	ConcurrencyStamp nvarchar(max) NULL,
	Email nvarchar(256) NULL,
	EmailConfirmed bit NOT NULL,
	LockoutEnabled bit NOT NULL,
	LockoutEnd datetimeoffset(7) NULL,
	NormalizedEmail nvarchar(256) NULL,
	NormalizedUserName nvarchar(256) NULL,
	PasswordHash nvarchar(max) NULL,
	PhoneNumber nvarchar(max) NULL,
	PhoneNumberConfirmed bit NOT NULL,
	SecurityStamp nvarchar(max) NULL,
	TwoFactorEnabled bit NOT NULL,
	UserName nvarchar(256) NULL
);

CREATE TABLE AspNetUserClaims
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ClaimType nvarchar(max) NULL,
	ClaimValue nvarchar(max) NULL,
	UserId nvarchar(450) NOT NULL
);

CREATE TABLE AspNetUserLogins
(
	LoginProvider nvarchar(450) NOT NULL,
	ProviderKey nvarchar(450) NOT NULL,
	ProviderDisplayName nvarchar(max) NULL,
	UserId nvarchar(450) NOT NULL,

	CONSTRAINT PK_AspNetUserLogin PRIMARY KEY
	(
		LoginProvider,
		ProviderKey
	)
);

CREATE TABLE AspNetUserRoles
(
	UserId nvarchar(450) NOT NULL,
	RoleId nvarchar(450) NOT NULL,

	CONSTRAINT PK_AspNetUserRole PRIMARY KEY
	(
		UserId,
		RoleId
	)
);

CREATE TABLE AspNetUserTokens
(
	UserId nvarchar(450) NOT NULL,
	LoginProvider nvarchar(450) NOT NULL,
	[Name] nvarchar(450) NOT NULL,
	[Value] nvarchar(max) NULL,

	CONSTRAINT PK_AspNetUserToken PRIMARY KEY 
	(
		UserId,
		LoginProvider,
		[Name]
	)
);
