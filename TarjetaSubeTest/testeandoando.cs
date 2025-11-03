using NUnit.Framework;
using Tarjeta;
using System;
using System.Collections.Generic;

namespace Tarjeta.Tests
{
    [TestFixture]
    public class AdditionalCoverageTests
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

        // Tests para Tarjeta.cs
        [Test]
        public void TestTarjetaConstructorSinParametros()
        {
            Tarjeta tarjeta = new Tarjeta();
            Assert.AreEqual(0, tarjeta.Saldo);
            Assert.IsNotNull(tarjeta.Id);
            Assert.IsNotEmpty(tarjeta.Id);
        }

        [Test]
        public void TestTarjetaGeneraIdAutomatico()
        {
            Tarjeta tarjeta1 = new Tarjeta(1000);
            Tarjeta tarjeta2 = new Tarjeta(2000);
            
            Assert.AreNotEqual(tarjeta1.Id, tarjeta2.Id);
            Assert.AreEqual(8, tarjeta1.Id.Length);
        }

        [Test]
        public void TestSetViajesRecientesParaTesting()
        {
            Tarjeta tarjeta = new Tarjeta(10000);
            DateTime fecha = new DateTime(2024, 1, 15, 8, 0, 0);
            
            var viajes = new List<ViajeReciente>
            {
                new ViajeReciente(fecha, "142", 1580),
                new ViajeReciente(fecha.AddMinutes(10), "K", 1580)
            };
            
            tarjeta.SetViajesRecientesParaTesting(viajes);
            Assert.AreEqual(2, tarjeta.ViajesRecientes.Count);
        }

        [Test]
        public void TestObtenerDescuentoAplicadoSinDescuento()
        {
            Tarjeta tarjeta = new Tarjeta(10000);
            int descuento = tarjeta.ObtenerDescuentoAplicado(1580);
            Assert.AreEqual(0, descuento);
        }

        [Test]
        public void TestCargarTodosLosMontos()
        {
            Tarjeta tarjeta = new Tarjeta(0);
            
            int[] montosValidos = { 2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000 };
            
            foreach (int monto in montosValidos)
            {
                Tarjeta t = new Tarjeta(0);
                bool resultado = t.Cargar(monto);
                Assert.IsTrue(resultado, $"Debería aceptar carga de {monto}");
                Assert.AreEqual(monto, t.Saldo, $"Saldo debería ser {monto}");
            }
        }

        [Test]
        public void TestCargarMontoInvalido()
        {
            Tarjeta tarjeta = new Tarjeta(0);
            
            int[] montosInvalidos = { 100, 500, 1000, 1500, 2500, 7000, 12000 };
            
            foreach (int monto in montosInvalidos)
            {
                bool resultado = tarjeta.Cargar(monto);
                Assert.IsFalse(resultado, $"No debería aceptar carga de {monto}");
                Assert.AreEqual(0, tarjeta.Saldo, "Saldo no debería cambiar");
            }
        }

        // Tests para HorarioFranquicia
        [Test]
        public void TestHorarioFranquiciaEsDiaHabil()
        {
            Assert.IsTrue(HorarioFranquicia.EsDiaHabil(new DateTime(2024, 1, 15))); // Lunes
            Assert.IsTrue(HorarioFranquicia.EsDiaHabil(new DateTime(2024, 1, 16))); // Martes
            Assert.IsTrue(HorarioFranquicia.EsDiaHabil(new DateTime(2024, 1, 17))); // Miércoles
            Assert.IsTrue(HorarioFranquicia.EsDiaHabil(new DateTime(2024, 1, 18))); // Jueves
            Assert.IsTrue(HorarioFranquicia.EsDiaHabil(new DateTime(2024, 1, 19))); // Viernes
            Assert.IsFalse(HorarioFranquicia.EsDiaHabil(new DateTime(2024, 1, 20))); // Sábado
            Assert.IsFalse(HorarioFranquicia.EsDiaHabil(new DateTime(2024, 1, 21))); // Domingo
        }

