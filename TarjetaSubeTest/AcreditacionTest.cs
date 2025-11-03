using NUnit.Framework;
using Tarjeta;

namespace Tarjeta.Tests
{
    [TestFixture]
    public class CargaLimiteTests
    {
        [Test]
        public void TestCargaQueSuperaLimite()
        {
            // Configurar tarjeta cerca del límite
            Tarjeta tarjeta = new Tarjeta(50000, "TEST001");
            
            // Intentar cargar 10000 (supera el límite de 56000)
            bool resultado = tarjeta.Cargar(10000);
            
            Assert.IsTrue(resultado, "La carga debería ser exitosa");
            Assert.AreEqual(56000, tarjeta.Saldo, "El saldo debería llegar al límite máximo");
            Assert.AreEqual(4000, tarjeta.SaldoPendiente, "El excedente debería quedar como pendiente");
        }

        [Test]
        public void TestCargaExactamenteAlLimite()
        {
            // Configurar tarjeta cerca del límite
            Tarjeta tarjeta = new Tarjeta(51000, "TEST002");
            
            // Cargar 5000 (llegaría exactamente a 56000)
            bool resultado = tarjeta.Cargar(5000);
            
            Assert.IsTrue(resultado);
            Assert.AreEqual(56000, tarjeta.Saldo, "El saldo debería ser exactamente el límite");
            Assert.AreEqual(0, tarjeta.SaldoPendiente, "No debería haber saldo pendiente");
        }

        [Test]
        public void TestCargaConSaldoPendienteExistente()
        {
            // Configurar tarjeta con saldo pendiente
            Tarjeta tarjeta = new Tarjeta(55000, "TEST003");
            tarjeta.Cargar(5000); // 55000 + 5000 = 60000 -> 56000 + 4000 pendiente
            
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);
            
            // Intentar cargar más
            bool resultado = tarjeta.Cargar(3000);
            
            Assert.IsTrue(resultado);
            Assert.AreEqual(56000, tarjeta.Saldo, "El saldo debería mantenerse en el límite");
            Assert.AreEqual(7000, tarjeta.SaldoPendiente, "El saldo pendiente debería acumularse"); // 4000 + 3000
        }

        [Test]
        public void TestAcreditarCargaManual()
        {
            // Configurar tarjeta con saldo pendiente
            Tarjeta tarjeta = new Tarjeta(55000, "TEST004");
            tarjeta.Cargar(5000); // 56000 saldo + 4000 pendiente
            
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);
            
            // Usar parte del saldo
            tarjeta.Descontar(1000); // Ahora saldo = 55000
            
            // Acreditar manualmente
            tarjeta.AcreditarCarga();
            
