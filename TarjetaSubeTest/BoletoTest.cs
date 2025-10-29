using NUnit.Framework;
using Tarjeta;
using System;

namespace Tarjeta.Tests
{
    [TestFixture]
    public class BoletoTests
    {
        [Test]
        public void TestConstructorBoleto()
        {
            Boleto boleto = new Boleto(1580, "K", 3420);
            
            Assert.AreEqual(1580, boleto.Monto);
            Assert.AreEqual("K", boleto.Linea);
            Assert.AreEqual(3420, boleto.SaldoRestante);
        }

        [Test]
        public void TestFechaBoleto()
        {
            DateTime antes = DateTime.Now;
            Boleto boleto = new Boleto(1580, "142", 5000);
            DateTime despues = DateTime.Now;
            
            Assert.GreaterOrEqual(boleto.Fecha, antes);
            Assert.LessOrEqual(boleto.Fecha, despues);
        }

        [Test]
        public void TestMontoPropiedad()
        {
            Boleto boleto = new Boleto(1580, "K", 3420);
            Assert.AreEqual(1580, boleto.Monto);
        }

        [Test]
        public void TestLineaPropiedad()
        {
            Boleto boleto = new Boleto(1580, "144", 3420);
            Assert.AreEqual("144", boleto.Linea);
        }

        [Test]
        public void TestSaldoRestantePropiedad()
        {
            Boleto boleto = new Boleto(1580, "K", 8420);
            Assert.AreEqual(8420, boleto.SaldoRestante);
        }
    }
}