--
--
SELECT		ArticleTalkSubjectPosts.*, AspNetUsers.UserName AS CreatedByAspNetUsername
								FROM		ArticleTalkSubjectPosts
								INNER JOIN	AspNetUsers ON ArticleTalkSubjectPosts.CreatedByAspNetUserId = AspNetUsers.Id
								WHERE		ArticleTalkSubjectPosts.ArticleTalkSubjectId = 1 AND
											ArticleTalkSubjectPosts.DateDeleted IS NULL
								ORDER BY	ArticleTalkSubjectPosts.DateCreated DESC

CREATE TABLE AspNetRoles( Id nvarchar(450) NOT NULL PRIMARY KEY, ConcurrencyStamp nvarchar(max) NULL, [Name] nvarchar(256) NULL, NormalizedName nvarchar(256) NULL );
CREATE TABLE AspNetRoleClaims( Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, ClaimType nvarchar(max) NULL, ClaimValue nvarchar(max) NULL, RoleId nvarchar(450) NOT NULL );
CREATE TABLE AspNetUsers( Id nvarchar(450) NOT NULL PRIMARY KEY, AccessFailedCount int NOT NULL, ConcurrencyStamp nvarchar(max) NULL, Email nvarchar(256) NULL, EmailConfirmed bit NOT NULL, LockoutEnabled bit NOT NULL, LockoutEnd datetimeoffset(7) NULL, NormalizedEmail nvarchar(256) NULL, NormalizedUserName nvarchar(256) NULL, PasswordHash nvarchar(max) NULL, PhoneNumber nvarchar(max) NULL, PhoneNumberConfirmed bit NOT NULL, SecurityStamp nvarchar(max) NULL, TwoFactorEnabled bit NOT NULL, UserName nvarchar(256) NULL );
CREATE TABLE AspNetUserClaims( Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, ClaimType nvarchar(max) NULL, ClaimValue nvarchar(max) NULL, UserId nvarchar(450) NOT NULL );
CREATE TABLE AspNetUserLogins( LoginProvider nvarchar(450) NOT NULL, ProviderKey nvarchar(450) NOT NULL, ProviderDisplayName nvarchar(max) NULL, UserId nvarchar(450) NOT NULL, CONSTRAINT PK_AspNetUsersLogin PRIMARY KEY( LoginProvider, ProviderKey ) );
CREATE TABLE AspNetUserRoles( UserId nvarchar(450) NOT NULL, RoleId nvarchar(450) NOT NULL, CONSTRAINT PK_AspNetUsersRole PRIMARY KEY( UserId, RoleId ) );
CREATE TABLE AspNetUserTokens( UserId nvarchar(450) NOT NULL, LoginProvider nvarchar(450) NOT NULL, [Name] nvarchar(450) NOT NULL, [Value] nvarchar(max) NULL, CONSTRAINT PK_AspNetUsersToken PRIMARY KEY( UserId, LoginProvider, [Name] ) );
GO

INSERT  AspNetUsers ( Id, AccessFailedCount, ConcurrencyStamp, Email, EmailConfirmed, LockoutEnabled, LockoutEnd, NormalizedEmail, NormalizedUserName, PasswordHash, PhoneNumber, PhoneNumberConfirmed, SecurityStamp, TwoFactorEnabled, UserName) VALUES ('7240be61-df81-46f9-8152-6a48b96abc40', 0, '3050d1a5-9a2c-4d80-a1be-58d6a79191a6', 'hello@magazedia.site', 1, 1, NULL, 'HELLO@MAGAZEDIA.SITE', 'QINGCHARLES', 'AQAAAAIAAYagAAAAEBB+4D8cFi426Wgg8vfO/4cBgISBNJWZVfnWXwSSv5pq171AC3LGZgH6qlvFTj25Dw==', NULL, 0, 'PSJMEDAFBWDTOWQG7J4FYSGOJ7HM3M4I', 0, 'QINGCHARLES');

