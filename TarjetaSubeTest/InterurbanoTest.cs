
using NUnit.Framework;
using Tarjeta;
using System;

namespace Tarjeta.Tests
{
    [TestFixture]
    public class LineasInterurbanasTests
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
        public void TestLineaUrbanaTarifaNormal()
        {
            // Línea urbana por defecto
            Colectivo colectivoUrbano = new Colectivo("142");
            Assert.AreEqual(TipoLinea.Urbana, colectivoUrbano.TipoLinea);
            Assert.AreEqual(1580, colectivoUrbano.Tarifa);
        }

        [Test]
        public void TestLineaInterurbanaTarifaEspecial()
        {
            // Línea interurbana explícita
            Colectivo colectivoInterurbano = new Colectivo("33", TipoLinea.Interurbana);
            Assert.AreEqual(TipoLinea.Interurbana, colectivoInterurbano.TipoLinea);
            Assert.AreEqual(3000, colectivoInterurbano.Tarifa);
        }

        [Test]
        public void TestTarjetaNormalLineaInterurbana()
        {
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 0, 0));
            
            Tarjeta tarjeta = new Tarjeta(10000, "NORMAL001");
            Colectivo colectivo = new Colectivo("33", TipoLinea.Interurbana); // Línea a Gálvez
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(3000, boleto.Monto, "Debería cobrar tarifa interurbana");
            Assert.AreEqual(7000, tarjeta.Saldo); // 10000 - 3000 = 7000
            Assert.AreEqual("33", boleto.Linea);
        }

        [Test]
        public void TestMedioBoletoLineaInterurbana()
        {
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 0, 0));
            
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(10000, "MB001");
            Colectivo colectivo = new Colectivo("33", TipoLinea.Interurbana); // Línea a Baigorria
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(1500, boleto.Monto, "Debería cobrar mitad de tarifa interurbana"); // 3000 / 2 = 1500
            Assert.AreEqual(8500, tarjeta.Saldo); // 10000 - 1500 = 8500
        }

        [Test]
        public void TestBoletoGratuitoLineaInterurbana()
        {
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 0, 0));
            
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(3000, "BG001");
            Colectivo colectivo = new Colectivo("33", TipoLinea.Interurbana);
            
            // Primer viaje gratuito
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(0, boleto1.Monto, "Primer viaje interurbano debería ser gratuito");
            
            // Segundo viaje gratuito
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 15, 0, 0));
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(0, boleto2.Monto, "Segundo viaje interurbano debería ser gratuito");
            
            // Tercer viaje (tarifa completa interurbana)
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 16, 0, 0));
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(3000, boleto3.Monto, "Tercer viaje interurbano debería ser tarifa completa");
        }

        [Test]
        public void TestFranquiciaCompletaLineaInterurbana()
        {
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 0, 0));
            
            FranquiciaCompleta tarjeta = new FranquiciaCompleta(0, "FC001");
            Colectivo colectivo = new Colectivo("33", TipoLinea.Interurbana);
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto, "Franquicia completa debería ser gratuita en líneas interurbanas");
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestSaldoNegativoLineaInterurbana()
        {
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 0, 0));
            
            Tarjeta tarjeta = new Tarjeta(2000, "NORMAL002"); // Saldo menor a tarifa interurbana
            Colectivo colectivo = new Colectivo("33", TipoLinea.Interurbana);
            
            // 2000 - 3000 = -1000 (dentro del límite -1200)
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto, "Debería permitir saldo negativo dentro del límite");
            Assert.AreEqual(3000, boleto.Monto);
            Assert.AreEqual(-1000, tarjeta.Saldo);
        }

        [Test]
        public void TestUsoFrecuenteLineaInterurbana()
        {
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 0, 0));
            
            Tarjeta tarjeta = new Tarjeta(50000, "NORMAL003");
            Colectivo colectivo = new Colectivo("33", TipoLinea.Interurbana);
            
            // Simular 35 viajes este mes (dentro del rango de 20% descuento)
            tarjeta.SetViajesParaTesting(35, DateTimeProvider.Now);
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            // 3000 * 0.8 = 2400 (20% de descuento)
            Assert.AreEqual(2400, boleto.Monto, "Debería aplicar descuento frecuente en línea interurbana");
            Assert.AreEqual(47600, tarjeta.Saldo); // 50000 - 2400 = 47600
        }

        [Test]
        public void TestFranjaHorariaLineaInterurbana()
        {
            // Las franquicias también aplican restricciones horarias en líneas interurbanas
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(5000, "MB002");
            Colectivo colectivo = new Colectivo("33", TipoLinea.Interurbana);
            
            // Fuera de franja horaria (sábado)
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 20, 12, 0, 0));
            
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNull(boleto, "No debería permitir viaje en franquicia fuera de franja horaria, incluso en línea interurbana");
        }

        [Test]
        public void TestRestriccion5MinutosLineaInterurbana()
        {
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 0, 0));
            
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(10000, "MB003");
            Colectivo colectivo = new Colectivo("33", TipoLinea.Interurbana);
            
            // Primer viaje
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1);
            
            // Intentar segundo viaje a los 3 minutos (debería fallar)
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 3, 0));
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto2, "No debería permitir viajar antes de 5 minutos en línea interurbana");
            
            // Viaje a los 5 minutos (debería funcionar)
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 5, 0));
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto3, "Debería permitir viajar después de 5 minutos en línea interurbana");
        }

        [Test]
        public void TestMezclaLineasUrbanasEInterurbanas()
        {
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 14, 0, 0));
            
            Tarjeta tarjeta = new Tarjeta(10000, "NORMAL004");
            
            // Viaje en línea urbana
            Colectivo urbano = new Colectivo("142"); // Por defecto urbana
            Boleto boletoUrbano = urbano.PagarCon(tarjeta);
            Assert.AreEqual(1580, boletoUrbano.Monto);
            Assert.AreEqual(8420, tarjeta.Saldo); // 10000 - 1580 = 8420
            
            // Viaje en línea interurbana
            DateTimeProvider.SetDateTimeProvider(() => new DateTime(2024, 1, 15, 15, 10, 0));
            Colectivo interurbano = new Colectivo("33", TipoLinea.Interurbana);
            Boleto boletoInterurbano = interurbano.PagarCon(tarjeta);
            Assert.AreEqual(3000, boletoInterurbano.Monto);
            Assert.AreEqual(5420, tarjeta.Saldo); // 8420 - 3000 = 5420
        }
    }
}
