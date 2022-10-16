using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Npgsql;
using System.Configuration;
using System.Data;
using Astana.Class.Log;

namespace Astana.Class.MainInfo
{
    class MainInfoClass
    {
        private Label lb_ProvodimostIshodWater;
        private Label lb_RashodIshodWater;
        private Label lb_PhIshodWater;
        private Label lb_Power;
        private Label lb_ProvodimostPermitata;
        private Label lb_RashodPermeata;
        private Label lb_PhPermeata;
        private Label lb_ProvodimostConcetrata;
        private Label lb_RashodConcetrata;
        private Label lb_DavlenieNaUstanoky;
        private Label lb_Recovery;
        private Label lb_Rejection;
        private Label lb_DavlenieIshodWaterNaRO;

        private Thread _thread;

        private List<int> _listId = new List<int>()
        {
            943, //Проводимость исходной воды
            930, //Расход исходной воды
            937, //Ph Исходной воды
            2818, //Мощность насоса
            1104, //Проводимость пермеата
            1126, //Расход пермеата
            1090, //Ph пермеата
            2931, //Проводимость Концентрата
            1125, // Расход концентрата
            936, // Давление на установку
            1478, // Рековери
            2905, // Rejection
            931 //Давление на установку RO 
        };

        private List<Label> _listLabels = new List<Label>();
        public MainInfoClass(Label ProvodimostIshodWater, Label RashodIshodWater, Label PhIshodWater, 
            Label Power, Label ProvodimostPermitata, Label RashodPermeata, Label PhPermeata, Label ProvodimostConcetrata,
            Label RashodConcetrata, Label davlenieNaUstanoky, Label recovery, Label rejection, Label davlenieIshodWaterNaRO)
        {
            lb_ProvodimostIshodWater = ProvodimostIshodWater;
            lb_RashodIshodWater = RashodIshodWater;
            lb_PhIshodWater = PhIshodWater;
            lb_Power = Power;
            lb_ProvodimostPermitata = ProvodimostPermitata;
            lb_RashodPermeata = RashodPermeata;
            lb_PhPermeata = PhPermeata;
            lb_ProvodimostConcetrata = ProvodimostConcetrata;
            lb_RashodConcetrata = RashodConcetrata;
            lb_DavlenieNaUstanoky = davlenieNaUstanoky;
            lb_Recovery = recovery;
            lb_Rejection = rejection;
            lb_DavlenieIshodWaterNaRO = davlenieIshodWaterNaRO;

            _listLabels.Add(lb_ProvodimostIshodWater);
            _listLabels.Add(lb_RashodIshodWater);
            _listLabels.Add(lb_PhIshodWater);
            _listLabels.Add(lb_Power);
            _listLabels.Add(lb_ProvodimostPermitata);
            _listLabels.Add(lb_RashodPermeata);
            _listLabels.Add(lb_PhPermeata);
            _listLabels.Add(lb_ProvodimostConcetrata);
            _listLabels.Add(lb_RashodConcetrata);
            _listLabels.Add(lb_DavlenieNaUstanoky);
            _listLabels.Add(lb_Recovery);
            _listLabels.Add(lb_Rejection);
            _listLabels.Add(lb_DavlenieIshodWaterNaRO);
        }
        public void Start()
        {
            if (_thread != null && _thread.IsAlive)
                Stop();

            _thread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
                        {
                            if (connection.State == ConnectionState.Closed) connection.Open();

                            for (int i = 0; i < _listId.Count; i++)
                            {
                                NpgsqlCommand command = new NpgsqlCommand
                                {
                                    Connection = connection,
                                    CommandText = "SELECT value " +
                                   "FROM masterscadadataraw " +
                                   $"WHERE itemid = {_listId[i]} " +
                                   $"ORDER BY \"Time\" DESC " +
                                   $"LIMIT 1"
                                };
                                using (NpgsqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        reader.Read();
                                        _listLabels[i].Invoke(new MethodInvoker(() =>
                                        {
                                            switch (i)
                                            {
                                                case 0:
                                                    if (int.TryParse(reader.GetValue(0).ToString(), out int value1))
                                                    {
                                                        _listLabels[i].Text = value1 + " мкСм/см";
                                                    }
                                                    break;
                                                case 1:
                                                    if (double.TryParse(reader.GetValue(0).ToString(), out double value2))
                                                    {
                                                        _listLabels[i].Text = String.Format("{0:f1}", value2) + " м\u00B3/ч";
                                                    }
                                                    break;
                                                case 2:
                                                    if (double.TryParse(reader.GetValue(0).ToString(), out double value3))
                                                    {
                                                        _listLabels[i].Text = "pH " + String.Format("{0:f2}", value3);
                                                    }
                                                    break;
                                                case 3:
                                                    if (double.TryParse(reader.GetValue(0).ToString(), out double value4))
                                                    {
                                                        _listLabels[i].Text = String.Format("{0:f2}", value4) + " кВт";
                                                    }
                                                    break;
                                                case 4:
                                                    if (int.TryParse(reader.GetValue(0).ToString(), out int value5))
                                                    {
                                                        _listLabels[i].Text = value5 + " мкСм/см";
                                                    }
                                                    break;
                                                case 5:
                                                    if (double.TryParse(reader.GetValue(0).ToString(), out double value6))
                                                    {
                                                        _listLabels[i].Text = String.Format("{0:f1}", value6) + " м\u00B3/ч";
                                                    }
                                                    break;
                                                case 6:
                                                    if (double.TryParse(reader.GetValue(0).ToString(), out double value7))
                                                    {
                                                        _listLabels[i].Text = "pH " + String.Format("{0:f2}", value7);
                                                    }
                                                    break;
                                                case 7:
                                                    if (int.TryParse(reader.GetValue(0).ToString(), out int value8))
                                                    {
                                                        _listLabels[i].Text = value8 + " мкСм/см";
                                                    }
                                                    break;
                                                case 8:
                                                    if (double.TryParse(reader.GetValue(0).ToString(), out double value9))
                                                    {
                                                        _listLabels[i].Text = String.Format("{0:f1}", value9) + " м\u00B3/ч";
                                                    }
                                                    break;
                                                case 9:
                                                    if (double.TryParse(reader.GetValue(0).ToString(), out double value10))
                                                    {
                                                        _listLabels[i].Text = String.Format("{0:f1}", value10) + " Bar";
                                                    }
                                                    break;
                                                case 10:
                                                    if (double.TryParse(reader.GetValue(0).ToString(), out double value11))
                                                    {
                                                        _listLabels[i].Text = "Recovery: " + String.Format("{0:f1}", value11) + " %";
                                                    }
                                                    break;
                                                case 11:
                                                    if (double.TryParse(reader.GetValue(0).ToString(), out double value12))
                                                    {
                                                        _listLabels[i].Text = "Rejection: " + String.Format("{0:f2}", value12) + " %";
                                                    }
                                                    break;
                                                case 12:
                                                    if (double.TryParse(reader.GetValue(0).ToString(), out double value13))
                                                    {
                                                        _listLabels[i].Text = String.Format("{0:f2}", value13) + " Bar";
                                                    }
                                                    break;
                                            }
                                        }));
                                    }
                                }
                            }
                        }
                        Thread.Sleep(5000);
                    }
                    catch (Exception ex)
                    {
                        LogClass.Write(ex.Message);
                    }
                }
            });

            _thread.Start();
        }
        public void Stop()
        {
            try
            {
                if (_thread != null && _thread.IsAlive)
                    _thread.Abort();
            }
            catch (Exception ex)
            {
                LogClass.Write(ex.Message);
            }
        }
    }
}
