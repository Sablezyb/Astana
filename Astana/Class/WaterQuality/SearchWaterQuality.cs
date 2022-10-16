using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System.Configuration;
using System.Threading;
using System.Data;
using System.Windows.Forms;
using Astana.Class.Log;

namespace Astana.Class.WaterQuality
{

    class SearchWaterQuality
    {
        public static List<WaterQualityErrorClass> listWaterQualityError = new List<WaterQualityErrorClass>();

        private Label _lbCountError;
        private List<string> listItemidZadvichka = new List<string>()
        {
            "1297", //1
            "1834", //2
            "1876", //3
            "1918", //4
            "1960", //5
            "2002" //6
        };
        private List<WaterQualityItem> _waterQualityItems = new List<WaterQualityItem>()
        {
            new WaterQualityItem () {ItemId = 973, Name = "Temperatura"}, // Температура исходной воды
            new WaterQualityItem () {ItemId = 943, Name = "ProvodimostIshodWater"}, // Проводимость исходной воды
            new WaterQualityItem () {ItemId = 937, Name = "IshodWaterAfterCorrect"}, // Исходная вода после коррекции pH
            new WaterQualityItem () {ItemId = 1104, Name = "ProvodimostPermeata"}, // Проводимость пермеата
            new WaterQualityItem () {ItemId = 1090, Name = "PhPermeata"} // pH пермеата
        };
        private NpgsqlConnection _connectionWaterQuality;
        private int itemidDavlenieNaUstanovky = 936;
        private Thread _thread;

        public SearchWaterQuality(Label label)
        {
            _lbCountError = label;

            foreach (WaterQualityItem item in _waterQualityItems)
            {
                item.ErrorEvent += ChangeCountError;
            }
        }
        public void Start()
        {
            GetActiveErrors();
            _connectionWaterQuality = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString);
            _connectionWaterQuality.Open();

            _thread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        bool isFindOpenZadvichka = false;
                        NpgsqlCommand command = new NpgsqlCommand { Connection = _connectionWaterQuality };
                        for (int i = 0; i < listItemidZadvichka.Count; i++)
                        {
                            command.CommandText = "SELECT value " +
                            "FROM masterscadadataraw " +
                            $"WHERE itemid = {listItemidZadvichka[i]} " +
                            "ORDER BY \"Time\" DESC " +
                            "LIMIT 1";

                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    int value = int.Parse(reader.GetValue(0).ToString());
                                    if (value == 1)
                                    {
                                        isFindOpenZadvichka = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (isFindOpenZadvichka == true)
                        {
                            command.CommandText = "SELECT value " +
                            "FROM masterscadadataraw " +
                            $"WHERE itemid = {itemidDavlenieNaUstanovky} " +
                            $"ORDER BY \"Time\" DESC " +
                            $"LIMIT 1";

                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    double value = double.Parse(reader.GetValue(0).ToString().Replace('.', ','));

                                    if (value > AllSettings.davlenieNaUstanovky)
                                    {
                                        foreach (WaterQualityItem item in _waterQualityItems)
                                        {
                                            if (item.IsCheckProblem == false)
                                                item.StartCheckProblem();
                                        }
                                    }
                                    else
                                    {
                                        foreach (WaterQualityItem item in _waterQualityItems)
                                        {
                                            if (item.IsProblem || item.IsCheckProblem)
                                            {
                                                item.StopCheckProblem();
                                                item.ChangeStatusProblemDataBase();
                                                item.RemoveErrorInList();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (WaterQualityItem item in _waterQualityItems)
                            {
                                if (item.IsProblem || item.IsCheckProblem)
                                {
                                    item.StopCheckProblem();
                                    item.ChangeStatusProblemDataBase();
                                    item.RemoveErrorInList();
                                }
                            }
                        }
                    }
                    catch (Exception ex) 
                    {
                        LogClass.Write("SearchWaterQuality:Start::" + ex.Message);
                    }
                    Thread.Sleep(1000);
                }
            });

            _thread.Start();
        }

        public void Stop()
        {
            try
            {
                if (_thread != null && (_thread.ThreadState == ThreadState.Running || _thread.ThreadState == ThreadState.WaitSleepJoin))
                    _thread.Abort();
            }
            catch { }

            foreach (var item in _waterQualityItems)
            {
                if (item.IsCheckProblem)
                    item.StopCheckProblem();
            }

            try
            {
                if (_connectionWaterQuality != null && _connectionWaterQuality.State == ConnectionState.Open)
                    _connectionWaterQuality.Close();
            }
            catch { }
        }
        private void GetActiveErrors()
        {
            using (NpgsqlConnection _connectionCleverOsmos = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
            {
                try
                {
                    _connectionCleverOsmos.Open();
                    if (_connectionCleverOsmos.State == ConnectionState.Open)
                    {
                        NpgsqlCommand command = new NpgsqlCommand
                        {
                            Connection = _connectionCleverOsmos,
                            CommandText = "SELECT itemid, name_problem FROM journal_messages " +
                            "WHERE is_active = true"
                        };
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int itemid = int.Parse(reader.GetValue(0).ToString());
                                    string nameProblem = reader.GetValue(1).ToString();
                                    WaterQualityItem waterQualityItem = _waterQualityItems.Where(x => x.ItemId == itemid).FirstOrDefault();
                                    if (waterQualityItem != null)
                                    {
                                        waterQualityItem.IsProblem = true;
                                        if (NameProblemWaterQuality.HIGH.ToString() == nameProblem)
                                        {
                                            waterQualityItem.NameProblem = NameProblemWaterQuality.HIGH;
                                            listWaterQualityError.Add(new WaterQualityErrorClass()
                                            {
                                                Name = waterQualityItem.Name,
                                                NameProblem = waterQualityItem.NameProblem
                                            });
                                        }
                                        else if (NameProblemWaterQuality.LOW.ToString() == nameProblem)
                                        {
                                            waterQualityItem.NameProblem = NameProblemWaterQuality.LOW;
                                            listWaterQualityError.Add(new WaterQualityErrorClass()
                                            {
                                                Name = waterQualityItem.Name,
                                                NameProblem = waterQualityItem.NameProblem
                                            });
                                        }

                                        ChangeCountError('+');
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClass.Write("SearchWaterQuality:GetActiveErrors::" + ex.Message);
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
    }
}
