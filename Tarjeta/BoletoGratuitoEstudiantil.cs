using System;
using System.Collections.Generic;
using System.Linq;

namespace Tarjeta
{
    public class BoletoGratuitoEstudiantil : Tarjeta
    {
        private List<DateTime> viajesDelDia;
        
        public BoletoGratuitoEstudiantil(int saldoInicial = 0) : base(saldoInicial) 
        {
            viajesDelDia = new List<DateTime>();
        }
        
        public BoletoGratuitoEstudiantil(int saldoInicial, string id) : base(saldoInicial, id) 
        {
            viajesDelDia = new List<DateTime>();
        }

        public override string TipoTarjeta
        {
            get { return "Boleto Gratuito Estudiantil"; }
        }

        public bool EstaEnFranjaHorariaPermitida()
        {
            return HorarioFranquicia.EstaEnFranjaHorariaPermitida(DateTimeProvider.Now);
        }

        public override bool Descontar(int monto)
        {
            // Para viajes gratuitos siempre permite el viaje
            // Para viajes pagos verifica el saldo
            if (monto == 0)
            {
                return true;
            }
            
            bool resultado = base.Descontar(monto);
            return resultado;
        }

        public override int CalcularMontoPasaje(int tarifaBase)
        {
            LimpiarViajesAntiguos();
            
            // Primeros dos viajes del día son gratuitos
            if (viajesDelDia.Count < 2)
            {
                return 0;
            }
            
            // A partir del tercer viaje, tarifa completa
            return tarifaBase;
        }

        public override int CalcularMontoRealAPagar(int tarifaBase)
        {
            return CalcularMontoPasaje(tarifaBase);
        }

        public new void RegistrarViaje()
        {
            DateTime ahora = DateTimeProvider.Now;
            viajesDelDia.Add(ahora);
            
            // También llamar al método base para registrar en el contador de uso frecuente
            base.RegistrarViaje();
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

        // Método para testing - permite establecer viajes específicos
        public void SetViajesParaTesting(List<DateTime> viajes)
        {
            this.viajesDelDia = viajes;
        }
    }
}