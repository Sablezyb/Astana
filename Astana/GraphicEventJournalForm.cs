using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Npgsql;
using System.Configuration;
using Astana.Class.JournalEvent.Graphick;
using Astana.Class.Log;

namespace Astana
{
    public partial class GraphicEventJournalForm : Form
    {
        private string _nameRO;
        private int _roId;
        public GraphicEventJournalForm(string nameRO)
        {
            InitializeComponent();
            _nameRO = nameRO;
            lb_nameRO.Text = _nameRO;

            switch (nameRO)
            {
                case "RO1":
                    _roId = 1;
                    cb_ROname.SelectedIndex = 0;
                    break;
                case "RO2":
                    _roId = 2;
                    cb_ROname.SelectedIndex = 1;
                    break;
                case "RO3":
                    _roId = 3;
                    cb_ROname.SelectedIndex = 2;
                    break;
                case "RO4":
                    _roId = 4;
                    cb_ROname.SelectedIndex = 3;
                    break;
                case "RO5":
                    _roId = 5;
                    cb_ROname.SelectedIndex = 4;
                    break;
                case "RO6":
                    _roId = 6;
                    cb_ROname.SelectedIndex = 5;
                    break;
                case "RO общ":
                    _roId = 7;
                    cb_ROname.SelectedIndex = 6;
                    break;
                default:
                    _roId = 0;
                    break;
            }

            cb_ROname.SelectedIndexChanged += cb_ROname_SelectedIndexChanged;
            GetLastFiveDataInDateBase();
        }

