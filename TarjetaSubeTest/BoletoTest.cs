
using NUnit.Framework;
using Tarjeta;
using System;

namespace Tarjeta.Tests
{
    [TestFixture]
    public class BoletoCompletoTests
    {
        [SetUp]
        public void Setup()
        {
            DateTimeProvider.ResetToDefault();
        }

        [TearDown]
        public void TearDown()
        {
            DateTimeProvider.ResetToDefault();
        }

        [Test]
        public void TestBoletoTarjetaNormal()
        {
            Tarjeta tarjeta = new Tarjeta(5000, "TEST001");
            Colectivo colectivo = new Colectivo("142");
            
            // Usar fecha dentro de franja horaria
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 0, 0));
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(1580, boleto.Monto);
            Assert.AreEqual("142", boleto.Linea);
            Assert.AreEqual(3420, boleto.SaldoRestante);
            Assert.AreEqual("Normal", boleto.TipoTarjeta);
            Assert.AreEqual("TEST001", boleto.IdTarjeta);
            Assert.AreEqual(1580, boleto.MontoTotalAbonado);
        }

        [Test]
        public void TestBoletoFranquiciaCompleta()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta(0, "FC001");
            Colectivo colectivo = new Colectivo("K");
            
            // Usar fecha dentro de franja horaria
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 0, 0));
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
            Assert.AreEqual("K", boleto.Linea);
            Assert.AreEqual(0, boleto.SaldoRestante);
            Assert.AreEqual("Franquicia Completa", boleto.TipoTarjeta);
            Assert.AreEqual("FC001", boleto.IdTarjeta);
            Assert.AreEqual(0, boleto.MontoTotalAbonado);
        }

        [Test]
        public void TestBoletoMedioBoletoEstudiantil()
        {
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(1000, "MB001");
            Colectivo colectivo = new Colectivo("144");
            
            // Usar fecha dentro de franja horaria
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 0, 0));
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(790, boleto.Monto); // 1580 / 2 = 790
            Assert.AreEqual("144", boleto.Linea);
            Assert.AreEqual(210, boleto.SaldoRestante); // 1000 - 790 = 210
            Assert.AreEqual("Medio Boleto Estudiantil", boleto.TipoTarjeta);
            Assert.AreEqual("MB001", boleto.IdTarjeta);
            Assert.AreEqual(790, boleto.MontoTotalAbonado);
        }

        [Test]
        public void TestBoletoGratuitoEstudiantil()
        {
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(0, "BG001");
            Colectivo colectivo = new Colectivo("135");
            
            // Usar fecha dentro de franja horaria
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 0, 0));
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
            Assert.AreEqual("135", boleto.Linea);
            Assert.AreEqual(0, boleto.SaldoRestante);
            Assert.AreEqual("Boleto Gratuito Estudiantil", boleto.TipoTarjeta);
            Assert.AreEqual("BG001", boleto.IdTarjeta);
            Assert.AreEqual(0, boleto.MontoTotalAbonado);
        }

        [Test]
        public void TestTodosLosTiposTarjetaEnBoletos()
        {
            Colectivo colectivo = new Colectivo("K");
            
            // Usar fecha dentro de franja horaria
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 0, 0));
            
            // Tarjeta Normal
            Tarjeta normal = new Tarjeta(5000, "NORMAL01");
            Boleto boletoNormal = colectivo.PagarCon(normal);
            Assert.IsNotNull(boletoNormal);
            Assert.AreEqual("Normal", boletoNormal.TipoTarjeta);
            
            // Franquicia Completa
            FranquiciaCompleta fc = new FranquiciaCompleta(0, "FRANQ01");
            Boleto boletoFC = colectivo.PagarCon(fc);
            Assert.IsNotNull(boletoFC);
            Assert.AreEqual("Franquicia Completa", boletoFC.TipoTarjeta);
            
            // Medio Boleto
            MedioBoletoEstudiantil mb = new MedioBoletoEstudiantil(1000, "MEDIO01");
            Boleto boletoMB = colectivo.PagarCon(mb);
            Assert.IsNotNull(boletoMB);
            Assert.AreEqual("Medio Boleto Estudiantil", boletoMB.TipoTarjeta);
            
            // Boleto Gratuito
            BoletoGratuitoEstudiantil bg = new BoletoGratuitoEstudiantil(0, "GRATIS01");
            Boleto boletoBG = colectivo.PagarCon(bg);
            Assert.IsNotNull(boletoBG);
            Assert.AreEqual("Boleto Gratuito Estudiantil", boletoBG.TipoTarjeta);
        }
    }
}
