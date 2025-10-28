namespace TarjetaSube
{
    public class MedioBoletoEstudiantil : Tarjeta
    {
        public MedioBoletoEstudiantil(int saldoInicial = 0) : base(saldoInicial)
        {
        }

        public override int CalcularMontoPasaje(int tarifaBase)
        {
            return tarifaBase / 2;
        }
    }
}