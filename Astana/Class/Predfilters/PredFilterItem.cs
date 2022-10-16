using System;
using System.Data;
using System.Linq;
using System.Threading;
using Npgsql;
using System.Configuration;
using Astana.Class.Log;

namespace Astana.Class.Predfilters
{
    class PredFilterItem
    {
        public delegate void ChangeErrorCountDelegate(char znak);
        public event ChangeErrorCountDelegate ErrorEvent;

        private int _timeStartPump = 10;
        private Thread _thread;
        public NameProblemPredfilter nameProblem;


        public string Name { get; private set; }
        public int H_On_Activ_Id { get; private set; }
        public int Dp_H_Id { get; private set; }


        public bool Is_Problem { get; set; } = false;
        public bool Is_Start_Pump { get; set; } = false;
        public bool Is_Check_Problem { get; set; } = false;

        public PredFilterItem(string name, int h_on_activ_id, int dp_h_id)
        {
            Name = name;
            H_On_Activ_Id = h_on_activ_id;
            Dp_H_Id = dp_h_id;
        }

        //Запуск проверки ошибки
        public void StartCheckProblem()
        {
            Is_Check_Problem = true;
            _thread = new Thread(() =>
            {
                Thread.Sleep(AllSettings.timeOut * 1000);

                bool isActivePump = CheckActivePump();

                if (isActivePump)
                {
                    bool isError = CheckError(out double valueParam);

                    if (isError && Is_Problem == false)
                    {
                        if (valueParam >= AllSettings.perepadSredZagrFiltra &&
                                    valueParam < AllSettings.perepadSilnoZagrFiltra)
                        {
                            nameProblem = NameProblemPredfilter.SredZagr;
                        }
                        else if (valueParam >= AllSettings.perepadSilnoZagrFiltra)
                        {
                            nameProblem = NameProblemPredfilter.SilnoZagr;
                        }

                        SearchProblePredfilter.listPredfilterError.Add
                        (
                            new PredfilterErrorClass() { Name = Name, NameProblem = nameProblem }
                        );
                        AddProblemDataBase();
                        ErrorEvent?.Invoke('+');
                        Is_Problem = true;

                        Is_Check_Problem = false;
                    }
                    else if (isError == false)
                    {
                        ChangeStatusProblemDataBase();
                        RemoveErrorInList();
                        Is_Problem = false;
                    }
                }
                Is_Check_Problem = false;
            });
            _thread.Start();
        }
        //Изменение название проблемы
        public void UpdateNameProblem(NameProblemPredfilter newNameProblem)
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
                            $"message = 'Фильтр {Name} {((newNameProblem == NameProblemPredfilter.SredZagr) ? " Среднее загрязнение" : " Сильное загрязнение")}. Требуется промывка' " +
                            $"WHERE itemid = {Dp_H_Id} AND is_active = true"
                        };
                        command.ExecuteNonQuery();
                        nameProblem = newNameProblem;
                        MainForm.journalMessage.UpdateError();
                    }
                }
                catch (Exception ex) 
                {
                    LogClass.Write("PredFilterItem:UpdateNameProblem::" + ex.Message);
                }
            }
        }
        //Запуск насоса
        public void StartPump()
        {
            Is_Check_Problem = true;

            if (_thread != null && _thread.IsAlive)
                _thread.Abort();

            _thread = new Thread(() =>
            {
                Thread.Sleep(_timeStartPump * 1000);

                bool isError = CheckError(out double valueParam);

                if (isError == false)
                {
                    ChangeStatusProblemDataBase();
                    RemoveErrorInList();
                }
                Is_Start_Pump = false;
                Is_Check_Problem = false;
            });
            _thread.Start();
        }
        //Остановка потока
        public void StopCheckProble()
        {
            try
            {
                if (_thread != null && (_thread.ThreadState == ThreadState.Running || _thread.ThreadState == ThreadState.WaitSleepJoin))
                    _thread.Abort();
            }
            catch { }
        }



        //Проверка работает ли насос
        private bool CheckActivePump()
        {
            bool isActivePump = false;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        NpgsqlCommand command = new NpgsqlCommand
                        {
                            Connection = connection,
                            CommandText = "SELECT value FROM masterscadadataraw " +
                        $"WHERE itemid = {H_On_Activ_Id} ORDER BY \"Time\" DESC LIMIT 1"
                        };

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                int value = int.Parse(reader.GetValue(0).ToString());

                                if (value == 1)
                                    isActivePump = true;
                                else
                                    isActivePump = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                LogClass.Write("PredFilterItem:CheckActivePump::" + ex.Message);
            }
            return isActivePump;
        }
        //Добавить ошибку в базу
        private void AddProblemDataBase()
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
                            CommandText = "INSERT INTO journal_messages (date_start, message, is_active, name_problem, itemid) " +
                            $"VALUES ('{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}', " +
                            $"'Фильтр {Name} {((nameProblem == NameProblemPredfilter.SredZagr) ? " Среднее загрязнение" : " Сильное загрязнение")}. Требуется промывка', " +
                            $"true, '{nameProblem}', {Dp_H_Id})"
                        };
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) 
                {
                    LogClass.Write("PredFilterItem:AddProblemDataBase::" + ex.Message);
                }
            }
        }
        //Изменить статус ошибки в базе
        private void ChangeStatusProblemDataBase()
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
                            $"WHERE is_active = true AND itemid = {Dp_H_Id}"
                        };
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) 
                {
                    LogClass.Write("PredFilterItem:ChangeStatusProblemDataBase::" + ex.Message);
                }
            }
        }
        //Проверка ошибки
        private bool CheckError(out double value)
        {
            value = -1;
            bool isError = false;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        NpgsqlCommand command = new NpgsqlCommand
                        {
                            Connection = connection,
                            CommandText = "SELECT value FROM masterscadadataraw " +
                            $"WHERE itemid = {Dp_H_Id} ORDER BY \"Time\" DESC LIMIT 1"
                        };
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                value = double.Parse(reader.GetValue(0).ToString());

                                if (value >= AllSettings.perepadSredZagrFiltra &&
                                    value < AllSettings.perepadSilnoZagrFiltra)
                                {
                                    isError = true;
                                }
                                else if (value >= AllSettings.perepadSilnoZagrFiltra)
                                {
                                    isError = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogClass.Write("PredFilterItem:CheckError::" + ex.Message);
            }
            return isError;
        }
        //Удаление ошибки из списка ошибок
        private void RemoveErrorInList()
        {
            PredfilterErrorClass itemError = SearchProblePredfilter.listPredfilterError.Where(x => x.Name == Name).FirstOrDefault();
            SearchProblePredfilter.listPredfilterError.Remove(itemError);
            ErrorEvent?.Invoke('-');
            Is_Problem = false;
            Is_Check_Problem = false;
        }
    }
}
