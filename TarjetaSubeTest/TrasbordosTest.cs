using NUnit.Framework;
using Tarjeta;
using System;
using System.Collections.Generic;

namespace Tarjeta.Tests
{
    [TestFixture]
    public class TrasbordosTests
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
        public void TestTrasbordoGratuitoEntre60Minutos()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 8, 0, 0); // Lunes 8:00
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Tarjeta tarjeta = new Tarjeta(10000, "NORMAL001");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            
            // Primer viaje
            Boleto boleto1 = colectivo1.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(1580, boleto1.Monto);
            Assert.IsFalse(boleto1.EsTrasbordo);
            
            // Segundo viaje en línea diferente dentro de 60 minutos
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(30));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(0, boleto2.Monto, "El trasbordo debería ser gratuito");
            Assert.IsTrue(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoDespuesDe60MinutosNOEsGratuito()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 8, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Tarjeta tarjeta = new Tarjeta(10000, "NORMAL002");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            
            // Primer viaje
            colectivo1.PagarCon(tarjeta);
            
            // Segundo viaje después de 61 minutos
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(61));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(1580, boleto2.Monto, "Después de 60 minutos debe cobrar tarifa completa");
            Assert.IsFalse(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoMismaLineaNOEsGratuito()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 8, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Tarjeta tarjeta = new Tarjeta(10000, "NORMAL003");
            Colectivo colectivo = new Colectivo("142");
            
            // Primer viaje
            colectivo.PagarCon(tarjeta);
            
            // Segundo viaje en la misma línea dentro de 60 minutos
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(30));
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(1580, boleto2.Monto, "Mismo colectivo no es trasbordo");
            Assert.IsFalse(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoFueraDeFranjaHoraria()
        {
            // Domingo a las 10:00 (fuera de franja lunes-sábado)
            DateTime fecha = new DateTime(2024, 1, 21, 10, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Tarjeta tarjeta = new Tarjeta(10000, "NORMAL004");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            
            // Primer viaje
            colectivo1.PagarCon(tarjeta);
            
            // Segundo viaje
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(30));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(1580, boleto2.Monto, "Fuera de franja no hay trasbordo");
            Assert.IsFalse(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoAntesDeLas7AM()
        {
            // Lunes a las 6:30 (antes de las 7:00)
            DateTime fecha = new DateTime(2024, 1, 15, 6, 30, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Tarjeta tarjeta = new Tarjeta(10000, "NORMAL005");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            
            colectivo1.PagarCon(tarjeta);
            
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(20));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.AreEqual(1580, boleto2.Monto);
            Assert.IsFalse(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoDespuesDeLas22()
        {
            // Lunes a las 22:00
            DateTime fecha = new DateTime(2024, 1, 15, 22, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Tarjeta tarjeta = new Tarjeta(10000, "NORMAL006");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            
            colectivo1.PagarCon(tarjeta);
            
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(20));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.AreEqual(1580, boleto2.Monto);
            Assert.IsFalse(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoSabadoDentroDeHorario()
        {
            // Sábado a las 10:00 (dentro de franja)
            DateTime fecha = new DateTime(2024, 1, 20, 10, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Tarjeta tarjeta = new Tarjeta(10000, "NORMAL007");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            
            colectivo1.PagarCon(tarjeta);
            
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(30));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(0, boleto2.Monto, "Sábado dentro de horario permite trasbordo");
            Assert.IsTrue(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoConMedioBoleto()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 8, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(10000, "MB001");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            
            // Primer viaje
            Boleto boleto1 = colectivo1.PagarCon(tarjeta);
            Assert.AreEqual(790, boleto1.Monto);
            
            // Trasbordo dentro de 60 minutos
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(30));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestTrasbordoConBoletoGratuito()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 8, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(5000, "BG001");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            
            // Primer viaje (gratuito)
            Boleto boleto1 = colectivo1.PagarCon(tarjeta);
            Assert.AreEqual(0, boleto1.Monto);
            
            // Segundo viaje en diferente línea
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(30));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            // Como el primer viaje fue gratuito (monto 0), no debería registrarse para trasbordo
            Assert.AreEqual(0, boleto2.Monto); // Segundo viaje gratuito del día
        }

        [Test]
        public void TestTrasbordoConFranquiciaCompleta()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 8, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            FranquiciaCompleta tarjeta = new FranquiciaCompleta(0, "FC001");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            
            // Primer viaje
            Boleto boleto1 = colectivo1.PagarCon(tarjeta);
            Assert.AreEqual(0, boleto1.Monto);
            
            // Segundo viaje
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(30));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(0, boleto2.Monto);
        }

        [Test]
        public void TestTrasbordoEnLineaInterurbana()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 8, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Tarjeta tarjeta = new Tarjeta(10000, "NORMAL008");
            Colectivo colectivo1 = new Colectivo("142"); // Urbana
            Colectivo colectivo2 = new Colectivo("33", TipoLinea.Interurbana);
            
            // Primer viaje urbano
            colectivo1.PagarCon(tarjeta);
            
            // Trasbordo a interurbana
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(30));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(0, boleto2.Monto, "Trasbordo debe ser gratuito incluso en interurbana");
            Assert.IsTrue(boleto2.EsTrasbordo);
        }

        [Test]
        public void TestMultiplesTrasbordos()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 8, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Tarjeta tarjeta = new Tarjeta(20000, "NORMAL009");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            Colectivo colectivo3 = new Colectivo("144");
            
            // Primer viaje
            Boleto boleto1 = colectivo1.PagarCon(tarjeta);
            Assert.AreEqual(1580, boleto1.Monto);
            Assert.IsFalse(boleto1.EsTrasbordo);
            
            // Segundo viaje (trasbordo)
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(20));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);
            
            // Tercer viaje en otra línea (NO es trasbordo porque el anterior fue gratuito)
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(80));
            Boleto boleto3 = colectivo3.PagarCon(tarjeta);
            Assert.AreEqual(1580, boleto3.Monto);
            Assert.IsFalse(boleto3.EsTrasbordo);
        }

        [Test]
        public void TestLimpiezaViajesRecientesExpirados()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 8, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Tarjeta tarjeta = new Tarjeta(20000, "NORMAL010");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            
            // Primer viaje
            colectivo1.PagarCon(tarjeta);
            Assert.AreEqual(1, tarjeta.ViajesRecientes.Count);
            
            // Avanzar más de 60 minutos
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(70));
            
            // Segundo viaje (no es trasbordo porque ya expiró)
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.AreEqual(1580, boleto2.Monto);
            Assert.IsFalse(boleto2.EsTrasbordo);
            
            // Verificar que se limpió el viaje anterior
            Assert.AreEqual(1, tarjeta.ViajesRecientes.Count);
        }

        [Test]
        public void TestPuedeHacerTrasbordoRetornaFalsoFueraDeHorario()
        {
            DateTime fecha = new DateTime(2024, 1, 21, 10, 0, 0); // Domingo
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Tarjeta tarjeta = new Tarjeta(10000, "NORMAL011");
            
            // Agregar un viaje reciente manualmente
            var viajesRecientes = new List<ViajeReciente>
            {
                new ViajeReciente(fecha.AddMinutes(-10), "142", 1580)
            };
            tarjeta.SetViajesRecientesParaTesting(viajesRecientes);
            
            bool puedeHacerTrasbordo = tarjeta.PuedeHacerTrasbordo("K");
            Assert.IsFalse(puedeHacerTrasbordo, "No debería poder hacer trasbordo fuera de horario");
        }

        [Test]
        public void TestViajeRecienteEsValidoParaTrasbordo()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 8, 0, 0);
            ViajeReciente viaje = new ViajeReciente(fecha, "142", 1580);
            
            // Dentro de 60 minutos
            Assert.IsTrue(viaje.EsValidoParaTrasbordo(fecha.AddMinutes(30)));
            Assert.IsTrue(viaje.EsValidoParaTrasbordo(fecha.AddMinutes(60)));
            
            // Fuera de 60 minutos
            Assert.IsFalse(viaje.EsValidoParaTrasbordo(fecha.AddMinutes(61)));
        }

        [Test]
        public void TestTrasbordoHelperEsDiaHabilTrasbordo()
        {
            // Lunes a sábado son hábiles
            Assert.IsTrue(TrasbordoHelper.EsDiaHabilTrasbordo(new DateTime(2024, 1, 15))); // Lunes
            Assert.IsTrue(TrasbordoHelper.EsDiaHabilTrasbordo(new DateTime(2024, 1, 20))); // Sábado
            
            // Domingo no es hábil
            Assert.IsFalse(TrasbordoHelper.EsDiaHabilTrasbordo(new DateTime(2024, 1, 21))); // Domingo
        }

        [Test]
        public void TestTrasbordoHelperEstaEnHorarioPermitido()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 0, 0, 0);
            
            // Antes de las 7
            Assert.IsFalse(TrasbordoHelper.EstaEnHorarioPermitidoTrasbordo(fecha.AddHours(6)));
            
            // Entre 7 y 22
            Assert.IsTrue(TrasbordoHelper.EstaEnHorarioPermitidoTrasbordo(fecha.AddHours(7)));
            Assert.IsTrue(TrasbordoHelper.EstaEnHorarioPermitidoTrasbordo(fecha.AddHours(15)));
            Assert.IsTrue(TrasbordoHelper.EstaEnHorarioPermitidoTrasbordo(fecha.AddHours(21)));
            
            // Después de las 22
            Assert.IsFalse(TrasbordoHelper.EstaEnHorarioPermitidoTrasbordo(fecha.AddHours(22)));
        }

        [Test]
        public void TestTrasbordoHelperGetFranjaHoraria()
        {
            string franja = TrasbordoHelper.GetFranjaHorariaTrasbordo();
            Assert.AreEqual("Lunes a Sábado de 7:00 a 22:00", franja);
        }

        [Test]
        public void TestTrasbordoConDescuentoFrecuente()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 8, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Tarjeta tarjeta = new Tarjeta(50000, "NORMAL012");
            Colectivo colectivo1 = new Colectivo("142");
            Colectivo colectivo2 = new Colectivo("K");
            
            // Simular 40 viajes este mes (para descuento 20%)
            tarjeta.SetViajesParaTesting(40, fecha.AddMinutes(-10));
            
            // Primer viaje (con descuento)
            Boleto boleto1 = colectivo1.PagarCon(tarjeta);
            Assert.AreEqual(1264, boleto1.Monto); // 1580 * 0.8
            
            // Trasbordo (gratuito, no aumenta contador)
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(30));
            Boleto boleto2 = colectivo2.PagarCon(tarjeta);
            Assert.AreEqual(0, boleto2.Monto);
            Assert.IsTrue(boleto2.EsTrasbordo);
            Assert.AreEqual(0, boleto2.DescuentoFrecuente);
        }
    }
}