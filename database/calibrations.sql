USE master
GO

-- Drop database if it exists
IF DB_ID('calibration_database') IS NOT NULL
BEGIN
	ALTER DATABASE calibration_database SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE calibration_database;
END

CREATE DATABASE calibration_database
GO

USE calibration_database
GO

-- Create Tables
CREATE TABLE Teams (
	team_id int IDENTITY,
	team_name VARCHAR(50) NOT NULL,
	CONSTRAINT PK_Teams PRIMARY KEY (team_id)
)

INSERT INTO Teams (team_name) VALUES ('Quality Assurance');
INSERT INTO Teams (team_name) VALUES ('Supervisor');
INSERT INTO Teams (team_name) VALUES ('Manager');
INSERT INTO Teams (team_name) VALUES ('Trainer');
INSERT INTO Teams (team_name) VALUES ('None');
GO

CREATE TABLE Users (
	user_id int IDENTITY,
	username varchar(50) NOT NULL,
	password_hash varchar(200) NOT NULL,
	salt varchar(200) NOT NULL,
	first_name varchar(50) NOT NULL,
	last_name varchar(50) NOT NULL,
	isActive BIT NOT NULL,
	calibrationPosition int,
	role varchar(15) NOT NULL,
	team_id int NOT NULL,
	-- calibrationPosition int NOT NULL,
	CONSTRAINT PK_Users PRIMARY KEY (user_id),
	CONSTRAINT FK_Users_Teams FOREIGN KEY (team_id) references Teams (team_id),
	CONSTRAINT UC_Users_user_id UNIQUE (user_id),
)

-- Populate default data for testing: user and admin with password of 'password'
-- These values should not be kept when going to Production

SET IDENTITY_INSERT Users ON
INSERT INTO Users (user_id, username, password_hash, salt, first_name, last_name, isActive, calibrationPosition, role, team_id) VALUES (0,'GROUPSCORES','IMPOSSIBLE', 'NOTHAPPENING','GROUP','SCORES',0,0,'Admin',5);
SET IDENTITY_INSERT Users OFF
INSERT INTO Users (username, password_hash, salt, first_name, last_name, isActive, calibrationPosition, role, team_id) VALUES ('sadmin','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','System','Admin',1,1,'Admin',1);
INSERT INTO Users (username, password_hash, salt, first_name, last_name, isActive, calibrationPosition, role, team_id) VALUES ('pkonkle','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','Phillip','Konkle',1,2,'Participant',1);
GO


CREATE TABLE Options (
	option_id int IDENTITY,
	form_id int NOT NULL,
	orderPosition int NOT NULL,
	isCategory bit NOT NULL,
	hasValue bit NOT NULL,
	option_value varchar(20) NOT NULL,
	points_earned decimal(3,2) NOT NULL,
	CONSTRAINT PK_Options PRIMARY KEY (option_id),
)

INSERT INTO Options (option_value,orderPosition,isCategory,points_earned,form_id,hasValue) VALUES ('Meets',1,0,0,1,0);
INSERT INTO Options (option_value,orderPosition,isCategory,points_earned,form_id,hasValue) VALUES ('Does Not Meet',2,0,0,1,0);
INSERT INTO Options (option_value,orderPosition,isCategory,points_earned,form_id,hasValue) VALUES ('Critical',3,0,0,1,0);
INSERT INTO Options (option_value,orderPosition,isCategory,points_earned,form_id,hasValue) VALUES ('100%',4,1,1,1,1);
INSERT INTO Options (option_value,orderPosition,isCategory,points_earned,form_id,hasValue) VALUES ('75%',5,1,.75,1,1);
INSERT INTO Options (option_value,orderPosition,isCategory,points_earned,form_id,hasValue) VALUES ('50%',6,1,.5,1,1);
INSERT INTO Options (option_value,orderPosition,isCategory,points_earned,form_id,hasValue) VALUES ('0%',7,1,0,1,1);
INSERT INTO Options (option_value,orderPosition,isCategory,points_earned,form_id,hasValue) VALUES ('N/A',8,1,0,1,0);
INSERT INTO Options (option_value,orderPosition,isCategory,points_earned,form_id,hasValue) VALUES ('N/A',9,0,0,1,0);
GO

CREATE TABLE ContactTypes (
	contact_id int IDENTITY,
	type varchar(20) NOT NULL,
	CONSTRAINT PK_Contacts PRIMARY KEY (contact_id),
)

INSERT INTO ContactTypes (type) VALUES ('Tier 1 - Chat');
INSERT INTO ContactTypes (type) VALUES ('Tier 2 - Phone');
INSERT INTO ContactTypes (type) VALUES ('Tier 1 - Email');
INSERT INTO ContactTypes (type) VALUES ('Tier 3');
INSERT INTO ContactTypes (type) VALUES ('Tier 1 - Phone');
INSERT INTO ContactTypes (type) VALUES ('Tier 2 - Back Office');
GO

CREATE TABLE Forms (
	form_id int IDENTITY,
	form_name varchar(100) NOT NULL,
	isArchived bit NOT NULL,
	CONSTRAINT PK_Forms PRIMARY KEY (form_id),
)

INSERT INTO Forms (form_name,isArchived) VALUES ('Phil''s Phake Phorm',0);
GO

