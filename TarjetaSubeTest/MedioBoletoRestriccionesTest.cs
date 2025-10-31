using NUnit.Framework;
using Tarjeta;
using System;
using System.Collections.Generic;

namespace Tarjeta.Tests
{
    [TestFixture]
    public class MedioBoletoRestriccionesTests
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
        public void TestNoPuedeViajarAntesDe5Minutos()
        {
            // Configurar
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(5000, "MB001");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            
            DateTime primerViaje = new DateTime(2024, 1, 15, 10, 0, 0);
            
            // Simular primer viaje
            DateTimeProvider.SetDateTimeProvider(() => primerViaje);
            Boleto boleto1 = colectivo1.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1, "Primer viaje debería ser exitoso");
            Assert.AreEqual(790, boleto1.Monto, "Primer viaje debería ser medio boleto");
            
            // Intentar segundo viaje a los 3 minutos (debería fallar)
            DateTime segundoViajeIntento = primerViaje.AddMinutes(3);
            DateTimeProvider.SetDateTimeProvider(() => segundoViajeIntento);
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.IsNull(boleto2, "No debería permitir viajar antes de 5 minutos");
            
            // Intentar segundo viaje a los 5 minutos (debería funcionar)
            DateTime segundoViajeExitoso = primerViaje.AddHours(2);
            DateTimeProvider.SetDateTimeProvider(() => segundoViajeExitoso);
            Boleto boleto3 = colectivo2.PagarCon(tarjeta);
            Assert.IsNotNull(boleto3, "Debería permitir viajar después de 5 minutos");
            Assert.AreEqual(790, boleto3.Monto, "Segundo viaje debería ser medio boleto");
        }

        [Test]
        public void TestMaximoDosViajesMedioBoletoPorDia()
        {
            // Configurar
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(10000, "MB002");
            Colectivo colectivo = new Colectivo("142");
            
            DateTime hoy = new DateTime(2024, 1, 15, 10, 0, 0);
            
            // Primer viaje del día
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(790, boleto1.Monto, "Primer viaje debería ser medio boleto");
            Assert.AreEqual(1, tarjeta.ViajesHoy());
            
            // Segundo viaje del día (después de 5 minutos)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddMinutes(10));
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(790, boleto2.Monto, "Segundo viaje debería ser medio boleto");
            Assert.AreEqual(2, tarjeta.ViajesHoy());
            
            // Tercer viaje del día (debería ser tarifa completa)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddMinutes(20));
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(1580, boleto3.Monto, "Tercer viaje debería ser tarifa completa");
            Assert.AreEqual(3, tarjeta.ViajesHoy());
            
            // Cuarto viaje del día (también tarifa completa)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddMinutes(30));
            Boleto boleto4 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto4);
            Assert.AreEqual(1580, boleto4.Monto, "Cuarto viaje debería ser tarifa completa");
            Assert.AreEqual(4, tarjeta.ViajesHoy());
        }

        [Test]
        public void TestContadorViajesSeReiniciaCadaDia()
        {
            // Configurar
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(10000, "MB003");
            Colectivo colectivo = new Colectivo("142");
            
            // Cambiar a horarios dentro de franja
            DateTime hoy = new DateTime(2024, 1, 15, 21, 50, 0); // Lunes 21:50 (dentro de franja - antes de las 22:00)
            
            // Dos viajes el día 15
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            colectivo.PagarCon(tarjeta); // Primer viaje
            
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddMinutes(5));
            colectivo.PagarCon(tarjeta); // Segundo viaje
            
            Assert.AreEqual(2, tarjeta.ViajesHoy(), "Debería tener 2 viajes el día 15");
            
            // Primer viaje del día siguiente (debería ser medio boleto otra vez)
            DateTime diaSiguiente = new DateTime(2024, 1, 16, 6, 10, 0); // Martes 6:10 (nuevo día, dentro de franja - después de las 6:00)
            DateTimeProvider.SetDateTimeProvider(() => diaSiguiente);
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(790, boleto.Monto, "Primer viaje del nuevo día debería ser medio boleto");
            Assert.AreEqual(1, tarjeta.ViajesHoy(), "Debería tener 1 viaje el día 16");
        }

        [Test]
        public void TestPuedeViajarDespuesDe5MinutosExactos()
        {
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(5000, "MB004");
            Colectivo colectivo = new Colectivo("142");
            
            DateTime primerViaje = new DateTime(2024, 1, 15, 14, 0, 0);
            
            DateTimeProvider.SetDateTimeProvider(() => primerViaje);
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1);
            
            // Exactamente 5 minutos después
            DateTime segundoViaje = primerViaje.AddMinutes(5);
            DateTimeProvider.SetDateTimeProvider(() => segundoViaje);
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2, "Debería permitir viajar exactamente a los 5 minutos");
        }

        [Test]
        public void TestViajesConDiferentesColectivosMismoDia()
        {
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(10000, "MB005");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            Colectivo colectivo3 = new Colectivo("144");
            
            DateTime hoy = new DateTime(2024, 1, 15, 8, 0, 0);
            
            // Viaje en línea 142
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            Boleto boleto1 = colectivo1.PagarCon(tarjeta);
            Assert.AreEqual(790, boleto1.Monto);
            Assert.AreEqual("142", boleto1.Linea);
            
            // Viaje en línea K después de 5 minutos
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(2));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.AreEqual(790, boleto2.Monto);
            Assert.AreEqual("K", boleto2.Linea);
            
            // Tercer viaje en línea 144 (tarifa completa)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddHours(4));
            Boleto boleto3 = colectivo3.PagarCon(tarjeta);
            Assert.AreEqual(1580, boleto3.Monto);
            Assert.AreEqual("144", boleto3.Linea);
        }

        [Test]
        public void TestSaldoSuficienteParaTarifaCompleta()
        {
            // Tarjeta con saldo justo para dos medios boletos pero no para tarifa completa
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(1580, "MB006"); // Justo para un viaje completo
            Colectivo colectivo = new Colectivo("142");
            
            DateTime hoy = new DateTime(2024, 1, 15, 10, 0, 0);
            
            // Primer viaje (medio boleto)
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(790, boleto1.Monto);
            Assert.AreEqual(790, tarjeta.Saldo); // 1580 - 790 = 790
            
            // Segundo viaje (medio boleto)
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddMinutes(10));
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(790, boleto2.Monto);
            Assert.AreEqual(0, tarjeta.Saldo); // 790 - 790 = 0
            
            // Tercer viaje (tarifa completa) - no tiene saldo suficiente
            DateTimeProvider.SetDateTimeProvider(() => hoy.AddMinutes(20));
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto3, "No debería permitir viaje sin saldo suficiente para tarifa completa");
            Assert.AreEqual(0, tarjeta.Saldo); // Saldo no cambia
        }

        [Test]
        public void TestViajeInmediatamenteDespuesDe5Minutos()
        {
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(5000, "MB007");
            Colectivo colectivo = new Colectivo("142");
            
            DateTime primerViaje = new DateTime(2024, 1, 15, 12, 0, 0);
            
            DateTimeProvider.SetDateTimeProvider(() => primerViaje);
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1);
            
            // 5 minutos y 1 segundo después
            DateTime segundoViaje = primerViaje.AddMinutes(5).AddSeconds(1);
            DateTimeProvider.SetDateTimeProvider(() => segundoViaje);
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2, "Debería permitir viajar después de 5 minutos y 1 segundo");
        }
    }
}