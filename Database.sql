
CREATE SCHEMA test;


CREATE TABLE test.Departments (
	DepartmentId int IDENTITY(1,1) NOT NULL,
	DepartmentName nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	dtmUpd datetime DEFAULT getdate() NULL,
	usrUpd nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK__Departme__B2079BEDCB50EAC3 PRIMARY KEY (DepartmentId)
);


CREATE TABLE test.DocumentRelatedDepartments (
	DocumentId varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	DepartmentId varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
);


CREATE TABLE test.Documents (
	DocumentId int IDENTITY(1,1) NOT NULL,
	DocumentNo nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DateCreated datetime DEFAULT getdate() NULL,
	ProductOrMaterial nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Description nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Status nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	dtmUpd datetime DEFAULT getdate() NULL,
	usrUpd nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DepartementRequestor int NULL,
	Pabrik nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	PathPdf nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK__Document__1ABEEF0F27077C47 PRIMARY KEY (DocumentId)
);


CREATE TABLE test.Users (
	UserId int IDENTITY(1,1) NOT NULL,
	UserName nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Email nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Role] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Password nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	dtmUpd datetime DEFAULT getdate() NULL,
	usrUpd nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Department int NULL,
	UplineId int NULL,
	CONSTRAINT PK__Users__1788CC4C2E42B0B5 PRIMARY KEY (UserId)
);

CREATE TABLE test.AuditTrail (
	AuditId int IDENTITY(1,1) NOT NULL,
	DocumentId int NOT NULL,
	[Action] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ActionBy int NOT NULL,
	ActionDate datetime DEFAULT getdate() NULL,
	dtmUpd datetime DEFAULT getdate() NULL,
	usrUpd nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK__AuditTra__A17F2398C03F541B PRIMARY KEY (AuditId),
	CONSTRAINT FK__AuditTrai__Actio__751F8F1F FOREIGN KEY (ActionBy) REFERENCES test.Users(UserId),
	CONSTRAINT FK__AuditTrai__Docum__742B6AE6 FOREIGN KEY (DocumentId) REFERENCES test.Documents(DocumentId)
);


CREATE TABLE test.DocumentApprovals (
	ApprovalId int IDENTITY(1,1) NOT NULL,
	DocumentId int NOT NULL,
	ApproverId int NOT NULL,
	ApprovalStatus nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ApprovalDate datetime NULL,
	dtmUpd datetime DEFAULT getdate() NULL,
	usrUpd nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	NextApprovalRole varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK__Document__328477F471E9F99C PRIMARY KEY (ApprovalId),
	CONSTRAINT FK__DocumentA__Appro__6F66B5C9 FOREIGN KEY (ApproverId) REFERENCES test.Users(UserId),
	CONSTRAINT FK__DocumentA__Docum__6E729190 FOREIGN KEY (DocumentId) REFERENCES test.Documents(DocumentId)
);

INSERT INTO test.Users (UserId, UserName, Email, [Role], Password, dtmUpd, usrUpd, Department, UplineId) VALUES(1, N'adit', N'pujipratiwi74@gmail.com', N'Requestor', N'test', '2024-09-21 10:46:50.257', N'system', NULL, NULL);
INSERT INTO test.Users (UserId, UserName, Email, [Role], Password, dtmUpd, usrUpd, Department, UplineId) VALUES(3, N'Andi Prasetyo', N'pujipratiwi74@gmail.com', N'Requestor', N'test', '2024-09-21 14:27:19.993', N'system', NULL, NULL);
INSERT INTO test.Users (UserId, UserName, Email, [Role], Password, dtmUpd, usrUpd, Department, UplineId) VALUES(4, N'Budi Santoso', N'adityarahmandeveloper@gmail.com', N'Manager', N'test', '2024-09-21 14:27:19.993', N'system', NULL, NULL);
INSERT INTO test.Users (UserId, UserName, Email, [Role], Password, dtmUpd, usrUpd, Department, UplineId) VALUES(5, N'Citra Dewi', N'adityarahmandeveloper@gmail.com', N'QA', N'test', '2024-09-21 14:27:19.993', N'system', NULL, NULL);
INSERT INTO test.Users (UserId, UserName, Email, [Role], Password, dtmUpd, usrUpd, Department, UplineId) VALUES(6, N'Dimas Aditya', N'adityarahmandeveloper@gmail.com', N'DCC', N'test', '2024-09-21 14:27:19.993', N'system', NULL, NULL);
INSERT INTO test.Users (UserId, UserName, Email, [Role], Password, dtmUpd, usrUpd, Department, UplineId) VALUES(7, N'test1234', N'adityarahmandeveloper@gmail.com', N'Requestor', N'test', '2024-09-22 19:44:36.270', N'test1234', NULL, NULL);
INSERT INTO test.Users (UserId, UserName, Email, [Role], Password, dtmUpd, usrUpd, Department, UplineId) VALUES(12, N'puji department 1', N'adityarahmandeveloper@gmail.com', N'Department', N'test', '2024-09-22 19:52:00.333', N'puji department 1', 1, NULL);
INSERT INTO test.Users (UserId, UserName, Email, [Role], Password, dtmUpd, usrUpd, Department, UplineId) VALUES(13, N'puji department 2', N'adityarahmandeveloper@gmail.com', N'Department', N'test', '2024-09-22 19:52:17.583', N'puji department 2', 2, NULL);
INSERT INTO test.Users (UserId, UserName, Email, [Role], Password, dtmUpd, usrUpd, Department, UplineId) VALUES(14, N'puji department 3', N'adityarahmandeveloper@gmail.com', N'Department', N'test', '2024-09-22 19:52:30.103', N'puji department 3', 4, NULL);
