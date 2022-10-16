using System;
using System.Linq;
using System.Threading;
using Npgsql;
using System.Configuration;
using System.Data;
using Astana.Class.Log;
using System.Diagnostics;

namespace Astana.Class.Membrans
{
    public class Membran
    {
        private Thread _thread;
       
        public event Action<char> ErrorEvent;
        public bool isFirstError { get; set; } = false;
        public string Name { get; set; }
        public bool IsCheckProblem { get; set; } = false;
        public bool IsProblem { get; set; } = false;
        public int ZadvichkaId { get; set; }
        public int NormalizeRashodId { get; set; }
        public int DavlenieNaUstanovkyId { get; set; }
        public int sumTimeError { get; set; }
        public int sumTimeReset { get; set; }
        public NameProblemMembran NameProblem { get; set; }
        public Stopwatch StopwatchTimeError { get; set; } = new Stopwatch();
        public Stopwatch StopwatchTimeReser { get; set; } = new Stopwatch();


        public void StartCheckProblem()
        {
            IsCheckProblem = true;
            _thread = new Thread(()=>
            {
                Thread.Sleep(GetTimeDalayForName(Name) * 1000 * 60);

                double normalize = GetNormalizeForDataBase();
                if (normalize < (GetProizvoditelnostForNameRO(Name) - ((GetProcentForNameRO(Name) * GetProizvoditelnostForNameRO(Name)) / 100)))
                {
                    int time = 8;
                    switch (Name)
                    {
                        case "RO1":
                            time = AllSettings.timeWhenNeedPromivkaRO1;
                            break;
                        case "RO2":
                            time = AllSettings.timeWhenNeedPromivkaRO2;
                            break;
                        case "RO3":
                            time = AllSettings.timeWhenNeedPromivkaRO3;
                            break;
                        case "RO4":
                            time = AllSettings.timeWhenNeedPromivkaRO4;
                            break;
                        case "RO5":
                            time = AllSettings.timeWhenNeedPromivkaRO5;
                            break;
                        case "RO6":
                            time = AllSettings.timeWhenNeedPromivkaRO6;
                            break;
                        default:
                            break;
                    }

                    if (StopwatchTimeError.Elapsed.TotalSeconds >= (time * 60 * 60) && NameProblem == NameProblemMembran.NizkoyaProizvoditelnost)
                    {
                        UpdateDataBase(NameProblemMembran.TrebuetsyaPromivka);
                        MembranErrorClass item = SearchProblemMembran.listMembranError.Where(x => x.Name == Name).FirstOrDefault();
                        if (item != null)
                            item.NameProblem = NameProblemMembran.TrebuetsyaPromivka;
                        StopwatchTimeError.Reset();
                        StopwatchTimeReser.Reset();
                    }

                    if (IsProblem == false)
                    {
                        if (isFirstError == false)
                        {
                            isFirstError = true;
                            StopwatchTimeReser.Start();
                        }
                        StopwatchTimeError.Start();    
                        NameProblem = NameProblemMembran.NizkoyaProizvoditelnost;

                        AddProblemDataBase();
                        SearchProblemMembran.listMembranError.Add(new MembranErrorClass()
                        {
                            Name = Name,
                            NameProblem = NameProblem
                        });
                        ErrorEvent?.Invoke('+');
                        IsProblem = true;
                    }
                }
                else
                {
                    if (IsProblem == true)
                    {
                        ChangeStatusProblemDataBase();
                        RemoveErrorInList();
                        StopwatchTimeError.Stop();
                        IsProblem = false;
                    }
                }

                IsCheckProblem = false;
            });
            _thread.Start();
        }

        public void StopCheckProblem()
        {
            try
            {
                if (_thread != null && (_thread.ThreadState == System.Threading.ThreadState.Running || _thread.ThreadState == System.Threading.ThreadState.WaitSleepJoin))
                    _thread.Abort();
            }
            catch { }
        }
        //Удалить запись с листа ошибок
        public void RemoveErrorInList()
        {
            MembranErrorClass itemError = SearchProblemMembran.listMembranError.Where(x => x.Name == Name).FirstOrDefault();
            if (itemError != null)
            {
                SearchProblemMembran.listMembranError.Remove(itemError);
                ErrorEvent?.Invoke('-');
                IsProblem = false;
                IsCheckProblem = false;
                NameProblem = NameProblemMembran.NULL;
            }
        }
        //Изменить статус у ошибки в базе
        public void ChangeStatusProblemDataBase()
        {
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
                            $"SET date_end = '{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}', " +
                            $"is_active = false " +
                            $"WHERE is_active = true AND itemid = {NormalizeRashodId}"
                        };
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) 
                {
                    LogClass.Write("Membran:ChangeStatusProblemDataBase::" + ex.Message);
                }
            }
        }


        private void UpdateDataBase(NameProblemMembran newNameProblem)
        {
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
                            $"SET name_problem = '{newNameProblem}', " +
                            $"message = 'Низкая производительность линии {Name}. Требуется промывка' " +
                            $"WHERE itemid = {NormalizeRashodId} AND is_active = true"
                        };
                        command.ExecuteNonQuery();
                        NameProblem = newNameProblem;
                        MainForm.journalMessage.UpdateError();
                    }
                }
                catch (Exception ex) 
                {
                    LogClass.Write("Membran:UpdateDataBase::" + ex.Message);
                }
            }
        }
        // Добавляем новое сообщение с проблемой в базу
        private void AddProblemDataBase()
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
                        $"'Низкая производительность линии {Name}', " +
                        $"true, " +
                        $"'{NameProblem}', " +
                        $"{NormalizeRashodId})";
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogClass.Write("Membran:AddProblemDataBase::" + ex.Message);
                }
            }
        }
        //Получаем нормализованое значение с базы Scada
        private double GetNormalizeForDataBase()
        {
            using (NpgsqlConnection conncetionArchive = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
            {
                try
                {
                    conncetionArchive.Open();
                    NpgsqlCommand commandArchive = new NpgsqlCommand
                    {
                        Connection = conncetionArchive
                    };
                    commandArchive.CommandText = "SELECT value FROM masterscadadataraw " +
                    $"WHERE itemid = {NormalizeRashodId} ORDER BY \"Time\" DESC LIMIT 1";

                    using (NpgsqlDataReader reader = commandArchive.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            return double.Parse(reader.GetValue(0).ToString().Replace('.', ','));
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClass.Write("Membran:GetNormalizeForDataBase::" + ex.Message);
                }
            }

            return -1;
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
        private int GetTimeDalayForName(string nameRO)
        {
            if (nameRO == "RO1")
                return AllSettings.timeDelayRo1;
            else if (nameRO == "RO2")
                return AllSettings.timeDelayRo2;
            else if (nameRO == "RO3")
                return AllSettings.timeDelayRo3;
            else if (nameRO == "RO4")
                return AllSettings.timeDelayRo4;
            else if (nameRO == "RO5")
                return AllSettings.timeDelayRo5;
            else
                return AllSettings.timeDelayRo6;
        }
    }
}
