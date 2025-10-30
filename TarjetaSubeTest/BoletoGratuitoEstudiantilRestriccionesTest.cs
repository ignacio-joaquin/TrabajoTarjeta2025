using NUnit.Framework;
using Tarjeta;
using System;
using System.Collections.Generic;

namespace Tarjeta.Tests
{
    [TestFixture]
    public class BoletoGratuitoRestriccionesTests
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
        public void TestPrimerosDosViajesGratuitos()
        {
            // Configurar
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(0, "BG001");
            Colectivo colectivo = new Colectivo("142");
            
            DateTime hoy = new DateTime(2024, 1, 15, 8, 0, 0);
            
            // Primer viaje gratuito
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1, "Primer viaje debería ser exitoso");
            Assert.AreEqual(0, boleto1.Monto, "Primer viaje debería ser gratuito");
            Assert.AreEqual(0, tarjeta.Saldo, "Saldo no debería cambiar en viaje gratuito");
            Assert.AreEqual(1, tarjeta.ViajesHoy(), "Debería tener 1 viaje registrado hoy");
            
            // Segundo viaje gratuito
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(2));
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2, "Segundo viaje debería ser exitoso");
            Assert.AreEqual(0, boleto2.Monto, "Segundo viaje debería ser gratuito");
            Assert.AreEqual(0, tarjeta.Saldo, "Saldo no debería cambiar en viaje gratuito");
            Assert.AreEqual(2, tarjeta.ViajesHoy(), "Debería tener 2 viajes registrados hoy");
        }

        [Test]
        public void TestTercerViajeCobraTarifaCompleta()
        {
            // Configurar tarjeta con saldo suficiente
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(5000, "BG002");
            Colectivo colectivo = new Colectivo("K");
            
            DateTime hoy = new DateTime(2024, 1, 15, 10, 0, 0);
            
            // Primer viaje gratuito
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(0, boleto1.Monto, "Primer viaje debería ser gratuito");
            Assert.AreEqual(5000, tarjeta.Saldo, "Saldo no debería cambiar");
            
            // Segundo viaje gratuito
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(1));
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(0, boleto2.Monto, "Segundo viaje debería ser gratuito");
            Assert.AreEqual(5000, tarjeta.Saldo, "Saldo no debería cambiar");
            
            // Tercer viaje (debería cobrar tarifa completa)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(2));
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto3, "Tercer viaje debería ser exitoso");
            Assert.AreEqual(1580, boleto3.Monto, "Tercer viaje debería ser tarifa completa");
            Assert.AreEqual(3420, tarjeta.Saldo, "Saldo debería descontar la tarifa completa"); // 5000 - 1580 = 3420
            Assert.AreEqual(3, tarjeta.ViajesHoy(), "Debería tener 3 viajes registrados hoy");
        }

        [Test]
        public void TestCuartoViajeTambienCobraTarifaCompleta()
        {
            // Configurar tarjeta con saldo suficiente
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(10000, "BG003");
            Colectivo colectivo = new Colectivo("144");
            
            DateTime hoy = new DateTime(2024, 1, 15, 7, 0, 0);
            
            // Primeros dos viajes gratuitos
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            colectivo.PagarCon(tarjeta); // Viaje 1 gratuito
            
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(1));
            colectivo.PagarCon(tarjeta); // Viaje 2 gratuito
            
            // Tercer viaje (tarifa completa)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(2));
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(1580, boleto3.Monto, "Tercer viaje debería ser tarifa completa");
            Assert.AreEqual(8420, tarjeta.Saldo); // 10000 - 1580 = 8420
            
            // Cuarto viaje (también tarifa completa)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(3));
            Boleto boleto4 = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(1580, boleto4.Monto, "Cuarto viaje debería ser tarifa completa");
            Assert.AreEqual(6840, tarjeta.Saldo); // 8420 - 1580 = 6840
            Assert.AreEqual(4, tarjeta.ViajesHoy(), "Debería tener 4 viajes registrados hoy");
        }

        [Test]
        public void TestContadorViajesSeReiniciaCadaDia()
        {
            // Configurar
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(5000, "BG004");
            Colectivo colectivo = new Colectivo("142");
            
            DateTime finDeDia = new DateTime(2024, 1, 15, 23, 30, 0);
            
            // Dos viajes el día 15
            DateTimeProvider.SetDateTimeProvider(() => finDeDia);
            colectivo.PagarCon(tarjeta); // Viaje 1 gratuito
            
            DateTimeProvider.SetDateTimeProvider(() => finDeDia.AddMinutes(20));
            colectivo.PagarCon(tarjeta); // Viaje 2 gratuito
            
            Assert.AreEqual(2, tarjeta.ViajesHoy(), "Debería tener 2 viajes el día 15");
            
            // Primer viaje del día siguiente (debería ser gratuito otra vez)
            DateTime diaSiguiente = new DateTime(2024, 1, 16, 0, 30, 0); // Nuevo día
            DateTimeProvider.SetDateTimeProvider(() => diaSiguiente);
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto, "Primer viaje del nuevo día debería ser gratuito");
            Assert.AreEqual(1, tarjeta.ViajesHoy(), "Debería tener 1 viaje el día 16");
        }

        [Test]
        public void TestTercerViajeSinSaldoSuficiente()
        {
            // Configurar tarjeta con saldo insuficiente para tarifa completa
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(0, "BG005");
            Colectivo colectivo = new Colectivo("K");
            
            DateTime hoy = new DateTime(2024, 1, 15, 12, 0, 0);
            
            // Primer viaje gratuito
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(0, boleto1.Monto, "Primer viaje debería ser gratuito");
            
            // Segundo viaje gratuito
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(1));
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(0, boleto2.Monto, "Segundo viaje debería ser gratuito");
            
            // Tercer viaje (no tiene saldo suficiente para tarifa completa)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(2));
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(0, tarjeta.Saldo, "Se cobra tarifa completa");
            Assert.AreEqual(2, tarjeta.ViajesHoy(), "Debería haber 3 viajes registrados");
        }

        [Test]
        public void TestViajesConDiferentesLineas()
        {
            // Verificar que el límite es por tarjeta, no por línea
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(5000, "BG006");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            Colectivo colectivo3 = new Colectivo("144");
            
            DateTime hoy = new DateTime(2024, 1, 15, 9, 0, 0);
            
            // Viaje en línea 142 (gratuito)
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            Boleto boleto1 = colectivo1.PagarCon(tarjeta);
            Assert.AreEqual(0, boleto1.Monto);
            Assert.AreEqual("142", boleto1.Linea);
            
            // Viaje en línea K (gratuito)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(1));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.AreEqual("K", boleto2.Linea);
            
            // Viaje en línea 144 (tarifa completa)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(2));
            Boleto boleto3 = colectivo3.PagarCon(tarjeta);
            Assert.AreEqual(1580, boleto3.Monto, "Tercer viaje debería ser tarifa completa sin importar la línea");
            Assert.AreEqual("144", boleto3.Linea);
        }

        [Test]
        public void TestViajesRapidosSinRestriccionDeTiempo()
        {
            // A diferencia del medio boleto, el boleto gratuito no tiene restricción de tiempo entre viajes
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(5000, "BG007");
            Colectivo colectivo = new Colectivo("142");
            
            DateTime hoy = new DateTime(2024, 1, 15, 14, 0, 0);
            
            // Primer viaje
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1);
            
            // Segundo viaje inmediatamente después (debería funcionar)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddSeconds(30));
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2, "Debería permitir viajes consecutivos sin restricción de tiempo");
            Assert.AreEqual(0, boleto2.Monto, "Segundo viaje debería ser gratuito");
            
            // Tercer viaje también rápido (pero tarifa completa)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddSeconds(60));
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto3, "Debería permitir tercer viaje rápidamente (pero con tarifa completa)");
            Assert.AreEqual(1580, boleto3.Monto, "Tercer viaje debería ser tarifa completa");
        }

        [Test]
        public void TestSaldoNegativoEnViajesPagados()
        {
            // Configurar tarjeta con saldo bajo para tercer viaje
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(1000, "BG008");
            Colectivo colectivo = new Colectivo("142");
            
            DateTime hoy = new DateTime(2024, 1, 15, 10, 0, 0);
            
            // Primeros dos viajes gratuitos
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            colectivo.PagarCon(tarjeta);
            
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(1));
            colectivo.PagarCon(tarjeta);
            
            // Tercer viaje con saldo negativo permitido
            // 1000 - 1580 = -580 (dentro del límite -1200)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(2));
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto3, "Debería permitir viaje con saldo negativo dentro del límite");
            Assert.AreEqual(1580, boleto3.Monto);
            Assert.AreEqual(-580, tarjeta.Saldo);
        }
    }
}