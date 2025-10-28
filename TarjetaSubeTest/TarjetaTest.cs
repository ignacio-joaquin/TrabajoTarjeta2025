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

        // ... (mantener todos los tests existentes de carga)

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
            Assert.IsFalse(resultado); // Debe ser false - sin saldo suficiente
            Assert.AreEqual(1000, tarjeta.Saldo);
        }
    }
}