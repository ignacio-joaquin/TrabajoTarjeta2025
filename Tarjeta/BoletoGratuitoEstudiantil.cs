namespace Tarjeta
{
    public class BoletoGratuitoEstudiantil : Tarjeta
    {
        public BoletoGratuitoEstudiantil(int saldoInicial = 0) : base(saldoInicial)
        {
        }

        public override bool Descontar(int monto)
        {
            // Viaje gratuito - siempre permite el viaje
            return true;
        }

        public override int CalcularMontoPasaje(int tarifaBase)
        {
            return 0;
        }
    }
}