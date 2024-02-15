using CalibrationApp.Models;
using System.Data.SqlClient;

namespace CalibrationApp.DAO
{
    public class SqlFormDAO : IFormDAO
    {
        private readonly string connectionString;
        public SqlFormDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public int SwitchIsArchivedForm(int formId)
        {
            int rowsAffected;

            const string sql = "UPDATE Forms " +
                               "SET isArchived = 1 - isArchived " +
                               "WHERE form_id = @formId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@formId", formId);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }

        public List<Form> GetAllForms()
        {
            List<Form> forms = new List<Form>();

            const string sql = "SELECT form_id, form_name, isArchived " +
                               "FROM Forms";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            forms.Add(BuildFormFromReader(reader));
                        }
                    }
                }
            }

            return forms;
        }

        public List<Form> GetActiveForms()
        {
            List<Form> forms = new List<Form>();

            const string sql = "SELECT form_id, form_name, isArchived " +
                               "FROM Forms " +
                               "WHERE isArchived = 0";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            forms.Add(BuildFormFromReader(reader));
                        }
                    }
                }
            }

            return forms;
        }

        private Form BuildFormFromReader(SqlDataReader reader)
        {
            Form form = new Form();

            form.FormId = Convert.ToInt32(reader["form_id"]);
            form.FormName = Convert.ToString(reader["form_name"]);
            form.IsArchived = Convert.ToBoolean(reader["isArchived"]);

            return form;
        }

        public Form GetFormById(int formId)
        {
            Form form = new Form();

            const string sql = "SELECT form_id, form_name, isArchived " +
                               "FROM Forms " +
                               "WHERE form_id = @formId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@formId", formId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            form = BuildFormFromReader(reader);
                        }
                    }
                }
            }

            return form;
        }

        public Form CreateNewForm(string newFormName)
        {
            Form form = new Form();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "INSERT INTO Forms (form_name, isArchived) " +
                                   "VALUES (@formName, @isArchived); " +
                                   "SELECT @@IDENTITY";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@formName", newFormName);
                    command.Parameters.AddWithValue("@isArchived", false);
                    form.FormId = Convert.ToInt32(command.ExecuteScalar());
                    form.FormName = newFormName;
                    form.IsArchived = false;
                }
            }

            return form;
        }

        public int UpdateFormName(Form form)
        {
            int rowsAffected;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "UPDATE Forms " +
                                   "SET form_name = @formName " +
                                   "WHERE form_id = @formId";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@formName", form.FormName);
                    command.Parameters.AddWithValue("@formId", form.FormId);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }
    }
}
