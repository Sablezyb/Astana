using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;
using Astana.Class.Membrans;
using Astana.Class.Predfilters;
using System.Configuration;
using Astana.Class.WaterQuality;
using Astana.Class.Economika;
using Astana.Class.JournalMessage;
using Astana.Class.MainInfo;

namespace Astana
{
    public enum NameProblemPredfilter
    {
        NULL,
        SredZagr,
        SilnoZagr
    }
    public enum NameProblemMembran
    {
        NULL,
        NizkoyaProizvoditelnost,
        TrebuetsyaPromivka
    }
    public enum NameRO
    {
        RO1,
        RO2,
        RO3,
        RO4,
        RO5,
        RO6,
        ROall
    }
    public enum NameProblemWaterQuality
    {
        NULL,
        LOW,
        HIGH
    }

    public partial class MainForm : Form
    {
        #region Журнал ошибок
        public static int _countError = 0;
        public static JournalMessageClass journalMessage;
        #endregion

        private SearchProblePredfilter _searchProblePredfilter;
        private SearchProblemMembran _searchProblemMembran;
        private SearchWaterQuality _searchWaterQuality;
        private EconomikaClass _economika;
        private MainInfoClass _mainInfoClass;

        private bool _isZommMyDataGrid = false;
        public MainForm()
        {
            InitializeComponent();

            try
            {
                //StartProgram();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void StartProgram()
        {
            //Получение настроек из базы
            using (NpgsqlConnection _connectionCleverOsmos = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
            {
                _connectionCleverOsmos.Open();
                if (_connectionCleverOsmos.State == ConnectionState.Open)
                {
                    string query = "SELECT * FROM Settings ORDER BY setting_id";
                    NpgsqlCommand command = new NpgsqlCommand(query, _connectionCleverOsmos);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            object nameSetting;
                            while (reader.Read())
                            {
                                nameSetting = reader.GetValue(1);
                                switch (nameSetting.ToString())
                                {
                                    case "perepadSredZagrFiltra":
                                        object perepadSredZagrFiltra = reader.GetValue(2);
                                        if (perepadSredZagrFiltra.ToString() != "")
                                            AllSettings.perepadSredZagrFiltra = double.Parse(perepadSredZagrFiltra.ToString().Replace('.', ','));
                                        break;
                                    case "perepadSilnoZagrFiltra":
                                        object perepadSilnoZagrFiltra = reader.GetValue(2);
                                        if (perepadSilnoZagrFiltra.ToString() != "")
                                            AllSettings.perepadSilnoZagrFiltra = double.Parse(perepadSilnoZagrFiltra.ToString().Replace('.', ','));
                                        break;
                                    case "timeOut":
                                        object timeOut = reader.GetValue(2);
                                        if (timeOut.ToString() != "")
                                            AllSettings.timeOut = int.Parse(timeOut.ToString());
                                        break;
                                    case "enterPressureRo1":
                                        object enterPressureRo1 = reader.GetValue(2);
                                        if (enterPressureRo1.ToString() != "")
                                            AllSettings.enterPressureRo1 = double.Parse(enterPressureRo1.ToString().Replace('.', ','));
                                        break;
                                    case "enterPressureRo2":
                                        object enterPressureRo2 = reader.GetValue(2);
                                        if (enterPressureRo2.ToString() != "")
                                            AllSettings.enterPressureRo2 = double.Parse(enterPressureRo2.ToString().Replace('.', ','));
                                        break;
                                    case "enterPressureRo3":
                                        object enterPressureRo3 = reader.GetValue(2);
                                        if (enterPressureRo3.ToString() != "")
                                            AllSettings.enterPressureRo3 = double.Parse(enterPressureRo3.ToString().Replace('.', ','));
                                        break;
                                    case "enterPressureRo4":
                                        object enterPressureRo4 = reader.GetValue(2);
                                        if (enterPressureRo4.ToString() != "")
                                            AllSettings.enterPressureRo4 = double.Parse(enterPressureRo4.ToString().Replace('.', ','));
                                        break;
                                    case "enterPressureRo5":
                                        object enterPressureRo5 = reader.GetValue(2);
                                        if (enterPressureRo5.ToString() != "")
                                            AllSettings.enterPressureRo5 = double.Parse(enterPressureRo5.ToString().Replace('.', ','));
                                        break;
                                    case "enterPressureRo6":
                                        object enterPressureRo6 = reader.GetValue(2);
                                        if (enterPressureRo6.ToString() != "")
                                            AllSettings.enterPressureRo6 = double.Parse(enterPressureRo6.ToString().Replace('.', ','));
                                        break;
                                    case "timeDelayRo1":
                                        object timeDelayRo1 = reader.GetValue(2);
                                        if (timeDelayRo1.ToString() != "")
                                            AllSettings.timeDelayRo1 = int.Parse(timeDelayRo1.ToString());
                                        break;
                                    case "timeDelayRo2":
                                        object timeDelayRo2 = reader.GetValue(2);
                                        if (timeDelayRo2.ToString() != "")
                                            AllSettings.timeDelayRo2 = int.Parse(timeDelayRo2.ToString());
                                        break;
                                    case "timeDelayRo3":
                                        object timeDelayRo3 = reader.GetValue(2);
                                        if (timeDelayRo3.ToString() != "")
                                            AllSettings.timeDelayRo3 = int.Parse(timeDelayRo3.ToString());
                                        break;
                                    case "timeDelayRo4":
                                        object timeDelayRo4 = reader.GetValue(2);
                                        if (timeDelayRo4.ToString() != "")
                                            AllSettings.timeDelayRo4 = int.Parse(timeDelayRo4.ToString());
                                        break;
                                    case "timeDelayRo5":
                                        object timeDelayRo5 = reader.GetValue(2);
                                        if (timeDelayRo5.ToString() != "")
                                            AllSettings.timeDelayRo5 = int.Parse(timeDelayRo5.ToString());
                                        break;
                                    case "timeDelayRo6":
                                        object timeDelayRo6 = reader.GetValue(2);
                                        if (timeDelayRo6.ToString() != "")
                                            AllSettings.timeDelayRo6 = int.Parse(timeDelayRo6.ToString());
                                        break;
                                    case "procentNizkoyProizvoditelnostiRo1":
                                        object procentNizkoyProizvoditelnostiRo1 = reader.GetValue(2);
                                        if (procentNizkoyProizvoditelnostiRo1.ToString() != "")
                                            AllSettings.procentNizkoyProizvoditelnostiRo1 = int.Parse(procentNizkoyProizvoditelnostiRo1.ToString());
                                        break;
                                    case "procentNizkoyProizvoditelnostiRo2":
                                        object procentNizkoyProizvoditelnostiRo2 = reader.GetValue(2);
                                        if (procentNizkoyProizvoditelnostiRo2.ToString() != "")
                                            AllSettings.procentNizkoyProizvoditelnostiRo2 = int.Parse(procentNizkoyProizvoditelnostiRo2.ToString());
                                        break;
                                    case "procentNizkoyProizvoditelnostiRo3":
                                        object procentNizkoyProizvoditelnostiRo3 = reader.GetValue(2);
                                        if (procentNizkoyProizvoditelnostiRo3.ToString() != "")
                                            AllSettings.procentNizkoyProizvoditelnostiRo3 = int.Parse(procentNizkoyProizvoditelnostiRo3.ToString());
                                        break;
                                    case "procentNizkoyProizvoditelnostiRo4":
                                        object procentNizkoyProizvoditelnostiRo4 = reader.GetValue(2);
                                        if (procentNizkoyProizvoditelnostiRo4.ToString() != "")
                                            AllSettings.procentNizkoyProizvoditelnostiRo4 = int.Parse(procentNizkoyProizvoditelnostiRo4.ToString());
                                        break;
                                    case "procentNizkoyProizvoditelnostiRo5":
                                        object procentNizkoyProizvoditelnostiRo5 = reader.GetValue(2);
                                        if (procentNizkoyProizvoditelnostiRo5.ToString() != "")
                                            AllSettings.procentNizkoyProizvoditelnostiRo5 = int.Parse(procentNizkoyProizvoditelnostiRo5.ToString());
                                        break;
                                    case "procentNizkoyProizvoditelnostiRo6":
                                        object procentNizkoyProizvoditelnostiRo6 = reader.GetValue(2);
                                        if (procentNizkoyProizvoditelnostiRo6.ToString() != "")
                                            AllSettings.procentNizkoyProizvoditelnostiRo6 = int.Parse(procentNizkoyProizvoditelnostiRo6.ToString());
                                        break;
                                    case "proizvoditelnostRO1":
                                        object proizvoditelnostRO1 = reader.GetValue(2);
                                        if (proizvoditelnostRO1.ToString() != "")
                                            AllSettings.proizvoditelnostRO1 = double.Parse(proizvoditelnostRO1.ToString().Replace('.', ','));
                                        break;
                                    case "proizvoditelnostRO2":
                                        object proizvoditelnostRO2 = reader.GetValue(2);
                                        if (proizvoditelnostRO2.ToString() != "")
                                            AllSettings.proizvoditelnostRO2 = double.Parse(proizvoditelnostRO2.ToString().Replace('.', ','));
                                        break;
                                    case "proizvoditelnostRO3":
                                        object proizvoditelnostRO3 = reader.GetValue(2);
                                        if (proizvoditelnostRO3.ToString() != "")
                                            AllSettings.proizvoditelnostRO3 = double.Parse(proizvoditelnostRO3.ToString().Replace('.', ','));
                                        break;
                                    case "proizvoditelnostRO4":
                                        object proizvoditelnostRO4 = reader.GetValue(2);
                                        if (proizvoditelnostRO4.ToString() != "")
                                            AllSettings.proizvoditelnostRO4 = double.Parse(proizvoditelnostRO4.ToString().Replace('.', ','));
                                        break;
                                    case "proizvoditelnostRO5":
                                        object proizvoditelnostRO5 = reader.GetValue(2);
                                        if (proizvoditelnostRO5.ToString() != "")
                                            AllSettings.proizvoditelnostRO5 = double.Parse(proizvoditelnostRO5.ToString().Replace('.', ','));
                                        break;
                                    case "proizvoditelnostRO6":
                                        object proizvoditelnostRO6 = reader.GetValue(2);
                                        if (proizvoditelnostRO6.ToString() != "")
                                            AllSettings.proizvoditelnostRO6 = double.Parse(proizvoditelnostRO6.ToString().Replace('.', ','));
                                        break;
                                    case "timeResetRO1":
                                        object timeResetRO1 = reader.GetValue(2);
                                        if (timeResetRO1.ToString() != "")
                                            AllSettings.timeResetRO1 = int.Parse(timeResetRO1.ToString());
                                        break;
                                    case "timeResetRO2":
                                        object timeResetRO2 = reader.GetValue(2);
                                        if (timeResetRO2.ToString() != "")
                                            AllSettings.timeResetRO2 = int.Parse(timeResetRO2.ToString());
                                        break;
                                    case "timeResetRO3":
                                        object timeResetRO3 = reader.GetValue(2);
                                        if (timeResetRO3.ToString() != "")
                                            AllSettings.timeResetRO3 = int.Parse(timeResetRO3.ToString());
                                        break;
                                    case "timeResetRO4":
                                        object timeResetRO4 = reader.GetValue(2);
                                        if (timeResetRO4.ToString() != "")
                                            AllSettings.timeResetRO4 = int.Parse(timeResetRO4.ToString());
                                        break;
                                    case "timeResetRO5":
                                        object timeResetRO5 = reader.GetValue(2);
                                        if (timeResetRO5.ToString() != "")
                                            AllSettings.timeResetRO5 = int.Parse(timeResetRO5.ToString());
                                        break;
                                    case "timeResetRO6":
                                        object timeResetRO6 = reader.GetValue(2);
                                        if (timeResetRO6.ToString() != "")
                                            AllSettings.timeResetRO6 = int.Parse(timeResetRO6.ToString());
                                        break;
                                    case "timeWhenNeedPromivkaRO1":
                                        object timeWhenNeedPromivkaRO1 = reader.GetValue(2);
                                        if (timeWhenNeedPromivkaRO1.ToString() != "")
                                            AllSettings.timeWhenNeedPromivkaRO1 = int.Parse(timeWhenNeedPromivkaRO1.ToString());
                                        break;
                                    case "timeWhenNeedPromivkaRO2":
                                        object timeWhenNeedPromivkaRO2 = reader.GetValue(2);
                                        if (timeWhenNeedPromivkaRO2.ToString() != "")
                                            AllSettings.timeWhenNeedPromivkaRO2 = int.Parse(timeWhenNeedPromivkaRO2.ToString());
                                        break;
                                    case "timeWhenNeedPromivkaRO3":
                                        object timeWhenNeedPromivkaRO3 = reader.GetValue(2);
                                        if (timeWhenNeedPromivkaRO3.ToString() != "")
                                            AllSettings.timeWhenNeedPromivkaRO3 = int.Parse(timeWhenNeedPromivkaRO3.ToString());
                                        break;
                                    case "timeWhenNeedPromivkaRO4":
                                        object timeWhenNeedPromivkaRO4 = reader.GetValue(2);
                                        if (timeWhenNeedPromivkaRO4.ToString() != "")
                                            AllSettings.timeWhenNeedPromivkaRO4 = int.Parse(timeWhenNeedPromivkaRO4.ToString());
                                        break;
                                    case "timeWhenNeedPromivkaRO5":
                                        object timeWhenNeedPromivkaRO5 = reader.GetValue(2);
                                        if (timeWhenNeedPromivkaRO5.ToString() != "")
                                            AllSettings.timeWhenNeedPromivkaRO5 = int.Parse(timeWhenNeedPromivkaRO5.ToString());
                                        break;
                                    case "timeWhenNeedPromivkaRO6":
                                        object timeWhenNeedPromivkaRO6 = reader.GetValue(2);
                                        if (timeWhenNeedPromivkaRO6.ToString() != "")
                                            AllSettings.timeWhenNeedPromivkaRO6 = int.Parse(timeWhenNeedPromivkaRO6.ToString());
                                        break;
                                    case "temperaturaWaterLow":
                                        object temperaturaWaterLow = reader.GetValue(2);
                                        if (temperaturaWaterLow.ToString() != "")
                                            AllSettings.temperaturaWaterLow = double.Parse(temperaturaWaterLow.ToString().Replace('.', ','));
                                        break;
                                    case "temperaturaWaterHigh":
                                        object temperaturaWaterHigh = reader.GetValue(2);
                                        if (temperaturaWaterHigh.ToString() != "")
                                            AllSettings.temperaturaWaterHigh = double.Parse(temperaturaWaterHigh.ToString().Replace('.', ','));
                                        break;
                                    case "provodimostIshodWaterLow":
                                        object provodimostIshodWaterLow = reader.GetValue(2);
                                        if (provodimostIshodWaterLow.ToString() != "")
                                            AllSettings.provodimostIshodWaterLow = double.Parse(provodimostIshodWaterLow.ToString().Replace('.', ','));
                                        break;
                                    case "provodimostIshodWaterHigh":
                                        object provodimostIshodWaterHigh = reader.GetValue(2);
                                        if (provodimostIshodWaterHigh.ToString() != "")
                                            AllSettings.provodimostIshodWaterHigh = double.Parse(provodimostIshodWaterHigh.ToString().Replace('.', ','));
                                        break;
                                    case "pHIshodWaterAfterCorrectLow":
                                        object pHIshodWaterAfterCorrectLow = reader.GetValue(2);
                                        if (pHIshodWaterAfterCorrectLow.ToString() != "")
                                            AllSettings.pHIshodWaterAfterCorrectLow = double.Parse(pHIshodWaterAfterCorrectLow.ToString().Replace('.', ','));
                                        break;
                                    case "pHIshodWaterAfterCorrectHigh":
                                        object pHIshodWaterAfterCorrectHigh = reader.GetValue(2);
                                        if (pHIshodWaterAfterCorrectHigh.ToString() != "")
                                            AllSettings.pHIshodWaterAfterCorrectHigh = double.Parse(pHIshodWaterAfterCorrectHigh.ToString().Replace('.', ','));
                                        break;
                                    case "provodimostPermeataLow":
                                        object provodimostPermeataLow = reader.GetValue(2);
                                        if (provodimostPermeataLow.ToString() != "")
                                            AllSettings.provodimostPermeataLow = double.Parse(provodimostPermeataLow.ToString().Replace('.', ','));
                                        break;
                                    case "provodimostPermeataHigh":
                                        object provodimostPermeataHigh = reader.GetValue(2);
                                        if (provodimostPermeataHigh.ToString() != "")
                                            AllSettings.provodimostPermeataHigh = double.Parse(provodimostPermeataHigh.ToString().Replace('.', ','));
                                        break;
                                    case "pHPermeataLow":
                                        object pHPermeataLow = reader.GetValue(2);
                                        if (pHPermeataLow.ToString() != "")
                                            AllSettings.pHPermeataLow = double.Parse(pHPermeataLow.ToString().Replace('.', ','));
                                        break;
                                    case "pHPermeataHigh":
                                        object pHPermeataHigh = reader.GetValue(2);
                                        if (pHPermeataHigh.ToString() != "")
                                            AllSettings.pHPermeataHigh = double.Parse(pHPermeataHigh.ToString().Replace('.', ','));
                                        break;
                                    case "davlenieNaUstanovky":
                                        object davlenieNaUstanovky = reader.GetValue(2);
                                        if (davlenieNaUstanovky.ToString() != "")
                                            AllSettings.davlenieNaUstanovky = double.Parse(davlenieNaUstanovky.ToString().Replace('.', ','));
                                        break;
                                    case "stoimostIshodWater":
                                        object stoimostIshodWater = reader.GetValue(2);
                                        if (stoimostIshodWater.ToString() != "")
                                            AllSettings.stoimostIshodWater = int.Parse(stoimostIshodWater.ToString());
                                        break;
                                    case "stoimostUtilConcentrata":
                                        object stoimostUtilConcentrata = reader.GetValue(2);
                                        if (stoimostUtilConcentrata.ToString() != "")
                                            AllSettings.stoimostUtilConcentrata = int.Parse(stoimostUtilConcentrata.ToString());
                                        break;
                                    case "stoimostAntiskalanta":
                                        object stoimostAntiskalanta = reader.GetValue(2);
                                        if (stoimostAntiskalanta.ToString() != "")
                                            AllSettings.stoimostAntiskalanta = int.Parse(stoimostAntiskalanta.ToString());
                                        break;
                                    case "stoimostMetoBisulvitaNatriya":
                                        object stoimostMetoBisulvitaNatriya = reader.GetValue(2);
                                        if (stoimostMetoBisulvitaNatriya.ToString() != "")
                                            AllSettings.stoimostMetoBisulvitaNatriya = int.Parse(stoimostMetoBisulvitaNatriya.ToString());
                                        break;
                                    case "stoimostElectroEnergy":
                                        object stoimostElectroEnergy = reader.GetValue(2);
                                        if (stoimostElectroEnergy.ToString() != "")
                                            AllSettings.stoimostElectroEnergy = int.Parse(stoimostElectroEnergy.ToString());
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            //Инициализируем журнал ошибок
            journalMessage = new JournalMessageClass(myDataGrid1);

            //Получаем активные ошибки по необходимости промывки или замене мембран
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    NpgsqlCommand command = new NpgsqlCommand
                    {
                        Connection = connection,
                        CommandText = "SELECT COUNT(*) " +
                        "FROM journal_messages " +
                        "WHERE itemid IN(1,2,3,4,5,6) AND is_active = true"
                    };

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            int count = int.Parse(reader.GetValue(0).ToString());
                            if (count > 0)
                            {
                                lb_count_error.Text = count.ToString();
                                journalMessage.UpdateError();
                            }
                        }
                    }
                }
            }

            _searchProblePredfilter = new SearchProblePredfilter(lb_count_error);
            _searchProblePredfilter.Start();

            _searchProblemMembran = new SearchProblemMembran(lb_count_error);
            _searchProblemMembran.Start();

            _searchWaterQuality = new SearchWaterQuality(lb_count_error);
            _searchWaterQuality.Start();

            _economika = new EconomikaClass(lb_priceIshodWater);
            _economika.Start();

            _mainInfoClass = new MainInfoClass(lb_ProvodimostIshodWater, lb_RashodIshodWater, lb_PhIshodWater, lb_Power, lb_ProvodimostPermeata,
                lb_RashodPermeata, lb_PhPermeata, lb_ProvodimostConcetrata, lb_RashodConcetrata, lb_davlenieNaUstanovky, lb_Recovery, lb_Rejection, lb_davlenieIshodWaterNaRO);
            _mainInfoClass.Start();
        }

        private void btn_journalParametr_Click(object sender, EventArgs e)
        {
            ParametrJournalForms journalForms = new ParametrJournalForms();
            journalForms.ShowDialog();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_journalEvent_Click(object sender, EventArgs e)
        {
            EventJournalForms form = new EventJournalForms(lb_count_error);
            form.ShowDialog();
        }

        private void btn_Settimgs_Click(object sender, EventArgs e)
        {
            FormSettings form = new FormSettings(lb_count_error);
            form.ShowDialog();
        }

        private void btn_JournalMessage_Click(object sender, EventArgs e)
        {
            JournalMessageArhive journalMessageArhive = new JournalMessageArhive();
            journalMessageArhive.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_searchProblePredfilter != null)
                    _searchProblePredfilter.Stop();

                if (_searchProblemMembran != null)
                    _searchProblemMembran.Stop();

                if (_searchWaterQuality != null)
                    _searchWaterQuality.Stop();

                if (_economika != null)
                    _economika.Stop();

                if (_mainInfoClass != null)
                    _mainInfoClass.Stop();
            }
            catch { }
        }

        private void btn_Zoom_Click(object sender, EventArgs e)
        {
            if (_isZommMyDataGrid == true)
            {
                myDataGrid1.Height = 282;
                btn_Zoom.Location = new Point(btn_Zoom.Location.X ,myDataGrid1.Location.Y + 4);
                _isZommMyDataGrid = false;
            }
            else
            {
                myDataGrid1.Height = (int)(this.Height / 1.3f);
                btn_Zoom.Location = new Point(btn_Zoom.Location.X, myDataGrid1.Location.Y + 4);
                _isZommMyDataGrid = true;
            }
        }
    }
}
