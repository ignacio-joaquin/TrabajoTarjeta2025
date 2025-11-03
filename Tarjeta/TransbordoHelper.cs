using System;

namespace Tarjeta
{
    public static class TrasbordoHelper
    {
        public static bool EstaEnFranjaHorariaTrasbordo(DateTime fecha)
        {
            // Lunes a sábado de 7:00 a 22:00
            return EsDiaHabilTrasbordo(fecha) && EstaEnHorarioPermitidoTrasbordo(fecha);
        }

        public static bool EsDiaHabilTrasbordo(DateTime fecha)
        {
            // Lunes = 1, Sábado = 6
            return fecha.DayOfWeek >= DayOfWeek.Monday && fecha.DayOfWeek <= DayOfWeek.Saturday;
        }

        public static bool EstaEnHorarioPermitidoTrasbordo(DateTime fecha)
        {
            // De 7:00 a 22:00 (incluyendo 7:00 pero excluyendo 22:00)
            return fecha.Hour >= 7 && fecha.Hour < 22;
        }

        public static string GetFranjaHorariaTrasbordo()
        {
            return "Lunes a Sábado de 7:00 a 22:00";
        }
    }
}
