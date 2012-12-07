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

INSERT [DictionaryList] ([DictionaryListID],[DictionaryKey],[DictionaryValue],[DictionaryCategory],[DictionaryDesc],[OrderNumber],[IsInner],[IsEnable],[PublishDate]) VALUES ( 1,N'changtuzhan',N'�ķ���;վ��',N'StoreType',N'�ķ���;վ��',1,0,1,N'2012-12-5 2:30:00')
INSERT [DictionaryList] ([DictionaryListID],[DictionaryKey],[DictionaryValue],[DictionaryCategory],[DictionaryDesc],[OrderNumber],[IsInner],[IsEnable],[PublishDate]) VALUES ( 2,N'gaokeyuan',N'��ɽ�߿�԰��',N'StoreType',N'��ɽ�߿�԰��',2,0,1,N'2012-12-5 2:31:00')
INSERT [DictionaryList] ([DictionaryListID],[DictionaryKey],[DictionaryValue],[DictionaryCategory],[DictionaryDesc],[OrderNumber],[IsInner],[IsEnable],[PublishDate]) VALUES ( 3,N'kaifaqu',N'�Ƶ���������',N'StoreType',N'�Ƶ���������',3,0,1,N'2012-12-5 2:32:00')
INSERT [DictionaryList] ([DictionaryListID],[DictionaryKey],[DictionaryValue],[DictionaryCategory],[DictionaryDesc],[OrderNumber],[IsInner],[IsEnable],[PublishDate]) VALUES ( 4,N'hetao',N'�������׵�',N'StoreType',N'�������׵�',4,0,1,N'2012-12-5 2:33:00')
INSERT [DictionaryList] ([DictionaryListID],[DictionaryKey],[DictionaryValue],[DictionaryCategory],[DictionaryDesc],[OrderNumber],[IsInner],[IsEnable],[PublishDate]) VALUES ( 5,N'quanzhi',N'ȫְ',N'CareersType',N'ȫְ',1,0,1,N'2012-12-5 2:34:00')
INSERT [DictionaryList] ([DictionaryListID],[DictionaryKey],[DictionaryValue],[DictionaryCategory],[DictionaryDesc],[OrderNumber],[IsInner],[IsEnable],[PublishDate]) VALUES ( 6,N'jianzhi',N'��ְ',N'CareersType',N'��ְ',2,0,1,N'2012-12-5 2:35:00')

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

INSERT [JobCategoryList] ([JobCategoryID],[JobCategoryName],[PublishDate],[IsEnable]) VALUES ( 1,N'�չ�',N'2012-11-29 11:58:50',1)
INSERT [JobCategoryList] ([JobCategoryID],[JobCategoryName],[PublishDate],[IsEnable]) VALUES ( 2,N'����',N'2012-11-29 11:59:22',1)

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

INSERT [JobKindList] ([JobKindID],[JobKindName],[IsHot],[IsEnable],[PublishDate]) VALUES ( 2,N'ǯ��',1,1,N'2012-11-30 8:34:59')

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

