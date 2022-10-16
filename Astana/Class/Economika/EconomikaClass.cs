using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Npgsql;
using System.Configuration;
using Astana.Class.Log;

namespace Astana.Class.Economika
{
    class EconomikaClass
    {
        private Thread _thread;
        private Label _labelPriceIshodWater;
        private List<int> idItem = new List<int>()
        {
            930, // Расход Исходной воды
            1125, // Расход концентрата
            974, // Текущ_дозир_мета
            975, // Текущ_дозир_антискал
            2818, // Общая мощность НСЕ-4
            1126 // Расход пермеата
        };

        public EconomikaClass(Label label)
        {
            _labelPriceIshodWater = label;
        }

        public void Start()
        {
            _thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(5000);

                    try
                    {
                        using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
                        {
                            connection.Open();
                            NpgsqlCommand command = new NpgsqlCommand { Connection = connection };
                            List<double> values = new List<double>();
                            for (int i = 0; i < idItem.Count; i++)
                            {
                                command.CommandText = "SELECT value " +
                               "FROM masterscadadataraw " +
                               $"WHERE itemid = {idItem[i]} " +
                               $"ORDER BY \"Time\" DESC " +
                               $"LIMIT 1";
                                using (NpgsqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        reader.Read();
                                        object temp = reader.GetValue(0);
                                        double value = double.Parse(temp.ToString().Replace('.', ','));
                                        values.Add(value);
                                    }
                                    else
                                    {
                                        values.Add(0);
                                    }
                                }
                            }

                            double rezult = (double)(((values[0] * AllSettings.stoimostIshodWater) + (values[1] * AllSettings.stoimostUtilConcentrata) + (values[3] / 1000 * AllSettings.stoimostAntiskalanta) + (values[2] * AllSettings.stoimostMetoBisulvitaNatriya / 1000) + (values[4] * AllSettings.stoimostElectroEnergy)) / values[5]);

                            if (_labelPriceIshodWater.InvokeRequired)
                            {
                                _labelPriceIshodWater.Invoke(new MethodInvoker(() =>
                               {
                                   _labelPriceIshodWater.Text = string.Format("{0:N1}", rezult) + " тг/м\u00b3";
                               }));
                            }
                            else
                            {
                                _labelPriceIshodWater.Text = string.Format("{0:N1}", rezult) + " тг/м\u00b3";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogClass.Write("EconomikaClass:Start::" + ex.Message);
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
            catch { }
        }
    }
}
