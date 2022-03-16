﻿using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IAnswerDAO
    {
        /*
        Answer SubmitAnswer(Answer answer);
        */
        /*
        List<Answer> GetAllAnswersForCalibration(int calibrationId);
        */
        /*
        List<User> GetParticipatingUsers(int calibrationId);
        */

        List<Answer> GetMyAnswers(int calibrationId, int userId);
        void SubmitAnswers(List<Answer> answers, int userId);
        void DeleteAnswers(int calibrationId, int userId);
        void SubmitScore(Score score, int userId);
        void DeleteScore(Score score, int userId);
    }
}
