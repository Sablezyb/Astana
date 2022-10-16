using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Npgsql;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using Astana.Class.Log;

namespace Astana.Class.Predfilters
{
    class SearchProblePredfilter
    {
        private Label _lbCountError;
        private Thread _thread;
        private NpgsqlConnection _connectionPredfilter;
        //Список items для проверок предфильтра
        private List<PredFilterItem> _predFilterItems = new List<PredFilterItem>()
        {
            new PredFilterItem("Ф1", 948, 3636),
            new PredFilterItem("Ф2", 950, 3637),
            new PredFilterItem("Ф3", 952, 3638),
            new PredFilterItem("Ф4", 954,3639)
        };
        //Список ошибок
        public static List<PredfilterErrorClass> listPredfilterError = new List<PredfilterErrorClass>();

        public SearchProblePredfilter(Label label)
        {
            _lbCountError = label;
            for (int i = 0; i < _predFilterItems.Count; i++)
            {
                _predFilterItems[i].ErrorEvent += ChangeCountError;
            }
        }

        public void Start()
        {
            _connectionPredfilter = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString);
            _connectionPredfilter.Open();

            GetActiveErrors();

            _thread = new Thread(() =>
            {
                do
                {
                    NpgsqlCommand commandPredFilter = new NpgsqlCommand { Connection = _connectionPredfilter };

                    foreach (PredFilterItem item in _predFilterItems)
                    {
                        commandPredFilter.CommandText = "SELECT value FROM masterscadadataraw " +
                        $"WHERE itemid = {item.H_On_Activ_Id} ORDER BY \"Time\" DESC LIMIT 1";
                        using (NpgsqlDataReader readerOnActivH = commandPredFilter.ExecuteReader())
                        {
                            try
                            {
                                if (readerOnActivH.HasRows)
                                {
                                    readerOnActivH.Read();
                                    int value_on_activ = int.Parse(readerOnActivH.GetValue(0).ToString());

                                    if (value_on_activ == 1)
                                    {
                                        readerOnActivH.Close();
                                        commandPredFilter.CommandText = "SELECT value FROM masterscadadataraw " +
                                        $"WHERE itemid = {item.Dp_H_Id} ORDER BY \"Time\" DESC LIMIT 1";
                                        using (NpgsqlDataReader readerDpH = commandPredFilter.ExecuteReader())
                                        {
                                            if (readerDpH.HasRows)
                                            {
                                                readerDpH.Read();
                                                double value_dp_h = double.Parse(readerDpH.GetValue(0).ToString());

                                                if (item.Is_Start_Pump && item.Is_Check_Problem == false)
                                                {
                                                    item.StartPump();
                                                }

                                                if (((value_dp_h >= AllSettings.perepadSredZagrFiltra &&
                                                    value_dp_h < AllSettings.perepadSilnoZagrFiltra) ||
                                                    (value_dp_h >= AllSettings.perepadSilnoZagrFiltra)) &&
                                                    item.Is_Problem == false &&
                                                    item.Is_Check_Problem == false &&
                                                    item.Is_Start_Pump == false)
                                                {
                                                    //Действия для среднего и сильного загрязнения
                                                    item.StartCheckProblem();
                                                }
                                                else if (value_dp_h >= AllSettings.perepadSredZagrFiltra &&
                                                    value_dp_h < AllSettings.perepadSilnoZagrFiltra &&
                                                    item.Is_Problem == true &&
                                                    item.nameProblem == NameProblemPredfilter.SilnoZagr &&
                                                    item.Is_Check_Problem == false)
                                                {
                                                    item.UpdateNameProblem(NameProblemPredfilter.SredZagr);
                                                }
                                                else if (value_dp_h >= AllSettings.perepadSilnoZagrFiltra &&
                                                    item.Is_Problem == true &&
                                                    item.nameProblem == NameProblemPredfilter.SredZagr &&
                                                    item.Is_Check_Problem == false)
                                                {
                                                    item.UpdateNameProblem(NameProblemPredfilter.SilnoZagr);
                                                }
                                                else if (value_dp_h < AllSettings.perepadSredZagrFiltra &&
                                                    item.Is_Problem && item.Is_Check_Problem == false)
                                                {
                                                    item.StartCheckProblem();
                                                }
                                            }
                                        }
                                    }
                                    else if (value_on_activ == 0)
                                    {
                                        if (item.Is_Problem == true && item.Is_Start_Pump == false)
                                        {
                                            item.Is_Start_Pump = true;
                                            item.StopCheckProble();
                                        }
                                    }
                                }
                            }
                            catch (Exception ex) 
                            {
                                LogClass.Write("Start:SearchProblePredfilter::" + ex.Message);
                            }
                        }
                    }
                    Thread.Sleep(1000);
                } while (true);
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

            foreach (PredFilterItem item in _predFilterItems)
            {
                item.StopCheckProble();
            }

            try
            {
                if (_connectionPredfilter != null && _connectionPredfilter.State == ConnectionState.Open)
                    _connectionPredfilter.Close();
            }
            catch { }
        }

        //Получить активные ошибки по ппредфильтру
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
                                    PredFilterItem filterItem = _predFilterItems.Where(x => x.Dp_H_Id == itemid).FirstOrDefault();
                                    if (filterItem != null)
                                    {
                                        filterItem.Is_Problem = true;
                                        if (NameProblemPredfilter.SilnoZagr.ToString() == nameProblem)
                                        {
                                            filterItem.nameProblem = NameProblemPredfilter.SilnoZagr;
                                            listPredfilterError.Add(new PredfilterErrorClass()
                                            {
                                                Name = filterItem.Name,
                                                NameProblem = NameProblemPredfilter.SilnoZagr
                                            });
                                        }
                                        else if (NameProblemPredfilter.SredZagr.ToString() == nameProblem)
                                        {
                                            filterItem.nameProblem = NameProblemPredfilter.SredZagr;
                                            listPredfilterError.Add(new PredfilterErrorClass()
                                            {
                                                Name = filterItem.Name,
                                                NameProblem = NameProblemPredfilter.SredZagr
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
                    LogClass.Write("SearchProblePredfilter:GetActiveErrors::" + ex.Message);
                }
            }
        }
        //Изменения количество ошибок
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
