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
	points_earned decimal(3,2),
	CONSTRAINT PK_Options PRIMARY KEY (option_id),
)

INSERT INTO Options (option_value,points_earned) VALUES ('Meets',NULL);
INSERT INTO Options (option_value,points_earned) VALUES ('Does Not Meet',NULL);
INSERT INTO Options (option_value,points_earned) VALUES ('Critical',NULL);
INSERT INTO Options (option_value,points_earned) VALUES ('100%',1);
INSERT INTO Options (option_value,points_earned) VALUES ('75%',.75);
INSERT INTO Options (option_value,points_earned) VALUES ('50%',.5);
INSERT INTO Options (option_value,points_earned) VALUES ('0%',0);
INSERT INTO Options (option_value,points_earned) VALUES ('N/A',NULL);
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

INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Create a Professional and Friendly Interaction',1,30);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Greeting / Closing',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Friendly / Professional',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Communicate Clearly',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Acknowledgement / Express Regret',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Defusing Skills',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Ensure a Complete and Accurate Experience',1,30);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Fulfill commitments made to the customer',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Use Appropriate Resources / Ask Questions',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Order Accuracy',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Follow All SOPs, Guidelines and Policies',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Document Account',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Feedback / VOC',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'First Call Resolution',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Provide Accurate/Correct Information',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Proactively assists with account issues',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Inappropriate Transfer',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Take Ownership / Take All Actions',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Respect Her Time by Efficiently Guiding the Interaction',1,20);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Delayed Opening / Closing',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Effectively Guide / Call Control',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Maintain Focus',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Navigation',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Transition From Personal to Business',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Be Concise, Not Repetitive',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Protect the Customer with Verification and Privacy Practices',1,20);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Verification',0,NULL);
INSERT INTO Questions (form_id,question,isCategory,points_possible) VALUES (1,'Privacy Practice / Sharing CC Data',0,NULL);
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

INSERT INTO Calibrations (calibration_date,contact_type,contact_id,tm_first_name,tm_last_name,group_score_earned,group_score_possible,form_id,isOpen) VALUES ('2022/01/04',2,'Call 123456','John','Doe',100,100,1,1);
INSERT INTO Calibrations (calibration_date,contact_type,contact_id,tm_first_name,tm_last_name,group_score_earned,group_score_possible,form_id,isOpen) VALUES ('2022/01/15',4,'Email 12345','Jane','Doe',60,85,1,1);
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

INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,1,2,1,'Because I dislike this rep');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,2,2,1,'Didn''t ask for the name');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,2,1,6,'Didn''t do something right');
INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) VALUES (1,2,3,2,'Needs more empathy');
GO

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
INSERT INTO Questions_Options (question_id,option_id) VALUES (3,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (3,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (3,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (3,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (4,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (4,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (4,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (4,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (5,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (5,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (5,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (5,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (6,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (6,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (6,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (6,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (7,4);
INSERT INTO Questions_Options (question_id,option_id) VALUES (7,5);
INSERT INTO Questions_Options (question_id,option_id) VALUES (7,6);
INSERT INTO Questions_Options (question_id,option_id) VALUES (7,7);
INSERT INTO Questions_Options (question_id,option_id) VALUES (8,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (8,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (8,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (8,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (9,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (9,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (9,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (9,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (10,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (10,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (10,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (10,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (11,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (11,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (11,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (11,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (12,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (12,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (12,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (12,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (13,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (13,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (13,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (13,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (14,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (14,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (14,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (14,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (15,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (15,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (15,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (15,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (16,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (16,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (16,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (16,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (17,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (17,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (17,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (17,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (18,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (18,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (18,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (18,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (19,4);
INSERT INTO Questions_Options (question_id,option_id) VALUES (19,5);
INSERT INTO Questions_Options (question_id,option_id) VALUES (19,6);
INSERT INTO Questions_Options (question_id,option_id) VALUES (19,7);
INSERT INTO Questions_Options (question_id,option_id) VALUES (20,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (20,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (20,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (20,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (21,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (21,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (21,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (21,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (22,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (22,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (22,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (22,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (23,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (23,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (23,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (23,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (24,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (24,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (24,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (24,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (25,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (25,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (25,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (25,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (26,4);
INSERT INTO Questions_Options (question_id,option_id) VALUES (26,7);
INSERT INTO Questions_Options (question_id,option_id) VALUES (26,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (27,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (27,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (27,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (27,8);
INSERT INTO Questions_Options (question_id,option_id) VALUES (28,1);
INSERT INTO Questions_Options (question_id,option_id) VALUES (28,2);
INSERT INTO Questions_Options (question_id,option_id) VALUES (28,3);
INSERT INTO Questions_Options (question_id,option_id) VALUES (28,8);
GO
