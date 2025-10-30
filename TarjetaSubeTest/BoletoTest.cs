
using NUnit.Framework;
using Tarjeta;
using System;

namespace Tarjeta.Tests
{
    [TestFixture]
    public class BoletoCompletoTests
    {
        [Test]
        public void TestBoletoTarjetaNormal()
        {
            DateTime fechaFija = new DateTime(2024, 1, 15, 14, 30, 0);
            Tarjeta tarjeta = new Tarjeta(5000, "TEST001");
            Colectivo colectivo = new Colectivo("142");
            
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
            DateTime fechaFija = new DateTime(2024, 1, 15, 14, 30, 0);
            FranquiciaCompleta tarjeta = new FranquiciaCompleta(0, "FC001");
            Colectivo colectivo = new Colectivo("K");
            
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
            DateTime fechaFija = new DateTime(2024, 1, 15, 14, 30, 0);
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(1000, "MB001");
            Colectivo colectivo = new Colectivo("144");
            
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
            DateTime fechaFija = new DateTime(2024, 1, 15, 14, 30, 0);
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(0, "BG001");
            Colectivo colectivo = new Colectivo("135");
            
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
        public void TestBoletoConSaldoNegativoYRecargo()
        {
            DateTime fechaFija = new DateTime(2024, 1, 15, 14, 30, 0);
            Tarjeta tarjeta = new Tarjeta(500, "TEST002");
            Colectivo colectivo = new Colectivo("K");
            
            // 500 - 1580 = -1080 (dentro del límite -1200)
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto);
            Assert.AreEqual(1580, boleto.Monto);
            Assert.AreEqual("K", boleto.Linea);
            Assert.AreEqual(-1080, boleto.SaldoRestante);
            Assert.AreEqual("Normal", boleto.TipoTarjeta);
            Assert.AreEqual("TEST002", boleto.IdTarjeta);
            // Total abonado = monto (1580) - saldo negativo anterior (0) = 1580
            Assert.AreEqual(1580, boleto.MontoTotalAbonado);

        }

        [Test]
        public void TestBoletoConSaldoNegativoPrevio()
        {
            DateTime fechaFija = new DateTime(2024, 1, 15, 14, 30, 0);
            Tarjeta tarjeta = new Tarjeta(-500, "TEST003");
            Colectivo colectivo = new Colectivo("142");
            
            // Ya tiene -500, paga 1580: nuevo saldo = -2080 (supera límite -1200)
            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNull(boleto); // No debería permitir el viaje
            Assert.AreEqual(-500, tarjeta.Saldo); // Saldo no cambia
        }

        [Test]
        public void TestBoletoConConstructorFechaEspecifica()
        {
            DateTime fechaEspecifica = new DateTime(2024, 1, 15, 14, 30, 0);
            Boleto boleto = new Boleto(1580, "142", 3420, "Normal", "TEST004", 
                                     1580,  fechaEspecifica);
            
            Assert.AreEqual(fechaEspecifica, boleto.Fecha);
            Assert.AreEqual(1580, boleto.Monto);
            Assert.AreEqual("142", boleto.Linea);
            Assert.AreEqual(3420, boleto.SaldoRestante);
            Assert.AreEqual("Normal", boleto.TipoTarjeta);
            Assert.AreEqual("TEST004", boleto.IdTarjeta);
            Assert.AreEqual(1580, boleto.MontoTotalAbonado);

        }

        [Test]
        public void TestBoletoManipulacionFechas()
        {
            DateTime fechaBase = new DateTime(2024, 1, 1, 10, 0, 0);
            
            // Test con fecha específica
            Boleto boleto1 = new Boleto(1580, "K", 5000, "Normal", "TEST005", 
                                      1580,  fechaBase);
            Assert.AreEqual(fechaBase, boleto1.Fecha);
            
            // Test sumando días
            DateTime fechaConDias = fechaBase.AddDays(5);
            Boleto boleto2 = new Boleto(1580, "142", 5000, "Normal", "TEST006", 
                                      1580,  fechaConDias);
            Assert.AreEqual(fechaBase.AddDays(5), boleto2.Fecha);
            
            // Test sumando horas
            DateTime fechaConHoras = fechaBase.AddHours(2.5);
            Boleto boleto3 = new Boleto(1580, "144", 5000, "Normal", "TEST007", 
                                      1580,  fechaConHoras);
            Assert.AreEqual(fechaBase.AddHours(2.5), boleto3.Fecha);
        }

        [Test]
        public void TestTodosLosTiposTarjetaEnBoletos()
        {
            Colectivo colectivo = new Colectivo("K");
            
            // Tarjeta Normal
            Tarjeta normal = new Tarjeta(5000, "NORMAL01");
            Boleto boletoNormal = colectivo.PagarCon(normal);
            Assert.AreEqual("Normal", boletoNormal.TipoTarjeta);
            
            // Franquicia Completa
            FranquiciaCompleta fc = new FranquiciaCompleta(0, "FRANQ01");
            Boleto boletoFC = colectivo.PagarCon(fc);
            Assert.AreEqual("Franquicia Completa", boletoFC.TipoTarjeta);
            
            // Medio Boleto
            MedioBoletoEstudiantil mb = new MedioBoletoEstudiantil(1000, "MEDIO01");
            Boleto boletoMB = colectivo.PagarCon(mb);
            Assert.AreEqual("Medio Boleto Estudiantil", boletoMB.TipoTarjeta);
            
            // Boleto Gratuito
            BoletoGratuitoEstudiantil bg = new BoletoGratuitoEstudiantil(0, "GRATIS01");
            Boleto boletoBG = colectivo.PagarCon(bg);
            Assert.AreEqual("Boleto Gratuito Estudiantil", boletoBG.TipoTarjeta);
        }
    }
}
