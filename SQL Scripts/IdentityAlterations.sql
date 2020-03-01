USE Bookkeeper;

ALTER TABLE [dbo].AspNetUsers ADD UserInfoID int NOT NULL;

ALTER TABLE [dbo].AspNetUsers ADD CONSTRAINT FK_AspNetUsers_UserInfos 
FOREIGN KEY (UserInfoID) REFERENCES [dbo].UserInfos (UserID)
ON DELETE CASCADE;

-- Necessary if you need to drop UserInfo table
ALTER TABLE [dbo].AspNetUsers DROP CONSTRAINT FK_AspNetUsers_UserInfos 