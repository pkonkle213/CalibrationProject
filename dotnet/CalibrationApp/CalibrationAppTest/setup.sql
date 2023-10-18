-- Put steps here to set up your database in a default good state for testing
DELETE FROM Scores;
DELETE FROM Answers;
DELETE FROM Calibrations;
DELETE FROM Questions_Options;
DELETE FROM Questions;
DELETE FROM Forms;
DELETE FROM ContactTypes;
DELETE FROM Options;
DELETE FROM Users;
DELETE FROM Teams;

SET IDENTITY_INSERT Teams ON

INSERT INTO Teams (team_id, team_name)
VALUES
(1, 'Quality Assurance'), 
(2, 'Supervisor'), 
(3, 'Manager'), 
(4, 'Trainer'), 
(5, 'None');

SET IDENTITY_INSERT Teams OFF

SET IDENTITY_INSERT Users ON

INSERT INTO Users (user_id, username, password_hash, salt, first_name, last_name, isActive, calibrationPosition, role, team_id)
VALUES 
(0, 'GROUPSCORES', 'IMPOSSIBLE', 'NOTHAPPENING', 'GROUP', 'SCORES', 0, 0, 'Admin', 5), 
(1, 'sadmin', '', '', 'System', 'Admin', 1, 1, 'Admin', 1), 
(2, 'suser', '', '', 'Second', 'User', 1, 2, 'Participant', 1), 
(3, 'ouser', '', '', 'Other', 'User', 0, 3, 'Participant', 2);

SET IDENTITY_INSERT Users OFF

SET IDENTITY_INSERT Options ON

INSERT INTO Options (option_id, option_value, orderPosition, isCategory, points_earned, form_id, hasValue)
VALUES
(1, 'Critical', 3, 0, 0, 1, 0), 
(2, '50%', 6, 1, .5, 1, 1), 
(3, 'Does Not Meet', 2, 0, 0, 1, 0), 
(4, '0%', 7, 1, 0, 1, 1), 
(5, 'Meets', 1, 0, 0, 1, 0), 
(6, '75%', 5, 1, .75, 1, 1), 
(7, '100%', 4, 1, 1, 1, 1), 
(8, 'N/A', 9, 0, 0, 1, 0), 
(9, 'N/A', 8, 1, 0, 1, 0);

SET IDENTITY_INSERT Options OFF

SET IDENTITY_INSERT ContactTypes ON

INSERT INTO ContactTypes (contact_id, type)
VALUES
(1, 'Tier 1 - Phone'), 
(2, 'Tier 3'), 
(3, 'Tier 1 - Email'), 
(4, 'Tier 2 - Phone'), 
(5, 'Tier 1 - Chat'), 
(6, 'Tier 2 - Back Office');

SET IDENTITY_INSERT ContactTypes OFF

SET IDENTITY_INSERT Forms ON

INSERT INTO Forms (form_id, form_name, isArchived)
VALUES
(1, 'A Fake Form', 0), 
(2, 'Another Semi Fake Form', 0);

SET IDENTITY_INSERT Forms OFF

SET IDENTITY_INSERT Questions ON

INSERT INTO Questions (question_id, form_id, form_position, question, isCategory, points_possible)
VALUES
(1, 1, 3, 'Category2', 1, 20),
(2, 1, 1, 'Category1', 1, 30),
(3, 1, 4, 'Question2', 0, 0),
(4, 1, 2, 'Question1', 0, 0);

SET IDENTITY_INSERT Questions OFF

SET IDENTITY_INSERT Calibrations ON

INSERT INTO Calibrations (calibration_id,calibration_date,leader_user_id,contact_type,contact_id,tm_first_name,tm_last_name,group_score_earned,group_score_possible,form_id,isOpen)
VALUES
(1,'2023/10/17',1,1,'','','',0,0,1,1),
(2,'2023/05/20',1,2,'','','',0,0,1,1),
(3,'2022/01/15',1,4,'','','',60,85,1,0),
(4,'2023/02/14',1,6,'','','',0,0,1,1),
(5,'2022/01/25',1,3,'','','',0,0,1,1),
(6,'2023/06/30',1,5,'','','',0,0,1,1);


SET IDENTITY_INSERT Calibrations OFF

INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment)
VALUES
(1,0,1,7,'idk'),
(1,0,2,1,'something'),
(1,0,3,7,'sounds good'),
(1,0,4,2,'w00t'),
(1,1,1,5,'stuff'),
(1,1,2,1,'words'),
(1,1,3,6,'things'),
(1,1,4,2,'yay'),
(1,2,1,4,'stuff'),
(1,2,2,2,'words'),
(1,2,3,7,'things'),
(1,2,4,1,'yay'),
(2,2,1,5,'Because I dislike this rep'),
(2,2,2,1,'Didn''t ask for the name'),
(2,2,3,6,'Didn''t do something right'),
(2,2,4,2,'Needs more empathy');



