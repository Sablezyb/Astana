using System;
using System.Linq;
using System.Threading;
using Npgsql;
using System.Configuration;
using System.Data;
using Astana.Class.Log;

namespace Astana.Class.WaterQuality
{
    class WaterQualityItem
    {
        private Thread _thread;

        public event Action<char> ErrorEvent;

        public NameProblemWaterQuality NameProblem { get; set; } = NameProblemWaterQuality.NULL;
        public string Name { get; set; }
        public int ItemId { get; set; }
        public bool IsCheckProblem { get; set; } = false;
        public bool IsProblem { get; set; } = false;

        public void StartCheckProblem()
        {
            IsCheckProblem = true;

            _thread = new Thread(() =>
            {
                Thread.Sleep(10000);

                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Archive"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        NpgsqlCommand command = new NpgsqlCommand 
                        {
                            Connection = connection,
                            CommandText = "SELECT value " +
                            "FROM masterscadadataraw " +
                            $"WHERE itemid = {ItemId} " +
                            $"ORDER BY \"Time\" DESC " +
                            $"LIMIT 1"
                        };

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                double value = double.Parse(reader.GetValue(0).ToString().Replace('.', ','));

                                switch (Name)
                                {
                                    case "Temperatura":
                                        if (value < AllSettings.temperaturaWaterLow)
                                        {
                                            if (IsProblem == true && NameProblem == NameProblemWaterQuality.HIGH)
                                            {
                                                UpdateDataBase(NameProblemWaterQuality.LOW, "Низкая температура исходной воды");
                                            }
                                            else if (IsProblem == false)
                                            {
                                                NameProblem = NameProblemWaterQuality.LOW;
                                                AddProblemDataBase("Низкая температура исходной воды");
                                                SearchWaterQuality.listWaterQualityError.Add(new WaterQualityErrorClass
                                                {
                                                    Name = Name,
                                                    NameProblem = NameProblem
                                                });
                                                ErrorEvent?.Invoke('+');
                                                IsProblem = true;
                                            }
                                        }
                                        else if (value > AllSettings.temperaturaWaterHigh)
                                        {
                                            if (IsProblem == true && NameProblem == NameProblemWaterQuality.LOW)
                                            {
                                                UpdateDataBase(NameProblemWaterQuality.HIGH, "Высокая температура исходной воды");
                                            }
                                            else if (IsProblem == false)
                                            {
                                                NameProblem = NameProblemWaterQuality.HIGH;
                                                AddProblemDataBase("Высокая температура исходной воды");
                                                SearchWaterQuality.listWaterQualityError.Add(new WaterQualityErrorClass
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
                                                IsProblem = false;
                                            }
                                        }
                                        break;
                                    case "ProvodimostIshodWater":
                                        if(value < AllSettings.provodimostIshodWaterLow)
                                        {
                                            if (IsProblem == true && NameProblem == NameProblemWaterQuality.HIGH)
                                            {
                                                UpdateDataBase(NameProblemWaterQuality.LOW, "Низкая проводимость исходной воды");
                                            }
                                            else if (IsProblem == false)
                                            {
                                                NameProblem = NameProblemWaterQuality.LOW;
                                                AddProblemDataBase("Низкая проводимость исходной воды");
                                                SearchWaterQuality.listWaterQualityError.Add(new WaterQualityErrorClass
                                                {
                                                    Name = Name,
                                                    NameProblem = NameProblem
                                                });
                                                ErrorEvent?.Invoke('+');
                                                IsProblem = true;
                                            }
                                        }
                                        else if (value > AllSettings.provodimostIshodWaterHigh)
                                        {
                                            if (IsProblem == true && NameProblem == NameProblemWaterQuality.LOW)
                                            {
                                                UpdateDataBase(NameProblemWaterQuality.HIGH, "Высокая проводимость исходной воды");
                                            }
                                            else if (IsProblem == false)
                                            {
                                                NameProblem = NameProblemWaterQuality.HIGH;
                                                AddProblemDataBase("Высокая проводимость исходной воды");
                                                SearchWaterQuality.listWaterQualityError.Add(new WaterQualityErrorClass
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
                                                IsProblem = false;
                                            }
                                        }
                                        break;
                                    case "IshodWaterAfterCorrect":
                                        if (value < AllSettings.pHIshodWaterAfterCorrectLow)
                                        {
                                            if (IsProblem == true && NameProblem == NameProblemWaterQuality.HIGH)
                                            {
                                                UpdateDataBase(NameProblemWaterQuality.LOW, "Низкий pH исходной воды");
                                            }
                                            else if (IsProblem == false)
                                            {
                                                NameProblem = NameProblemWaterQuality.LOW;
                                                AddProblemDataBase("Низкий pH исходной воды");
                                                SearchWaterQuality.listWaterQualityError.Add(new WaterQualityErrorClass
                                                {
                                                    Name = Name,
                                                    NameProblem = NameProblem
                                                });
                                                ErrorEvent?.Invoke('+');
                                                IsProblem = true;
                                            }
                                        }
                                        else if (value > AllSettings.pHIshodWaterAfterCorrectHigh)
                                        {
                                            if (IsProblem == true && NameProblem == NameProblemWaterQuality.LOW)
                                            {
                                                UpdateDataBase(NameProblemWaterQuality.HIGH, "Высокий pH исходной воды");
                                            }
                                            else if (IsProblem == false)
                                            {
                                                NameProblem = NameProblemWaterQuality.HIGH;
                                                AddProblemDataBase("Высокий pH исходной воды");
                                                SearchWaterQuality.listWaterQualityError.Add(new WaterQualityErrorClass
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
                                                IsProblem = false;
                                            }
                                        }
                                        break;
                                    case "ProvodimostPermeata":
                                        if (value < AllSettings.provodimostPermeataLow)
                                        {
                                            if (IsProblem == true && NameProblem == NameProblemWaterQuality.HIGH)
                                            {
                                                UpdateDataBase(NameProblemWaterQuality.LOW, "Низкая проводимость пермеата");
                                            }
                                            else if (IsProblem == false)
                                            {
                                                NameProblem = NameProblemWaterQuality.LOW;
                                                AddProblemDataBase("Низкая проводимость пермеата");
                                                SearchWaterQuality.listWaterQualityError.Add(new WaterQualityErrorClass
                                                {
                                                    Name = Name,
                                                    NameProblem = NameProblem
                                                });
                                                ErrorEvent?.Invoke('+');
                                                IsProblem = true;
                                            }
                                        }
                                        else if (value > AllSettings.provodimostPermeataHigh)
                                        {
                                            if (IsProblem == true && NameProblem == NameProblemWaterQuality.LOW)
                                            {
                                                UpdateDataBase(NameProblemWaterQuality.HIGH, "Высокая проводимость пермеата");
                                            }
                                            else if (IsProblem == false)
                                            {
                                                NameProblem = NameProblemWaterQuality.HIGH;
                                                AddProblemDataBase("Высокая проводимость пермеата");
                                                SearchWaterQuality.listWaterQualityError.Add(new WaterQualityErrorClass
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
                                                IsProblem = false;
                                            }
                                        }
                                        break;
                                    case "PhPermeata":
                                        if (value < AllSettings.pHPermeataLow)
                                        {
                                            if (IsProblem == true && NameProblem == NameProblemWaterQuality.HIGH)
                                            {
                                                UpdateDataBase(NameProblemWaterQuality.LOW, "Низкий pH пермеата");
                                            }
                                            else if (IsProblem == false)
                                            {
                                                NameProblem = NameProblemWaterQuality.LOW;
                                                AddProblemDataBase("Низкий pH пермеата");
                                                SearchWaterQuality.listWaterQualityError.Add(new WaterQualityErrorClass
                                                {
                                                    Name = Name,
                                                    NameProblem = NameProblem
                                                });
                                                ErrorEvent?.Invoke('+');
                                                IsProblem = true;
                                            }
                                        }
                                        else if (value > AllSettings.pHPermeataHigh)
                                        {
                                            if (IsProblem == true && NameProblem == NameProblemWaterQuality.LOW)
                                            {
                                                UpdateDataBase(NameProblemWaterQuality.HIGH, "Высокий pH пермеата");
                                            }
                                            else if (IsProblem == false)
                                            {
                                                NameProblem = NameProblemWaterQuality.HIGH;
                                                AddProblemDataBase("Высокий pH пермеата");
                                                SearchWaterQuality.listWaterQualityError.Add(new WaterQualityErrorClass
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
                                                IsProblem = false;
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception ex) 
                    {
                        LogClass.Write("WaterQualityItem:StartCheckProblem::" + ex.Message);
                    }
                }

                IsCheckProblem = false;
            });

            _thread.Start();
        }

        private void AddProblemDataBase(string message)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
            {
                try
                {

                }
                catch (Exception ex)
                {
                    LogClass.Write("WaterQualityItem:AddProblemDataBase::" + ex.Message);
                }
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand
                {
                    Connection = connection
                };
                command.CommandText = "INSERT INTO journal_messages (date_start, message, is_active, name_problem, itemid) " +
                    $"VALUES ('{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}', " +
                    $"'{message}', " +
                    $"true, " +
                    $"'{NameProblem}', " +
                    $"{ItemId})";
                command.ExecuteNonQuery();
            }
        }
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
                            $"WHERE is_active = true AND itemid = {ItemId}"
                        };
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) 
                {
                    LogClass.Write("WaterQualityItem:ChangeStatusProblemDataBase::" + ex.Message);
                }
            }
        }
        private void UpdateDataBase(NameProblemWaterQuality newNameProblem, string message)
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
                            $"message = '{message}' " +
                            $"WHERE itemid = {ItemId} AND is_active = true"
                        };
                        command.ExecuteNonQuery();
                        NameProblem = newNameProblem;
                        MainForm.journalMessage.UpdateError();
                    }
                }
                catch (Exception ex) 
                {
                    LogClass.Write("WaterQualityItem:UpdateDataBase::" + ex.Message);
                }
            }
        }
        public void RemoveErrorInList()
        {
            WaterQualityErrorClass itemError = SearchWaterQuality.listWaterQualityError.Where(x => x.Name == Name).FirstOrDefault();
            if (itemError != null)
            {
                SearchWaterQuality.listWaterQualityError.Remove(itemError);
                ErrorEvent?.Invoke('-');
                IsProblem = false;
                IsCheckProblem = false;
                NameProblem = NameProblemWaterQuality.NULL;
            }
        }
        public void StopCheckProblem()
        {
            try
            {
                IsCheckProblem = false;

                if (_thread != null && (_thread.ThreadState == ThreadState.Running || _thread.ThreadState == ThreadState.WaitSleepJoin))
                    _thread.Abort();
            }
            catch { }
        }
    }
}
