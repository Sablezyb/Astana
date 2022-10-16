using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astana
{
    public static class AllSettings
    {
        #region Предфильтр
        public static double perepadSredZagrFiltra;
        public static double perepadSilnoZagrFiltra;
        public static int timeOut;
        #endregion

        #region Мембрана
        //Указывается в часас, сброс таймера до необходимости промывки
        public static int timeResetRO1;
        public static int timeResetRO2;
        public static int timeResetRO3;
        public static int timeResetRO4;
        public static int timeResetRO5;
        public static int timeResetRO6;

        //Указывается в часас, таймер до необходимости промывки
        public static int timeWhenNeedPromivkaRO1;
        public static int timeWhenNeedPromivkaRO2;
        public static int timeWhenNeedPromivkaRO3;
        public static int timeWhenNeedPromivkaRO4;
        public static int timeWhenNeedPromivkaRO5;
        public static int timeWhenNeedPromivkaRO6;

        public static double enterPressureRo1;
        public static double enterPressureRo2;
        public static double enterPressureRo3;
        public static double enterPressureRo4;
        public static double enterPressureRo5;
        public static double enterPressureRo6;

        public static int timeDelayRo1;
        public static int timeDelayRo2;
        public static int timeDelayRo3;
        public static int timeDelayRo4;
        public static int timeDelayRo5;
        public static int timeDelayRo6;

        public static double procentNizkoyProizvoditelnostiRo1;
        public static double procentNizkoyProizvoditelnostiRo2;
        public static double procentNizkoyProizvoditelnostiRo3;
        public static double procentNizkoyProizvoditelnostiRo4;
        public static double procentNizkoyProizvoditelnostiRo5;
        public static double procentNizkoyProizvoditelnostiRo6;

        public static double proizvoditelnostRO1;
        public static double proizvoditelnostRO2;
        public static double proizvoditelnostRO3;
        public static double proizvoditelnostRO4;
        public static double proizvoditelnostRO5;
        public static double proizvoditelnostRO6;
        #endregion

        #region Качество воды
        public static double temperaturaWaterLow;
        public static double temperaturaWaterHigh;
        public static double provodimostIshodWaterLow;
        public static double provodimostIshodWaterHigh;
        public static double pHIshodWaterAfterCorrectLow;
        public static double pHIshodWaterAfterCorrectHigh;
        public static double provodimostPermeataLow;
        public static double provodimostPermeataHigh;
        public static double pHPermeataLow;
        public static double pHPermeataHigh;
        public static double davlenieNaUstanovky;
        #endregion

        #region Экономика
        public static int stoimostIshodWater;
        public static int stoimostUtilConcentrata;
        public static int stoimostAntiskalanta;
        public static int stoimostMetoBisulvitaNatriya;
        public static int stoimostElectroEnergy;
        #endregion
    }
}
