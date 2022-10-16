using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using System.Data;
using Astana.Class.Log;

namespace Astana.Class.Membrans
{
    class SearchProblemMembran
    {
        private Label _lbCountError;
        private Thread _thread;
        private NpgsqlConnection _connectionMembran;
        public static List<Membran> _membranItems = new List<Membran>()
        {
            new Membran()
            {
                Name = "RO1",
                ZadvichkaId = 1297,
                NormalizeRashodId = 2912,
                DavlenieNaUstanovkyId = 936
            },
            new Membran()
            {
                Name = "RO2",
                ZadvichkaId = 1834,
                NormalizeRashodId = 2913,
                DavlenieNaUstanovkyId = 936
            },
            new Membran()
            {
                Name = "RO3",
                ZadvichkaId = 1876,
                NormalizeRashodId = 2914,
                DavlenieNaUstanovkyId = 936
            },
            new Membran()
            {
                Name = "RO4",
                ZadvichkaId = 1918,
                NormalizeRashodId = 2915,
                DavlenieNaUstanovkyId = 936
            },
            new Membran()
            {
                Name = "RO5",
                ZadvichkaId = 1960,
                NormalizeRashodId = 2916,
                DavlenieNaUstanovkyId = 936
            },
            new Membran()
            {
                Name = "RO6",
                ZadvichkaId = 2002,
                NormalizeRashodId = 2917,
                DavlenieNaUstanovkyId = 936
            }
        };
        public static List<MembranErrorClass> listMembranError = new List<MembranErrorClass>();
        public SearchProblemMembran(Label label)
        {
            _lbCountError = label;
            foreach (Membran item in _membranItems)
            {
                item.ErrorEvent += ChangeCountError;
            }
        }