CREATE TABLE Questions (
	question_id int IDENTITY,
	form_id int NOT NULL,
	form_position int NOT NULL,
	question varchar(200) NOT NULL,
	isCategory bit NOT NULL,
	points_possible int NOT NULL,
	CONSTRAINT PK_Questions PRIMARY KEY (question_id),
	CONSTRAINT FK_Questions_Forms FOREIGN KEY (form_id) references Forms (form_id),
	CONSTRAINT UC_Questions UNIQUE (form_id, form_position),
)

INSERT INTO Questions (form_id,form_position,question,isCategory,points_possible) VALUES (1,3,'Category2',1,20);
INSERT INTO Questions (form_id,form_position,question,isCategory,points_possible) VALUES (1,1,'Category1',1,30);
INSERT INTO Questions (form_id,form_position,question,isCategory,points_possible) VALUES (1,4,'Question2',0,0);
INSERT INTO Questions (form_id,form_position,question,isCategory,points_possible) VALUES (1,2,'Question1',0,0);
GO  

CREATE TABLE Calibrations (
	calibration_id int IDENTITY,
	calibration_date dateTime NOT NULL,
	leader_user_id int NOT NULL,
	contact_type int NOT NULL,
	contact_id varchar(100) NOT NULL,
	tm_first_name varchar(100) NOT NULL,
	tm_last_name varchar(100) NOT NULL,
	group_score_earned decimal(5,2) NOT NULL,
	group_score_possible decimal(5,2) NOT NULL,
	form_id int NOT NULL,
	isOpen BIT NOT NULL,
	CONSTRAINT PK_Calibrations PRIMARY KEY (calibration_id),
	CONSTRAINT FK_Calibrations_Forms FOREIGN KEY (form_id) references Forms (form_id),
	CONSTRAINT FK_Calibrations_ContactTypes FOREIGN KEY (contact_type) references ContactTypes (contact_id),
	CONSTRAINT FK_Calibrations_Users FOREIGN KEY (leader_user_id) references Users (user_id),
)

INSERT INTO Calibrations (calibration_date,leader_user_id,contact_type,contact_id,tm_first_name,tm_last_name,group_score_earned,group_score_possible,form_id,isOpen) VALUES ('2022/01/15',1,4,'Email 12345','Jane','Doe',60,85,1,0);
INSERT INTO Calibrations (calibration_date,leader_user_id,contact_type,contact_id,tm_first_name,tm_last_name,group_score_earned,group_score_possible,form_id,isOpen) VALUES ('2022/01/25',1,3,'Chat 43211','Ender','Wiggin',0,0,1,1);

CREATE TABLE Answers (
	calibration_id int NOT NULL,
	user_id int NOT NULL,
	question_id int NOT NULL,
	option_id int NOT NULL,
	comment varchar(500) NOT NULL,
	CONSTRAINT PK_Answers PRIMARY KEY (calibration_id,user_id,question_id),
	CONSTRAINT FK_Answers_Calibrations FOREIGN KEY (calibration_id) references Calibrations (calibration_id),
	CONSTRAINT FK_Answers_Users FOREIGN KEY (user_id) references Users (user_id),
	CONSTRAINT FK_Answers_Questions FOREIGN KEY (question_id) references Questions (question_id),
	CONSTRAINT FK_Answers_Options FOREIGN KEY (option_id) references Options (option_id),
)

INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,0,1,7,'idk');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,0,2,1,'something');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,0,3,7,'sounds good');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,0,4,2,'w00t');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,1,1,5,'stuff');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,1,2,1,'words');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,1,3,6,'things');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,1,4,2,'yay');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,2,1,4,'stuff');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,2,2,2,'words');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,2,3,7,'things');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,2,4,1,'yay');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (2,2,1,5,'Because I dislike this rep');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (2,2,2,1,'Didn''t ask for the name');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (2,2,3,6,'Didn''t do something right');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (2,2,4,2,'Needs more empathy');
GO

CREATE TABLE Scores(
	user_id int NOT NULL,
	calibration_id int NOT NULL,
	points_earned decimal(5,2) NOT NULL,
	points_possible decimal(5,2) NOT NULL,
	CONSTRAINT PK_Scores PRIMARY KEY (user_id,calibration_id),
	CONSTRAINT FK_Scores_Users FOREIGN KEY (user_id) references Users (user_id),
	CONSTRAINT FK_Scores_Calibrations FOREIGN KEY (calibration_id) references Calibrations (calibration_id),
)

INSERT INTO Scores (user_id,calibration_id,points_earned,points_possible) VALUES (2,1,30,50);
INSERT INTO Scores (user_id,calibration_id,points_earned,points_possible) VALUES (2,2,100,100);
INSERT INTO Scores (user_id,calibration_id,points_earned,points_possible) VALUES (1,1,32.5,50);
GO

CREATE TABLE Questions_Options (
	question_id int NOT NULL,
	option_id int NOT NULL,
	CONSTRAINT PK_Questions_Options PRIMARY KEY (question_id, option_id),
	CONSTRAINT FK_Questions_Options_Questions FOREIGN KEY (question_id) REFERENCES Questions (question_id),
	CONSTRAINT FK_Questions_Options_Options FOREIGN KEY (option_id) REFERENCES Options (option_id),
)