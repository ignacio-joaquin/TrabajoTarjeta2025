using System;

namespace Tarjeta
{
    public class ViajeReciente
    {
        public DateTime Fecha { get; set; }
        public string Linea { get; set; }
        public int Monto { get; set; }

        public ViajeReciente(DateTime fecha, string linea, int monto)
        {
            Fecha = fecha;
            Linea = linea;
            Monto = monto;
        }

        public bool EsValidoParaTrasbordo(DateTime ahora)
        {
            TimeSpan tiempoTranscurrido = ahora - Fecha;
            return tiempoTranscurrido.TotalMinutes <= 60;
        }
    }
}
