namespace Tarjeta
{
    public class MedioBoletoEstudiantil : Tarjeta
    {
        public MedioBoletoEstudiantil(int saldoInicial = 0) : base(saldoInicial) { }
        public MedioBoletoEstudiantil(int saldoInicial, string id) : base(saldoInicial, id) { }

        public override string TipoTarjeta
        {
            get { return "Medio Boleto Estudiantil"; }
        }

        public override int CalcularMontoPasaje(int tarifaBase)
        {
            return tarifaBase / 2;
        }

        public override int CalcularMontoRealAPagar(int tarifaBase, out bool tuvoRecargo)
        {
            tuvoRecargo = false;
            return tarifaBase / 2;
        }
    }
}