INSERT [NewsList] ([NewsID],[NewsTitle],[NewsSummary],[NewsContent],[PublishDate],[IsEnable],[IsHot]) VALUES ( 1,N'1�ձ�פ����ʹ�����ձ���С͵���������й�����',N'1�ձ�פ����ʹ������һ�ɽ�����ʱ�ع���ְ����7�·���ʧ�ԡ����ٻع�ʱ�ĳ�Ĭ��ͬ������20�ջص�ĸУ�����ݴ�ѧ�������ݽ����ݹ�ͬ�籨�������ἰ���������ڵ��㵺�����ϵĶ���ʱ�����𾯸�˵����ζ����Ĳ��',N'<div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1�ձ�פ����ʹ������һ�ɽ�����ʱ�ع���ְ����7�·���&#8220;ʧ��&#8221;���ٻع�ʱ�ĳ�Ĭ��ͬ������20�ջص�ĸУ�����ݴ�ѧ�������ݽ����ݹ�ͬ�籨�������ἰ���������ڵ��㵺�����ϵĶ���ʱ�����𾯸�˵����ζ����Ĳ��������&#8220;��ȫ��ͬ���շ��б�Ҫ��ǿ��ʶ&#8221;�������Լ��ǳ����ǣ��������£����а������40�����ĳɹ�������һ����</div>
<div>&nbsp;</div>
<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ����ƣ�&#8220;���������շǳ����ţ��ձ������͹����δ���ܵ�����������ԡ�&#8221;����Ϊ����11���й�����쵼����ɻ����������ϵҲ���Ժܿ��ת�������ڽ����л�����˵���ձ��������������㵺&#8220;���л�&#8221;�����ڱ����˳����ʱ�⵽�й��˵��ӣ�&#8220;�ձ���С͵�����뷨��ֲ�����й������������У���ǳ���������&#8221;�����ݽ�����󣬵����ʾ�ձ�����б�ҪΪ�������й�ϵ̤ʵ�ظ���Ŭ����</p>
<div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ���������š������ƣ������ʹ�ڽ����в�����¶��ǿ�ҵ�Σ���У�����һ�ֽ��ǸУ��������ڽ�����˵&#8220;�ձ���ȫû�б�Ҫ�ó�����Ȩ&#8221;������ȴ����ᵽ&#8220;��������&#8221;�����ձ��������������й���&#8220;��������&#8221;��<br /></div>
<div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ���������š���������Ļ���ʾ����18������ձ�����Ұ�����ʱ���Ѿ����Լ����ܵ���&#8220;Σ����&#8221;���ݸ������ˡ���ÿ�����š������ƣ��������ݽ��л��ᵽ������������ʷ��ָ��&#8220;�������������Ե��쵼����������ϵ�Լ����������֣���Ϊ���ǿ˷����������һ��Կ�ס����й�ϵҲ�б�Ҫ�������ֻ���������Ҫ�������ϵ�Ŭ����&#8221;</div>',N'2012-10-22 4:16:50',1,1)
INSERT [NewsList] ([NewsID],[NewsTitle],[NewsSummary],[NewsContent],[PublishDate],[IsEnable],[IsHot]) VALUES ( 2,N'�������һ�߳�����н9000Ԫ����š����̿֡�',N'���գ�һ�ݵ����������ѹ�ע����������Ϊ����н���ٻ���������Ӧ�ĳ�������̿֡���������ʾ���Ϻ���������һ�߳�����9000Ԫ���ң����ɶ��������ȶ��߳�������5000Ԫ���¸�����',N'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ���գ�һ�ݵ����������ѹ�ע����������Ϊ&#8220;��н���ٻ���������Ӧ�ĳ�������̿�&#8221;��������ʾ���Ϻ���������һ�߳�����9000Ԫ���ң����ɶ��������ȶ��߳�������5000Ԫ���¸����� 
<p>������н���&#8220;�̿�&#8221;������⣬���ǵ����������������ϣ����ܲ���̫��ı�����Ȼ�����ܶ��и��������Ⱥ�ڲɷ��У�Ҳ��������Ƕ������&#8220;����&#8221;��������ɫɳ��˽����죬������4000Ԫ���ҡ��ڱ����������һ�ݲ�������룬��Ϊ����������׷�����Ҫ���ڻ�������һ���¾ͽ�2000Ԫ�����ԣ�ʣ�µ�Ǯ��ֻ����������ܲ�������&#8220;���ھ����������������鷢������������һ�ʲ�С�Ŀ�֧��&#8221;</p><!--advertisement code begin--><!--none--><!--advertisement code end-->

<p>������Ȼ�����Ƕ�����ֵ̿�ԭ������н������̫���ˡ���ʵ��ÿ���˶������Ƿ����㣬Դ���Լ����ĵ��ڴ����ڲɷ��У�̸����δ��������ڴ�ʱ��&#8220;���˽���&#8221;��&#8220;���ɵ���̬&#8221;��&#8220;��ŮТ˳&#8221;�ȶ����������ἰ������нֻ������֮һ��&#8220;�ҵ�����ֻ��2000Ԫ���ң�������������еĶ��С�&#8221;��������˵�����������У�&#8220;Ǯ����Զû������&#8221;�����ԣ�ֻҪ���źþ��ܺ������ã����Ͳ�������ô��Ļ̿֡�</p><!--function: content() parse end  0ms cost! --><!--������ ͼ�갴ť-->',N'2012-10-22 4:44:59',1,1)
INSERT [NewsList] ([NewsID],[NewsTitle],[NewsSummary],[NewsContent],[PublishDate],[IsEnable],[IsHot]) VALUES ( 3,N'1111111111',N'112222222222222',N'<p><img width="160" height="100" src="/userfiles/summer-fields-wallpapers_22222_1920x1200.jpg" alt="" /></p>',N'2012-11-6 4:39:20',1,1)
INSERT [NewsList] ([NewsID],[NewsTitle],[NewsSummary],[NewsContent],[PublishDate],[IsEnable],[IsHot]) VALUES ( 4,N'111',N'2222',N'<p>3333���ķ����ط���ʦ��</p>
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

INSERT [NewsTypeList] ([NewsTypeID],[NewsTypeName],[IsEnable],[PublishDate]) VALUES ( 1,N'���߷���',1,N'2012-10-21 7:55:41')
INSERT [NewsTypeList] ([NewsTypeID],[NewsTypeName],[IsEnable],[PublishDate]) VALUES ( 2,N'��ҵ����',1,N'2012-10-21 7:55:58')
INSERT [NewsTypeList] ([NewsTypeID],[NewsTypeName],[IsEnable],[PublishDate]) VALUES ( 3,N'���¹���',1,N'2012-10-21 7:56:25')
INSERT [NewsTypeList] ([NewsTypeID],[NewsTypeName],[IsEnable],[PublishDate]) VALUES ( 4,N'��ҵ����',1,N'2012-11-6 2:22:29')

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