CREATE TABLE DownloadUrls
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SiteId int NOT NULL,
	[Filename] nvarchar(1000) NOT NULL,
	Filesize int NOT NULL,
	HashSha256 varbinary(64) NOT NULL,
	DownloadUrlOne nvarchar(2000) NULL,
	DownloadUrlTwo nvarchar(2000) NULL,
	DownloadUrlThree nvarchar(2000) NULL,
	CreatedByAspNetUserId nvarchar(450) NOT NULL,
	DateCreated datetime2(7) NOT NULL,
	DateModified datetime2(7) NOT NULL,
	DateDeleted datetime2(7) NULL
)

sudo docker exec -it sql1 "bash"
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "<YourStrong@Passw0rd>"

-- this needs to be checked
CREATE TABLE DownloadUrls( Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, SiteId int NOT NULL, [Filename] nvarchar(1000) NOT NULL, Filesize int NOT NULL, HashSha256 varbinary(64) NOT NULL, DownloadUrlOne nvarchar(2000) NULL, DownloadUrlTwo nvarchar(2000) NULL, DownloadUrlThree nvarchar(2000) NULL, CreatedByAspNetUserId nvarchar(450) NOT NULL, DateCreated datetime2(7) NOT NULL, DateModified datetime2(7) NOT NULL, DateDeleted datetime2(7) NULL );







The October 2022 issue of GQ features Alexandria Ocasio-Cortez on the cover and includes a conversation about masculinity, power and politics in post-Roe America³. 

(1) GQ, October 2022 – Issues Magazine Shop. https://issuesmagshop.com/products/gq-october-2022.
(2) Issue October 2022 - GQ. https://www.zinio.com/gq-mag/october-2022-i547211.
(3) AOC on the Fight for Abortion Rights and Whether She’ll Ever Be .... https://www.gq.com/story/alexandria-ocasio-cortez-october-cover-profile.




CREATE TABLE ArticleTalkSubjects
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SiteId int NOT NULL,
	[Language] nvarchar(20) NOT NULL,
	ArticleTitle nvarchar(1000) NOT NULL,
	[Subject] nvarchar(300) NOT NULL,
	UrlSlug nvarchar(300) NOT NULL,
	[Text] nvarchar(2000) NOT NULL,
	HasBeenEdited bit NOT NULL DEFAULT 0,
	CreatedByAspNetUserId nvarchar(450) NOT NULL,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateModified datetime2(7) NULL,
	DateDeleted datetime2(7) NULL
)

CREATE TABLE ArticleTalkSubjects( Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, ArticleTitle nvarchar(1000) NOT NULL, SiteId int NOT NULL, [Language] nvarchar(20) NOT NULL, [Subject] nvarchar(300) NOT NULL, UrlSlug nvarchar(300) NOT NULL, [Text] nvarchar(2000) NOT NULL, HasBeenEdited bit NOT NULL DEFAULT 0, CreatedByAspNetUserId nvarchar(450) NOT NULL, DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(), DateModified datetime2(7) NULL, DateDeleted datetime2(7) NULL );
INSERT ArticleTalkSubjects ( SiteId, [Language], ArticleTitle, [Subject], CreatedByAspNetUserId ) VALUES ( 1, 'en', 'GQ (USA) - November 2020', 'Is this the Man of the Year issue?', '7240be61-df81-46f9-8152-6a48b96abc40' );


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
)

