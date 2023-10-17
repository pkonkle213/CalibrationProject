﻿using CalibrationApp.Models;
using System.Data.SqlClient;
using System.Data;

namespace CalibrationApp.DAO
{
    public class SqlOptionDAO
    {
        private readonly string connectionString;
        
        public SqlOptionDAO(string sqlConnectionString)
        {
            connectionString = sqlConnectionString;
        }

        public List<Option> GetAllOptions(int formId)
        {
            List<Option> options = new List<Option>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand("SelectAllOptions", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@formId", formId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Option option = new Option();

                            option.Id = Convert.ToInt32(reader["option_id"]);
                            option.OrderPosition = Convert.ToInt32(reader["orderPosition"]);
                            option.FormId = Convert.ToInt32(reader["form_id"]);
                            option.OptionValue = Convert.ToString(reader["option_value"]);
                            option.IsCategory = Convert.ToBoolean(reader["isCategory"]);
                            option.HasValue = Convert.ToBoolean(reader["hasValue"]);
                            option.PointsEarned = Convert.ToDecimal(reader["points_earned"]);

                            options.Add(option);
                        }
                    }
                }
            }

            return options;
        }

        public List<Option> GetEnabledOptionsForQuestion(int questionId)
        {
            List<Option> options = new List<Option>();

            const string sql = "SELECT o.option_id, o.orderPosition, o.option_value, o.points_earned, o.isCategory " +
                               "FROM Questions_Options qo " +
                               "INNER JOIN Options o ON o.option_id = qo.option_id " +
                               "WHERE qo.question_id = @questionId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@questionId", questionId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Option option = new Option();

                            option.Id = Convert.ToInt32(reader["option_id"]);
                            option.OrderPosition = Convert.ToInt32(reader["orderPosition"]);
                            option.OptionValue = Convert.ToString(reader["option_value"]);
                            option.PointsEarned = Convert.ToDecimal(reader["points_earned"]);
                            option.IsCategory = Convert.ToBoolean(reader["isCategory"]);

                            options.Add(option);
                        }
                    }

                }
            }

            return options;
        }
    }
}
