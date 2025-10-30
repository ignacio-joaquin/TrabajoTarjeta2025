namespace Tarjeta
{
    public class FranquiciaCompleta : Tarjeta
    {
        public FranquiciaCompleta(int saldoInicial = 0) : base(saldoInicial) { }
        public FranquiciaCompleta(int saldoInicial, string id) : base(saldoInicial, id) { }

        public override string TipoTarjeta
        {
            get { return "Franquicia Completa"; }
        }

        public override bool Descontar(int monto)
        {
            // Viaje gratuito - siempre permite el viaje
            // Pero aún así llamamos a base.Descontar para que acredite saldo pendiente si existe
            base.Descontar(0);
            return true;
        }

        public override int CalcularMontoPasaje(int tarifaBase)
        {
            return 0;
        }

        public override int CalcularMontoRealAPagar(int tarifaBase)
        {

            return 0;
        }
    }
}