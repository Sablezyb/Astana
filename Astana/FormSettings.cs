using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Npgsql;
using System.Configuration;
using System.Text.RegularExpressions;
using Astana.Class.Log;
using Astana.Class.Membrans;

namespace Astana
{
    public partial class FormSettings : Form
    {
        private bool _isChangeSetting = false;
        private Regex _regex = new Regex(@"^\d+(\.?|\,?)\d*$");
        private readonly Label label;

        public FormSettings(Label label)
        {
            InitializeComponent();

            this.label = label;

            #region Предфильтр
            tb_perepadSredZagrFiltra.Text = AllSettings.perepadSredZagrFiltra.ToString();
            tb_perebadSilnoZagrFiltra.Text = AllSettings.perepadSilnoZagrFiltra.ToString();
            tb_timeOut.Text = AllSettings.timeOut.ToString();

            tb_perepadSredZagrFiltra.TextChanged += tb_change_text;
            tb_perebadSilnoZagrFiltra.TextChanged += tb_change_text;
            tb_timeOut.TextChanged += tb_change_text;
            #endregion

            #region Мембраны
            tb_EnterPressureRO1.Text = AllSettings.enterPressureRo1.ToString();
            tb_EnterPressureRO2.Text = AllSettings.enterPressureRo2.ToString();
            tb_EnterPressureRO3.Text = AllSettings.enterPressureRo3.ToString();
            tb_EnterPressureRO4.Text = AllSettings.enterPressureRo4.ToString();
            tb_EnterPressureRO5.Text = AllSettings.enterPressureRo5.ToString();
            tb_EnterPressureRO6.Text = AllSettings.enterPressureRo6.ToString();

            tb_procentNizkoyProisvoditelnostiRO1.Text = AllSettings.procentNizkoyProizvoditelnostiRo1.ToString();
            tb_procentNizkoyProisvoditelnostiRO2.Text = AllSettings.procentNizkoyProizvoditelnostiRo2.ToString();
            tb_procentNizkoyProisvoditelnostiRO3.Text = AllSettings.procentNizkoyProizvoditelnostiRo3.ToString();
            tb_procentNizkoyProisvoditelnostiRO4.Text = AllSettings.procentNizkoyProizvoditelnostiRo4.ToString();
            tb_procentNizkoyProisvoditelnostiRO5.Text = AllSettings.procentNizkoyProizvoditelnostiRo5.ToString();
            tb_procentNizkoyProisvoditelnostiRO6.Text = AllSettings.procentNizkoyProizvoditelnostiRo6.ToString();

            tb_TimeDelayRO1.Text = AllSettings.timeDelayRo1.ToString();
            tb_TimeDelayRO2.Text = AllSettings.timeDelayRo2.ToString();
            tb_TimeDelayRO3.Text = AllSettings.timeDelayRo3.ToString();
            tb_TimeDelayRO4.Text = AllSettings.timeDelayRo4.ToString();
            tb_TimeDelayRO5.Text = AllSettings.timeDelayRo5.ToString();
            tb_TimeDelayRO6.Text = AllSettings.timeDelayRo6.ToString();

            tb_proizvoditelnostRO1.Text = AllSettings.proizvoditelnostRO1.ToString();
            tb_proizvoditelnostRO2.Text = AllSettings.proizvoditelnostRO2.ToString();
            tb_proizvoditelnostRO3.Text = AllSettings.proizvoditelnostRO3.ToString();
            tb_proizvoditelnostRO4.Text = AllSettings.proizvoditelnostRO4.ToString();
            tb_proizvoditelnostRO5.Text = AllSettings.proizvoditelnostRO5.ToString();
            tb_proizvoditelnostRO6.Text = AllSettings.proizvoditelnostRO6.ToString();

            tb_timeResetRO1.Text = AllSettings.timeResetRO1.ToString();
            tb_timeResetRO2.Text = AllSettings.timeResetRO2.ToString();
            tb_timeResetRO3.Text = AllSettings.timeResetRO3.ToString();
            tb_timeResetRO4.Text = AllSettings.timeResetRO4.ToString();
            tb_timeResetRO5.Text = AllSettings.timeResetRO5.ToString();
            tb_timeResetRO6.Text = AllSettings.timeResetRO6.ToString();

            tb_timeWhenNeedPromivkaRO1.Text = AllSettings.timeWhenNeedPromivkaRO1.ToString();
            tb_timeWhenNeedPromivkaRO2.Text = AllSettings.timeWhenNeedPromivkaRO2.ToString();
            tb_timeWhenNeedPromivkaRO3.Text = AllSettings.timeWhenNeedPromivkaRO3.ToString();
            tb_timeWhenNeedPromivkaRO4.Text = AllSettings.timeWhenNeedPromivkaRO4.ToString();
            tb_timeWhenNeedPromivkaRO5.Text = AllSettings.timeWhenNeedPromivkaRO5.ToString();
            tb_timeWhenNeedPromivkaRO6.Text = AllSettings.timeWhenNeedPromivkaRO6.ToString();

            tb_EnterPressureRO1.TextChanged += tb_change_text;
            tb_EnterPressureRO2.TextChanged += tb_change_text;
            tb_EnterPressureRO3.TextChanged += tb_change_text;
            tb_EnterPressureRO4.TextChanged += tb_change_text;
            tb_EnterPressureRO5.TextChanged += tb_change_text;
            tb_EnterPressureRO6.TextChanged += tb_change_text;

            tb_procentNizkoyProisvoditelnostiRO1.TextChanged += tb_change_text;
            tb_procentNizkoyProisvoditelnostiRO2.TextChanged += tb_change_text;
            tb_procentNizkoyProisvoditelnostiRO3.TextChanged += tb_change_text;
            tb_procentNizkoyProisvoditelnostiRO4.TextChanged += tb_change_text;
            tb_procentNizkoyProisvoditelnostiRO5.TextChanged += tb_change_text;
            tb_procentNizkoyProisvoditelnostiRO6.TextChanged += tb_change_text;

            tb_TimeDelayRO1.TextChanged += tb_change_text;
            tb_TimeDelayRO2.TextChanged += tb_change_text;
            tb_TimeDelayRO3.TextChanged += tb_change_text;
            tb_TimeDelayRO4.TextChanged += tb_change_text;
            tb_TimeDelayRO5.TextChanged += tb_change_text;
            tb_TimeDelayRO6.TextChanged += tb_change_text;

            tb_proizvoditelnostRO1.TextChanged += tb_change_text;
            tb_proizvoditelnostRO2.TextChanged += tb_change_text;
            tb_proizvoditelnostRO3.TextChanged += tb_change_text;
            tb_proizvoditelnostRO4.TextChanged += tb_change_text;
            tb_proizvoditelnostRO5.TextChanged += tb_change_text;
            tb_proizvoditelnostRO6.TextChanged += tb_change_text;

            tb_timeResetRO1.TextChanged += tb_change_text;
            tb_timeResetRO2.TextChanged += tb_change_text;
            tb_timeResetRO3.TextChanged += tb_change_text;
            tb_timeResetRO4.TextChanged += tb_change_text;
            tb_timeResetRO5.TextChanged += tb_change_text;
            tb_timeResetRO6.TextChanged += tb_change_text;

            tb_timeWhenNeedPromivkaRO1.TextChanged += tb_change_text;
            tb_timeWhenNeedPromivkaRO2.TextChanged += tb_change_text;
            tb_timeWhenNeedPromivkaRO3.TextChanged += tb_change_text;
            tb_timeWhenNeedPromivkaRO4.TextChanged += tb_change_text;
            tb_timeWhenNeedPromivkaRO5.TextChanged += tb_change_text;
            tb_timeWhenNeedPromivkaRO6.TextChanged += tb_change_text;
            #endregion

            #region Качесто воды
            tb_temperaturaLow.Text = AllSettings.temperaturaWaterLow.ToString();
            tb_temperaturaHigh.Text = AllSettings.temperaturaWaterHigh.ToString();
            tb_provodimostIshodWaterLow.Text = AllSettings.provodimostIshodWaterLow.ToString();
            tb_provodimostIshodWaterHigh.Text = AllSettings.provodimostIshodWaterHigh.ToString();
            tb_pHIshodWaterAfterCorrectLow.Text = AllSettings.pHIshodWaterAfterCorrectLow.ToString();
            tb_pHIshodWaterAfterCorrectHigh.Text = AllSettings.pHIshodWaterAfterCorrectHigh.ToString();
            tb_provodimostPermeataLow.Text = AllSettings.provodimostPermeataLow.ToString();
            tb_provodimostPermeataHigh.Text = AllSettings.provodimostPermeataHigh.ToString();
            tb_pHPermeataLow.Text = AllSettings.pHPermeataLow.ToString();
            tb_pHPermeataHigh.Text = AllSettings.pHPermeataHigh.ToString();
            tb_davlenieNaUstanovky.Text = AllSettings.davlenieNaUstanovky.ToString();

            tb_temperaturaLow.TextChanged += tb_change_text;
            tb_temperaturaHigh.TextChanged += tb_change_text;
            tb_provodimostIshodWaterLow.TextChanged += tb_change_text;
            tb_provodimostIshodWaterHigh.TextChanged += tb_change_text;
            tb_pHIshodWaterAfterCorrectLow.TextChanged += tb_change_text;
            tb_pHIshodWaterAfterCorrectHigh.TextChanged += tb_change_text;
            tb_provodimostPermeataLow.TextChanged += tb_change_text;
            tb_provodimostPermeataHigh.TextChanged += tb_change_text;
            tb_pHPermeataLow.TextChanged += tb_change_text;
            tb_pHPermeataHigh.TextChanged += tb_change_text;
            tb_davlenieNaUstanovky.TextChanged += tb_change_text;
            #endregion

            #region Экономика
            tb_stoimostIshodWater.Text = AllSettings.stoimostIshodWater.ToString();
            tb_stoimostUtilConcentrata.Text = AllSettings.stoimostUtilConcentrata.ToString();
            tb_stoimostAntiskalanta.Text = AllSettings.stoimostAntiskalanta.ToString();
            tb_stoimostMetoBisulvitaNatriya.Text = AllSettings.stoimostMetoBisulvitaNatriya.ToString();
            tb_stoimostElectroEnergy.Text = AllSettings.stoimostElectroEnergy.ToString();

            tb_stoimostIshodWater.TextChanged += tb_change_text;
            tb_stoimostUtilConcentrata.TextChanged += tb_change_text;
            tb_stoimostAntiskalanta.TextChanged += tb_change_text;
            tb_stoimostMetoBisulvitaNatriya.TextChanged += tb_change_text;
            tb_stoimostElectroEnergy.TextChanged += tb_change_text;
            #endregion
        }
        private void btn_closeForm_Click(object sender, EventArgs e)
        {
            if (_isChangeSetting)
            {
                if (MessageBox.Show("Были внесены изменения, Сохранить ?", "Уведомление", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    double sredFiltra = double.Parse(tb_perepadSredZagrFiltra.Text.Replace('.', ','));
                    double silnoFiltra = double.Parse(tb_perebadSilnoZagrFiltra.Text.Replace('.', ','));

                    if (sredFiltra >= silnoFiltra)
                    {
                        MessageBox.Show("Перепад среднего загрязнения не может быть больше или равен перепаду сильного");
                        return;
                    }

                    #region Предфильтр
                    AllSettings.perepadSredZagrFiltra = sredFiltra;
                    AllSettings.perepadSilnoZagrFiltra = silnoFiltra;
                    AllSettings.timeOut = int.Parse(tb_timeOut.Text.Replace(",", "").Replace(".", ""));
                    #endregion

                    #region Мембраны
                    AllSettings.enterPressureRo1 = double.Parse(tb_EnterPressureRO1.Text.Replace('.', ','));
                    AllSettings.enterPressureRo2 = double.Parse(tb_EnterPressureRO2.Text.Replace('.', ','));
                    AllSettings.enterPressureRo3 = double.Parse(tb_EnterPressureRO3.Text.Replace('.', ','));
                    AllSettings.enterPressureRo4 = double.Parse(tb_EnterPressureRO4.Text.Replace('.', ','));
                    AllSettings.enterPressureRo5 = double.Parse(tb_EnterPressureRO5.Text.Replace('.', ','));
                    AllSettings.enterPressureRo6 = double.Parse(tb_EnterPressureRO6.Text.Replace('.', ','));

                    AllSettings.timeDelayRo1 = int.Parse(tb_TimeDelayRO1.Text.Replace(".", "").Replace(",", ""));
                    AllSettings.timeDelayRo2 = int.Parse(tb_TimeDelayRO2.Text.Replace(".", "").Replace(",", ""));
                    AllSettings.timeDelayRo3 = int.Parse(tb_TimeDelayRO3.Text.Replace(".", "").Replace(",", ""));
                    AllSettings.timeDelayRo4 = int.Parse(tb_TimeDelayRO4.Text.Replace(".", "").Replace(",", ""));
                    AllSettings.timeDelayRo5 = int.Parse(tb_TimeDelayRO5.Text.Replace(".", "").Replace(",", ""));
                    AllSettings.timeDelayRo6 = int.Parse(tb_TimeDelayRO6.Text.Replace(".", "").Replace(",", ""));

                    AllSettings.procentNizkoyProizvoditelnostiRo1 = double.Parse(tb_procentNizkoyProisvoditelnostiRO1.Text.Replace('.', ','));
                    AllSettings.procentNizkoyProizvoditelnostiRo2 = double.Parse(tb_procentNizkoyProisvoditelnostiRO2.Text.Replace('.', ','));
                    AllSettings.procentNizkoyProizvoditelnostiRo3 = double.Parse(tb_procentNizkoyProisvoditelnostiRO3.Text.Replace('.', ','));
                    AllSettings.procentNizkoyProizvoditelnostiRo4 = double.Parse(tb_procentNizkoyProisvoditelnostiRO4.Text.Replace('.', ','));
                    AllSettings.procentNizkoyProizvoditelnostiRo5 = double.Parse(tb_procentNizkoyProisvoditelnostiRO5.Text.Replace('.', ','));
                    AllSettings.procentNizkoyProizvoditelnostiRo6 = double.Parse(tb_procentNizkoyProisvoditelnostiRO6.Text.Replace('.', ','));

                    AllSettings.proizvoditelnostRO1 = double.Parse(tb_proizvoditelnostRO1.Text.Replace('.', ','));
                    AllSettings.proizvoditelnostRO2 = double.Parse(tb_proizvoditelnostRO2.Text.Replace('.', ','));
                    AllSettings.proizvoditelnostRO3 = double.Parse(tb_proizvoditelnostRO3.Text.Replace('.', ','));
                    AllSettings.proizvoditelnostRO4 = double.Parse(tb_proizvoditelnostRO4.Text.Replace('.', ','));
                    AllSettings.proizvoditelnostRO5 = double.Parse(tb_proizvoditelnostRO5.Text.Replace('.', ','));
                    AllSettings.proizvoditelnostRO6 = double.Parse(tb_proizvoditelnostRO6.Text.Replace('.', ','));

                    AllSettings.timeResetRO1 = int.Parse(tb_timeResetRO1.Text);
                    AllSettings.timeResetRO2 = int.Parse(tb_timeResetRO2.Text);
                    AllSettings.timeResetRO3 = int.Parse(tb_timeResetRO3.Text);
                    AllSettings.timeResetRO4 = int.Parse(tb_timeResetRO4.Text);
                    AllSettings.timeResetRO5 = int.Parse(tb_timeResetRO5.Text);
                    AllSettings.timeResetRO6 = int.Parse(tb_timeResetRO6.Text);

                    AllSettings.timeWhenNeedPromivkaRO1 = int.Parse(tb_timeWhenNeedPromivkaRO1.Text);
                    AllSettings.timeWhenNeedPromivkaRO2 = int.Parse(tb_timeWhenNeedPromivkaRO2.Text);
                    AllSettings.timeWhenNeedPromivkaRO3 = int.Parse(tb_timeWhenNeedPromivkaRO3.Text);
                    AllSettings.timeWhenNeedPromivkaRO4 = int.Parse(tb_timeWhenNeedPromivkaRO4.Text);
                    AllSettings.timeWhenNeedPromivkaRO5 = int.Parse(tb_timeWhenNeedPromivkaRO5.Text);
                    AllSettings.timeWhenNeedPromivkaRO6 = int.Parse(tb_timeWhenNeedPromivkaRO6.Text);
                    #endregion

                    #region Качество воды
                    AllSettings.temperaturaWaterLow = double.Parse(tb_temperaturaLow.Text.Replace('.',','));
                    AllSettings.temperaturaWaterHigh = double.Parse(tb_temperaturaHigh.Text.Replace('.',','));
                    AllSettings.provodimostIshodWaterLow = double.Parse(tb_provodimostIshodWaterLow.Text.Replace('.', ','));
                    AllSettings.provodimostIshodWaterHigh = double.Parse(tb_provodimostIshodWaterHigh.Text.Replace('.', ','));
                    AllSettings.pHIshodWaterAfterCorrectLow = double.Parse(tb_pHIshodWaterAfterCorrectLow.Text.Replace('.', ','));
                    AllSettings.pHIshodWaterAfterCorrectHigh = double.Parse(tb_pHIshodWaterAfterCorrectHigh.Text.Replace('.', ','));
                    AllSettings.provodimostPermeataLow = double.Parse(tb_provodimostPermeataLow.Text.Replace('.', ','));
                    AllSettings.provodimostPermeataHigh = double.Parse(tb_provodimostPermeataHigh.Text.Replace('.', ','));
                    AllSettings.pHPermeataLow = double.Parse(tb_pHPermeataLow.Text.Replace('.', ','));
                    AllSettings.pHPermeataHigh = double.Parse(tb_pHPermeataHigh.Text.Replace('.', ','));
                    AllSettings.davlenieNaUstanovky = double.Parse(tb_davlenieNaUstanovky.Text.Replace('.', ','));
                    #endregion

                    #region Экономика
                    AllSettings.stoimostIshodWater = int.Parse(tb_stoimostIshodWater.Text.Replace(",", "").Replace(".", ""));
                    AllSettings.stoimostUtilConcentrata = int.Parse(tb_stoimostUtilConcentrata.Text.Replace(",", "").Replace(".", ""));
                    AllSettings.stoimostAntiskalanta = int.Parse(tb_stoimostAntiskalanta.Text.Replace(",", "").Replace(".", ""));
                    AllSettings.stoimostMetoBisulvitaNatriya = int.Parse(tb_stoimostMetoBisulvitaNatriya.Text.Replace(",", "").Replace(".", ""));
                    AllSettings.stoimostElectroEnergy = int.Parse(tb_stoimostElectroEnergy.Text.Replace(",", "").Replace(".", ""));
                    #endregion

                    using (NpgsqlConnection con = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
                    {
                        try
                        {
                            con.Open();
                            if (con.State == ConnectionState.Open)
                            {
                                NpgsqlCommand command = new NpgsqlCommand() { Connection = con };
                                #region Предфильтр
                                string query = $"UPDATE Settings SET setting_val = {tb_perepadSredZagrFiltra.Text.Replace(',', '.')} WHERE setting_id = 1";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_perebadSilnoZagrFiltra.Text.Replace(',', '.')} WHERE setting_id = 2";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {AllSettings.timeOut} WHERE setting_id = 3";
                                command.CommandText = query;
                                command.ExecuteNonQuery();
                                #endregion

                                #region Мембраны
                                /*--------------RO1--------------------*/
                                query = $"UPDATE Settings SET setting_val = {tb_EnterPressureRO1.Text.Replace(',', '.')} " +
                                    $"WHERE setting_id = 4";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_TimeDelayRO1.Text} " +
                                    $"WHERE setting_id = 5";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_procentNizkoyProisvoditelnostiRO1.Text.Replace(',', '.')} " +
                                    $"WHERE setting_id = 6";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_proizvoditelnostRO1.Text.Replace(',', '.')} " +
                                    $"WHERE setting_id = 22";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_timeResetRO1.Text} " +
                                    $"WHERE setting_id = 28";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_timeWhenNeedPromivkaRO1.Text} " +
                                    $"WHERE setting_id = 34";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                /*--------------RO2--------------------*/
                                query = $"UPDATE Settings SET setting_val = {tb_EnterPressureRO2.Text.Replace(',', '.')} " +
                                    $"WHERE setting_id = 7";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_TimeDelayRO2.Text} " +
                                    $"WHERE setting_id = 8";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_procentNizkoyProisvoditelnostiRO2.Text.Replace(',', '.')} " +
                                    $"WHERE setting_id = 9";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_proizvoditelnostRO2.Text.Replace(',', '.')} " +
                                   $"WHERE setting_id = 23";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_timeResetRO2.Text} " +
                                   $"WHERE setting_id = 29";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_timeWhenNeedPromivkaRO2.Text} " +
                                    $"WHERE setting_id = 35";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                /*--------------RO3--------------------*/
                                query = $"UPDATE Settings SET setting_val = {tb_EnterPressureRO3.Text.Replace(',', '.')} " +
                                    $"WHERE setting_id = 10";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_TimeDelayRO3.Text} " +
                                    $"WHERE setting_id = 11";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_procentNizkoyProisvoditelnostiRO3.Text.Replace(',', '.')} " +
                                    $"WHERE setting_id = 12";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_proizvoditelnostRO3.Text.Replace(',', '.')} " +
                                   $"WHERE setting_id = 24";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_timeResetRO3.Text} " +
                                  $"WHERE setting_id = 30";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_timeWhenNeedPromivkaRO3.Text} " +
                                   $"WHERE setting_id = 36";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                /*--------------RO4--------------------*/
                                query = $"UPDATE Settings SET setting_val = {tb_EnterPressureRO4.Text.Replace(',', '.')} " +
                                    $"WHERE setting_id = 13";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_TimeDelayRO4.Text} " +
                                    $"WHERE setting_id = 14";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_procentNizkoyProisvoditelnostiRO4.Text.Replace(',', '.')} " +
                                    $"WHERE setting_id = 15";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_proizvoditelnostRO4.Text.Replace(',', '.')} " +
                                   $"WHERE setting_id = 25";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_timeResetRO4.Text} " +
                                  $"WHERE setting_id = 31";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_timeWhenNeedPromivkaRO4.Text} " +
                                   $"WHERE setting_id = 37";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                /*--------------RO5--------------------*/
                                query = $"UPDATE Settings SET setting_val = {tb_EnterPressureRO5.Text.Replace(',', '.')} " +
                                    $"WHERE setting_id = 16";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_TimeDelayRO5.Text} " +
                                    $"WHERE setting_id = 17";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_procentNizkoyProisvoditelnostiRO5.Text.Replace(',', '.')} " +
                                    $"WHERE setting_id = 18";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_proizvoditelnostRO5.Text.Replace(',', '.')} " +
                                   $"WHERE setting_id = 26";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_timeResetRO5.Text} " +
                                  $"WHERE setting_id = 32";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_timeWhenNeedPromivkaRO5.Text} " +
                                  $"WHERE setting_id = 38";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                /*--------------RO6--------------------*/
                                query = $"UPDATE Settings SET setting_val = {tb_EnterPressureRO6.Text.Replace(',', '.')} " +
                                    $"WHERE setting_id = 19";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_TimeDelayRO6.Text} " +
                                    $"WHERE setting_id = 20";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_procentNizkoyProisvoditelnostiRO6.Text.Replace(',', '.')} " +
                                    $"WHERE setting_id = 21";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_proizvoditelnostRO6.Text.Replace(',', '.')} " +
                                   $"WHERE setting_id = 27";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_timeResetRO6.Text} " +
                                  $"WHERE setting_id = 33";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_timeWhenNeedPromivkaRO6.Text} " +
                                  $"WHERE setting_id = 39";
                                command.CommandText = query;
                                command.ExecuteNonQuery();
                                #endregion

                                #region Качество воды
                                query = $"UPDATE Settings SET setting_val = {tb_temperaturaLow.Text} " +
                                  $"WHERE setting_id = 40";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_temperaturaHigh.Text} " +
                                 $"WHERE setting_id = 41";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_provodimostIshodWaterLow.Text} " +
                                 $"WHERE setting_id = 42";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_provodimostIshodWaterHigh.Text} " +
                                 $"WHERE setting_id = 43";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_pHIshodWaterAfterCorrectLow.Text} " +
                                 $"WHERE setting_id = 44";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_pHIshodWaterAfterCorrectHigh.Text} " +
                                 $"WHERE setting_id = 45";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_provodimostPermeataLow.Text} " +
                                 $"WHERE setting_id = 46";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_provodimostPermeataHigh.Text} " +
                                 $"WHERE setting_id = 47";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_pHPermeataLow.Text} " +
                                 $"WHERE setting_id = 48";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_pHPermeataHigh.Text} " +
                                 $"WHERE setting_id = 49";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_davlenieNaUstanovky.Text} " +
                                 $"WHERE setting_id = 50";
                                command.CommandText = query;
                                command.ExecuteNonQuery();
                                #endregion

                                #region Экономика
                                query = $"UPDATE Settings SET setting_val = {tb_stoimostIshodWater.Text} " +
                                  $"WHERE setting_id = 51";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_stoimostUtilConcentrata.Text} " +
                                  $"WHERE setting_id = 52";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_stoimostAntiskalanta.Text} " +
                                  $"WHERE setting_id = 53";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_stoimostMetoBisulvitaNatriya.Text} " +
                                  $"WHERE setting_id = 54";
                                command.CommandText = query;
                                command.ExecuteNonQuery();

                                query = $"UPDATE Settings SET setting_val = {tb_stoimostElectroEnergy.Text} " +
                                  $"WHERE setting_id = 55";
                                command.CommandText = query;
                                command.ExecuteNonQuery();
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            LogClass.Write("FormSettings:btn_closeForm_Click::" + ex.Message);
                        }
                    }
                }
            }
            this.Close();
        }
        private void tb_change_text (object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;

            if (_isChangeSetting == false)
                _isChangeSetting = true;

            Match match = _regex.Match(textBox.Text);
            if (!match.Success)
            {
                if (textBox.Text.Length > 0)
                {
                    textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1, 1);
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }
        private void tb_leave_textBox (object sender, EventArgs e)
        {
            TextBox temp = (TextBox)sender;
            temp.Text = temp.Text.TrimEnd(',', '.');
        }
        private void FormSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.I)
                    if (panelDeveloper.Visible) panelDeveloper.Visible = false;
                    else panelDeveloper.Visible = true;
        }
        private void btn_resetNeedChangeMembran_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Сброс 'рекомендуется замена мембраны'", "Уведомление", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CleverOsmos"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        NpgsqlCommand command = new NpgsqlCommand
                        {
                            Connection = connection,
                            CommandText = "UPDATE journal_messages " +
                            $"SET is_active = 'false', date_end = '{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}' " +
                            $"WHERE itemid IN(1,2,3,4,5,6) AND is_active = true"
                        };

                        int count = command.ExecuteNonQuery();

                        MainForm._countError -= count;
                        if (MainForm._countError < 0)
                            MainForm._countError = 0;

                        label.Text = MainForm._countError.ToString();
                        MainForm.journalMessage.UpdateError();
                    }
                }
            }
        }
        private void btn_reset_membran(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (MessageBox.Show($"Сброс требуется промывка мембраны по линии {btn.Tag}", "Уведомление", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                MembranErrorClass item;
                switch (btn.Tag)
                {
                    case "RO1":
                        item = SearchProblemMembran.listMembranError.Where(x => x.Name == "RO1").FirstOrDefault();
                        if (item != null)
                        {
                            Membran membran = SearchProblemMembran._membranItems.Where(x => x.Name == "RO1").FirstOrDefault();
                            if (membran != null)
                            {
                                membran.isFirstError = false;
                                membran.sumTimeError = 0;
                                membran.ChangeStatusProblemDataBase();
                                membran.RemoveErrorInList();
                            }
                        }
                        break;
                    case "RO2":
                        item = SearchProblemMembran.listMembranError.Where(x => x.Name == "RO2").FirstOrDefault();
                        if (item != null)
                        {
                            Membran membran = SearchProblemMembran._membranItems.Where(x => x.Name == "RO2").FirstOrDefault();
                            if (membran != null)
                            {
                                membran.isFirstError = false;
                                membran.sumTimeError = 0;
                                membran.ChangeStatusProblemDataBase();
                                membran.RemoveErrorInList();
                            }
                        }
                        break;
                    case "RO3":
                        item = SearchProblemMembran.listMembranError.Where(x => x.Name == "RO3").FirstOrDefault();
                        if (item != null)
                        {
                            Membran membran = SearchProblemMembran._membranItems.Where(x => x.Name == "RO3").FirstOrDefault();
                            if (membran != null)
                            {
                                membran.isFirstError = false;
                                membran.sumTimeError = 0;
                                membran.ChangeStatusProblemDataBase();
                                membran.RemoveErrorInList();
                            }
                        }
                        break;
                    case "RO4":
                        item = SearchProblemMembran.listMembranError.Where(x => x.Name == "RO4").FirstOrDefault();
                        if (item != null)
                        {
                            Membran membran = SearchProblemMembran._membranItems.Where(x => x.Name == "RO4").FirstOrDefault();
                            if (membran != null)
                            {
                                membran.isFirstError = false;
                                membran.sumTimeError = 0;
                                membran.ChangeStatusProblemDataBase();
                                membran.RemoveErrorInList();
                            }
                        }
                        break;
                    case "RO5":
                        item = SearchProblemMembran.listMembranError.Where(x => x.Name == "RO5").FirstOrDefault();
                        if (item != null)
                        {
                            Membran membran = SearchProblemMembran._membranItems.Where(x => x.Name == "RO5").FirstOrDefault();
                            if (membran != null)
                            {
                                membran.isFirstError = false;
                                membran.sumTimeError = 0;
                                membran.ChangeStatusProblemDataBase();
                                membran.RemoveErrorInList();
                            }
                        }
                        break;
                    case "RO6":
                        item = SearchProblemMembran.listMembranError.Where(x => x.Name == "RO6").FirstOrDefault();
                        if (item != null)
                        {
                            Membran membran = SearchProblemMembran._membranItems.Where(x => x.Name == "RO6").FirstOrDefault();
                            if (membran != null)
                            {
                                membran.isFirstError = false;
                                membran.sumTimeError = 0;
                                membran.ChangeStatusProblemDataBase();
                                membran.RemoveErrorInList();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void btn_refreshTime_Click(object sender, EventArgs e)
        {
            lb_time1.Text = SearchProblemMembran._membranItems[0].StopwatchTimeError.Elapsed.ToString();
            lb_time2.Text = SearchProblemMembran._membranItems[1].StopwatchTimeError.Elapsed.ToString();
            lb_time3.Text = SearchProblemMembran._membranItems[2].StopwatchTimeError.Elapsed.ToString();
            lb_time4.Text = SearchProblemMembran._membranItems[3].StopwatchTimeError.Elapsed.ToString();
            lb_time5.Text = SearchProblemMembran._membranItems[4].StopwatchTimeError.Elapsed.ToString();
            lb_time6.Text = SearchProblemMembran._membranItems[5].StopwatchTimeError.Elapsed.ToString();

            lb_errorIsActive1.Text = SearchProblemMembran._membranItems[0].IsProblem.ToString();
            lb_errorIsActive2.Text = SearchProblemMembran._membranItems[1].IsProblem.ToString();
            lb_errorIsActive3.Text = SearchProblemMembran._membranItems[2].IsProblem.ToString();
            lb_errorIsActive4.Text = SearchProblemMembran._membranItems[3].IsProblem.ToString();
            lb_errorIsActive5.Text = SearchProblemMembran._membranItems[4].IsProblem.ToString();
            lb_errorIsActive6.Text = SearchProblemMembran._membranItems[5].IsProblem.ToString();

            lb_timeReset1.Text = SearchProblemMembran._membranItems[0].StopwatchTimeReser.Elapsed.ToString();
            lb_timeReset2.Text = SearchProblemMembran._membranItems[1].StopwatchTimeReser.Elapsed.ToString();
            lb_timeReset3.Text = SearchProblemMembran._membranItems[2].StopwatchTimeReser.Elapsed.ToString();
            lb_timeReset4.Text = SearchProblemMembran._membranItems[3].StopwatchTimeReser.Elapsed.ToString();
            lb_timeReset5.Text = SearchProblemMembran._membranItems[4].StopwatchTimeReser.Elapsed.ToString();
            lb_timeReset6.Text = SearchProblemMembran._membranItems[5].StopwatchTimeReser.Elapsed.ToString();
        }
    }
}