CREATE TABLE ArticleTalkSubjectPosts( Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, ArticleTalkSubjectId int NOT NULL, ParentTalkSubjectPostId int NULL, [Text] nvarchar(2000) NOT NULL, HasBeenEdited bit NOT NULL DEFAULT 0, CreatedByAspNetUserId nvarchar(450) NOT NULL, DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(), DateModified datetime2(7) NULL, DateDeleted datetime2(7) NULL );
INSERT ArticleTalkSubjectPosts( ArticleTalkSubjectId, ParentTalkSubjectPostId, [Text], CreatedByAspNetUserId ) VALUES ( 1, NULL, 'Yes I think it is the MOTY issue. Can anyone confirm?', '7240be61-df81-46f9-8152-6a48b96abc40' );
INSERT ArticleTalkSubjectPosts( ArticleTalkSubjectId, ParentTalkSubjectPostId, [Text], CreatedByAspNetUserId ) VALUES ( 1, NULL, 'OK, I found the answer. It is the MOTY issue!', '7240be61-df81-46f9-8152-6a48b96abc40' );


CREATE TABLE ArticleCultureLinks 
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SiteId int NOT NULL,
	ArticleId int NOT NULL,
	ArticleCultureLinkGroupId int NOT NULL,
	DateDeleted datetime2(7) NULL
);

DECLARE @ArticleId INT;

INSERT ArticleLanguageLinks ( SiteId, ArticleLanguageGroupId, Culture, ArticleTitle )
VALUES ( 1, 1, N'ja', N'GQ (アメリカ)' );

INSERT ArticleLanguageLinks ( SiteId, ArticleLanguageGroupId, Culture, ArticleTitle )
VALUES ( 1, 1, N'ar', N'جي كيو (الأمريكي)' );

INSERT Articles (Title, UrlSlug, SiteId, Culture)
VALUES (N'GQ (USA)', N'gq-usa', 1, 'en');

INSERT ArticleRevisions (ArticleId, [Text], RevisionReason, CreatedByAspNetUserId)
VALUES (@ArticleId, N'GQ (formerly Gentlemen’s Quarterly and Apparel Arts) is an American international monthly men’s magazine based in New York City and founded in 1931. The publication focuses on fashion, style, and culture for men, though articles on food, movies, fitness, sex, music, travel, celebrities, sports, technology, and books are also featured.{{Tag GQ franchises}}{{Tag GQ magazines}}{{Tag Magazines in English}}{{Tag Magazines founded in 1931}}{{Tag Magazines founded in the 1900s}}{{Tag Magazines founded in the 1930s}}{{Tag Fashion magazines}}{{Tag Men’s magazines}}', N'Article created.', '7240be61-df81-46f9-8152-6a48b96abc40');

INSERT ArticleLanguageLinks ( SiteId, ArticleLanguageGroupId, ArticleId )
VALUES ( 1, 1, @ArticleId );

INSERT Articles (Title, UrlSlug, SiteId, Culture)
VALUES (N'GQ (USA) - November 2020', N'gq-usa-november-2020', 1, 'en');

INSERT ArticleRevisions (ArticleId, [Text], RevisionReason, CreatedByAspNetUserId)
VALUES (SCOPE_IDENTITY(), N'The third issue of [GQ (USA)] published in 2020.', N'Article created.', '7240be61-df81-46f9-8152-6a48b96abc40');

DECLARE @ArticleId INT;

INSERT Articles (Title, UrlSlug, SiteId, Culture)
VALUES (N'GQ (USA) - November 2020', N'gq-usa-november-2020', 1, 'en');

SET @ArticleId = SCOPE_IDENTITY();

INSERT ArticleRevisions (ArticleId, [Text], RevisionReason, CreatedByAspNetUserId)
VALUES (@ArticleId, N'The third issue of [GQ (USA)](gq-usa) publisshed in 2020.', N'Article created.', '7240be61-df81-46f9-8152-6a48b96abc40');

INSERT ArticleRevisions (ArticleId, [Text], RevisionReason, CreatedByAspNetUserId)
VALUES (@ArticleId, N'The third issue of [GQ (USA)](gq-usa) published in 2020.', N'Typo fixed.', '7240be61-df81-46f9-8152-6a48b96abc40');


SELECT ar.*
FROM ArticleRevisions ar
INNER JOIN Articles a ON ar.ArticleId = a.Id
WHERE a.UrlSlug = 'gq-usa-november-2020';


