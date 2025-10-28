using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class ColectivoTests
    {
        [Test]
        public void TestConstructorConLinea()
        {
            Colectivo colectivo = new Colectivo("K");
            Assert.AreEqual("K", colectivo.Linea);
        }

        [Test]
        public void TestPagarConTarjetaConSaldo()
        {
            Colectivo colectivo = new Colectivo("142");
            Tarjeta tarjeta = new Tarjeta(5000);
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(1580, boleto.Monto);
            Assert.AreEqual("142", boleto.Linea);
            Assert.AreEqual(3420, tarjeta.Saldo);
            Assert.AreEqual(3420, boleto.SaldoRestante);
        }

        [Test]
        public void TestPagarConTarjetaSinSaldo()
        {
            Colectivo colectivo = new Colectivo("K");
            Tarjeta tarjeta = new Tarjeta(1000);
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNull(boleto); // Debe ser null porque no hay saldo suficiente
            Assert.AreEqual(1000, tarjeta.Saldo);
        }

        [Test]
        public void TestPagarConTarjetaNull()
        {
            Colectivo colectivo = new Colectivo("K");
            
            Boleto boleto = colectivo.PagarCon(null);
            
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestPagarConSaldoJusto()
        {
            Colectivo colectivo = new Colectivo("144");
            Tarjeta tarjeta = new Tarjeta(1580);
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, tarjeta.Saldo);
            Assert.AreEqual(0, boleto.SaldoRestante);
        }
    }
}