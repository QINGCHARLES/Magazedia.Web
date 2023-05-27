--
-- Default QINGCHARLES user
INSERT  AspNetUsers ( Id, AccessFailedCount, ConcurrencyStamp, Email, EmailConfirmed, LockoutEnabled, LockoutEnd, NormalizedEmail, NormalizedUserName, PasswordHash, PhoneNumber, PhoneNumberConfirmed, SecurityStamp, TwoFactorEnabled, UserName)
VALUES ('7240be61-df81-46f9-8152-6a48b96abc40', 0, '3050d1a5-9a2c-4d80-a1be-58d6a79191a6', 'hello@magazedia.site', 1, 1, NULL, 'HELLO@MAGAZEDIA.SITE', 'QINGCHARLES', 'AQAAAAIAAYagAAAAEBB+4D8cFi426Wgg8vfO/4cBgISBNJWZVfnWXwSSv5pq171AC3LGZgH6qlvFTj25Dw==', NULL, 0, 'PSJMEDAFBWDTOWQG7J4FYSGOJ7HM3M4I', 0, 'QINGCHARLES');
--

        TestSpec("https://example.com", "Image: ![alt text](/image.jpg)", "https://example.com/image.jpg");
        TestSpec("https://example.com", "Image: ![alt text](image.jpg \"title\")", "https://example.com/image.jpg");
        TestSpec(null, "Image: ![alt text](/image.jpg)", "/image.jpg");
    }

    public static void TestSpec(string baseUrl, string markdown, string expectedLink)
    {

    var pipeline = new MarkdownPipelineBuilder().Build();

        var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);
        if (baseUrl != null)
            renderer.BaseUrl = new Uri(baseUrl);
        pipeline.Setup(renderer);

INSERT Articles (Title, UrlSlug, SiteId, Culture)
VALUES ( N'GQ (USA) - November 2020', N'gq-usa-november-2020', 1, 'en' );
INSERT ArticleRevisions (ArticleId, [Text], RevisionReason, CreatedByAspNetUserId)
VALUES (SCOPE_IDENTITY(), N'The November 2020 issue of GQ Magazine, an American monthly men’s magazine, prominently featured the actor Timothée Chalamet on its cover. The cover story was captured by fashion photographer Renell Medrano, with styling by Mobolaji Dawodu, tailoring from Ksenia Golub, and production by Wei-Li Wang at Hudson Hill Production​1​.' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) + N'In addition to Chalamet, who was soon to appear in Denis Villeneuve''s film adaptation of the sci-fi classic "Dune", the issue also featured several other notable figures. Among them were young skateboarder and internet star Sean Pablo, Paul Mescal (known for his role in "Normal People"), and prominent music producers Timbaland and Swizz Beatz​2​.' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) + N'The issue showcased a long, in-depth interview with Chalamet, further solidifying GQ Magazine''s reputation for delivering exclusive and engaging content that reaches a wide audience, ranging from fashion enthusiasts to followers of popular culture. The November 2020 issue is a testament to GQ''s commitment to keeping readers informed about leading figures in various fields, including film, music, and sports.{{Categories Magazine issues|GQ (USA) magazine issues|GQ magazine issues|English magazine issues|Fashion magazine issues|Men’s interest magazine issues}}', N'Article created.', '7240be61-df81-46f9-8152-6a48b96abc40');

The November 2020 issue of [GQ (USA)](gq-usa), an American monthly men’s magazine, prominently featured the actor [Timothée Chalamet](timothée-chalamet) on its cover. The cover story was captured by fashion photographer [Renell Medrano](renell-medrano), with styling by [Mobolaji Dawodu](mobolaji-dawodu), tailoring from Ksenia Golub, and production by Wei-Li Wang at Hudson Hill Production​​.

In addition to Chalamet, who was soon to appear in [Denis Villeneuve’s](denis-villeneuve) film adaptation of the sci-fi classic [“Dune”](dune-2021), the issue also featured several other notable figures. Among them were young skateboarder and internet star [Sean Pablo](sean-pablo), [Paul Mescal](paul-mescal) (known for his role in [“Normal People”](normal-people)), and prominent music producers [Timbaland](timbaland) and [Swizz Beatz​](swizz-beatz)​.

The issue showcased a long, in-depth interview with Chalamet, further solidifying GQ Magazine’s reputation for delivering exclusive and engaging content that reaches a wide audience, ranging from fashion enthusiasts to followers of popular culture. The November 2020 issue is a testament to GQ’s commitment to keeping readers informed about leading figures in various fields, including film, music, and sports.{{Categories Magazine issues|GQ (USA) magazine issues|GQ magazine issues|English magazine issues|Fashion magazine issues|Men's interest magazine issues}}


CREATE TABLE Articles
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SiteId int NOT NULL,
	Culture nvarchar(20) NOT NULL,
	Title nvarchar(1000) NOT NULL,
	UrlSlug nvarchar(1000) NOT NULL,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateDeleted datetime2(7) NULL
);

CREATE NONCLUSTERED INDEX IX_Articles_UrlSlug
ON Articles (UrlSlug);

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

CREATE NONCLUSTERED INDEX IX_ArticleRevisions_ArticleId
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
	CreatedByAspNetUserId nvarchar(450) NOT NULL,
	DateCreated datetime2(7) NOT NULL,
	DateModified datetime2(7) NOT NULL,
	DateDeleted datetime2(7) NULL
);

CREATE TABLE ArticleTalkSubjects
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SiteId int NOT NULL,
	[Language] nvarchar(20) NOT NULL,
	ArticleId int NOT NULL,
	[Subject] nvarchar(300) NOT NULL,
	UrlSlug nvarchar(300) NOT NULL,
	[Text] nvarchar(2000) NOT NULL,
	HasBeenEdited bit NOT NULL DEFAULT 0,
	CreatedByAspNetUserId nvarchar(450) NOT NULL,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateModified datetime2(7) NULL,
	DateDeleted datetime2(7) NULL
);

INSERT ArticleTalkSubjects ( SiteId, [Language], ArticleTitle, [Subject], CreatedByAspNetUserId )
VALUES ( 1, 'en', 'GQ (USA) - November 2020', 'Is this the Man of the Year issue?', '7240be61-df81-46f9-8152-6a48b96abc40' );

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

INSERT ArticleTalkSubjectPosts( ArticleTalkSubjectId, ParentTalkSubjectPostId, [Text], CreatedByAspNetUserId )
VALUES ( 1, NULL, 'Yes I think it is the MOTY issue. Can anyone confirm?', '7240be61-df81-46f9-8152-6a48b96abc40' );
INSERT ArticleTalkSubjectPosts( ArticleTalkSubjectId, ParentTalkSubjectPostId, [Text], CreatedByAspNetUserId )
VALUES ( 1, NULL, 'OK, I found the answer. It is *not* the MOTY issue!', '7240be61-df81-46f9-8152-6a48b96abc40' );


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