CREATE TABLE Articles
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SiteId int NOT NULL,
	Culture nvarchar(20) NOT NULL,
	Title nvarchar(1000) NOT NULL,
	UrlSlug nvarchar(1000) NOT NULL,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateDeleted datetime2(7) NULL
)

CREATE TABLE ArticleRevisions
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ArticleId int NOT NULL,
	[Text] nvarchar(max) NOT NULL,
	RevisionReason nvarchar(1000) NOT NULL,
  	CreatedByAspNetUserId nvarchar(450) NOT NULL,
	DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(),
	DateDeleted datetime2(7) NULL
)



GO

CREATE TABLE Articles( Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, SiteId int NOT NULL, Language nvarchar(20) NOT NULL, Title nvarchar(1000) NOT NULL, UrlSlug nvarchar(1000) NOT NULL, [Text] nvarchar(max) NOT NULL, RevisionReason nvarchar(1000) NOT NULL, CreatedByAspNetUserId nvarchar(450) NOT NULL, DateCreated datetime2(7) NOT NULL DEFAULT GETDATE(), DateDeleted datetime2(7) NULL );
 
GO 


--INSERT Articles (Title, UrlSlug, [Text], RevisionReason, CreatedByAspNetUserId, SiteId, Language) VALUES ("GQ (USA) - November 2020", "gq-usa-november-2020", "The November 2020 issue of [GQ (USA) magazine](gq-usa) features [Timothée Chalamet](https://en.wikipedia.org/wiki/Timoth%C3%A9e_Chalamet) on the cover. The issue includes a long interview with Chalamet, who discusses his upcoming role in the sci-fi film [Dune](https://en.wikipedia.org/wiki/Dune_(2021_film)), his thoughts on fame, and his plans for the future. The issue also features profiles of skateboarder [Sean Pablo](https://en.wikipedia.org/wiki/Sean_Pablo), actor [Paul Mescal](https://en.wikipedia.org/wiki/Paul_Mescal), and producers [Timbaland](https://en.wikipedia.org/wiki/Timbaland) and [Swiss Beatz](https://en.wikipedia.org/wiki/Swizz_Beatz).{{Tag GQ (USA) magazine issues}}{{Tag GQ magazine issues}}{{Tag Magazine issues published in November 2020}}{{Tag Magazine issues published in 2020}}{{Tag Magazine issues in English}}{{Tag Magazine issues with covers featuring Timothée Chalamet}}{{Tag Magazine issues featuring Timothée Chalamet}}{{Tag Magazine issues featuring Timbaland}}{{Tag Magazine issues featuring Swizz Beats}}{{Tag Magazine issues featuring Sean Pablo}}{{Tag Magazine issues featuring Paul Mescal}}{{Tag Magazine issues featuring the movie Dune (2021)}}", "New article.", "Blahuser", 1, "en");


