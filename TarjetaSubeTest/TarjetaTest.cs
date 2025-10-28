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
    }
    
}