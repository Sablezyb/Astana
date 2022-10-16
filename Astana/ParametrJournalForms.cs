using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Astana.Class.JournalPatametr;
using Npgsql;
using System.Configuration;
using System.Text.RegularExpressions;
using Astana.Class.Log;

namespace Astana
{
    public partial class ParametrJournalForms : Form
    {
        private Thread _thread;
        private string[] lbStatusText = { "Запрос выполняется.", "Запрос выполняется..", "Запрос выполняется..." };
        private NameRO _nameRO = NameRO.ROall;
        private List<int> listId = new List<int>()
        {
            1008, //Расход исходной воды на установку 
            1126, //Расход пермеата
            1125, //Расход концентрата
            936, //Давление на установку
            943, //Проводимость исходной воды
            1104, //Проводимость пермеата
            2931, //Проводимость концентрата
            944, //pH Исходной воды
            937, //pH  Исходной воды после коррекции
            1090, //pH Пермеата
            973, //Температура исходной воды
            1478, //Recovery 
            2905 //Rejection
        };
        private Regex _regex = new Regex(@"^\d+$");
        public ParametrJournalForms()
        {
            InitializeComponent();
        }
        private void btn_closeForm_Click(object sender, EventArgs e)
        {
            try
            {
                if (_thread != null &&
                    (_thread.ThreadState == ThreadState.Running ||
                    _thread.ThreadState == ThreadState.WaitSleepJoin ||
                    _thread.ThreadState == ThreadState.Stopped))
                    _thread.Abort();
            }
            catch { }

            this.Close();
        }
        private void Open_RO_Journal (object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Text)
            {
                case "RO1":
                    lb_name.Text = btn.Text;
                    myDataGrid1.Visible = false;
                    myDataGrid2.Visible = true;
                    ChangeBackGroundButton(Color.Lime, btn_RO1);
                    ChangeBackGroundButton(Color.FromArgb(224, 224, 224), btn_RO2, btn_RO3, btn_RO4, btn_RO5, btn_RO6, btn_ROall);
                    _nameRO = NameRO.RO1;
                    break;
                case "RO2":
                    lb_name.Text = btn.Text;
                    myDataGrid1.Visible = false;
                    myDataGrid2.Visible = true;
                    ChangeBackGroundButton(Color.Lime, btn_RO2);
                    ChangeBackGroundButton(Color.FromArgb(224, 224, 224), btn_RO1, btn_RO3, btn_RO4, btn_RO5, btn_RO6, btn_ROall);
                    _nameRO = NameRO.RO2;
                    break;
                case "RO3":
                    lb_name.Text = btn.Text;
                    myDataGrid1.Visible = false;
                    myDataGrid2.Visible = true;
                    ChangeBackGroundButton(Color.Lime, btn_RO3);
                    ChangeBackGroundButton(Color.FromArgb(224, 224, 224), btn_RO2, btn_RO1, btn_RO4, btn_RO5, btn_RO6, btn_ROall);
                    _nameRO = NameRO.RO3;
                    break;
                case "RO4":
                    lb_name.Text = btn.Text;
                    myDataGrid1.Visible = false;
                    myDataGrid2.Visible = true;
                    ChangeBackGroundButton(Color.Lime, btn_RO4);
                    ChangeBackGroundButton(Color.FromArgb(224, 224, 224), btn_RO2, btn_RO1, btn_RO3, btn_RO5, btn_RO6, btn_ROall);
                    _nameRO = NameRO.RO4;
                    break;
                case "RO5":
                    lb_name.Text = btn.Text;
                    myDataGrid1.Visible = false;
                    myDataGrid2.Visible = true;
                    ChangeBackGroundButton(Color.Lime, btn_RO5);
                    ChangeBackGroundButton(Color.FromArgb(224, 224, 224), btn_RO2, btn_RO1, btn_RO3, btn_RO4, btn_RO6, btn_ROall);
                    _nameRO = NameRO.RO5;
                    break;
                case "RO6":
                    lb_name.Text = btn.Text;
                    myDataGrid1.Visible = false;
                    myDataGrid2.Visible = true;
                    ChangeBackGroundButton(Color.Lime, btn_RO6);
                    ChangeBackGroundButton(Color.FromArgb(224, 224, 224), btn_RO2, btn_RO1, btn_RO3, btn_RO5, btn_RO4, btn_ROall);
                    _nameRO = NameRO.RO6;
                    break;
                case "Установка":
                    lb_name.Text = btn.Text;
                    myDataGrid1.Visible = true;
                    myDataGrid2.Visible = false;
                    ChangeBackGroundButton(Color.Lime, btn_ROall);
                    ChangeBackGroundButton(Color.FromArgb(224, 224, 224), btn_RO2, btn_RO1, btn_RO3, btn_RO5, btn_RO6, btn_RO4);
                    _nameRO = NameRO.ROall;
                    break;
            }
        }
        private void UpdateDataROall()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    DateTime time = dateTimePickerStart.Value;
                    int interval = int.Parse(tb_interval.Text);
                    List<JournalParametr> listParam = new List<JournalParametr>();
                    int indexLbStatusText = -1;
                    do
                    {
                        Console.WriteLine(Thread.CurrentThread.Name);

                        if (++indexLbStatusText >= lbStatusText.Length) indexLbStatusText = 0;
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = lbStatusText[indexLbStatusText];
                        }));

                        JournalParametr journalParametr = new JournalParametr();
                        bool isData = false;
                        foreach (var item in listId)
                        {
                            NpgsqlCommand command = new NpgsqlCommand
                            {
                                Connection = connection,
                                CommandText = "SELECT value " +
                                "FROM masterscadadataraw " +
                                $"WHERE layer = 1 AND itemid = {item} AND \"Time\" " +
                                $"BETWEEN {time.Ticks} AND {time.AddSeconds(interval).Ticks} " +
                                $"ORDER BY \"Time\" DESC " +
                                $"LIMIT 1"
                            };
                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    isData = true;
                                   
                                    string value = reader.GetValue(0).ToString();
                                    if (value != "")
                                        value = string.Format("{0:f2}", double.Parse(value));
                                    switch (item)
                                    {
                                        case 1008:
                                            journalParametr.RashodIshodWater = value;
                                            break;
                                        case 1126:
                                            journalParametr.RashodPermeata = value;
                                            break;
                                        case 1125:
                                            journalParametr.RashodConcentrata = value;
                                            break;
                                        case 936:
                                            journalParametr.DavlenieNaUstanovky = value;
                                            break;
                                        case 943:
                                            journalParametr.ProvodimostIshodWater = value;
                                            break;
                                        case 1104:
                                            journalParametr.ProvodimostPermeata = value;
                                            break;
                                        case 2931:
                                            journalParametr.ProvodimostConcentrata = value;
                                            break;
                                        case 944:
                                            journalParametr.pHIshodWater = value;
                                            break;
                                        case 937:
                                            journalParametr.pHIshodWaterPosleCorekci = value;
                                            break;
                                        case 1090:
                                            journalParametr.pHPermeata = value;
                                            break;
                                        case 973:
                                            journalParametr.TemperaturaIshodWater = value;
                                            break;
                                        case 1478:
                                            journalParametr.Recovery = value;
                                            break;
                                        case 2905:
                                            journalParametr.Rejection = value;
                                            break;
                                    }
                                }
                            }
                        }

                        if (isData)
                        {
                            journalParametr.Date = time.ToString("dd.MM.yyyy HH:mm:ss");
                            listParam.Add(journalParametr);
                        }
                    } while ((time = time.AddSeconds(interval)).Ticks < dateTimePickerEnd.Value.Ticks);


                    if (myDataGrid1.InvokeRequired)
                    {
                        myDataGrid1.Invoke(new MethodInvoker(() =>
                        {
                            myDataGrid1.DataSource = listParam;
                        }));
                    }
                    else
                    {
                        myDataGrid1.DataSource = listParam;
                    }

                    if (lb_status.InvokeRequired)
                    {
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = "Запрос завершен";
                        }));
                    }
                    else
                        lb_status.Text = "Запрос завершен";

                    if (btn_Success.InvokeRequired)
                        btn_Success.Invoke(new MethodInvoker(() =>
                        {
                            btn_Success.Enabled = true;
                        }));
                    else
                        btn_Success.Enabled = true;
                }
                catch (Exception ex)
                {
                    LogClass.Write("ParametrJournalForms:UpdateData::" + ex.Message);
                }
            }
        }
        private void btn_Success_Click(object sender, EventArgs e)
        {
            if (dateTimePickerStart.Value >= dateTimePickerEnd.Value)
            {
                MessageBox.Show("Не корректный выбор даты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Match match = _regex.Match(tb_interval.Text);
            if (match.Success == false)
            {
                MessageBox.Show("Не корректный выбор интервала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lb_status.Text = "Запрос выполняется";
            btn_Success.Enabled = false;
            btn_Cancel.Enabled = true;

            if (_nameRO == NameRO.RO1) (_thread = new Thread(WorkRO1) { IsBackground = false, Name = "RO1" }).Start();
            else if (_nameRO == NameRO.RO2)(_thread = new Thread(WorkRO2) { IsBackground = false }).Start();
            else if (_nameRO == NameRO.RO3)(_thread = new Thread(WorkRO3) { IsBackground = false }).Start();
            else if (_nameRO == NameRO.RO4)(_thread = new Thread(WorkRO4) { IsBackground = false }).Start();
            else if (_nameRO == NameRO.RO5)(_thread = new Thread(WorkRO5) { IsBackground = false }).Start();
            else if (_nameRO == NameRO.RO6)(_thread = new Thread(WorkRO6) { IsBackground = false }).Start();
            else if (_nameRO == NameRO.ROall)(_thread = new Thread(UpdateDataROall) { IsBackground = false, Name = "MyThread" }).Start();

            //switch (_nameRO)
            //{
            //    case NameRO.RO1:
            //        _thread = new Thread(WorkRO1) { IsBackground = false };
            //        _thread.Start();
            //        break;
            //    case NameRO.RO2:
            //        _thread = new Thread(WorkRO2) { IsBackground = false };
            //        _thread.Start();
            //        break;
            //    case NameRO.RO3:
            //        _thread = new Thread(WorkRO3) { IsBackground = false };
            //        _thread.Start();
            //        break;
            //    case NameRO.RO4:
            //        _thread = new Thread(WorkRO4) { IsBackground = false };
            //        _thread.Start();
            //        break;
            //    case NameRO.RO5:
            //        _thread = new Thread(WorkRO5) { IsBackground = false };
            //        _thread.Start();
            //        break;
            //    case NameRO.RO6:
            //        _thread = new Thread(WorkRO6) { IsBackground = false };
            //        _thread.Start();
            //        break;
            //    case NameRO.ROall:
            //        _thread = new Thread(UpdateDataROall) { IsBackground = false, Name = "My Thread" };
            //        _thread.Start();
            //        break;
            //}
        }

        private void WorkRO1()
        {
            List<int> listId = new List<int>()
            {
                1051, 1127, 2912,
                1063, 936, 1073,
                1085, 943, 1092,
                2925, 973, 1514,
                1472, 2899
            };
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    DateTime time = dateTimePickerStart.Value;
                    int interval = int.Parse(tb_interval.Text);
                    List<JournalParametrRO> listParam = new List<JournalParametrRO>();
                    int indexLbStatusText = -1;
                    do
                    {
                        if (++indexLbStatusText >= lbStatusText.Length) indexLbStatusText = 0;
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = lbStatusText[indexLbStatusText];
                        }));

                        JournalParametrRO journalParametrRO = new JournalParametrRO();
                        bool isData = false;
                        foreach (var item in listId)
                        {
                            NpgsqlCommand command = new NpgsqlCommand
                            {
                                Connection = connection,
                                CommandText = "SELECT value " +
                                "FROM masterscadadataraw " +
                                $"WHERE layer = 1 AND itemid = {item} AND \"Time\" " +
                                $"BETWEEN {time.Ticks} AND {time.AddSeconds(interval).Ticks} " +
                                $"ORDER BY \"Time\" DESC " +
                                $"LIMIT 1"
                            };
                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    isData = true;
                                    //double value = double.Parse(reader.GetValue(0).ToString());
                                    string value = reader.GetValue(0).ToString();
                                    if (value != "")
                                        value = string.Format("{0:f2}", double.Parse(value));
                                    switch (item)
                                    {
                                        case 1051:
                                            journalParametrRO.RashodIshodWater = value;
                                            break;
                                        case 1127:
                                            journalParametrRO.RashodPermeata = value;
                                            break;
                                        case 2912:
                                            journalParametrRO.NormalizeRashod = value;
                                            break;
                                        case 1063:
                                            journalParametrRO.RashodConcentrata = value;
                                            break;
                                        case 936:
                                            journalParametrRO.DavlenieNaMB = value;
                                            break;
                                        case 1073:
                                            journalParametrRO.DavleniePermeata = value;
                                            break;
                                        case 1085:
                                            journalParametrRO.DavleniePosleMB = value;
                                            break;
                                        case 943:
                                            journalParametrRO.ProvodimostIshodWater = value;
                                            break;
                                        case 1092:
                                            journalParametrRO.ProvodimostPermeata = value;
                                            break;
                                        case 2925:
                                            journalParametrRO.ProvodimostConcentrata = value;
                                            break;
                                        case 973:
                                            journalParametrRO.TemperaturaIshodWater = value;
                                            break;
                                        case 1514:
                                            journalParametrRO.Flux = value;
                                            break;
                                        case 1472:
                                            journalParametrRO.Recovery = value;
                                            break;
                                        case 2899:
                                            journalParametrRO.Rejection = value;
                                            break;
                                    }
                                }
                            }
                        }

                        if (isData)
                        {
                            journalParametrRO.Date = time.ToString("dd.MM.yyyy HH:mm:ss");
                            listParam.Add(journalParametrRO);
                        }
                    } while ((time = time.AddSeconds(interval)) < dateTimePickerEnd.Value);



                    if (myDataGrid2.InvokeRequired)
                    {
                        myDataGrid2.Invoke(new MethodInvoker(() =>
                        {
                            myDataGrid2.DataSource = listParam;
                        }));
                    }
                    else
                    {
                        myDataGrid2.DataSource = listParam;
                    }

                    if (lb_status.InvokeRequired)
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = "Запрос завершен";
                        }));
                    else
                        lb_status.Text = "Запрос завершен";

                    if (btn_Success.InvokeRequired)
                        btn_Success.Invoke(new MethodInvoker(() =>
                        {
                            btn_Success.Enabled = true;
                        }));
                    else
                        btn_Success.Enabled = true;

                    if (btn_Cancel.InvokeRequired)
                        btn_Cancel.Invoke(new MethodInvoker(() =>
                        {
                            btn_Cancel.Enabled = false;
                        }));
                    else
                        btn_Cancel.Enabled = false;
                }
                catch (Exception ex)
                {
                    LogClass.Write("RO_Journal:WorkRO1::" + ex.Message);
                }
            }
        }
        private void WorkRO2()
        {
            List<int> listId = new List<int>()
            {
                1053, 1128, 2913,
                1065, 936, 1075,
                1087, 943, 1094,
                2926, 973, 1515,
                1473, 2900
            };
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    DateTime time = dateTimePickerStart.Value;
                    int interval = int.Parse(tb_interval.Text);
                    List<JournalParametrRO> listParam = new List<JournalParametrRO>();
                    int indexLbStatusText = -1;
                    do
                    {
                        if (++indexLbStatusText >= lbStatusText.Length) indexLbStatusText = 0;
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = lbStatusText[indexLbStatusText];
                        }));

                        JournalParametrRO journalParametrRO = new JournalParametrRO();
                        bool isData = false;
                        foreach (var item in listId)
                        {
                            NpgsqlCommand command = new NpgsqlCommand
                            {
                                Connection = connection,
                                CommandText = "SELECT value " +
                                "FROM masterscadadataraw " +
                                $"WHERE layer = 1 AND itemid = {item} AND \"Time\" " +
                                $"BETWEEN {time.Ticks} AND {time.AddSeconds(interval).Ticks} " +
                                $"ORDER BY \"Time\" DESC " +
                                $"LIMIT 1"
                            };
                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    isData = true;
                                    //double value = double.Parse(reader.GetValue(0).ToString());
                                    string value = reader.GetValue(0).ToString();
                                    if (value != "")
                                        value = string.Format("{0:f2}", double.Parse(value));
                                    switch (item)
                                    {
                                        case 1053:
                                            journalParametrRO.RashodIshodWater = value;
                                            break;
                                        case 1128:
                                            journalParametrRO.RashodPermeata = value;
                                            break;
                                        case 2913:
                                            journalParametrRO.NormalizeRashod = value;
                                            break;
                                        case 1065:
                                            journalParametrRO.RashodConcentrata = value;
                                            break;
                                        case 936:
                                            journalParametrRO.DavlenieNaMB = value;
                                            break;
                                        case 1075:
                                            journalParametrRO.DavleniePermeata = value;
                                            break;
                                        case 1087:
                                            journalParametrRO.DavleniePosleMB = value;
                                            break;
                                        case 943:
                                            journalParametrRO.ProvodimostIshodWater = value;
                                            break;
                                        case 1094:
                                            journalParametrRO.ProvodimostPermeata = value;
                                            break;
                                        case 2926:
                                            journalParametrRO.ProvodimostConcentrata = value;
                                            break;
                                        case 973:
                                            journalParametrRO.TemperaturaIshodWater = value;
                                            break;
                                        case 1515:
                                            journalParametrRO.Flux = value;
                                            break;
                                        case 1473:
                                            journalParametrRO.Recovery = value;
                                            break;
                                        case 2900:
                                            journalParametrRO.Rejection = value;
                                            break;
                                    }
                                }
                            }
                        }

                        if (isData)
                        {
                            journalParametrRO.Date = time.ToString("dd.MM.yyyy HH:mm:ss");
                            listParam.Add(journalParametrRO);
                        }
                    } while ((time = time.AddSeconds(interval)) < dateTimePickerEnd.Value);



                    if (myDataGrid2.InvokeRequired)
                    {
                        myDataGrid2.Invoke(new MethodInvoker(() =>
                        {
                            myDataGrid2.DataSource = listParam;
                        }));
                    }
                    else
                    {
                        myDataGrid2.DataSource = listParam;
                    }

                    if (lb_status.InvokeRequired)
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = "Запрос завершен";
                        }));
                    else
                        lb_status.Text = "Запрос завершен";

                    if (btn_Success.InvokeRequired)
                        btn_Success.Invoke(new MethodInvoker(() =>
                        {
                            btn_Success.Enabled = true;
                        }));
                    else
                        btn_Success.Enabled = true;

                    if (btn_Cancel.InvokeRequired)
                        btn_Cancel.Invoke(new MethodInvoker(() =>
                        {
                            btn_Cancel.Enabled = false;
                        }));
                    else
                        btn_Cancel.Enabled = false;
                }
                catch (Exception ex)
                {
                    LogClass.Write("RO_Journal:WorkRO2::" + ex.Message);
                }
            }
        }
        private void WorkRO3()
        {
            List<int> listId = new List<int>()
            {
                1055, 1129, 2914,
                1043, 936, 1077,
                1089, 943, 1096,
                2927, 973, 1516,
                1474, 2901
            };
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    DateTime time = dateTimePickerStart.Value;
                    int interval = int.Parse(tb_interval.Text);
                    List<JournalParametrRO> listParam = new List<JournalParametrRO>();
                    int indexLbStatusText = -1;
                    do
                    {
                        if (++indexLbStatusText >= lbStatusText.Length) indexLbStatusText = 0;
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = lbStatusText[indexLbStatusText];
                        }));

                        JournalParametrRO journalParametrRO = new JournalParametrRO();
                        bool isData = false;
                        foreach (var item in listId)
                        {
                            NpgsqlCommand command = new NpgsqlCommand
                            {
                                Connection = connection,
                                CommandText = "SELECT value " +
                                "FROM masterscadadataraw " +
                                $"WHERE layer = 1 AND itemid = {item} AND \"Time\" " +
                                $"BETWEEN {time.Ticks} AND {time.AddSeconds(interval).Ticks} " +
                                $"ORDER BY \"Time\" DESC " +
                                $"LIMIT 1"
                            };
                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    isData = true;
                                    //double value = double.Parse(reader.GetValue(0).ToString());
                                    string value = reader.GetValue(0).ToString();
                                    if (value != "")
                                        value = string.Format("{0:f2}", double.Parse(value));
                                    switch (item)
                                    {
                                        case 1055:
                                            journalParametrRO.RashodIshodWater = value;
                                            break;
                                        case 1129:
                                            journalParametrRO.RashodPermeata = value;
                                            break;
                                        case 2914:
                                            journalParametrRO.NormalizeRashod = value;
                                            break;
                                        case 1043:
                                            journalParametrRO.RashodConcentrata = value;
                                            break;
                                        case 936:
                                            journalParametrRO.DavlenieNaMB = value;
                                            break;
                                        case 1077:
                                            journalParametrRO.DavleniePermeata = value;
                                            break;
                                        case 1089:
                                            journalParametrRO.DavleniePosleMB = value;
                                            break;
                                        case 943:
                                            journalParametrRO.ProvodimostIshodWater = value;
                                            break;
                                        case 1096:
                                            journalParametrRO.ProvodimostPermeata = value;
                                            break;
                                        case 2927:
                                            journalParametrRO.ProvodimostConcentrata = value;
                                            break;
                                        case 973:
                                            journalParametrRO.TemperaturaIshodWater = value;
                                            break;
                                        case 1516:
                                            journalParametrRO.Flux = value;
                                            break;
                                        case 1474:
                                            journalParametrRO.Recovery = value;
                                            break;
                                        case 2901:
                                            journalParametrRO.Rejection = value;
                                            break;
                                    }
                                }
                            }
                        }

                        if (isData)
                        {
                            journalParametrRO.Date = time.ToString("dd.MM.yyyy HH:mm:ss");
                            listParam.Add(journalParametrRO);
                        }
                    } while ((time = time.AddSeconds(interval)) < dateTimePickerEnd.Value);



                    if (myDataGrid2.InvokeRequired)
                    {
                        myDataGrid2.Invoke(new MethodInvoker(() =>
                        {
                            myDataGrid2.DataSource = listParam;
                        }));
                    }
                    else
                    {
                        myDataGrid2.DataSource = listParam;
                    }

                    if (lb_status.InvokeRequired)
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = "Запрос завершен";
                        }));
                    else
                        lb_status.Text = "Запрос завершен";

                    if (btn_Success.InvokeRequired)
                        btn_Success.Invoke(new MethodInvoker(() =>
                        {
                            btn_Success.Enabled = true;
                        }));
                    else
                        btn_Success.Enabled = true;

                    if (btn_Cancel.InvokeRequired)
                        btn_Cancel.Invoke(new MethodInvoker(() =>
                        {
                            btn_Cancel.Enabled = false;
                        }));
                    else
                        btn_Cancel.Enabled = false;
                }
                catch (Exception ex)
                {
                    LogClass.Write("RO_Journal:WorkRO3::" + ex.Message);
                }
            }
        }
        private void WorkRO4()
        {
            List<int> listId = new List<int>()
            {
                1057, 1130, 2915,
                1045, 936, 1079,
                1067, 943, 1098,
                2928, 973, 1517,
                1475, 2902
            };
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    DateTime time = dateTimePickerStart.Value;
                    int interval = int.Parse(tb_interval.Text);
                    List<JournalParametrRO> listParam = new List<JournalParametrRO>();
                    int indexLbStatusText = -1;
                    do
                    {
                        if (++indexLbStatusText >= lbStatusText.Length) indexLbStatusText = 0;
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = lbStatusText[indexLbStatusText];
                        }));

                        JournalParametrRO journalParametrRO = new JournalParametrRO();
                        bool isData = false;
                        foreach (var item in listId)
                        {
                            NpgsqlCommand command = new NpgsqlCommand
                            {
                                Connection = connection,
                                CommandText = "SELECT value " +
                                "FROM masterscadadataraw " +
                                $"WHERE layer = 1 AND itemid = {item} AND \"Time\" " +
                                $"BETWEEN {time.Ticks} AND {time.AddSeconds(interval).Ticks} " +
                                $"ORDER BY \"Time\" DESC " +
                                $"LIMIT 1"
                            };
                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    isData = true;
                                    //double value = double.Parse(reader.GetValue(0).ToString());
                                    string value = reader.GetValue(0).ToString();
                                    if (value != "")
                                        value = string.Format("{0:f2}", double.Parse(value));
                                    switch (item)
                                    {
                                        case 1057:
                                            journalParametrRO.RashodIshodWater = value;
                                            break;
                                        case 1130:
                                            journalParametrRO.RashodPermeata = value;
                                            break;
                                        case 2915:
                                            journalParametrRO.NormalizeRashod = value;
                                            break;
                                        case 1045:
                                            journalParametrRO.RashodConcentrata = value;
                                            break;
                                        case 936:
                                            journalParametrRO.DavlenieNaMB = value;
                                            break;
                                        case 1079:
                                            journalParametrRO.DavleniePermeata = value;
                                            break;
                                        case 1067:
                                            journalParametrRO.DavleniePosleMB = value;
                                            break;
                                        case 943:
                                            journalParametrRO.ProvodimostIshodWater = value;
                                            break;
                                        case 1098:
                                            journalParametrRO.ProvodimostPermeata = value;
                                            break;
                                        case 2928:
                                            journalParametrRO.ProvodimostConcentrata = value;
                                            break;
                                        case 973:
                                            journalParametrRO.TemperaturaIshodWater = value;
                                            break;
                                        case 1517:
                                            journalParametrRO.Flux = value;
                                            break;
                                        case 1475:
                                            journalParametrRO.Recovery = value;
                                            break;
                                        case 2902:
                                            journalParametrRO.Rejection = value;
                                            break;
                                    }
                                }
                            }
                        }

                        if (isData)
                        {
                            journalParametrRO.Date = time.ToString("dd.MM.yyyy HH:mm:ss");
                            listParam.Add(journalParametrRO);
                        }
                    } while ((time = time.AddSeconds(interval)) < dateTimePickerEnd.Value);



                    if (myDataGrid2.InvokeRequired)
                    {
                        myDataGrid2.Invoke(new MethodInvoker(() =>
                        {
                            myDataGrid2.DataSource = listParam;
                        }));
                    }
                    else
                    {
                        myDataGrid2.DataSource = listParam;
                    }

                    if (lb_status.InvokeRequired)
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = "Запрос завершен";
                        }));
                    else
                        lb_status.Text = "Запрос завершен";

                    if (btn_Success.InvokeRequired)
                        btn_Success.Invoke(new MethodInvoker(() =>
                        {
                            btn_Success.Enabled = true;
                        }));
                    else
                        btn_Success.Enabled = true;

                    if (btn_Cancel.InvokeRequired)
                        btn_Cancel.Invoke(new MethodInvoker(() =>
                        {
                            btn_Cancel.Enabled = false;
                        }));
                    else
                        btn_Cancel.Enabled = false;
                }
                catch (Exception ex)
                {
                    LogClass.Write("RO_Journal:WorkRO4::" + ex.Message);
                }
            }
        }
        private void WorkRO5()
        {
            List<int> listId = new List<int>()
            {
                1059, 1131, 2916,
                1047, 936, 1081,
                1069, 943, 1100,
                2929, 973, 1518,
                1476, 2903
            };
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    DateTime time = dateTimePickerStart.Value;
                    int interval = int.Parse(tb_interval.Text);
                    List<JournalParametrRO> listParam = new List<JournalParametrRO>();
                    int indexLbStatusText = -1;
                    do
                    {
                        if (++indexLbStatusText >= lbStatusText.Length) indexLbStatusText = 0;
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = lbStatusText[indexLbStatusText];
                        }));

                        JournalParametrRO journalParametrRO = new JournalParametrRO();
                        bool isData = false;
                        foreach (var item in listId)
                        {
                            NpgsqlCommand command = new NpgsqlCommand
                            {
                                Connection = connection,
                                CommandText = "SELECT value " +
                                "FROM masterscadadataraw " +
                                $"WHERE layer = 1 AND itemid = {item} AND \"Time\" " +
                                $"BETWEEN {time.Ticks} AND {time.AddSeconds(interval).Ticks} " +
                                $"ORDER BY \"Time\" DESC " +
                                $"LIMIT 1"
                            };
                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    isData = true;
                                    //double value = double.Parse(reader.GetValue(0).ToString());
                                    string value = reader.GetValue(0).ToString();
                                    if (value != "")
                                        value = string.Format("{0:f2}", double.Parse(value));
                                    switch (item)
                                    {
                                        case 1059:
                                            journalParametrRO.RashodIshodWater = value;
                                            break;
                                        case 1131:
                                            journalParametrRO.RashodPermeata = value;
                                            break;
                                        case 2916:
                                            journalParametrRO.NormalizeRashod = value;
                                            break;
                                        case 1047:
                                            journalParametrRO.RashodConcentrata = value;
                                            break;
                                        case 936:
                                            journalParametrRO.DavlenieNaMB = value;
                                            break;
                                        case 1081:
                                            journalParametrRO.DavleniePermeata = value;
                                            break;
                                        case 1069:
                                            journalParametrRO.DavleniePosleMB = value;
                                            break;
                                        case 943:
                                            journalParametrRO.ProvodimostIshodWater = value;
                                            break;
                                        case 1100:
                                            journalParametrRO.ProvodimostPermeata = value;
                                            break;
                                        case 2929:
                                            journalParametrRO.ProvodimostConcentrata = value;
                                            break;
                                        case 973:
                                            journalParametrRO.TemperaturaIshodWater = value;
                                            break;
                                        case 1518:
                                            journalParametrRO.Flux = value;
                                            break;
                                        case 1476:
                                            journalParametrRO.Recovery = value;
                                            break;
                                        case 2903:
                                            journalParametrRO.Rejection = value;
                                            break;
                                    }
                                }
                            }
                        }

                        if (isData)
                        {
                            journalParametrRO.Date = time.ToString("dd.MM.yyyy HH:mm:ss");
                            listParam.Add(journalParametrRO);
                        }
                    } while ((time = time.AddSeconds(interval)) < dateTimePickerEnd.Value);



                    if (myDataGrid2.InvokeRequired)
                    {
                        myDataGrid2.Invoke(new MethodInvoker(() =>
                        {
                            myDataGrid2.DataSource = listParam;
                        }));
                    }
                    else
                    {
                        myDataGrid2.DataSource = listParam;
                    }

                    if (lb_status.InvokeRequired)
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = "Запрос завершен";
                        }));
                    else
                        lb_status.Text = "Запрос завершен";

                    if (btn_Success.InvokeRequired)
                        btn_Success.Invoke(new MethodInvoker(() =>
                        {
                            btn_Success.Enabled = true;
                        }));
                    else
                        btn_Success.Enabled = true;

                    if (btn_Cancel.InvokeRequired)
                        btn_Cancel.Invoke(new MethodInvoker(() =>
                        {
                            btn_Cancel.Enabled = false;
                        }));
                    else
                        btn_Cancel.Enabled = false;
                }
                catch (Exception ex)
                {
                    LogClass.Write("RO_Journal:WorkRO5::" + ex.Message);
                }
            }
        }
        private void WorkRO6()
        {
            List<int> listId = new List<int>()
            {
                1061, 1132, 2917,
                1049, 936, 1083,
                1071, 943, 1102,
                2930, 973, 1519,
                1477, 2904
            };
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    DateTime time = dateTimePickerStart.Value;
                    int interval = int.Parse(tb_interval.Text);
                    List<JournalParametrRO> listParam = new List<JournalParametrRO>();
                    int indexLbStatusText = -1;
                    do
                    {
                        if (++indexLbStatusText >= lbStatusText.Length) indexLbStatusText = 0;
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = lbStatusText[indexLbStatusText];
                        }));

                        JournalParametrRO journalParametrRO = new JournalParametrRO();
                        bool isData = false;
                        foreach (var item in listId)
                        {
                            NpgsqlCommand command = new NpgsqlCommand
                            {
                                Connection = connection,
                                CommandText = "SELECT value " +
                                "FROM masterscadadataraw " +
                                $"WHERE layer = 1 AND itemid = {item} AND \"Time\" " +
                                $"BETWEEN {time.Ticks} AND {time.AddSeconds(interval).Ticks} " +
                                $"ORDER BY \"Time\" DESC " +
                                $"LIMIT 1"
                            };
                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    isData = true;
                                    //double value = double.Parse(reader.GetValue(0).ToString());
                                    string value = reader.GetValue(0).ToString();
                                    if (value != "")
                                        value = string.Format("{0:f2}", double.Parse(value));
                                    switch (item)
                                    {
                                        case 1061:
                                            journalParametrRO.RashodIshodWater = value;
                                            break;
                                        case 1132:
                                            journalParametrRO.RashodPermeata = value;
                                            break;
                                        case 2917:
                                            journalParametrRO.NormalizeRashod = value;
                                            break;
                                        case 1049:
                                            journalParametrRO.RashodConcentrata = value;
                                            break;
                                        case 936:
                                            journalParametrRO.DavlenieNaMB = value;
                                            break;
                                        case 1083:
                                            journalParametrRO.DavleniePermeata = value;
                                            break;
                                        case 1071:
                                            journalParametrRO.DavleniePosleMB = value;
                                            break;
                                        case 943:
                                            journalParametrRO.ProvodimostIshodWater = value;
                                            break;
                                        case 1102:
                                            journalParametrRO.ProvodimostPermeata = value;
                                            break;
                                        case 2930:
                                            journalParametrRO.ProvodimostConcentrata = value;
                                            break;
                                        case 973:
                                            journalParametrRO.TemperaturaIshodWater = value;
                                            break;
                                        case 1519:
                                            journalParametrRO.Flux = value;
                                            break;
                                        case 1477:
                                            journalParametrRO.Recovery = value;
                                            break;
                                        case 2904:
                                            journalParametrRO.Rejection = value;
                                            break;
                                    }
                                }
                            }
                        }

                        if (isData)
                        {
                            journalParametrRO.Date = time.ToString("dd.MM.yyyy HH:mm:ss");
                            listParam.Add(journalParametrRO);
                        }
                    } while ((time = time.AddSeconds(interval)) < dateTimePickerEnd.Value);



                    if (myDataGrid2.InvokeRequired)
                    {
                        myDataGrid2.Invoke(new MethodInvoker(() =>
                        {
                            myDataGrid2.DataSource = listParam;
                        }));
                    }
                    else
                    {
                        myDataGrid2.DataSource = listParam;
                    }

                    if (lb_status.InvokeRequired)
                        lb_status.Invoke(new MethodInvoker(() =>
                        {
                            lb_status.Text = "Запрос завершен";
                        }));
                    else
                        lb_status.Text = "Запрос завершен";

                    if (btn_Success.InvokeRequired)
                        btn_Success.Invoke(new MethodInvoker(() =>
                        {
                            btn_Success.Enabled = true;
                        }));
                    else
                        btn_Success.Enabled = true;

                    if (btn_Cancel.InvokeRequired)
                        btn_Cancel.Invoke(new MethodInvoker(() =>
                        {
                            btn_Cancel.Enabled = false;
                        }));
                    else
                        btn_Cancel.Enabled = false;
                }
                catch (Exception ex)
                {
                    LogClass.Write("RO_Journal:WorkRO6::" + ex.Message);
                }
            }
        }

        //Изменить цвет кнопок или кнопки
        private void ChangeBackGroundButton(Color color, params Button [] buttons)
        {
            foreach (var item in buttons)
            {
                item.BackColor = color;
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_thread != null && 
                    (_thread.ThreadState == ThreadState.Running || 
                    _thread.ThreadState == ThreadState.WaitSleepJoin))
                    _thread.Abort();

                btn_Cancel.Enabled = false;
                btn_Success.Enabled = true;

                lb_status.Text = "Отмена";
            }
            catch { }
        }
    }
}