CREATE TABLE AspNetRoles( Id nvarchar(450) NOT NULL PRIMARY KEY, ConcurrencyStamp nvarchar(max) NULL, [Name] nvarchar(256) NULL, NormalizedName nvarchar(256) NULL );
CREATE TABLE AspNetRoleClaims( Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, ClaimType nvarchar(max) NULL, ClaimValue nvarchar(max) NULL, RoleId nvarchar(450) NOT NULL );
CREATE TABLE AspNetUsers( Id nvarchar(450) NOT NULL PRIMARY KEY, AccessFailedCount int NOT NULL, ConcurrencyStamp nvarchar(max) NULL, Email nvarchar(256) NULL, EmailConfirmed bit NOT NULL, LockoutEnabled bit NOT NULL, LockoutEnd datetimeoffset(7) NULL, NormalizedEmail nvarchar(256) NULL, NormalizedUserName nvarchar(256) NULL, PasswordHash nvarchar(max) NULL, PhoneNumber nvarchar(max) NULL, PhoneNumberConfirmed bit NOT NULL, SecurityStamp nvarchar(max) NULL, TwoFactorEnabled bit NOT NULL, UserName nvarchar(256) NULL );
CREATE TABLE AspNetUserClaims( Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, ClaimType nvarchar(max) NULL, ClaimValue nvarchar(max) NULL, UserId nvarchar(450) NOT NULL );
CREATE TABLE AspNetUserLogins( LoginProvider nvarchar(450) NOT NULL, ProviderKey nvarchar(450) NOT NULL, ProviderDisplayName nvarchar(max) NULL, UserId nvarchar(450) NOT NULL, CONSTRAINT PK_AspNetUsersLogin PRIMARY KEY( LoginProvider, ProviderKey ) );
CREATE TABLE AspNetUserRoles( UserId nvarchar(450) NOT NULL, RoleId nvarchar(450) NOT NULL, CONSTRAINT PK_AspNetUsersRole PRIMARY KEY( UserId, RoleId ) );
CREATE TABLE AspNetUserTokens( UserId nvarchar(450) NOT NULL, LoginProvider nvarchar(450) NOT NULL, [Name] nvarchar(450) NOT NULL, [Value] nvarchar(max) NULL, CONSTRAINT PK_AspNetUsersToken PRIMARY KEY( UserId, LoginProvider, [Name] ) );
GO


UPDATE CRLF SET col1 = REPLACE(col1, '@', CHAR(13))


128.140.38.19
mguiJLbasW3AifV4tCxN





INSERT Articles (Title, UrlSlug, [Text], RevisionReason, CreatedByAspNetUserId, SiteId, Language) VALUES ('GQ (USA)', 'gq-usa', 'GQ (formerly Gentlemen''s Quarterly and Apparel Arts) is an American international monthly men''s magazine based in New York City and founded in 1931. The publication focuses on fashion, style, and culture for men, though articles on food, movies, fitness, sex, music, travel, celebrities'' sports, technology, and books are also featured.{{Tag GQ franchises}}{{Tag GQ magazines}}{{Tag Magazines in English}}{{Tag Magazines founded in 1931}}{{Tag Magazines founded in the 1900s}}{{Tag Magazines founded in the 1930s}}{{Tag Fashion magazines}}{{Tag Men’s magazines}}', 'New article.', 'Blahuser', 1, 'en');

INSERT Article (Title, UrlSlug, [Text], CreatedByAspNetUserId, SiteId, Language) VALUES ("GQ (Japan)", "gq-japan", "GQ Japan", "Blahuser", 1, "en");

INSERT Article (Title, UrlSlug, [Text], CreatedByAspNetUserId, SiteId, Language) VALUES ("GQ (Japan)", "gq-japan", "GQ Japan", "Blahuser", 1, "ja");


