using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;
using System.Configuration;
using Astana.Class.Log;

namespace Astana
{
    public partial class EventJournalForms : Form
    {
        private Label _lbCountError;
        public EventJournalForms(Label label)
        {
            InitializeComponent();
            cb_ListRO.SelectedIndex = 6;
            myDataGrid1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            myDataGrid2.SelectionMode = DataGridViewSelectionMode.CellSelect;

            _lbCountError = label;
        }
        private void ShowDataRO(int name_ro_id)
        {
            if (!cb_checkPoint.Checked && !cb_flushing.Checked && !cb_changeMembran.Checked)
                return;

            string query = "";

            switch (name_ro_id)
            {
                case 1:
                    goto case 6;
                case 2:
                    goto case 6;
                case 3:
                    goto case 6;
                case 4:
                    goto case 6;
                case 5:
                    goto case 6;
                case 6:
                    query = "SELECT date_event, сonsumption_original_water, сonsumption_permeat, сonsumption_normalized_water, " +
                    $"сonsumption_concentrate, pressure_on_mb, pressure_permeat, pressure_after_mb, conductivity_original_water, " +
                    $"conductivity_permeat, conductivity_concentrate, t_original_water, flux, recovery, rejection, " +
                    $"name_ro_id, name_event_id ";
                    break;
                case 7:
                    query = "SELECT date_event, сonsumption_original_water, сonsumption_permeat, сonsumption_concentrate, " +
                    $"pressure_on_ustanovky, conductivity_original_water, conductivity_permeat, conductivity_concentrate, " +
                    $"pH_ishod_water, pH_ishod_water_after_corect, pH_permeata, t_original_water, " +
                    $"recovery, rejection, name_ro_id, name_event_id ";
                    break;
            }

            if (cb_checkPoint.Checked && cb_flushing.Checked && cb_changeMembran.Checked)
            {
                query += $"FROM journalevents WHERE name_ro_id = {name_ro_id}";
            }
            else if (cb_checkPoint.Checked && cb_flushing.Checked && !cb_changeMembran.Checked)
            {
                query += $"FROM journalevents WHERE name_ro_id = {name_ro_id} AND (name_event_id = {1} OR name_event_id = {2})";
            }
            else if (cb_checkPoint.Checked && cb_changeMembran.Checked && !cb_flushing.Checked)
            {
                query += $"FROM journalevents WHERE name_ro_id = {name_ro_id} AND (name_event_id = {1} OR name_event_id = {3})";
            }
            else if (cb_checkPoint.Checked && !cb_flushing.Checked && !cb_changeMembran.Checked)
            {
                query += $"FROM journalevents WHERE name_ro_id = {name_ro_id} AND name_event_id = {1}";
            }
            else if (cb_flushing.Checked && !cb_checkPoint.Checked && !cb_changeMembran.Checked)
            {
                query += $"FROM journalevents WHERE name_ro_id = {name_ro_id} AND name_event_id = {2}";
            }
            else if (!cb_checkPoint.Checked && cb_flushing.Checked && cb_changeMembran.Checked)
            {
                query += $"FROM journalevents WHERE name_ro_id = {name_ro_id} AND (name_event_id = {2} OR name_event_id = {3})";
            }
            else if (cb_changeMembran.Checked && !cb_checkPoint.Checked && !cb_flushing.Checked)
            {
                query += $"FROM journalevents WHERE name_ro_id = {name_ro_id} AND name_event_id = {3}";
            }
            query += " ORDER BY date_event DESC";

            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        NpgsqlCommand command = new NpgsqlCommand(query, connection);
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    object[] values;
                                    if (name_ro_id != 7)
                                        values = new object[15];
                                    else
                                        values = new object[14];

                                    reader.GetValues(values);

                                    int indexRow;
                                    if (name_ro_id != 7)
                                        indexRow = myDataGrid1.Rows.Add(values);
                                    else
                                        indexRow = myDataGrid2.Rows.Add(values);

                                    object id_event;
                                    if (name_ro_id != 7)
                                        id_event = reader.GetValue(16);
                                    else
                                        id_event = reader.GetValue(15);

                                    if (name_ro_id != 7)
                                    {
                                        if ((int)id_event == 1)
                                        {
                                            myDataGrid1.Rows[indexRow].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 128);
                                        }
                                        else if ((int)id_event == 2)
                                        {
                                            myDataGrid1.Rows[indexRow].DefaultCellStyle.BackColor = Color.FromArgb(128, 255, 128);
                                        }
                                        else if ((int)id_event == 3)
                                        {
                                            myDataGrid1.Rows[indexRow].DefaultCellStyle.BackColor = Color.Cyan;
                                        }
                                    }
                                    else
                                    {
                                        if ((int)id_event == 1)
                                        {
                                            myDataGrid2.Rows[indexRow].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 128);
                                        }
                                        else if ((int)id_event == 2)
                                        {
                                            myDataGrid2.Rows[indexRow].DefaultCellStyle.BackColor = Color.FromArgb(128, 255, 128);
                                        }
                                        else if ((int)id_event == 3)
                                        {
                                            myDataGrid2.Rows[indexRow].DefaultCellStyle.BackColor = Color.Cyan;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClass.Write("EventJournalForms:ShowDataRO::" + ex.Message);
                }
            }
        }
        private void WriteDataBase(string btn_tag, int idEvent)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    if (connection.State == ConnectionState.Open)
                    {
                        List<int> listId;

                        if (btn_tag == "RO1")
                            listId = new List<int>()
                            {
                                1051, 1127, 2912,
                                1063, 936, 1073,
                                1085, 943, 1092,
                                2925,  973, 1514,
                                1472, 2899
                            };
                        else if (btn_tag == "RO2")
                            listId = new List<int>()
                            {
                                1053, 1128, 2913,
                                1065, 936, 1075,
                                1087, 943, 1094,
                                2926,  973, 1515,
                                1473, 2900
                            };
                        else if (btn_tag == "RO3")
                            listId = new List<int>()
                            {
                                1055, 1129, 2914,
                                1043, 936, 1077,
                                1089, 943, 1096,
                                2927,  973, 1516,
                                1474, 2901
                            };
                        else if (btn_tag == "RO4")
                            listId = new List<int>()
                            {
                                1057, 1130, 2915,
                                1045, 936, 1079,
                                1067, 943, 1098,
                                2928,  973, 1517,
                                1475, 2902
                            };
                        else if (btn_tag == "RO5")
                            listId = new List<int>()
                            {
                                1059, 1131, 2916,
                                1047, 936, 1081,
                                1069, 943, 1100,
                                2929,  973, 1518,
                                1476, 2903
                            };
                        else if (btn_tag == "RO6")
                            listId = new List<int>()
                            {
                                1061, 1132, 2917,
                                1049, 936, 1083,
                                1071, 943, 1102,
                                2930,  973, 1519,
                                1477, 2904
                            };
                        else // RO общ
                            listId = new List<int>()
                            {
                                1008, 1126, 1125,
                                936, 943, 1104,
                                2931,  944, 937,
                                1090, 973, 1478,
                                2905
                            };

                        List<object> datas = new List<object>();
                        datas.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                        for (int i = 0; i < listId.Count; i++)
                        {
                            string query = "SELECT value " +
                                "FROM masterscadadataraw " +
                                $"WHERE itemid = {listId[i]} " +
                                $"ORDER BY \"Time\" DESC " +
                                "LIMIT 1";

                            NpgsqlCommand command = new NpgsqlCommand(query, connection);
                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    object data = reader.GetValue(0);

                                    datas.Add(data);
                                }
                                else
                                {
                                    datas.Add("null");
                                }
                            }
                        }

                        for (int i = 0; i < datas.Count; i++)
                        {
                            if (datas[i] != null)
                                datas[i] = datas[i].ToString().Replace(',', '.');
                        }

                        using (NpgsqlConnection connectionCleverosOsmos = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
                        {
                            connectionCleverosOsmos.Open();
                            if (connectionCleverosOsmos.State == ConnectionState.Open)
                            {
                                int idRO = -1;
                                if (btn_tag == "RO1") idRO = 1;
                                else if (btn_tag == "RO2") idRO = 2;
                                else if (btn_tag == "RO3") idRO = 3;
                                else if (btn_tag == "RO4") idRO = 4;
                                else if (btn_tag == "RO5") idRO = 5;
                                else if (btn_tag == "RO6") idRO = 6;
                                else idRO = 7;

                                string query = "";
                                switch (idRO)
                                {
                                    case 1:
                                        goto case 6;
                                    case 2:
                                        goto case 6;
                                    case 3:
                                        goto case 6;
                                    case 4:
                                        goto case 6;
                                    case 5:
                                        goto case 6;
                                    case 6:
                                        query = $"INSERT INTO " +
                                    $"journalevents(date_event, сonsumption_original_water, сonsumption_permeat, сonsumption_normalized_water, " +
                                    $"сonsumption_concentrate, pressure_on_mb, pressure_permeat, pressure_after_mb, conductivity_original_water, " +
                                    $"conductivity_permeat, conductivity_concentrate, t_original_water, flux, recovery, rejection, name_ro_id, name_event_id) " +
                                    $"VALUES ('{datas[0]}', {datas[1]}, {datas[2]}, " +
                                    $"{datas[3]}, {datas[4]}, {datas[5]}, " +
                                    $"{datas[6]}, {datas[7]}, {datas[8]}, " +
                                    $"{datas[9]}, {datas[10]}, {datas[11]}, " +
                                    $"{datas[12]}, {datas[13]}, {datas[14]}, " +
                                    $"{idRO}, {idEvent})";
                                        break;
                                    case 7:
                                        query = $"INSERT INTO " +
                                    $"journalevents(date_event, сonsumption_original_water, сonsumption_permeat, сonsumption_concentrate, " +
                                    $"pressure_on_ustanovky, conductivity_original_water, conductivity_permeat, conductivity_concentrate, " +
                                    $"pH_ishod_water, pH_ishod_water_after_corect, pH_permeata, t_original_water, " +
                                    $"recovery, rejection, name_ro_id, name_event_id) " +
                                    $"VALUES ('{datas[0]}', {datas[1]}, {datas[2]}, " +
                                    $"{datas[3]}, {datas[4]}, {datas[5]}, " +
                                    $"{datas[6]}, {datas[7]}, {datas[8]}, " +
                                    $"{datas[9]}, {datas[10]}, {datas[11]}, " +
                                    $"{datas[12]}, {datas[13]}, " +
                                    $"{idRO}, {idEvent})";
                                        break;
                                    default:
                                        break;
                                }

                                if (query != "")
                                {
                                    NpgsqlCommand command = new NpgsqlCommand(query, connectionCleverosOsmos);
                                    command.ExecuteNonQuery();

                                    if (cb_ListRO.SelectedIndex != idRO - 1)
                                        cb_ListRO.SelectedIndex = idRO - 1;
                                    else
                                    {
                                        myDataGrid1.Rows.Clear();
                                        ShowDataRO(cb_ListRO.SelectedIndex + 1);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClass.Write("EventJournalForms:WriteDataBase::" + ex.Message);
                }
            }
        }
        private void CheckChangeMembran(string btn_tag)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    if (connection.State == ConnectionState.Open)
                    {
                        int idRO = -1;
                        if (btn_tag == "RO1") idRO = 1;
                        else if (btn_tag == "RO2") idRO = 2;
                        else if (btn_tag == "RO3") idRO = 3;
                        else if (btn_tag == "RO4") idRO = 4;
                        else if (btn_tag == "RO5") idRO = 5;
                        else if (btn_tag == "RO6") idRO = 6;
                        else idRO = 7;

                        NpgsqlCommand commandCheckError = new NpgsqlCommand
                        {
                            Connection = connection,
                            CommandText = "SELECT COUNT(*) " +
                        "FROM journal_messages " +
                        $"WHERE itemid = {idRO} AND is_active = true"
                        };

                        bool isFindEror = false;
                        using (NpgsqlDataReader reader = commandCheckError.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                int countError = int.Parse(reader.GetValue(0).ToString());

                                if (countError > 0)
                                    isFindEror = true;
                            }
                        }

                        if (isFindEror == false)
                        {
                            NpgsqlCommand command = new NpgsqlCommand
                            {
                                Connection = connection,
                                CommandText = "SELECT date_event " +
                            "FROM journalevents " +
                            $"WHERE name_event_id = 3 AND name_ro_id = {idRO} " +
                            $"ORDER BY date_event DESC " +
                            $"LIMIT 1"
                            };

                            bool isFindChangeMembran = false;
                            string dateCnahgeMembran = "";
                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                reader.Read();
                                if (reader.HasRows)
                                {
                                    isFindChangeMembran = true;
                                    dateCnahgeMembran = reader.GetValue(0).ToString();
                                }
                            }

                            if (isFindChangeMembran == true)
                            {
                                command.CommandText = "SELECT сonsumption_normalized_water " +
                                    "FROM journalevents " +
                                    $"WHERE date_event BETWEEN '{dateCnahgeMembran}' AND '{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}' " +
                                    $"AND name_event_id = 2 AND name_ro_id = {idRO} " +
                                    $"ORDER BY date_event DESC " +
                                    $"LIMIT 5";

                                using (NpgsqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        int countErro = 0;
                                        while (reader.Read())
                                        {
                                            double normalize = double.Parse(reader.GetValue(0).ToString().Replace('.', ','));
                                            if (normalize < (GetProizvoditelnostForNameRO(btn_tag) - ((GetProcentForNameRO(btn_tag) * GetProizvoditelnostForNameRO(btn_tag)) / 100)))
                                            {
                                                countErro++;
                                            }
                                        }

                                        if (countErro >= 5)
                                        {
                                            AddProblemDataBase(btn_tag, idRO);
                                            ChangeCountError('+');
                                        }
                                    }
                                }
                            }
                            else
                            {
                                command.CommandText = "SELECT сonsumption_normalized_water " +
                                    "FROM journalevents " +
                                    $"WHERE name_event_id = 2 AND name_ro_id = {idRO} " +
                                    $"ORDER BY date_event DESC " +
                                    $"LIMIT 5";

                                using (NpgsqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        int countErro = 0;
                                        while (reader.Read())
                                        {
                                            double normalize = double.Parse(reader.GetValue(0).ToString().Replace('.', ','));
                                            if (normalize < (GetProizvoditelnostForNameRO(btn_tag) - ((GetProcentForNameRO(btn_tag) * GetProizvoditelnostForNameRO(btn_tag)) / 100)))
                                            {
                                                countErro++;
                                            }
                                        }

                                        if (countErro >= 5)
                                        {
                                            AddProblemDataBase(btn_tag, idRO);
                                            ChangeCountError('+');
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) 
                {
                    LogClass.Write("EventJournalForms:CheckChangeMembran::" + ex.Message);
                }
            }
        }
        private double GetProcentForNameRO(string nameRO)
        {
            if (nameRO == "RO1")
                return AllSettings.procentNizkoyProizvoditelnostiRo1;
            else if (nameRO == "RO2")
                return AllSettings.procentNizkoyProizvoditelnostiRo2;
            else if (nameRO == "RO3")
                return AllSettings.procentNizkoyProizvoditelnostiRo3;
            else if (nameRO == "RO4")
                return AllSettings.procentNizkoyProizvoditelnostiRo4;
            else if (nameRO == "RO5")
                return AllSettings.procentNizkoyProizvoditelnostiRo5;
            else
                return AllSettings.procentNizkoyProizvoditelnostiRo6;
        }
        private double GetProizvoditelnostForNameRO(string nameRO)
        {
            if (nameRO == "RO1")
                return AllSettings.proizvoditelnostRO1;
            else if (nameRO == "RO2")
                return AllSettings.proizvoditelnostRO2;
            else if (nameRO == "RO3")
                return AllSettings.proizvoditelnostRO3;
            else if (nameRO == "RO4")
                return AllSettings.proizvoditelnostRO4;
            else if (nameRO == "RO5")
                return AllSettings.proizvoditelnostRO5;
            else
                return AllSettings.proizvoditelnostRO6;
        }
        private void AddProblemDataBase(string nameRO, int idRO)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    command.CommandText = "INSERT INTO journal_messages (date_start, message, is_active, name_problem, itemid) " +
                        $"VALUES ('{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}', " +
                        $"'Рекомендуется замена мембран линии {nameRO}', " +
                        $"true, " +
                        $"'NeedChangeMembran', " +
                        $"{idRO})";
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogClass.Write("EventJournalForms:AddProblemDataBase::" + ex.Message);
                }
            }
        }
        private void ChangeCountError(char znak)
        {
            switch (znak)
            {
                case '+':
                    MainForm._countError++;
                    MainForm.journalMessage.UpdateError();
                    break;
                case '-':
                    MainForm.journalMessage.UpdateError();
                    if (MainForm._countError > 0)
                        MainForm._countError--;
                    break;
            }

            if (_lbCountError.InvokeRequired)
            {
                _lbCountError.Invoke(new MethodInvoker(() =>
                {
                    _lbCountError.Text = MainForm._countError.ToString();
                }));
            }
            else
            {
                _lbCountError.Text = MainForm._countError.ToString();
            }
        }
        private void ResetNeedChangeMembranForNameRO(string btn_tag)
        {
            int idRO = -1;
            if (btn_tag == "RO1") idRO = 1;
            else if (btn_tag == "RO2") idRO = 2;
            else if (btn_tag == "RO3") idRO = 3;
            else if (btn_tag == "RO4") idRO = 4;
            else if (btn_tag == "RO5") idRO = 5;
            else if (btn_tag == "RO6") idRO = 6;
            else idRO = 7;

            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        NpgsqlCommand command = new NpgsqlCommand
                        {
                            Connection = connection,
                            CommandText = "UPDATE journal_messages " +
                                $"SET is_active = 'false', date_end = '{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}' " +
                                $"WHERE itemid = {idRO} AND is_active = true"
                        };

                        int count = command.ExecuteNonQuery();
                        MainForm._countError -= count;
                        if (MainForm._countError < 0)
                            MainForm._countError = 0;
                        _lbCountError.Text = MainForm._countError.ToString();

                        if (count > 0)
                            MainForm.journalMessage.UpdateError();
                    }
                }
                catch (Exception ex)
                {
                    LogClass.Write("EventJournalForms:ResetNeedChangeMembranForNameRO::" + ex.Message);
                }
            }
        }



        private void btn_closeForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btn_checkPoint_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
           
            if (MessageBox.Show($"Выполнить {btn.Text} {btn.Tag} ?", "Уведомление", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                WriteDataBase(btn.Tag.ToString(), 1);
            }
        }

        private void btn_flushing_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (MessageBox.Show($"Выполнить {btn.Text} {btn.Tag} ?", "Уведомление", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                WriteDataBase(btn.Tag.ToString(), 2);
                CheckChangeMembran(btn.Tag.ToString());
            }
        }

        private void btn_change_membran_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (MessageBox.Show($"Выполнить {btn.Text} {btn.Tag} ?", "Уведомление", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                WriteDataBase(btn.Tag.ToString(), 3);
                ResetNeedChangeMembranForNameRO(btn.Tag.ToString());
            }
        }

        private void cb_ListRO_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cb_ListRO.SelectedIndex)
            {
                case 0:
                    myDataGrid1.Rows.Clear();
                    ShowDataRO(1);
                    myDataGrid1.Visible = true;
                    myDataGrid2.Visible = false;
                    break;
                case 1:
                    myDataGrid1.Rows.Clear();
                    ShowDataRO(2);
                    myDataGrid1.Visible = true;
                    myDataGrid2.Visible = false;
                    break;
                case 2:
                    myDataGrid1.Rows.Clear();
                    ShowDataRO(3);
                    myDataGrid1.Visible = true;
                    myDataGrid2.Visible = false;
                    break;
                case 3:
                    myDataGrid1.Rows.Clear();
                    ShowDataRO(4);
                    myDataGrid1.Visible = true;
                    myDataGrid2.Visible = false;
                    break;
                case 4:
                    myDataGrid1.Rows.Clear();
                    ShowDataRO(5);
                    myDataGrid1.Visible = true;
                    myDataGrid2.Visible = false;
                    break;
                case 5:
                    myDataGrid1.Rows.Clear();
                    ShowDataRO(6);
                    myDataGrid1.Visible = true;
                    myDataGrid2.Visible = false;
                    break;
                case 6:
                    myDataGrid2.Rows.Clear();
                    ShowDataRO(7);
                    myDataGrid1.Visible = false;
                    myDataGrid2.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            myDataGrid1.Rows.Clear();
            switch (cb_ListRO.SelectedIndex)
            {
                case 0:
                    ShowDataRO(1);
                    myDataGrid1.Visible = true;
                    myDataGrid2.Visible = false;
                    break;
                case 1:
                    ShowDataRO(2);
                    myDataGrid1.Visible = true;
                    myDataGrid2.Visible = false;
                    break;
                case 2:
                    ShowDataRO(3);
                    myDataGrid1.Visible = true;
                    myDataGrid2.Visible = false;
                    break;
                case 3:
                    ShowDataRO(4);
                    myDataGrid1.Visible = true;
                    myDataGrid2.Visible = false;
                    break;
                case 4:
                    ShowDataRO(5);
                    myDataGrid1.Visible = true;
                    myDataGrid2.Visible = false;
                    break;
                case 5:
                    ShowDataRO(6);
                    myDataGrid1.Visible = true;
                    myDataGrid2.Visible = false;
                    break;
                case 6:
                    ShowDataRO(7);
                    myDataGrid1.Visible = false;
                    myDataGrid2.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void btn_graphick_Click(object sender, EventArgs e)
        {
            GraphicEventJournalForm form = new GraphicEventJournalForm(cb_ListRO.Text);
            form.ShowDialog();
        }
    }
}