        [Test]
        public void TestHorarioFranquiciaEstaEnHorarioPermitido()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 0, 0, 0);
            
            // Antes de las 6
            Assert.IsFalse(HorarioFranquicia.EstaEnHorarioPermitido(fecha.AddHours(5)));
            
            // Entre 6 y 22
            Assert.IsTrue(HorarioFranquicia.EstaEnHorarioPermitido(fecha.AddHours(6)));
            Assert.IsTrue(HorarioFranquicia.EstaEnHorarioPermitido(fecha.AddHours(12)));
            Assert.IsTrue(HorarioFranquicia.EstaEnHorarioPermitido(fecha.AddHours(21)));
            
            // A las 22 y después
            Assert.IsFalse(HorarioFranquicia.EstaEnHorarioPermitido(fecha.AddHours(22)));
            Assert.IsFalse(HorarioFranquicia.EstaEnHorarioPermitido(fecha.AddHours(23)));
        }

        [Test]
        public void TestHorarioFranquiciaGetFranjaHoraria()
        {
            string franja = HorarioFranquicia.GetFranjaHorariaPermitida();
            Assert.AreEqual("Lunes a Viernes de 6:00 a 22:00", franja);
        }

        [Test]
        public void TestHorarioFranquiciaEstaEnFranjaCompleta()
        {
            // Día hábil en horario correcto
            DateTime validoLunes = new DateTime(2024, 1, 15, 10, 0, 0);
            Assert.IsTrue(HorarioFranquicia.EstaEnFranjaHorariaPermitida(validoLunes));
            
            // Día no hábil (sábado)
            DateTime sabado = new DateTime(2024, 1, 20, 10, 0, 0);
            Assert.IsFalse(HorarioFranquicia.EstaEnFranjaHorariaPermitida(sabado));
            
            // Día hábil pero fuera de horario
            DateTime muyTemprano = new DateTime(2024, 1, 15, 5, 0, 0);
            Assert.IsFalse(HorarioFranquicia.EstaEnFranjaHorariaPermitida(muyTemprano));
        }

        // Tests para MedioBoletoEstudiantil
        [Test]
        public void TestMedioBoletoConstructorSinParametros()
        {
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil();
            Assert.AreEqual(0, tarjeta.Saldo);
            Assert.AreEqual("Medio Boleto Estudiantil", tarjeta.TipoTarjeta);
        }

        [Test]
        public void TestMedioBoletoGetUltimoViaje()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 10, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(5000);
            Colectivo colectivo = new Colectivo("142");
            
            Assert.IsNull(tarjeta.GetUltimoViaje());
            
            colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(tarjeta.GetUltimoViaje());
            Assert.AreEqual(fecha, tarjeta.GetUltimoViaje());
        }


        [Test]
        public void TestMedioBoletoCalcularMontoRealAPagar()
        {
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(5000);
            
            // Primer viaje del día
            int monto = tarjeta.CalcularMontoRealAPagar(1580);
            Assert.AreEqual(790, monto);
        }

        // Tests para BoletoGratuitoEstudiantil
        [Test]
        public void TestBoletoGratuitoConstructorSinParametros()
        {
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil();
            Assert.AreEqual(0, tarjeta.Saldo);
            Assert.AreEqual("Boleto Gratuito Estudiantil", tarjeta.TipoTarjeta);
        }

        [Test]
        public void TestBoletoGratuitoSetViajesParaTesting()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 10, 0, 0);
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(5000);
            
            var viajes = new List<DateTime>
            {
                fecha,
                fecha.AddMinutes(10),
                fecha.AddMinutes(20)
            };
            
            tarjeta.SetViajesParaTesting(viajes);
            
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(30));
            Assert.AreEqual(3, tarjeta.ViajesHoy());
        }

        [Test]
        public void TestBoletoGratuitoDescontarMontoPositivo()
        {
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(5000);
            
            bool resultado = tarjeta.Descontar(1580);
            Assert.IsTrue(resultado);
            Assert.AreEqual(3420, tarjeta.Saldo);
        }


        // Tests para FranquiciaCompleta
        [Test]
        public void TestFranquiciaCompletaConstructorSinParametros()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            Assert.AreEqual(0, tarjeta.Saldo);
            Assert.AreEqual("Franquicia Completa", tarjeta.TipoTarjeta);
        }

        [Test]
        public void TestFranquiciaCompletaCalcularMontoRealAPagar()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta();
            int monto = tarjeta.CalcularMontoRealAPagar(1580);
            Assert.AreEqual(0, monto);
        }

        [Test]
        public void TestFranquiciaCompletaDescontarConSaldoPendiente()
        {
            FranquiciaCompleta tarjeta = new FranquiciaCompleta(55000, "FC001");
            tarjeta.Cargar(5000); // Genera saldo pendiente
            
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);
            
            // Al viajar, no descuenta pero sí intenta acreditar
            bool resultado = tarjeta.Descontar(0);
            Assert.IsTrue(resultado);
            Assert.AreEqual(56000, tarjeta.Saldo);
        }

        // Tests para Colectivo
        [Test]
        public void TestColectivoConstructorSimple()
        {
            Colectivo colectivo = new Colectivo("142");
            Assert.AreEqual("142", colectivo.Linea);
            Assert.AreEqual(TipoLinea.Urbana, colectivo.TipoLinea);
            Assert.AreEqual(1580, colectivo.Tarifa);
        }

        [Test]
        public void TestColectivoConstructorInterurbano()
        {
            Colectivo colectivo = new Colectivo("33", TipoLinea.Interurbana);
            Assert.AreEqual("33", colectivo.Linea);
            Assert.AreEqual(TipoLinea.Interurbana, colectivo.TipoLinea);
            Assert.AreEqual(3000, colectivo.Tarifa);
        }

        // Tests para Boleto
        [Test]
        public void TestBoletoConstructorCompleto()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 10, 0, 0);
            Boleto boleto = new Boleto(1580, "142", 3420, "Normal", "TEST001", 1580, 0, false, fecha);
            
            Assert.AreEqual(1580, boleto.Monto);
            Assert.AreEqual("142", boleto.Linea);
            Assert.AreEqual(3420, boleto.SaldoRestante);
            Assert.AreEqual("Normal", boleto.TipoTarjeta);
            Assert.AreEqual("TEST001", boleto.IdTarjeta);
            Assert.AreEqual(1580, boleto.MontoTotalAbonado);
            Assert.AreEqual(0, boleto.DescuentoFrecuente);
            Assert.IsFalse(boleto.EsTrasbordo);
            Assert.AreEqual(fecha, boleto.Fecha);
        }

        [Test]
        public void TestBoletoToStringNormal()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 10, 30, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Boleto boleto = new Boleto(1580, "142", 3420, "Normal", "TEST001", 1580, 0);
            string str = boleto.ToString();
            
            Assert.IsTrue(str.Contains("Línea: 142"));
            Assert.IsTrue(str.Contains("Tipo: Normal"));
            Assert.IsTrue(str.Contains("ID: TEST001"));
            Assert.IsTrue(str.Contains("Monto: $1580"));
            Assert.IsTrue(str.Contains("Total abonado: $1580"));
            Assert.IsTrue(str.Contains("Saldo restante: $3420"));
        }

        [Test]
        public void TestBoletoToStringConDescuento()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 10, 30, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Boleto boleto = new Boleto(1264, "K", 5000, "Normal", "TEST002", 1264, 316);
            string str = boleto.ToString();
            
            Assert.IsTrue(str.Contains("[Descuento frecuente: -$316]"));
        }

        [Test]
        public void TestBoletoToStringTrasbordo()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 10, 30, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            Boleto boleto = new Boleto(0, "142", 5000, "Normal", "TEST003", 0, 0, true);
            string str = boleto.ToString();
            
            Assert.IsTrue(str.Contains("[TRASBORDO GRATUITO]"));
        }

        // Tests para DateTimeProvider
        [Test]
        public void TestDateTimeProviderNow()
        {
            DateTimeProvider.ResetToDefault();
            DateTime antes = DateTime.Now;
            DateTime now = DateTimeProvider.Now;
            DateTime despues = DateTime.Now;
            
            Assert.IsTrue(now >= antes && now <= despues);
        }

        // Tests para ViajeReciente
        [Test]
        public void TestViajeRecientePropiedades()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 10, 0, 0);
            ViajeReciente viaje = new ViajeReciente(fecha, "142", 1580);
            
            Assert.AreEqual(fecha, viaje.Fecha);
            Assert.AreEqual("142", viaje.Linea);
            Assert.AreEqual(1580, viaje.Monto);
        }

        // Tests adicionales para aumentar cobertura
        [Test]
        public void TestTarjetaConSaldoNegativoInicialYCarga()
        {
            Tarjeta tarjeta = new Tarjeta(-500);
            bool resultado = tarjeta.Cargar(2000);
            
            Assert.IsTrue(resultado);
            Assert.AreEqual(1500, tarjeta.Saldo); // -500 + 2000
        }

        [Test]
        public void TestBoletoGratuitoLimpiaViajesDeOtroDia()
        {
            DateTime ayer = new DateTime(2024, 1, 14, 10, 0, 0);
            DateTime hoy = new DateTime(2024, 1, 15, 10, 0, 0);
            
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(5000);
            
            // Establecer viajes de ayer
            var viajesAyer = new List<DateTime> { ayer, ayer.AddHours(1) };
            tarjeta.SetViajesParaTesting(viajesAyer);
            
            // Cambiar a hoy
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            
            // Verificar que se limpien los viajes de ayer
            Assert.AreEqual(0, tarjeta.ViajesHoy());
        }

        [Test]
        public void TestMedioBoletoLimpiaViajesDeOtroDia()
        {
            DateTime ayer = new DateTime(2024, 1, 14, 10, 0, 0);
            DateTime hoy = new DateTime(2024, 1, 15, 10, 0, 0);
            
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(5000);
            
            // Establecer viajes de ayer
            var viajesAyer = new List<DateTime> { ayer, ayer.AddHours(1) };
            tarjeta.SetViajesParaTesting(viajesAyer);
            
            // Cambiar a hoy
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            
            // Verificar que se limpien los viajes de ayer
            Assert.AreEqual(0, tarjeta.ViajesHoy());
        }

        [Test]
        public void TestColectivoPagarConTarjetaNullRetornaNull()
        {
            Colectivo colectivo = new Colectivo("142");
            Boleto boleto = colectivo.PagarCon(null);
            Assert.IsNull(boleto);
        }

        [Test]
        public void TestBoletoConstructorSimplificadoConFecha()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 10, 0, 0);
            Boleto boleto = new Boleto(1580, "142", 3420, "Normal", "TEST001", 1580, fecha);
            
            Assert.AreEqual(fecha, boleto.Fecha);
            Assert.AreEqual(0, boleto.DescuentoFrecuente);
            Assert.IsFalse(boleto.EsTrasbordo);
        }

        [Test]
        public void TestBoletoConstructorSimplificadoSinFecha()
        {
            DateTime antes = DateTimeProvider.Now;
            Boleto boleto = new Boleto(1580, "142", 3420, "Normal", "TEST001", 1580, 0);
            DateTime despues = DateTimeProvider.Now;
            
            Assert.IsTrue(boleto.Fecha >= antes && boleto.Fecha <= despues);
        }

        [Test]
        public void TestTarjetaViajesEsteMesLlamaCambioMes()
        {
            DateTime enero = new DateTime(2024, 1, 15, 10, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => enero);
            
            Tarjeta tarjeta = new Tarjeta(10000);
            tarjeta.SetViajesParaTesting(40, enero);
            
            Assert.AreEqual(40, tarjeta.ViajesEsteMes);
            
            // Cambiar a febrero
            DateTime febrero = new DateTime(2024, 2, 1, 10, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => febrero);
            
            // Debería reiniciar el contador
            Assert.AreEqual(0, tarjeta.ViajesEsteMes);
        }

        [Test]
        public void TestCalcularMontoConDescuentoFrecuenteNoAplicaAFranquicias()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 10, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            
            // Medio Boleto
            MedioBoletoEstudiantil mb = new MedioBoletoEstudiantil(10000);
            mb.SetViajesParaTesting(40, fecha);
            int montoMB = mb.CalcularMontoConDescuentoFrecuente(1580);
            Assert.AreEqual(1580, montoMB); // No aplica descuento frecuente
            
            // Boleto Gratuito
            BoletoGratuitoEstudiantil bg = new BoletoGratuitoEstudiantil(10000);
            bg.SetViajesParaTesting(40, fecha);
            int montoBG = bg.CalcularMontoConDescuentoFrecuente(1580);
            Assert.AreEqual(1580, montoBG);
            
            // Franquicia Completa
            FranquiciaCompleta fc = new FranquiciaCompleta();
            fc.SetViajesParaTesting(40, fecha);
            int montoFC = fc.CalcularMontoConDescuentoFrecuente(0);
            Assert.AreEqual(0, montoFC);
        }
    }
}