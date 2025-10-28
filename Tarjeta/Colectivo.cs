using System;

namespace TarjetaSube
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

            int montoAPagar = tarjeta.CalcularMontoPasaje(TARIFA_BASICA);
            
            if (!tarjeta.Descontar(montoAPagar))
            {
                return null;
            }

            return new Boleto(montoAPagar, this.linea, tarjeta.Saldo);
        }
    }
}