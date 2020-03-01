USE Bookkeeper;

DROP TABLE IF EXISTS [dbo].UserInfos;
CREATE TABLE [dbo].UserInfos (
	UserID int IDENTITY(1, 1),
	AccountCreation DATE NOT NULL,
	LastActivity SMALLDATETIME NOT NULL,
	TotalCurrentTransactions INT NOT NULL,
	TotalStatements INT NOT NULL
	CONSTRAINT PK_UserInfos_UserID PRIMARY KEY CLUSTERED (UserID)
);

DROP TABLE IF EXISTS [Recording].JournalTransactions;
CREATE TABLE [Recording].JournalTransactions (
	TransactionID int IDENTITY(1, 1) NOT NULL,
	Memo VARCHAR(350) NOT NULL,
	RecordedDate DATE NOT NULL,
	RecordedTime TIME NOT NULL,
	TotalAmount MONEY NOT NULL,
	UserID int NOT NULL,
	CONSTRAINT PK_JournalTransactions_TransactionID PRIMARY KEY CLUSTERED (TransactionID),
	CONSTRAINT FK_JournalTransactions_UserInfos FOREIGN KEY (UserID) 
	REFERENCES [dbo].[UserInfos] (UserID)
	ON DELETE CASCADE
);

DROP TABLE IF EXISTS [Recording].JournalEntries;
CREATE TABLE [Recording].JournalEntries (
	EntryID int IDENTITY(1, 1) NOT NULL,
	AccountName VARCHAR(60) NOT NULL,
	AccountType VARCHAR(30) NOT NULL,
	IsDebit BIT NOT NULL,
	Amount MONEY NOT NULL,
	ParentTransactionID INT NOT NULL,
	CONSTRAINT PK_JournalEntries_EntryID PRIMARY KEY CLUSTERED (EntryID),
	CONSTRAINT FK_JournalEntries_JournalTransactions FOREIGN KEY (ParentTransactionID) 
	REFERENCES [Recording].[JournalTransactions] (TransactionID)
	ON DELETE CASCADE
);
