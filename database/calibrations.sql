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
INSERT INTO Users (username, password_hash, salt, first_name, last_name, role_id,team_id) VALUES ('user','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','Phillip','Konkle',2,1);
INSERT INTO Users (username, password_hash, salt, first_name, last_name, role_id,team_id) VALUES ('admin','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','Alex','Douglas',1,1);
GO


CREATE TABLE Options (
	option_id int IDENTITY,
	option_value varchar(20) NOT NULL,
	CONSTRAINT PK_Options PRIMARY KEY (option_id),
)

INSERT INTO Options (option_value) VALUES ('Meets');
INSERT INTO Options (option_value) VALUES ('Does Not Meet');
INSERT INTO Options (option_value) VALUES ('Critical');
INSERT INTO Options (option_value) VALUES ('100%');
INSERT INTO Options (option_value) VALUES ('75%');
INSERT INTO Options (option_value) VALUES ('50%');
INSERT INTO Options (option_value) VALUES ('0%');
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
	CONSTRAINT PK_Forms PRIMARY KEY (form_id),
)

INSERT INTO Forms (form_name) VALUES ('Zulily form');
GO

CREATE TABLE Categories (
	category_id int IDENTITY,
	category_name varchar(50) NOT NULL,
	points int NOT NULL,
	CONSTRAINT PK_Categories PRIMARY KEY (category_id),
)

INSERT INTO Categories (category_name,points) VALUES ('Soft Skills',30);
INSERT INTO Categories (category_name,points) VALUES ('Technical Skills',30);
INSERT INTO Categories (category_name,points) VALUES ('Efficiency',20);
INSERT INTO Categories (category_name,points) VALUES ('Verification',20);
GO

CREATE TABLE Questions (
	question_id int IDENTITY,
	question varchar(200) NOT NULL,
	category_id int NOT NULL,
	CONSTRAINT PK_Questions PRIMARY KEY (question_id),
	CONSTRAINT FK_Questions_Categories FOREIGN KEY (category_id) references Categories (category_id),
)

INSERT INTO Questions (question,category_id) VALUES ('Did the team member verify the customer?',4);
INSERT INTO Questions (question,category_id) VALUES ('Did the team member leave order notes?',2);
INSERT INTO Questions (question,category_id) VALUES ('Did the team member give the correct resolution?',2);
INSERT INTO Questions (question,category_id) VALUES ('Did the team member make a connection?',1);
INSERT INTO Questions (question,category_id) VALUES ('Did the team member ask efficient and direct questions?',3);
GO

CREATE TABLE Calibrations (
	calibration_id int IDENTITY,
	calibration_date dateTime NOT NULL,
	contact_type int NOT NULL,
	tm_first_name varchar(100) NOT NULL,
	tm_last_name varchar(100) NOT NULL,
	form_id int NOT NULL,
	CONSTRAINT PK_Calibrations PRIMARY KEY (calibration_id),
	CONSTRAINT FK_Calibrations_Forms FOREIGN KEY (form_id) references Forms (form_id),
	CONSTRAINT FK_Calibrations_Contacts FOREIGN KEY (contact_type) references Contacts (contact_id),
)

INSERT INTO Calibrations (calibration_date,contact_type,tm_first_name,tm_last_name,form_id) VALUES ('2022/01/04',2,'John','Doe',1);
INSERT INTO Calibrations (calibration_date,contact_type,tm_first_name,tm_last_name,form_id) VALUES ('2022/01/04',4,'Jane','Doe',1);

CREATE TABLE Answers (
	result_id int IDENTITY,
	calibration_id int NOT NULL,
	user_id int NOT NULL,
	question_id int NOT NULL,
	option_id int NOT NULL,
	CONSTRAINT PK_Answers PRIMARY KEY (result_id),
	CONSTRAINT FK_Answers_Calibrations FOREIGN KEY (calibration_id) references Calibrations (calibration_id),
	CONSTRAINT FK_Answers_Users FOREIGN KEY (user_id) references Users (user_id),
	CONSTRAINT FK_Answers_Questions FOREIGN KEY (question_id) references Questions (question_id),
	CONSTRAINT FK_Answers_Options FOREIGN KEY (option_id) references Options (option_id),
)

INSERT INTO Answers (calibration_id,user_id,question_id,option_id) VALUES (1,1,2,1);
INSERT INTO Answers (calibration_id,user_id,question_id,option_id) VALUES (1,2,2,2);
INSERT INTO Answers (calibration_id,user_id,question_id,option_id) VALUES (1,2,1,1);
INSERT INTO Answers (calibration_id,user_id,question_id,option_id) VALUES (1,2,1,4);
GO

CREATE TABLE Users_Calibrations(
	user_id int NOT NULL,
	calibration_id int NOT NULL,
	CONSTRAINT PK_Users_Calibrations PRIMARY KEY (user_id,calibration_id),
	CONSTRAINT FK_Users_Calibrations_Users FOREIGN KEY (user_id) references Users (user_id),
	CONSTRAINT FK_Users_Calibrations_Calibrations FOREIGN KEY (calibration_id) references Calibrations (calibration_id),
)

CREATE TABLE Forms_Categories(
	category_id int NOT NULL,
	form_id int NOT NULL,
	CONSTRAINT Forms_Categories PRIMARY KEY (category_id,form_id),
	CONSTRAINT FK_Forms_Categories_Forms FOREIGN KEY (form_id) references Forms (form_id),
	CONSTRAINT FK_Forms_Categories_Categories FOREIGN KEY (category_id) references Categories (category_id),
)

CREATE TABLE Questions_Options(
	question_id int NOT NULL,
	option_id int NOT NULL,
	CONSTRAINT PK_Questions_Options PRIMARY KEY (question_id,option_id),
	CONSTRAINT FK_Questions_Options_Questions FOREIGN KEY (question_id) references Questions (question_id),
	CONSTRAINT FK_Questions_Options_Options FOREIGN KEY (option_id) references Options (option_id),
)