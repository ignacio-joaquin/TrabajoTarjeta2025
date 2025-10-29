using NUnit.Framework;
using Tarjeta;

namespace Tarjeta.Tests
{
    [TestFixture]
    public class TarjetaTestsIteracion2
    {
        [Test]
        public void TestSaldoNegativoPermitidoHastaLimite()
        {
            Tarjeta tarjeta = new Tarjeta(1000); // Saldo menor a un viaje
            Colectivo colectivo = new Colectivo("K");
            
            // Este viaje debería permitirse: 1000 - 1580 = -580 (dentro del límite -1200)
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(-580, tarjeta.Saldo);
        }

        [Test]
        public void TestSaldoNegativoNoPermitidoSuperaLimite()
        {
            Tarjeta tarjeta = new Tarjeta(500); // Saldo muy bajo
            Colectivo colectivo = new Colectivo("K");
            
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.AreEqual(-1080, tarjeta.Saldo); 
        }

        [Test]
        public void TestCargarConSaldoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta(1000);
            Colectivo colectivo = new Colectivo("K");
            
            // Hacer un viaje que deje saldo negativo
            colectivo.PagarCon(tarjeta); // Saldo: 1000 - 1580 = -580
            
            Assert.AreEqual(-580, tarjeta.Saldo);
            
            // Cargar tarjeta con saldo negativo
            bool cargaExitosa = tarjeta.Cargar(2000);
            Assert.IsTrue(cargaExitosa);
            
            // 2000 - 580 = 1420
            Assert.AreEqual(1420, tarjeta.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaSiemprePuedePagar()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta(0);
            Colectivo colectivo = new Colectivo("142");
            
            // Debe poder pagar incluso con saldo 0
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto); // Viaje gratuito
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoPagaMitad()
        {
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(1000);
            Colectivo colectivo = new Colectivo("K");
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(790, boleto.Monto); // 1580 / 2 = 790
            Assert.AreEqual(210, tarjeta.Saldo); // 1000 - 790 = 210
        }

        [Test]
        public void TestMedioBoletoConSaldoNegativoPermitido()
        {
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(500);
            Colectivo colectivo = new Colectivo("K");
            
            // Medio boleto: 790, Saldo: 500 - 790 = -290 (DENTRO del límite -1200)
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(790, boleto.Monto);
            Assert.AreEqual(-290, tarjeta.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoEstudiantil()
        {
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(0);
            Colectivo colectivo = new Colectivo("144");
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestLimiteExactoSaldoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta(380); // 380 - 1580 = -1200 (límite exacto)
            Colectivo colectivo = new Colectivo("K");
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(-1200, tarjeta.Saldo);
        }

        [Test]
        public void TestSuperaLimiteSaldoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta(379); // 379 - 1580 = -1201 (supera límite)
            Colectivo colectivo = new Colectivo("K");
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNull(boleto);
            Assert.AreEqual(379, tarjeta.Saldo);
        }
    }
}