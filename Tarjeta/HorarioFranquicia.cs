using System;

namespace Tarjeta
{
    public static class HorarioFranquicia
    {
        public static bool EstaEnFranjaHorariaPermitida(DateTime fecha)
        {
            // Lunes a viernes de 6 a 22
            return EsDiaHabil(fecha) && EstaEnHorarioPermitido(fecha);
        }

        public static bool EsDiaHabil(DateTime fecha)
        {
            // Lunes = 1, Viernes = 5
            return fecha.DayOfWeek >= DayOfWeek.Monday && fecha.DayOfWeek <= DayOfWeek.Friday;
        }

        public static bool EstaEnHorarioPermitido(DateTime fecha)
        {
            // De 6:00 a 22:00 (incluyendo 6:00 pero excluyendo 22:00)
            return fecha.Hour >= 6 && fecha.Hour < 22;
        }

        public static string GetFranjaHorariaPermitida()
        {
            return "Lunes a Viernes de 6:00 a 22:00";
        }
    }
}