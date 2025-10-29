namespace Tarjeta
{
    public class BoletoGratuitoEstudiantil : Tarjeta
    {
        public BoletoGratuitoEstudiantil(int saldoInicial = 0) : base(saldoInicial) { }
        public BoletoGratuitoEstudiantil(int saldoInicial, string id) : base(saldoInicial, id) { }

        public override string TipoTarjeta
        {
            get { return "Boleto Gratuito Estudiantil"; }
        }

        public override bool Descontar(int monto)
        {
            return true;
        }

        public override int CalcularMontoPasaje(int tarifaBase)
        {
            return 0;
        }

        public override int CalcularMontoRealAPagar(int tarifaBase, out bool tuvoRecargo)
        {
            tuvoRecargo = false;
            return 0;
        }
    }
}