using System;
using System.Collections.Generic;
using System.Linq;

namespace Tarjeta
{
    public class MedioBoletoEstudiantil : Tarjeta
    {
        private List<DateTime> viajesDelDia;
        private DateTime? ultimoViaje;
        
        public MedioBoletoEstudiantil(int saldoInicial = 0) : base(saldoInicial) 
        {
            viajesDelDia = new List<DateTime>();
        }
        
        public MedioBoletoEstudiantil(int saldoInicial, string id) : base(saldoInicial, id) 
        {
            viajesDelDia = new List<DateTime>();
        }

        public override string TipoTarjeta
        {
            get { return "Medio Boleto Estudiantil"; }
        }

        public override int CalcularMontoPasaje(int tarifaBase)
        {
            LimpiarViajesAntiguos();
            
            if (viajesDelDia.Count >= 2)
            {
                return tarifaBase;
            }
            
            return tarifaBase / 2;
        }

        public override int CalcularMontoRealAPagar(int tarifaBase, out bool tuvoRecargo)
        {
            tuvoRecargo = false;
            return CalcularMontoPasaje(tarifaBase);
        }

        public bool PuedeViajar()
        {
            if (ultimoViaje.HasValue)
            {
                TimeSpan tiempoDesdeUltimoViaje = DateTimeProvider.Now - ultimoViaje.Value;
                if (tiempoDesdeUltimoViaje.TotalMinutes < 5)
                {
                    return false;
                }
            }
            
            return true;
        }

        public void RegistrarViaje()
        {
            DateTime ahora = DateTimeProvider.Now;
            viajesDelDia.Add(ahora);
            ultimoViaje = ahora;
        }

        public int ViajesHoy()
        {
            LimpiarViajesAntiguos();
            return viajesDelDia.Count;
        }

        private void LimpiarViajesAntiguos()
        {
            DateTime hoy = DateTimeProvider.Now.Date;
            viajesDelDia = viajesDelDia.Where(v => v.Date == hoy).ToList();
        }

        public DateTime? GetUltimoViaje()
        {
            return ultimoViaje;
        }

        public void SetViajesParaTesting(List<DateTime> viajes, DateTime? ultimoViaje = null)
        {
            this.viajesDelDia = viajes;
            this.ultimoViaje = ultimoViaje;
        }
    }
}