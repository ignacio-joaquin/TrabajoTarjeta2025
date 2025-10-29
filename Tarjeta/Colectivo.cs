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

            bool tuvoRecargo;
            int montoAPagar = tarjeta.CalcularMontoRealAPagar(TARIFA_BASICA, out tuvoRecargo);
            int saldoAnterior = tarjeta.Saldo;
            
            // Si hay saldo negativo y el monto a pagar es mayor que el saldo disponible,
            // calculamos el monto total abonado (incluyendo la deuda)
            int montoTotalAbonado = montoAPagar;
            if (saldoAnterior < 0 && montoAPagar > 0)
            {
                montoTotalAbonado = montoAPagar - saldoAnterior; // Restamos porque saldoAnterior es negativo
            }

            if (!tarjeta.Descontar(montoAPagar))
            {
                return null;
            }

            return new Boleto(montoAPagar, this.linea, tarjeta.Saldo, 
                            tarjeta.TipoTarjeta, tarjeta.Id, montoTotalAbonado, tuvoRecargo);
        }
    }
}