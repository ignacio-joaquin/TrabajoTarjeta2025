using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class TarjetaTests
    {
        [Test]
        public void TestConstructorSaldoInicial()
        {
            Tarjeta tarjeta = new Tarjeta(5000);
            Assert.AreEqual(5000, tarjeta.Saldo);
        }

        [Test]
        public void TestConstructorSinParametros()
        {
            Tarjeta tarjeta = new Tarjeta();
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarMonto2000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(2000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarMonto3000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(3000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(3000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarMonto4000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(4000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(4000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarMonto5000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(5000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(5000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarMonto8000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(8000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(8000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarMonto10000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(10000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(10000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarMonto15000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(15000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(15000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarMonto20000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(20000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(20000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarMonto25000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(25000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(25000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarMonto30000()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(30000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(30000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarMontoNoAceptado()
        {
            Tarjeta tarjeta = new Tarjeta();
            bool resultado = tarjeta.Cargar(1000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarSuperaLimite()
        {
            Tarjeta tarjeta = new Tarjeta(35000);
            bool resultado = tarjeta.Cargar(10000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(35000, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarHastaLimite()
        {
            Tarjeta tarjeta = new Tarjeta(30000);
            bool resultado = tarjeta.Cargar(10000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(40000, tarjeta.Saldo);
        }

        [Test]
        public void TestDescontarConSaldoSuficiente()
        {
            Tarjeta tarjeta = new Tarjeta(5000);
            bool resultado = tarjeta.Descontar(1580);
            Assert.IsTrue(resultado);
            Assert.AreEqual(3420, tarjeta.Saldo);
        }

        [Test]
        public void TestDescontarSinSaldoSuficiente()
        {
            Tarjeta tarjeta = new Tarjeta(1000);
            bool resultado = tarjeta.Descontar(1580);
            Assert.IsFalse(resultado);
            Assert.AreEqual(1000, tarjeta.Saldo);
        }
    }
        [TestFixture]
    public class TarjetaTestsIteracion2
    {
        [Test]
        public void TestSaldoNegativoPermitido()
        {
            Tarjeta tarjeta = new Tarjeta(1000);
            Colectivo colectivo = new Colectivo("K");
            
            // Primer viaje - queda en negativo
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(-580, tarjeta.Saldo);
            Assert.AreEqual(1, tarjeta.ViajesPlus);
            
            // Segundo viaje - sigue en negativo
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(-2160, tarjeta.Saldo);
            Assert.AreEqual(2, tarjeta.ViajesPlus);
            
            // Tercer viaje - no permitido (límite negativo)
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto3);
            Assert.AreEqual(-2160, tarjeta.Saldo);
        }

        [Test]
        public void TestCargarConSaldoNegativo()
        {
            Tarjeta tarjeta = new Tarjeta(1000);
            Colectivo colectivo = new Colectivo("K");
            
            // Hacer dos viajes Plus
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
            
            Assert.AreEqual(-2160, tarjeta.Saldo);
            Assert.AreEqual(2, tarjeta.ViajesPlus);
            
            // Cargar tarjeta - debe descontar viajes Plus primero
            bool cargaExitosa = tarjeta.Cargar(5000);
            Assert.IsTrue(cargaExitosa);
            Assert.AreEqual(500, tarjeta.Saldo); // 5000 - 1580*2 - 2160 = 500
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
            
            // Primer viaje - medio boleto normal
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(790, boleto1.Monto);
            
            // Segundo viaje - medio boleto con Plus
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(790, boleto2.Monto);
            Assert.AreEqual(1, tarjeta.ViajesPlus);
        }
    }
}