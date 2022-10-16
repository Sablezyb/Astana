using Astana.Class.Log;
using Npgsql;
using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace Astana.Class.JournalMessage
{
    public class JournalMessageClass
    {
        MyDataGrid myDataGrid1;
        public static object lockObj = new object();
        public JournalMessageClass(MyDataGrid myDataGrid)
        {
            myDataGrid1 = myDataGrid;
        }
        public void UpdateError()
        {
            lock (lockObj)
            {
                if (myDataGrid1.InvokeRequired)
                    myDataGrid1.Invoke(new MethodInvoker(() =>
                    {
                        myDataGrid1.Rows.Clear();
                    }));
                else
                    myDataGrid1.Rows.Clear();

                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        NpgsqlCommand command = new NpgsqlCommand
                        {
                            Connection = connection,
                            CommandText = "SELECT date_start, message " +
                        "FROM journal_messages WHERE is_active = true " +
                        "ORDER BY date_start DESC"
                        };
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string dateStart = reader.GetValue(0).ToString();
                                    string message = reader.GetValue(1).ToString();
                                    int index = -1;
                                    if (myDataGrid1.InvokeRequired)
                                        myDataGrid1.Invoke(new MethodInvoker(() =>
                                        {
                                            index = myDataGrid1.Rows.Add(dateStart, message);
                                            myDataGrid1.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(255, 60, 60);
                                        }));
                                    else
                                    {
                                        index = myDataGrid1.Rows.Add(dateStart, message);
                                        myDataGrid1.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(255, 60, 60);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogClass.Write("JournalMessageForm:UpdateError::" + ex.Message);
                    }
                }
            }
        }
    }
}
