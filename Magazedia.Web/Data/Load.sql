-- Default QINGCHARLES user
INSERT  AspNetUsers ( Id, AccessFailedCount, ConcurrencyStamp, Email, EmailConfirmed, LockoutEnabled, LockoutEnd, NormalizedEmail, NormalizedUserName, PasswordHash, PhoneNumber, PhoneNumberConfirmed, SecurityStamp, TwoFactorEnabled, UserName)
VALUES ('7240be61-df81-46f9-8152-6a48b96abc40', 0, '3050d1a5-9a2c-4d80-a1be-58d6a79191a6', 'hello@magazedia.site', 1, 1, NULL, 'HELLO@MAGAZEDIA.SITE', 'QINGCHARLES', 'AQAAAAIAAYagAAAAEBB+4D8cFi426Wgg8vfO/4cBgISBNJWZVfnWXwSSv5pq171AC3LGZgH6qlvFTj25Dw==', NULL, 0, 'PSJMEDAFBWDTOWQG7J4FYSGOJ7HM3M4I', 0, 'QINGCHARLES');

INSERT Articles (Title, UrlSlug, SiteId, Culture)
VALUES ( N'@QINGCHARLES', N'@QINGCHARLES', 1, 'en' );
INSERT ArticleRevisions (ArticleId, [Text], RevisionReason, CreatedByAspNetUserId)
VALUES (SCOPE_IDENTITY(), N'Hello. I''m QINGCHARLES and I made this. This web site thing.', N'Article created.', '7240be61-df81-46f9-8152-6a48b96abc40');