            Assert.AreEqual(56000, tarjeta.Saldo, "Debería acreditarse hasta el límite");
            Assert.AreEqual(3000, tarjeta.SaldoPendiente, "Debería quedar el resto como pendiente"); // 4000 - 1000
        }

        [Test]
        public void TestViajeAcreditaSaldoPendiente()
        {
            // Configurar tarjeta con saldo pendiente
            Tarjeta tarjeta = new Tarjeta(55500, "TEST005");
            tarjeta.Cargar(5000); // 56000 saldo + 4500 pendiente (55500 + 5000 = 60500 -> 56000 + 4500)
            
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4500, tarjeta.SaldoPendiente);
            
            // Realizar un viaje
            Colectivo colectivo = new Colectivo("142");
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(1580, boleto.Monto);
            
            // Después del viaje, debería haberse acreditado saldo pendiente
            // Saldo después del viaje: 56000 - 1580 = 54420
            // Espacio disponible: 56000 - 54420 = 1580
            // Se acredita: min(4500, 1580) = 1580
            // Nuevo saldo: 54420 + 1580 = 56000
            // Saldo pendiente restante: 4500 - 1580 = 2920
            Assert.AreEqual(56000, tarjeta.Saldo, "Debería acreditarse saldo pendiente después del viaje");
            Assert.AreEqual(2920, tarjeta.SaldoPendiente, "Debería reducirse el saldo pendiente");
        }

        [Test]
        public void TestMultiplesViajesAcreditanSaldoPendiente()
        {
            // Configurar tarjeta con saldo pendiente
            Tarjeta tarjeta = new Tarjeta(55000, "TEST006");
            tarjeta.Cargar(5000); // 56000 saldo + 4000 pendiente
            
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);
            
            // Primer viaje
            Colectivo colectivo = new Colectivo("142");
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            
            // Después del primer viaje:
            // Saldo: 56000 - 1580 = 54420
            // Se acredita: min(4000, 1580) = 1580
            // Nuevo saldo: 54420 + 1580 = 56000
            // Pendiente: 4000 - 1580 = 2420
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(2420, tarjeta.SaldoPendiente);
            
            // Segundo viaje
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            
            // Después del segundo viaje:
            // Saldo: 56000 - 1580 = 54420
            // Se acredita: min(2420, 1580) = 1580
            // Nuevo saldo: 54420 + 1580 = 56000
            // Pendiente: 2420 - 1580 = 840
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(840, tarjeta.SaldoPendiente);
            
            // Tercer viaje
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            
            // Después del tercer viaje:
            // Saldo: 56000 - 1580 = 54420
            // Se acredita: min(840, 1580) = 840
            // Nuevo saldo: 54420 + 840 = 55260
            // Pendiente: 0
            Assert.AreEqual(55260, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente, "Todo el saldo pendiente debería haberse acreditado");
        }

        [Test]
        public void TestCargaGrandeConMultiplesAcreditaciones()
        {
            // Configurar tarjeta vacía
            Tarjeta tarjeta = new Tarjeta(0, "TEST007");
            
            // Cargar 30000 (dentro del límite)
            tarjeta.Cargar(30000);
            Assert.AreEqual(30000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente);
            
            // Cargar 30000 más (supera el límite)
            tarjeta.Cargar(30000);
            // 30000 + 30000 = 60000 -> 56000 + 4000 pendiente
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(4000, tarjeta.SaldoPendiente);
            
            // Usar la tarjeta varias veces para acreditar el saldo pendiente
            Colectivo colectivo = new Colectivo("142");
            
            // Primer viaje
            colectivo.PagarCon(tarjeta);
            // Saldo: 56000 - 1580 = 54420 + 1580 acreditado = 56000
            // Pendiente: 4000 - 1580 = 2420
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(2420, tarjeta.SaldoPendiente);
            
            // Segundo viaje
            colectivo.PagarCon(tarjeta);
            // Saldo: 56000 - 1580 = 54420 + 1580 acreditado = 56000
            // Pendiente: 2420 - 1580 = 840
            Assert.AreEqual(56000, tarjeta.Saldo);
            Assert.AreEqual(840, tarjeta.SaldoPendiente);
            
            // Tercer viaje (acredita el resto)
            colectivo.PagarCon(tarjeta);
            // Saldo: 56000 - 1580 = 54420 + 840 acreditado = 55260
            // Pendiente: 0
            Assert.AreEqual(55260, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente);
        }

        [Test]
        public void TestCargaConTarjetasEspeciales()
        {
            // Test con Medio Boleto
            DateTime hoy = new DateTime(2024, 1, 15, 21, 50, 0);
            DateTimeProvider.SetDateTimeProvider(() => hoy);
            MedioBoletoEstudiantil medioBoleto = new MedioBoletoEstudiantil(55000, "MB001");
            medioBoleto.Cargar(5000); // 56000 + 4000 pendiente
            
            Assert.AreEqual(56000, medioBoleto.Saldo);
            Assert.AreEqual(4000, medioBoleto.SaldoPendiente);
            
            // Realizar viaje (medio boleto)
            Colectivo colectivo = new Colectivo("142");
            Boleto boleto = colectivo.PagarCon(medioBoleto);
            
            Assert.AreEqual(56000, medioBoleto.Saldo);
            Assert.AreEqual(3210, medioBoleto.SaldoPendiente);
            
            // Test con Boleto Gratuito
            BoletoGratuitoEstudiantil boletoGratuito = new BoletoGratuitoEstudiantil(55000, "BG001");
            boletoGratuito.Cargar(5000); // 56000 + 4000 pendiente
            
            // Primer viaje gratuito
            Boleto boletoGratis = colectivo.PagarCon(boletoGratuito);
            Assert.AreEqual(0, boletoGratis.Monto);
            // Como no se descuenta saldo, no se acredita pendiente
            Assert.AreEqual(56000, boletoGratuito.Saldo);
            Assert.AreEqual(4000, boletoGratuito.SaldoPendiente);
        }

        [Test]
        public void TestCargaMontoNoAceptadoConSaldoPendiente()
        {
            Tarjeta tarjeta = new Tarjeta(55000, "TEST008");
            
            // Intentar cargar monto no aceptado
            bool resultado = tarjeta.Cargar(1500);
            
            Assert.IsFalse(resultado, "No debería aceptar montos no válidos");
            Assert.AreEqual(55000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente, "No debería haber saldo pendiente con carga fallida");
        }

        [Test]
        public void TestAcreditarCargaSinSaldoPendiente()
        {
            Tarjeta tarjeta = new Tarjeta(10000, "TEST009");
            
            // Acreditar sin saldo pendiente
            tarjeta.AcreditarCarga();
            
            Assert.AreEqual(10000, tarjeta.Saldo);
            Assert.AreEqual(0, tarjeta.SaldoPendiente, "No debería cambiar nada sin saldo pendiente");
        }
    }
}