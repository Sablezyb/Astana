using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Astana.Class.JournalMessage.Arhive;
using Npgsql;
using System.Configuration;
using Astana.Class.Log;

namespace Astana
{
    public partial class JournalMessageArhive : Form
    {
        private List<Arhive> _arhives;
        public JournalMessageArhive()
        {
            InitializeComponent();
        }

        private void btn_success_Click(object sender, EventArgs e)
        {
            if (dateTimePickerStart.Value >= dateTimePickerEnd.Value)
            {
                MessageBox.Show("Не корректный выбор даты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand
                    {
                        Connection = connection,
                        CommandText = "SELECT date_start, date_end, message " +
                        "FROM journal_messages " +
                        $"WHERE date_start BETWEEN '{dateTimePickerStart.Value}' AND '{dateTimePickerEnd.Value}' " +
                        $"ORDER BY date_start DESC"
                    };

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        _arhives = new List<Arhive>();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string date_start = reader.GetValue(0).ToString();
                                string date_end = reader.GetValue(1).ToString();
                                string message = reader.GetValue(2).ToString();

                                _arhives.Add(new Arhive() { Begin = date_start, End = date_end, Message = message });
                            }
                        }
                        myDataGrid1.DataSource = _arhives;
                    }
                }
                catch (Exception ex) 
                {
                    LogClass.Write("JournalMessageArhive:btn_success_Click::" + ex.Message);
                }
            }
        }

        private void btn_closeForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
