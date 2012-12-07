if exists (select * from sysobjects where id = OBJECT_ID('[AdminList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [AdminList]

CREATE TABLE [AdminList] (
[AdaminID] [int]  IDENTITY (1, 1)  NOT NULL,
[AdminGUID] [uniqueidentifier]  NULL,
[AdminName] [nvarchar]  (50) NULL,
[AdminPassword] [nvarchar]  (50) NULL,
[IsEnable] [int]  NULL,
[PublishDate] [datetime]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nvarchar]  (50) NULL)

ALTER TABLE [AdminList] WITH NOCHECK ADD  CONSTRAINT [PK_AdminList] PRIMARY KEY  NONCLUSTERED ( [AdaminID] )
SET IDENTITY_INSERT [AdminList] ON

INSERT [AdminList] ([AdaminID],[AdminName],[AdminPassword],[IsEnable],[PublishDate]) VALUES ( 1,N'admin',N'123',1,N'2012-10-19 15:26:10')
INSERT [AdminList] ([AdaminID],[AdminName],[AdminPassword],[IsEnable],[PublishDate]) VALUES ( 2,N'jones',N'123',1,N'2012-10-20 10:31:20')
INSERT [AdminList] ([AdaminID],[AdminName],[AdminPassword],[IsEnable],[PublishDate]) VALUES ( 3,N'test',N'test123',1,N'2012-10-20 13:13:14')
INSERT [AdminList] ([AdaminID],[AdminName],[AdminPassword],[IsEnable],[PublishDate]) VALUES ( 4,N'test11',N'111',1,N'2012-10-20 13:52:49')
INSERT [AdminList] ([AdaminID],[AdminName],[AdminPassword],[IsEnable],[PublishDate]) VALUES ( 5,N'admintest',N'123123',1,N'2012-10-20 13:53:21')
INSERT [AdminList] ([AdaminID],[AdminName],[AdminPassword],[IsEnable],[PublishDate]) VALUES ( 8,N'lich',N'123',1,N'2012-10-21 7:21:02')
INSERT [AdminList] ([AdaminID],[AdminName],[AdminPassword],[IsEnable],[PublishDate]) VALUES ( 10,N'wj1',N'123',1,N'2012-11-4 6:07:51')

SET IDENTITY_INSERT [AdminList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[BannerList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [BannerList]

CREATE TABLE [BannerList] (
[BannerID] [int]  IDENTITY (1, 1)  NOT NULL,
[BannerGUID] [uniqueidentifier]  NULL,
[BannerImage] [nvarchar]  (200) NULL,
[BannerLinks] [nvarchar]  (200) NULL,
[IsEnable] [int]  NULL,
[IsHot] [int]  NULL,
[PublishDate] [datetime]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nvarchar]  (50) NULL)

ALTER TABLE [BannerList] WITH NOCHECK ADD  CONSTRAINT [PK_BannerList] PRIMARY KEY  NONCLUSTERED ( [BannerID] )
SET IDENTITY_INSERT [BannerList] ON


SET IDENTITY_INSERT [BannerList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[CareersList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [CareersList]

CREATE TABLE [CareersList] (
[CareersID] [int]  IDENTITY (1, 1)  NOT NULL,
[CareersGUID] [uniqueidentifier]  NULL,
[DictionaryKey] [nvarchar]  (50) NULL,
[DepartmentName] [nvarchar]  (50) NULL,
[CareersTitle] [nvarchar]  (50) NULL,
[CareersCount] [int]  NULL,
[WorkAdd] [nvarchar]  (100) NULL,
[Educational] [nvarchar]  (50) NULL,
[Salary] [nvarchar]  (50) NULL,
[WorkExperience] [nvarchar]  (MAX) NULL,
[Responsibilities] [nvarchar]  (MAX) NULL,
[Qualifications] [nvarchar]  (MAX) NULL,
[EMail] [nvarchar]  (50) NULL,
[Telephone] [nvarchar]  (50) NULL,
[ContactMan] [nvarchar]  (50) NULL,
[IsEnable] [int]  NULL,
[IsHot] [int]  NULL,
[PublishDate] [datetime]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nvarchar]  (50) NULL,
[Other03] [nvarchar]  (50) NULL)

ALTER TABLE [CareersList] WITH NOCHECK ADD  CONSTRAINT [PK_CareersList] PRIMARY KEY  NONCLUSTERED ( [CareersID] )
SET IDENTITY_INSERT [CareersList] ON


SET IDENTITY_INSERT [CareersList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[CustomerShowList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [CustomerShowList]

CREATE TABLE [CustomerShowList] (
[CustomerShowID] [int]  IDENTITY (1, 1)  NOT NULL,
[CustomerShowName] [nvarchar]  (50) NULL,
[CustomerShowLogo] [nvarchar]  (200) NULL,
[CustomerShowTage] [nvarchar]  (50) NULL,
[PublihDate] [datetime]  NULL,
[IsEnable] [int]  NULL,
[IsHot] [int]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nvarchar]  (50) NULL,
[Other03] [nvarchar]  (50) NULL,
[Other04] [nvarchar]  (50) NULL)

ALTER TABLE [CustomerShowList] WITH NOCHECK ADD  CONSTRAINT [PK_CustomerShowList] PRIMARY KEY  NONCLUSTERED ( [CustomerShowID] )
SET IDENTITY_INSERT [CustomerShowList] ON


SET IDENTITY_INSERT [CustomerShowList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[DictionaryList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [DictionaryList]

CREATE TABLE [DictionaryList] (
[DictionaryListID] [int]  IDENTITY (1, 1)  NOT NULL,
[DictionaryKey] [nvarchar]  (50) NULL,
[DictionaryValue] [nvarchar]  (200) NULL,
[DictionaryCategory] [nvarchar]  (50) NULL,
[DictionaryDesc] [nvarchar]  (MAX) NULL,
[OrderNumber] [int]  NULL,
[IsInner] [int]  NULL,
[IsEnable] [int]  NULL,
[PublishDate] [datetime]  NULL)

ALTER TABLE [DictionaryList] WITH NOCHECK ADD  CONSTRAINT [PK_DictionaryList] PRIMARY KEY  NONCLUSTERED ( [DictionaryListID] )
SET IDENTITY_INSERT [DictionaryList] ON

INSERT [DictionaryList] ([DictionaryListID],[DictionaryKey],[DictionaryValue],[DictionaryCategory],[DictionaryDesc],[OrderNumber],[IsInner],[IsEnable],[PublishDate]) VALUES ( 1,N'changtuzhan',N'四方长途站店',N'StoreType',N'四方长途站店',1,0,1,N'2012-12-5 2:30:00')
INSERT [DictionaryList] ([DictionaryListID],[DictionaryKey],[DictionaryValue],[DictionaryCategory],[DictionaryDesc],[OrderNumber],[IsInner],[IsEnable],[PublishDate]) VALUES ( 2,N'gaokeyuan',N'崂山高科园店',N'StoreType',N'崂山高科园店',2,0,1,N'2012-12-5 2:31:00')
INSERT [DictionaryList] ([DictionaryListID],[DictionaryKey],[DictionaryValue],[DictionaryCategory],[DictionaryDesc],[OrderNumber],[IsInner],[IsEnable],[PublishDate]) VALUES ( 3,N'kaifaqu',N'黄岛开发区店',N'StoreType',N'黄岛开发区店',3,0,1,N'2012-12-5 2:32:00')
INSERT [DictionaryList] ([DictionaryListID],[DictionaryKey],[DictionaryValue],[DictionaryCategory],[DictionaryDesc],[OrderNumber],[IsInner],[IsEnable],[PublishDate]) VALUES ( 4,N'hetao',N'城阳河套店',N'StoreType',N'城阳河套店',4,0,1,N'2012-12-5 2:33:00')
INSERT [DictionaryList] ([DictionaryListID],[DictionaryKey],[DictionaryValue],[DictionaryCategory],[DictionaryDesc],[OrderNumber],[IsInner],[IsEnable],[PublishDate]) VALUES ( 5,N'quanzhi',N'全职',N'CareersType',N'全职',1,0,1,N'2012-12-5 2:34:00')
INSERT [DictionaryList] ([DictionaryListID],[DictionaryKey],[DictionaryValue],[DictionaryCategory],[DictionaryDesc],[OrderNumber],[IsInner],[IsEnable],[PublishDate]) VALUES ( 6,N'jianzhi',N'兼职',N'CareersType',N'兼职',2,0,1,N'2012-12-5 2:35:00')

SET IDENTITY_INSERT [DictionaryList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[JobCategoryList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [JobCategoryList]

CREATE TABLE [JobCategoryList] (
[JobCategoryID] [int]  IDENTITY (1, 1)  NOT NULL,
[JobCategoryGUID] [uniqueidentifier]  NULL,
[JobCategoryName] [nvarchar]  (50) NULL,
[JobCount] [int]  NULL,
[PublishDate] [datetime]  NULL,
[IsEnable] [int]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nvarchar]  (50) NULL)

ALTER TABLE [JobCategoryList] WITH NOCHECK ADD  CONSTRAINT [PK_JobCategoryList] PRIMARY KEY  NONCLUSTERED ( [JobCategoryID] )
SET IDENTITY_INSERT [JobCategoryList] ON

INSERT [JobCategoryList] ([JobCategoryID],[JobCategoryName],[PublishDate],[IsEnable]) VALUES ( 1,N'普工',N'2012-11-29 11:58:50',1)
INSERT [JobCategoryList] ([JobCategoryID],[JobCategoryName],[PublishDate],[IsEnable]) VALUES ( 2,N'技工',N'2012-11-29 11:59:22',1)

SET IDENTITY_INSERT [JobCategoryList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[JobKindList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [JobKindList]

CREATE TABLE [JobKindList] (
[JobKindID] [int]  IDENTITY (1, 1)  NOT NULL,
[JobCategoryGUID] [uniqueidentifier]  NULL,
[JobKindName] [nvarchar]  (50) NULL,
[JobCount] [int]  NULL,
[IsHot] [int]  NULL,
[IsEnable] [int]  NULL,
[PublishDate] [datetime]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nvarchar]  (50) NULL,
[Other03] [int]  NULL)

ALTER TABLE [JobKindList] WITH NOCHECK ADD  CONSTRAINT [PK_JobKindList] PRIMARY KEY  NONCLUSTERED ( [JobKindID] )
SET IDENTITY_INSERT [JobKindList] ON

INSERT [JobKindList] ([JobKindID],[JobKindName],[IsHot],[IsEnable],[PublishDate]) VALUES ( 2,N'钳工',1,1,N'2012-11-30 8:34:59')

SET IDENTITY_INSERT [JobKindList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[LinkList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [LinkList]

CREATE TABLE [LinkList] (
[LinkID] [int]  IDENTITY (1, 1)  NOT NULL,
[LinkTitle] [nvarchar]  (50) NULL,
[LinkTarget] [nvarchar]  (200) NULL,
[LinkImage] [nvarchar]  (200) NULL,
[PublishDate] [datetime]  NULL,
[LinkType] [int]  NULL,
[LinkDesc] [nvarchar]  (MAX) NULL,
[IsEnable] [int]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nvarchar]  (50) NULL,
[Other03] [nvarchar]  (50) NULL)

ALTER TABLE [LinkList] WITH NOCHECK ADD  CONSTRAINT [PK_LinkList] PRIMARY KEY  NONCLUSTERED ( [LinkID] )
SET IDENTITY_INSERT [LinkList] ON


SET IDENTITY_INSERT [LinkList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[MessageList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [MessageList]

CREATE TABLE [MessageList] (
[MessageID] [int]  IDENTITY (1, 1)  NOT NULL,
[MessageGUID] [uniqueidentifier]  NULL,
[PublishUserName] [nvarchar]  (20) NULL,
[PublishContent] [nvarchar]  (MAX) NULL,
[PublishDate] [datetime]  NULL,
[ReplyUserGUID] [uniqueidentifier]  NULL,
[ReplyContent] [nvarchar]  (MAX) NULL,
[ReplyDate] [datetime]  NULL,
[IsReply] [int]  NULL,
[IsEnable] [int]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nvarchar]  (50) NULL,
[Other03] [nvarchar]  (50) NULL,
[Other04] [int]  NULL)

ALTER TABLE [MessageList] WITH NOCHECK ADD  CONSTRAINT [PK_MessageList] PRIMARY KEY  NONCLUSTERED ( [MessageID] )
SET IDENTITY_INSERT [MessageList] ON


SET IDENTITY_INSERT [MessageList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[NewsList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [NewsList]

CREATE TABLE [NewsList] (
[NewsID] [int]  IDENTITY (1, 1)  NOT NULL,
[NewsGUID] [uniqueidentifier]  NULL,
[NewsTypeGUID] [uniqueidentifier]  NULL,
[NewsTitle] [nvarchar]  (100) NULL,
[NewsTag] [nvarchar]  (50) NULL,
[NewsSummary] [nvarchar]  (100) NULL,
[NewsContent] [nvarchar]  (MAX) NULL,
[NewsSource] [nvarchar]  (50) NULL,
[NewsSourceLink] [nvarchar]  (200) NULL,
[PublishUserGUID] [uniqueidentifier]  NULL,
[PublishDate] [datetime]  NULL,
[IsEnable] [int]  NULL,
[IsHot] [int]  NULL,
[ViewCount] [int]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nvarchar]  (50) NULL,
[Other03] [nvarchar]  (50) NULL)

ALTER TABLE [NewsList] WITH NOCHECK ADD  CONSTRAINT [PK_NewsList] PRIMARY KEY  NONCLUSTERED ( [NewsID] )
SET IDENTITY_INSERT [NewsList] ON

INSERT [NewsList] ([NewsID],[NewsTitle],[NewsSummary],[NewsContent],[PublishDate],[IsEnable],[IsHot]) VALUES ( 1,N'1日本驻华大使：“日本是小偷”已深入中国人心',N'1日本驻华大使丹羽宇一郎近日临时回国述职。与7月份因“失言”被召回国时的沉默不同，丹羽20日回到母校名古屋大学发表了演讲。据共同社报道，在提及日中两国在钓鱼岛问题上的对立时，丹羽警告说，这次对立的层次',N'<div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1日本驻华大使丹羽宇一郎近日临时回国述职。与7月份因&#8220;失言&#8221;被召回国时的沉默不同，丹羽20日回到母校名古屋大学发表了演讲。据共同社报道，在提及日中两国在钓鱼岛问题上的对立时，丹羽警告说，这次对立的层次与以往&#8220;完全不同，日方有必要加强认识&#8221;。他称自己非常担忧，在最坏情况下，日中邦交正常化40年来的成果将毁于一旦。</div>
<div>&nbsp;</div>
<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 丹羽称，&#8220;北京的气氛非常紧张，日本政府和国民均未感受到问题的严重性。&#8221;他认为，到11月中国最高领导层完成换届后，两国关系也难以很快好转。丹羽在讲演中还介绍说，日本政府宣布将钓鱼岛&#8220;国有化&#8221;后，他在北京乘车外出时遭到中国人敌视，&#8220;日本是小偷这种想法已植根于中国的年轻人心中，这非常令人忧虑&#8221;。在演讲的最后，丹羽表示日本今后有必要为改善日中关系踏实地付出努力。</p>
<div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 《读卖新闻》分析称，丹羽大使在讲演中不仅流露出强烈的危机感，还有一种焦虑感，尽管他在讲演中说&#8220;日本完全没有必要让出领土权&#8221;，但他却多次提到&#8220;领土问题&#8221;，而日本政府不承认与中国有&#8220;领土问题&#8221;。<br /></div>
<div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 《朝日新闻》引述丹羽的话表示，他18日面见日本首相野田佳彦时，已经把自己感受到的&#8220;危机感&#8221;传递给首相了。《每日新闻》报道称，丹羽在演讲中还提到法德两国的历史，指出&#8220;这两个国家首脑的领导力和信赖关系以及安定的政局，成为他们克服领土问题的一把钥匙。日中关系也有必要创造这种环境，这需要持续不断地努力。&#8221;</div>',N'2012-10-22 4:16:50',1,1)
INSERT [NewsList] ([NewsID],[NewsTitle],[NewsSummary],[NewsContent],[PublishDate],[IsEnable],[IsHot]) VALUES ( 2,N'调查称在一线城市月薪9000元生活才“不惶恐”',N'近日，一份调查引发网友关注，调查内容为“月薪多少会让你在相应的城市生活不惶恐”。数据显示，上海、北京等一线城市需9000元左右，而成都、大连等二线城市则在5000元上下浮动。',N'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 近日，一份调查引发网友关注，调查内容为&#8220;月薪多少会让你在相应的城市生活不惶恐&#8221;。数据显示，上海、北京等一线城市需9000元左右，而成都、大连等二线城市则在5000元上下浮动。 
<p>　　低薪族的&#8220;惶恐&#8221;不难理解，他们的收入大多用在生活上，承受不了太多的变数。然而，很多中高收入的人群在采访中，也表达了他们对生活的&#8220;担忧&#8221;。网友蓝色沙是私企白领，月收入4000元左右。在别人眼里，这是一份不错的收入，但为结婚他买了套房子需要长期还房贷，一个月就近2000元，所以，剩下的钱就只够生活，根本攒不下来。&#8220;现在就怕生病等意外事情发生，看病可是一笔不小的开支。&#8221;</p><!--advertisement code begin--><!--none--><!--advertisement code end-->

<p>　　显然，人们对生活惶恐的原因，用月薪来衡量太简单了。其实，每个人对生活是否满足，源于自己内心的期待。在采访中，谈到对未来生活的期待时，&#8220;家人健康&#8221;，&#8220;放松的心态&#8221;，&#8220;子女孝顺&#8221;等都被网友所提及，而月薪只是其中之一。&#8220;我的收入只有2000元左右，但生活还不错，该有的都有。&#8221;网友涛涛说，在他的眼中，&#8220;钱是永远没个够的&#8221;，所以，只要安排好就能合理利用，社会就不会有那么多的惶恐。</p><!--function: content() parse end  0ms cost! --><!--我来帮 图标按钮-->',N'2012-10-22 4:44:59',1,1)
INSERT [NewsList] ([NewsID],[NewsTitle],[NewsSummary],[NewsContent],[PublishDate],[IsEnable],[IsHot]) VALUES ( 3,N'1111111111',N'112222222222222',N'<p><img width="160" height="100" src="/userfiles/summer-fields-wallpapers_22222_1920x1200.jpg" alt="" /></p>',N'2012-11-6 4:39:20',1,1)
INSERT [NewsList] ([NewsID],[NewsTitle],[NewsSummary],[NewsContent],[PublishDate],[IsEnable],[IsHot]) VALUES ( 4,N'111',N'2222',N'<p>3333撒的发生地方大师傅</p>
<p>&nbsp;</p>
<p><img width="301" height="169" alt="" src="/newsImage/image/flowers-and-sprout-wallpapers_34373_852x480.jpg" /></p>',N'2012-11-6 4:52:54',1,1)

SET IDENTITY_INSERT [NewsList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[NewsTypeList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [NewsTypeList]

CREATE TABLE [NewsTypeList] (
[NewsTypeID] [int]  IDENTITY (1, 1)  NOT NULL,
[NewsTypeGUID] [uniqueidentifier]  NULL,
[NewsTypeName] [nvarchar]  (50) NULL,
[IsEnable] [int]  NULL,
[PublishDate] [datetime]  NULL,
[NewsTypeOrder] [int]  NULL,
[NewsCount] [int]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nvarchar]  (50) NULL)

ALTER TABLE [NewsTypeList] WITH NOCHECK ADD  CONSTRAINT [PK_NewsTypeList] PRIMARY KEY  NONCLUSTERED ( [NewsTypeID] )
SET IDENTITY_INSERT [NewsTypeList] ON

INSERT [NewsTypeList] ([NewsTypeID],[NewsTypeName],[IsEnable],[PublishDate]) VALUES ( 1,N'政策法规',1,N'2012-10-21 7:55:41')
INSERT [NewsTypeList] ([NewsTypeID],[NewsTypeName],[IsEnable],[PublishDate]) VALUES ( 2,N'企业新闻',1,N'2012-10-21 7:55:58')
INSERT [NewsTypeList] ([NewsTypeID],[NewsTypeName],[IsEnable],[PublishDate]) VALUES ( 3,N'最新公告',1,N'2012-10-21 7:56:25')
INSERT [NewsTypeList] ([NewsTypeID],[NewsTypeName],[IsEnable],[PublishDate]) VALUES ( 4,N'行业新闻',1,N'2012-11-6 2:22:29')

SET IDENTITY_INSERT [NewsTypeList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[PartnersList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [PartnersList]

CREATE TABLE [PartnersList] (
[PartnerID] [int]  IDENTITY (1, 1)  NOT NULL,
[PartnerGUID] [uniqueidentifier]  NULL,
[PartnerName] [nvarchar]  (200) NULL,
[PartnerTitle] [nvarchar]  (200) NULL,
[PartnerIntroduction] [nvarchar]  (MAX) NULL,
[PartnerLogo] [nvarchar]  (200) NULL,
[PartnerKind] [int]  NULL,
[PartnerCategory] [nvarchar]  (50) NULL,
[IsHot] [int]  NULL,
[IsEnable] [int]  NULL,
[PublishDate] [datetime]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nchar]  (10) NULL,
[Other03] [nvarchar]  (50) NULL)

ALTER TABLE [PartnersList] WITH NOCHECK ADD  CONSTRAINT [PK_PartnersList] PRIMARY KEY  NONCLUSTERED ( [PartnerID] )
SET IDENTITY_INSERT [PartnersList] ON


SET IDENTITY_INSERT [PartnersList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[SpecialTopicList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [SpecialTopicList]

CREATE TABLE [SpecialTopicList] (
[SpecialTopicID] [int]  IDENTITY (1, 1)  NOT NULL,
[SpecialTopicGUID] [uniqueidentifier]  NULL,
[SpecialTopicTitle] [nvarchar]  (200) NULL,
[SpecialTopicImages] [nvarchar]  (200) NULL,
[SpecialTopicLinks] [nvarchar]  (200) NULL,
[IsEnable] [int]  NULL,
[IsShow] [int]  NULL,
[PublishDate] [datetime]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nvarchar]  (50) NULL)

ALTER TABLE [SpecialTopicList] WITH NOCHECK ADD  CONSTRAINT [PK_SpecialTopicList] PRIMARY KEY  NONCLUSTERED ( [SpecialTopicID] )
SET IDENTITY_INSERT [SpecialTopicList] ON


SET IDENTITY_INSERT [SpecialTopicList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[StorefrontEleganceList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [StorefrontEleganceList]

CREATE TABLE [StorefrontEleganceList] (
[StorefrontEleganceID] [int]  IDENTITY (1, 1)  NOT NULL,
[DictionaryKey] [nvarchar]  (50) NULL,
[StorefrontEleganceGUID] [uniqueidentifier]  NULL,
[StorefrontEleganceTitle] [nvarchar]  (50) NULL,
[StorefrontEleganceDescription] [nvarchar]  (MAX) NULL,
[StorefrontEleganceImages] [nvarchar]  (200) NULL,
[MainPush] [nvarchar]  (MAX) NULL,
[StoreProfitability] [nvarchar]  (200) NULL,
[RecruitmentAmount] [nvarchar]  (200) NULL,
[RecruitsRatio] [nvarchar]  (200) NULL)

ALTER TABLE [StorefrontEleganceList] WITH NOCHECK ADD  CONSTRAINT [PK_StorefrontEleganceList] PRIMARY KEY  NONCLUSTERED ( [StorefrontEleganceID] )
SET IDENTITY_INSERT [StorefrontEleganceList] ON


SET IDENTITY_INSERT [StorefrontEleganceList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[StoreStatisticsList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [StoreStatisticsList]

CREATE TABLE [StoreStatisticsList] (
[StoreStatisticsID] [int]  IDENTITY (1, 1)  NOT NULL,
[DictionaryKey] [nvarchar]  (50) NULL,
[StoreProfitability] [nvarchar]  (200) NULL,
[RecruitmentAmount] [nvarchar]  (200) NULL,
[RecruitsRatio] [nvarchar]  (200) NULL,
[StoreStatisticsOrder] [int]  NULL,
[PublishDate] [datetime]  NULL,
[IsEnable] [int]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nvarchar]  (50) NULL,
[Other03] [nvarchar]  (50) NULL)

ALTER TABLE [StoreStatisticsList] WITH NOCHECK ADD  CONSTRAINT [PK_StoreStatisticsList] PRIMARY KEY  NONCLUSTERED ( [StoreStatisticsID] )
SET IDENTITY_INSERT [StoreStatisticsList] ON


SET IDENTITY_INSERT [StoreStatisticsList] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[sysdiagrams]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [sysdiagrams]

CREATE TABLE [sysdiagrams] (
[name] [sysname]  NOT NULL,
[principal_id] [int]  NOT NULL,
[diagram_id] [int]  IDENTITY (1, 1)  NOT NULL,
[version] [int]  NULL,
[definition] [varbinary]  (MAX) NULL)

ALTER TABLE [sysdiagrams] WITH NOCHECK ADD  CONSTRAINT [PK_sysdiagrams] PRIMARY KEY  NONCLUSTERED ( [diagram_id] )
SET IDENTITY_INSERT [sysdiagrams] ON


SET IDENTITY_INSERT [sysdiagrams] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[VentureStarList]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [VentureStarList]

CREATE TABLE [VentureStarList] (
[VentureStarID] [int]  IDENTITY (1, 1)  NOT NULL,
[VentureStarGUID] [uniqueidentifier]  NULL,
[VentureStarSource] [nvarchar]  (100) NULL,
[VentureStarName] [nvarchar]  (50) NULL,
[VentureStarContent] [nvarchar]  (MAX) NULL,
[VentureStarImage] [nvarchar]  (200) NULL,
[IsEnable] [int]  NULL,
[PublishDate] [datetime]  NULL,
[Other01] [nvarchar]  (50) NULL,
[Other02] [nvarchar]  (50) NULL)

ALTER TABLE [VentureStarList] WITH NOCHECK ADD  CONSTRAINT [PK_VentureStarList] PRIMARY KEY  NONCLUSTERED ( [VentureStarID] )
SET IDENTITY_INSERT [VentureStarList] ON


SET IDENTITY_INSERT [VentureStarList] OFF
