CREATE PROCEDURE SelectAllOptions @formId int
AS
SELECT option_id, orderPosition, isCategory, option_value, points_earned, form_id, hasValue
FROM Options
WHERE form_id = @formId
ORDER BY isCategory, orderPosition
GO