        public void Start()
        {
            GetActiveErrors();
            _connectionMembran = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString);
            _connectionMembran.Open();
            _thread = new Thread(() =>
            {
                while (true)
                {
                    NpgsqlCommand command = new NpgsqlCommand { Connection = _connectionMembran };

                    foreach (Membran membran in _membranItems)
                    {
                        try
                        {
                            if (membran.isFirstError)
                            {
                                int time = 24; //Время до сброса таймера для промывки
                                switch (membran.Name)
                                {
                                    case "RO1":
                                        time = AllSettings.timeResetRO1;
                                        break;
                                    case "RO2":
                                        time = AllSettings.timeResetRO2;
                                        break;
                                    case "RO3":
                                        time = AllSettings.timeResetRO3;
                                        break;
                                    case "RO4":
                                        time = AllSettings.timeResetRO4;
                                        break;
                                    case "RO5":
                                        time = AllSettings.timeResetRO5;
                                        break;
                                    case "RO6":
                                        time = AllSettings.timeResetRO6;
                                        break;
                                }

                                if (membran.StopwatchTimeReser.Elapsed.TotalSeconds >= time * 60 * 60)
                                {
                                    if (membran.IsProblem == false)
                                    {
                                        membran.isFirstError = false;
                                        membran.StopwatchTimeReser.Reset();
                                        membran.StopwatchTimeError.Reset();
                                    }
                                    else
                                    {
                                        membran.StopwatchTimeReser.Restart();
                                        membran.StopwatchTimeError.Restart();
                                    }
                                }    
                            }

                            command.CommandText = "SELECT value FROM masterscadadataraw " +
                                $"WHERE itemid = {membran.ZadvichkaId} ORDER BY \"Time\" DESC LIMIT 1";
                            using (NpgsqlDataReader readerZadvichka = command.ExecuteReader())
                            {
                                if (readerZadvichka.HasRows)
                                {
                                    readerZadvichka.Read();
                                    int value = int.Parse(readerZadvichka.GetValue(0).ToString());
                                    if (value == 1)
                                    {
                                        readerZadvichka.Close();
                                        command.CommandText = "SELECT value FROM masterscadadataraw " +
                                            $"WHERE itemid = {membran.DavlenieNaUstanovkyId} ORDER BY \"Time\" DESC LIMIT 1";

                                        using (NpgsqlDataReader readerDavlenie = command.ExecuteReader())
                                        {
                                            if (readerDavlenie.HasRows)
                                            {
                                                readerDavlenie.Read();
                                                double davlenie = double.Parse(readerDavlenie.GetValue(0).ToString().Replace('.', ','));

                                                if (davlenie > GetDavlenieForNameRO(membran.Name))
                                                {
                                                    if (membran.NameProblem == NameProblemMembran.TrebuetsyaPromivka) 
                                                        continue;

                                                    if (membran.IsCheckProblem == false)
                                                        membran.StartCheckProblem();
                                                }
                                                else
                                                {
                                                    if ((membran.IsProblem || membran.IsCheckProblem) && 
                                                    membran.NameProblem != NameProblemMembran.TrebuetsyaPromivka)
                                                    {
                                                        membran.StopCheckProblem();
                                                        membran.ChangeStatusProblemDataBase();
                                                        membran.RemoveErrorInList();
                                                        membran.StopwatchTimeError.Stop();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((membran.IsProblem || membran.IsCheckProblem) &&
                                        membran.NameProblem != NameProblemMembran.TrebuetsyaPromivka)
                                        {
                                            membran.StopCheckProblem();
                                            membran.ChangeStatusProblemDataBase();
                                            membran.RemoveErrorInList();
                                            membran.StopwatchTimeError.Stop();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogClass.Write("SearchProblemMembran:Start::" + ex.Message);
                        }
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

            foreach (Membran item in _membranItems)
            {
                try
                {
                    item.StopCheckProblem();
                }
                catch { }
            }

            try
            {
                if (_connectionMembran != null && _connectionMembran.State == ConnectionState.Open)
                    _connectionMembran.Close();
            }
            catch { }
        }
        //Получить активные ошибки по мембранам
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
                                    Membran membranItem = _membranItems.Where(x => x.NormalizeRashodId == itemid).FirstOrDefault();
                                    if (membranItem != null)
                                    {
                                        membranItem.isFirstError = true;
                                        membranItem.IsProblem = true;
                                        if (NameProblemMembran.NizkoyaProizvoditelnost.ToString() == nameProblem)
                                        {
                                            membranItem.NameProblem = NameProblemMembran.NizkoyaProizvoditelnost;
                                            listMembranError.Add(new MembranErrorClass()
                                            {
                                                Name = membranItem.Name,
                                                NameProblem = NameProblemMembran.NizkoyaProizvoditelnost
                                            });
                                            membranItem.StopwatchTimeError.Start();
                                            membranItem.StopwatchTimeReser.Start();
                                        }
                                        else if (NameProblemMembran.TrebuetsyaPromivka.ToString() == nameProblem)
                                        {
                                            membranItem.NameProblem = NameProblemMembran.TrebuetsyaPromivka;
                                            listMembranError.Add(new MembranErrorClass()
                                            {
                                                Name = membranItem.Name,
                                                NameProblem = NameProblemMembran.TrebuetsyaPromivka
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
                    LogClass.Write("SearchProblemMembran:GetActiveErrors::" + ex.Message);
                }
            }
        }
        //Получить давление по имени линии
        private double GetDavlenieForNameRO(string nameRO)
        {
            if (nameRO == "RO1")
                return AllSettings.enterPressureRo1;
            else if (nameRO == "RO2")
                return AllSettings.enterPressureRo2;
            else if (nameRO == "RO3")
                return AllSettings.enterPressureRo3;
            else if (nameRO == "RO4")
                return AllSettings.enterPressureRo4;
            else if (nameRO == "RO5")
                return AllSettings.enterPressureRo5;
            else
                return AllSettings.enterPressureRo6;

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