INSERT Article (Title, UrlSlug, [Text], CreatedByAspNetUserId, SiteId, Language) VALUES ("GQ (USA)", "gq-usa", GQ هي مجلة أمريكية شهرية دولية للرجال مقرها في مدينة نيويورك وتأسست في عام 1931. يركز المنشور على الموضة والأناقة والثقافة للرجال ، من خلال مقالات عن الطعام والأفلام واللياقة والجنس والموسيقى والسفر ورياضة المشاهير ، التكنولوجيا والكتب واردة أيضا.{{Tag Fashion magazines}}", "Blahuser", 1, "ar");

GQ هي مجلة أمريكية شهرية دولية للرجال مقرها في مدينة نيويورك وتأسست في عام 1931. يركز المنشور على الموضة والأناقة والثقافة للرجال ، من خلال مقالات عن الطعام والأفلام واللياقة والجنس والموسيقى والسفر ورياضة المشاهير ، التكنولوجيا والكتب واردة أيضا.
GQ は、1931 年に創刊されたニューヨーク市に本拠を置くアメリカの国際的な月刊男性誌です。この出版物は、男性のファッション、スタイル、文化に焦点を当てていますが、食べ物、映画、フィットネス、セックス、音楽、旅行、有名人のスポーツ、技術、本も紹介されています。


GQ is an American international monthly men's magazine based in New York City and founded in 1931. The publication focuses on fashion, style, and culture for men, though articles on food, movies, fitness, sex, music, travel, celebrities' sports, technology, and books are also featured.

GQ is one of the most popular men's magazines in the world, with a circulation of over 900,000. The magazine has been praised for its stylish photography, its in-depth interviews with celebrities and thought leaders, and its coverage of the latest trends in fashion, grooming, and lifestyle.

GQ has been home to some of the most iconic covers in men's magazine history, including covers featuring David Beckham, Brad Pitt, Ryan Reynolds, and Barack Obama. The magazine has also won numerous awards, including the National Magazine Award for General Excellence, the American Society of Magazine Editors Award for General Excellence, and the Clio Award for Best Magazine Advertising Campaign.

GQ (formerly Gentlemen's Quarterly and Apparel Arts) is an American international monthly men's magazine based in New York City and founded in 1931. The publication focuses on fashion, style, and culture for men, though articles on food, movies, fitness, sex, music, travel, celebrities' sports, technology, and books are also featured.

GQ is one of the most popular men's magazines in the world, with a circulation of over 900,000 copies. The magazine has been praised for its high-quality photography, its in-depth interviews with celebrities and other notable figures, and its stylish editorial spreads.

In recent years, GQ has become more socially conscious, publishing articles on issues such as gender equality, mental health, and the environment. The magazine has also been at the forefront of the conversation about men's fashion, helping to redefine what it means to be a stylish man in the 21st century.

GQ is a must-read for any man who wants to stay up-to-date on the latest trends in fashion, style, and culture. The magazine is also a valuable resource for men who are looking for inspiration on how to live a more well-rounded and fulfilling life.

Here are some of the things that make GQ unique:

High-quality photography: GQ is known for its stunning photography, which features some of the most talented photographers in the world.
In-depth interviews: GQ's interviews with celebrities and other notable figures are some of the most insightful and revealing in print.
Stylish editorial spreads: GQ's editorial spreads are always stylish and inspiring, and they offer a unique perspective on men's fashion.
Socially conscious: GQ is committed to covering important issues that affect men, such as gender equality, mental health, and the environment.
Fashion-forward: GQ is at the forefront of the conversation about men's fashion, and it helps to redefine what it means to be a stylish man in the 21st century.


GQ Style magazine is a spin-off publication of GQ magazine, which is focused specifically on men's fashion and style. It was first launched in the UK in 2005 and has since expanded to several other countries, including the US, Australia, and Germany.~GQ Style is published twice a year and features in-depth articles and photo shoots that showcase the latest trends and styles in men's fashion. It covers everything from high-end designer labels to streetwear and emerging fashion trends, as well as grooming and lifestyle topics. The magazine also includes interviews and profiles of fashion designers, models, and other influential figures in the fashion industry.~One of the key features of GQ Style is its emphasis on high-quality photography and art direction. The magazine often features visually striking photo shoots that are intended to inspire and inform readers about the latest fashion trends and styles.~Overall, GQ Style is a must-read publication for men who are interested in fashion and style, as well as those who work in the fashion industry. Its combination of high-quality photography, in-depth articles, and expert analysis make it a valuable resource for anyone looking to stay up-to-date on the latest trends and styles in men's fashion.

INSERT Articles (Title, UrlSlug, [Text], RevisionReason, CreatedByAspNetUserId, SiteId, Language) VALUES (N'GQ (USA) - November 2020', 'gq-usa-november-2020', N'The third issue of [GQ (USA)] published in 2020.', N'Article created', '7240be61-df81-46f9-8152-6a48b96abc40', 1, 'en');

GO

 
 
INSERT Article (Title, UrlSlug, [Text], CreatedByAspNetUserId, SiteId) VALUES ("GQ (USA) - November 2020", "gq-usa-november-2020", "The third issue of [GQ (USA)] published in 2020.", "Blahuser", 1);
 
GO 
 
CREATE TABLE AspNetRole( Id nvarchar(450) NOT NULL PRIMARY KEY, ConcurrencyStamp nvarchar(max) NULL, [Name] nvarchar(256) NULL, NormalizedName nvarchar(256) NULL );
CREATE TABLE AspNetRoleClaim( Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, ClaimType nvarchar(max) NULL, ClaimValue nvarchar(max) NULL, RoleId nvarchar(450) NOT NULL );
CREATE TABLE AspNetUser( Id nvarchar(450) NOT NULL PRIMARY KEY, AccessFailedCount int NOT NULL, ConcurrencyStamp nvarchar(max) NULL, Email nvarchar(256) NULL, EmailConfirmed bit NOT NULL, LockoutEnabled bit NOT NULL, LockoutEnd datetimeoffset(7) NULL, NormalizedEmail nvarchar(256) NULL, NormalizedUserName nvarchar(256) NULL, PasswordHash nvarchar(max) NULL, PhoneNumber nvarchar(max) NULL, PhoneNumberConfirmed bit NOT NULL, SecurityStamp nvarchar(max) NULL, TwoFactorEnabled bit NOT NULL, UserName nvarchar(256) NULL );
CREATE TABLE AspNetUserClaim( Id int IDENTITY(1,1) NOT NULL PRIMARY KEY, ClaimType nvarchar(max) NULL, ClaimValue nvarchar(max) NULL, UserId nvarchar(450) NOT NULL );
CREATE TABLE AspNetUserLogin( LoginProvider nvarchar(450) NOT NULL, ProviderKey nvarchar(450) NOT NULL, ProviderDisplayName nvarchar(max) NULL, UserId nvarchar(450) NOT NULL, CONSTRAINT PK_AspNetUserLogin PRIMARY KEY( LoginProvider, ProviderKey ) );
CREATE TABLE AspNetUserRole( UserId nvarchar(450) NOT NULL, RoleId nvarchar(450) NOT NULL, CONSTRAINT PK_AspNetUserRole PRIMARY KEY( UserId, RoleId ) );
CREATE TABLE AspNetUserToken( UserId nvarchar(450) NOT NULL, LoginProvider nvarchar(450) NOT NULL, [Name] nvarchar(450) NOT NULL, [Value] nvarchar(max) NULL, CONSTRAINT PK_AspNetUserToken PRIMARY KEY( UserId, LoginProvider, [Name] ) );


 

 



CREATE TABLE AspNetRole
(
	Id nvarchar(450) NOT NULL PRIMARY KEY,
	ConcurrencyStamp nvarchar(max) NULL,
	[Name] nvarchar(256) NULL,
	NormalizedName nvarchar(256) NULL
)

GO

CREATE TABLE AspNetRoleClaim
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ClaimType nvarchar(max) NULL,
	ClaimValue nvarchar(max) NULL,
	RoleId nvarchar(450) NOT NULL
)

GO

CREATE TABLE AspNetUser
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
)

GO

CREATE TABLE AspNetUserClaim
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ClaimType nvarchar(max) NULL,
	ClaimValue nvarchar(max) NULL,
	UserId nvarchar(450) NOT NULL
)

GO

CREATE TABLE AspNetUserLogin
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
)

GO

CREATE TABLE AspNetUserRole
(
	UserId nvarchar(450) NOT NULL,
	RoleId nvarchar(450) NOT NULL,

	CONSTRAINT PK_AspNetUserRole PRIMARY KEY
	(
		UserId,
		RoleId
	)
)

GO

CREATE TABLE AspNetUserToken
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
)

GO

