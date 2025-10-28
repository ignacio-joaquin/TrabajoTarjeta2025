using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class TarjetaTestsIteracion2
    {
        [Test]
        public void TestSaldoNegativoPermitido()
        {
            Tarjeta tarjeta = new Tarjeta(1580); // Saldo exacto para un viaje
            Colectivo colectivo = new Colectivo("K");
            
            // Primer viaje - normal
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(0, tarjeta.Saldo);
            
            // Segundo viaje - primer Plus
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(-1580, tarjeta.Saldo);
            Assert.AreEqual(1, tarjeta.ViajesPlus);
            
            // Tercer viaje - segundo Plus
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(-3160, tarjeta.Saldo);
            Assert.AreEqual(2, tarjeta.ViajesPlus);
            
            // Cuarto viaje - no permitido (límite excedido)
            Boleto boleto4 = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto4);
        }

        [Test]
        public void TestCargarConSaldoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta(1580);
            Colectivo colectivo = new Colectivo("K");
            
            // Hacer dos viajes Plus
            colectivo.PagarCon(tarjeta); // Saldo a 0
            colectivo.PagarCon(tarjeta); // Primer Plus: -1580
            colectivo.PagarCon(tarjeta); // Segundo Plus: -3160
            
            Assert.AreEqual(-3160, tarjeta.Saldo);
            Assert.AreEqual(2, tarjeta.ViajesPlus);
            
            // Cargar tarjeta - debe descontar viajes Plus primero
            bool cargaExitosa = tarjeta.Cargar(5000);
            Assert.IsTrue(cargaExitosa);
            
            // 5000 - (2 * 1580) = 5000 - 3160 = 1840
            Assert.AreEqual(1840, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.ViajesPlus);
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
        public void TestViajesPlusConFranquicia()
        {
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(500);
            Colectivo colectivo = new Colectivo("K");
            
            // Primer viaje - medio boleto con Plus (500 < 790)
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(790, boleto1.Monto);
            Assert.AreEqual(-290, tarjeta.Saldo);
            Assert.AreEqual(1, tarjeta.ViajesPlus);
        }

        [Test]
        public void TestNoPermitirMasDeDosViajesPlus()
        {
            Tarjeta tarjeta = new Tarjeta(1580);
            Colectivo colectivo = new Colectivo("K");
            
            // Consumir saldo normal
            colectivo.PagarCon(tarjeta); // Saldo: 0
            
            // Dos viajes Plus
            colectivo.PagarCon(tarjeta); // Plus 1: -1580
            colectivo.PagarCon(tarjeta); // Plus 2: -3160
            
            // Tercer viaje Plus - no permitido
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto);
            Assert.AreEqual(-3160, tarjeta.Saldo);
            Assert.AreEqual(2, tarjeta.ViajesPlus);
        }
    }
}