        private void GetLastFiveDataInDateBase()
        {
            List<GraphickEvent> listCheckPoint = new List<GraphickEvent>();
            List<GraphickEvent> listFlushing = new List<GraphickEvent>();

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand
                    {
                        Connection = connection,
                        CommandText = "SELECT date_event, сonsumption_normalized_water, сonsumption_permeat " +
                        "FROM journalevents " +
                        $"WHERE name_ro_id = {_roId} " +
                        $"AND name_event_id = 1 " +
                        $"ORDER BY date_event DESC " +
                        $"LIMIT 5"
                    };

                    //Конт. точка
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                GraphickEvent item = new GraphickEvent();
                                item.Date = reader.GetValue(0).ToString();
                                item.NormalizeRashodWater = Math.Round(double.Parse(reader.GetValue(1).ToString()), 2);
                                item.RashodPermiata = Math.Round(double.Parse(reader.GetValue(2).ToString()), 2);

                                listCheckPoint.Add(item);
                            }
                        }
                    }


                    command.CommandText = "SELECT date_event, сonsumption_normalized_water, сonsumption_permeat " +
                        "FROM journalevents " +
                        $"WHERE name_ro_id = {_roId} " +
                        $"AND name_event_id = 2 " +
                        $"ORDER BY date_event DESC " +
                        $"LIMIT 5";

                    //Промывка
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                GraphickEvent item = new GraphickEvent();
                                item.Date = reader.GetValue(0).ToString();
                                item.NormalizeRashodWater = Math.Round(double.Parse(reader.GetValue(1).ToString()), 2);
                                item.RashodPermiata = Math.Round(double.Parse(reader.GetValue(2).ToString()), 2);

                                listFlushing.Add(item);
                            }
                        }
                    }
                }

                for (int i = listCheckPoint.Count - 1; i >= 0; i--)
                {
                    chart1.Series[0].Points.AddXY(listCheckPoint[i].Date, listCheckPoint[i].NormalizeRashodWater);
                    chart1.Series[1].Points.AddXY(listCheckPoint[i].Date, listCheckPoint[i].RashodPermiata);
                }

                for (int i = listFlushing.Count - 1; i >= 0; i--)
                {
                    chart1.Series[2].Points.AddXY(listFlushing[i].Date, listFlushing[i].NormalizeRashodWater);
                    chart1.Series[3].Points.AddXY(listFlushing[i].Date, listFlushing[i].RashodPermiata);
                }
            }
            catch (Exception ex)
            {
                LogClass.Write("GraphickEventJournalForms:btn_success_Click::" + ex.Message);
            }
        }
        private void GetDateInDataBase()
        {
            List<GraphickEvent> listCheckPoint = new List<GraphickEvent>();
            List<GraphickEvent> listFlushing = new List<GraphickEvent>();

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand
                    {
                        Connection = connection,
                        CommandText = "SELECT date_event, сonsumption_normalized_water, сonsumption_permeat " +
                        "FROM journalevents " +
                        $"WHERE date_event BETWEEN '{dateTimePickerStart.Value}' AND '{dateTimePickerEnd.Value}' " +
                        $"AND name_ro_id = {_roId} " +
                        $"AND name_event_id = 1 " +
                        $"ORDER BY date_event"
                    };

                    //Конт. точка
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                GraphickEvent item = new GraphickEvent();
                                item.Date = reader.GetValue(0).ToString();
                                item.NormalizeRashodWater = Math.Round(double.Parse(reader.GetValue(1).ToString()), 2);
                                item.RashodPermiata = Math.Round(double.Parse(reader.GetValue(2).ToString()), 2);

                                listCheckPoint.Add(item);
                            }
                        }
                    }


                    command.CommandText = "SELECT date_event, сonsumption_normalized_water, сonsumption_permeat " +
                        "FROM journalevents " +
                        $"WHERE date_event BETWEEN '{dateTimePickerStart.Value}' AND '{dateTimePickerEnd.Value}' " +
                        $"AND name_ro_id = {_roId} " +
                        $"AND name_event_id = 2 " +
                        $"ORDER BY date_event";

                    //Промывка
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                GraphickEvent item = new GraphickEvent();
                                item.Date = reader.GetValue(0).ToString();
                                item.NormalizeRashodWater = Math.Round(double.Parse(reader.GetValue(1).ToString()), 2);
                                item.RashodPermiata = Math.Round(double.Parse(reader.GetValue(2).ToString()), 2);

                                listFlushing.Add(item);
                            }
                        }
                    }
                }

                foreach (GraphickEvent item in listCheckPoint)
                {
                    chart1.Series[0].Points.AddXY(item.Date, item.NormalizeRashodWater);
                    chart1.Series[1].Points.AddXY(item.Date, item.RashodPermiata);
                }

                foreach (GraphickEvent item in listFlushing)
                {
                    chart1.Series[2].Points.AddXY(item.Date, item.NormalizeRashodWater);
                    chart1.Series[3].Points.AddXY(item.Date, item.RashodPermiata);
                }
            }
            catch (Exception ex)
            {
                LogClass.Write("GraphickEventJournalForms:btn_success_Click::" + ex.Message);
            }
        }

        private void btn_closeForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_success_Click(object sender, EventArgs e)
        {
            foreach (var item in chart1.Series)
            {
                item.Points.Clear();
            }

            if (dateTimePickerStart.Value >= dateTimePickerEnd.Value)
            {
                MessageBox.Show("Не корректный ввод даты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GetDateInDataBase();
        }

        private void cb_CheckPoint_CheckedChanged(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].Visible = cb_CheckPoint.Checked;
        }

        private void cb_Flushing_CheckedChanged(object sender, EventArgs e)
        {
            chart1.ChartAreas[1].Visible = cb_Flushing.Checked;
        }

        private void cb_ROname_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cb_ROname.SelectedIndex)
            {
                case 0:
                    _roId = 1;
                    break;
                case 1:
                    _roId = 2;
                    break;
                case 2:
                    _roId = 3;
                    break;
                case 3:
                    _roId = 4;
                    break;
                case 4:
                    _roId = 5;
                    break;
                case 5:
                    _roId = 6;
                    break;
                case 6:
                    _roId = 7;
                    break;
            }
        }
    }
}
