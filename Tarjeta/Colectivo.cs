using System;

namespace Tarjeta
{
    public class Colectivo
    {
        private const int TARIFA_BASICA = 1580;
        private string linea;

        public Colectivo(string linea)
        {
            this.linea = linea;
        }

        public string Linea
        {
            get { return linea; }
        }

        public Boleto PagarCon(Tarjeta tarjeta)
        {
            if (tarjeta == null)
            {
                return null;
            }

            // Validaciones específicas para Medio Boleto
            if (tarjeta is MedioBoletoEstudiantil medioBoleto)
            {
                if (!medioBoleto.PuedeViajar())
                {
                    return null; // No pasaron 5 minutos desde el último viaje
                }
            }

            bool tuvoRecargo;
            int montoAPagar = tarjeta.CalcularMontoRealAPagar(TARIFA_BASICA, out tuvoRecargo);
            int saldoAnterior = tarjeta.Saldo;
            
            int montoTotalAbonado = montoAPagar;
            if (saldoAnterior < 0 && montoAPagar > 0)
            {
                montoTotalAbonado = montoAPagar - saldoAnterior;
            }

            if (!tarjeta.Descontar(montoAPagar))
            {
                return null;
            }

            // Registrar el viaje para Medio Boleto
            if (tarjeta is MedioBoletoEstudiantil medioBoletoRegistro)
            {
                medioBoletoRegistro.RegistrarViaje();
            }

            // Registrar el viaje para Boleto Gratuito
            if (tarjeta is BoletoGratuitoEstudiantil boletoGratuitoRegistro)
            {
                boletoGratuitoRegistro.RegistrarViaje();
            }

            return new Boleto(montoAPagar, this.linea, tarjeta.Saldo, 
                            tarjeta.TipoTarjeta, tarjeta.Id, montoTotalAbonado, tuvoRecargo);
        }
    }
}