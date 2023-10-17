-- Put steps here to set up your database in a default good state for testing
DELETE FROM Scores;
DELETE FROM Answers;
DELETE FROM Calibrations;
DELETE FROM Questions_Options;
DELETE FROM Questions;
DELETE FROM Forms;
DELETE FROM Contacts;
DELETE FROM Options;
DELETE FROM Users;
DELETE FROM Teams;

SET IDENTITY_INSERT Teams ON

INSERT INTO Teams (team_id, team_name) VALUES (1, 'Quality Assurance');
INSERT INTO Teams (team_id, team_name) VALUES (2, 'Supervisor');
INSERT INTO Teams (team_id, team_name) VALUES (3, 'Manager');
INSERT INTO Teams (team_id, team_name) VALUES (4, 'Trainer');
INSERT INTO Teams (team_id, team_name) VALUES (5, 'None');

SET IDENTITY_INSERT Teams OFF
