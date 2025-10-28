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

            if (!tarjeta.Descontar(TARIFA_BASICA))
            {
                return null;
            }

            return new Boleto(TARIFA_BASICA, this.linea, tarjeta.Saldo);
        }
    }
}