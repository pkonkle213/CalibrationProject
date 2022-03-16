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
CREATE TABLE Roles (
	role_id int IDENTITY,
	role_name VARCHAR(50) NOT NULL,
	CONSTRAINT PK_Roles PRIMARY KEY (role_id),
)

INSERT INTO Roles (role_name) VALUES ('Admin');
INSERT INTO Roles (role_name) VALUES ('Leader');
INSERT INTO Roles (role_name) VALUES ('Participant');
GO

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
	role_id int NOT NULL,
	team_id int NOT NULL,
	CONSTRAINT PK_Users PRIMARY KEY (user_id),
	CONSTRAINT FK_Users_Roles FOREIGN KEY (role_id) references Roles (role_id),
	CONSTRAINT FK_Users_Teams FOREIGN KEY (team_id) references Teams (team_id),
)

-- Populate default data for testing: user and admin with password of 'password'
-- These values should not be kept when going to Production

SET IDENTITY_INSERT Users ON
INSERT INTO Users (user_id, username, password_hash, salt, first_name, last_name, role_id,team_id) VALUES (0,'GROUPSCORES','IMPOSSIBLE', 'NOTHAPPENING','GROUP','SCORES',3,5);
SET IDENTITY_INSERT Users OFF
INSERT INTO Users (username, password_hash, salt, first_name, last_name, role_id,team_id) VALUES ('adouglas','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','Boss','Man',1,1);
INSERT INTO Users (username, password_hash, salt, first_name, last_name, role_id,team_id) VALUES ('pkonkle','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','Phillip','Konkle',2,1);
GO


CREATE TABLE Options (
	option_id int IDENTITY,
	option_value varchar(20) NOT NULL,
	points_earned decimal(3,2) NOT NULL,
	CONSTRAINT PK_Options PRIMARY KEY (option_id),
)

INSERT INTO Options (option_value,points_earned) VALUES ('Meets',0);
INSERT INTO Options (option_value,points_earned) VALUES ('Does Not Meet',0);
INSERT INTO Options (option_value,points_earned) VALUES ('Critical',0);
INSERT INTO Options (option_value,points_earned) VALUES ('100%',1);
INSERT INTO Options (option_value,points_earned) VALUES ('75%',.75);
INSERT INTO Options (option_value,points_earned) VALUES ('50%',.5);
INSERT INTO Options (option_value,points_earned) VALUES ('0%',0);
INSERT INTO Options (option_value,points_earned) VALUES ('N/A',0);
GO


CREATE TABLE Contacts (
	contact_id int IDENTITY,
	type varchar(20) NOT NULL,
	CONSTRAINT PK_Contacts PRIMARY KEY (contact_id),
)

INSERT INTO Contacts (type) VALUES ('Tier 1 - Chat');
INSERT INTO Contacts (type) VALUES ('Tier 1 - Email');
INSERT INTO Contacts (type) VALUES ('Tier 1 - Phone');
INSERT INTO Contacts (type) VALUES ('Tier 2 - Back Office');
INSERT INTO Contacts (type) VALUES ('Tier 2 - Phone');
INSERT INTO Contacts (type) VALUES ('Tier 3');
GO

CREATE TABLE Forms (
	form_id int IDENTITY,
	form_name varchar(100) NOT NULL,
	isArchived bit NOT NULL,
	CONSTRAINT PK_Forms PRIMARY KEY (form_id),
)

INSERT INTO Forms (form_name,isArchived) VALUES ('Zulily form',0);
GO

CREATE TABLE Questions (
	question_id int IDENTITY,
	form_id int NOT NULL,
	question varchar(200) NOT NULL,
	isCategory bit NOT NULL,
	points_possible int,
	CONSTRAINT PK_Questions PRIMARY KEY (question_id),
	CONSTRAINT FK_Questions_Forms FOREIGN KEY (form_id) references Forms (form_id),
)

INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Category1',1,30);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Question1',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Category2',1,20);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Question2',0,NULL);
GO

CREATE TABLE Calibrations (
	calibration_id int IDENTITY,
	calibration_date dateTime NOT NULL,
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
	CONSTRAINT FK_Calibrations_Contacts FOREIGN KEY (contact_type) references Contacts (contact_id),
)

INSERT INTO Calibrations (calibration_date,contact_type,contact_id,tm_first_name,tm_last_name,group_score_earned,group_score_possible,form_id,isOpen) VALUES ('2022/01/15',4,'Email 12345','Jane','Doe',60,85,1,0);
INSERT INTO Calibrations (calibration_date,contact_type,contact_id,tm_first_name,tm_last_name,group_score_earned,group_score_possible,form_id,isOpen) VALUES ('2022/01/25',3,'Chat 43211','Ender','Wiggin',85.5,100,1,1);

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

INSERT INTO Scores (user_id,calibration_id,points_earned,points_possible) VALUES (2,1,70,85);
INSERT INTO Scores (user_id,calibration_id,points_earned,points_possible) VALUES (2,2,100,100);

CREATE TABLE Questions_Options(
	question_id int NOT NULL,
	option_id int NOT NULL,
	CONSTRAINT PK_Questions_Options PRIMARY KEY (question_id,option_id),
	CONSTRAINT FK_Questions_Options_Questions FOREIGN KEY (question_id) references Questions (question_id),
	CONSTRAINT FK_Questions_Options_Options FOREIGN KEY (option_id) references Options (option_id),
)

INSERT INTO Questions_Options (question_id,option_id) VALUES (1,4);
INSERT INTO Questions_Options (question_id,option_id) VALUES (1,5);
INSERT INTO Questions_Options (question_id,option_id) VALUES (1,6);
INSERT INTO Questions_Options (question_id,option_id) VALUES (1,7);
INSERT INTO Questions_Options (question_id,option_id) VALUES (2,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (2,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (2,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (2,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (3,4);
INSERT INTO Questions_Options (question_id,option_id) VALUES (3,5);
INSERT INTO Questions_Options (question_id,option_id) VALUES (3,6);
INSERT INTO Questions_Options (question_id,option_id) VALUES (3,7);
INSERT INTO Questions_Options (question_id,option_id) VALUES (4,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (4,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (4,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (4,8);
GO
