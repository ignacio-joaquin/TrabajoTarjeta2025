
using NUnit.Framework;
using Tarjeta;
using System;
using System.Collections.Generic;

namespace Tarjeta.Tests
{
    [TestFixture]
    public class BoletoFrecuenteTests
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
        public void TestViajes1a29_TarifaNormal()
        {
            // Configurar tarjeta normal CON SUFICIENTE SALDO
            Tarjeta tarjeta = new Tarjeta(50000, "NORMAL001"); // Saldo suficiente para muchos viajes
            Colectivo colectivo = new Colectivo("142");
            
            DateTime fecha = new DateTime(2024, 1, 15, 10, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);

            // Viajes 1-29 deberían ser tarifa normal
            for (int i = 1; i <= 29; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta);
                Assert.IsNotNull(boleto, $"El boleto {i} no debería ser null - saldo: {tarjeta.Saldo}");
                Assert.AreEqual(1580, boleto.Monto, $"Viaje {i} debería ser tarifa normal");
                Assert.AreEqual(i, tarjeta.ViajesEsteMes, $"Debería tener {i} viajes este mes");
                
                // Avanzar el tiempo para evitar problemas con el mismo timestamp
                DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(i));
            }
        }

        [Test]
        public void TestViajes30a59_20PorcientoDescuento()
        {
            // Configurar tarjeta con 29 viajes ya realizados Y SUFICIENTE SALDO
            Tarjeta tarjeta = new Tarjeta(50000, "NORMAL002");
            Colectivo colectivo = new Colectivo("K");
            
            DateTime fecha = new DateTime(2024, 1, 15, 10, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);

            // Simular 29 viajes previos (usando el método de testing)
            tarjeta.SetViajesParaTesting(29, fecha.AddMinutes(-10));

            // Viaje 30 - debería tener 20% de descuento
            Boleto boleto30 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto30, "El boleto 30 no debería ser null");
            Assert.AreEqual(30, tarjeta.ViajesEsteMes);
            Assert.AreEqual((int)(1580 * 0.8), boleto30.Monto, "Viaje 30 debería tener 20% de descuento");
            Assert.AreEqual(1264, boleto30.Monto); // 1580 * 0.8 = 1264


            // Viaje 31 también con 20% de descuento
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(1));
            Boleto boleto31 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto31);
            Assert.AreEqual(1264, boleto31.Monto, "Viaje 31 debería tener 20% de descuento");
            Assert.AreEqual(31, tarjeta.ViajesEsteMes);
        }

        [Test]
        public void TestViajes60a80_25PorcientoDescuento()
        {
            // Configurar tarjeta con 59 viajes ya realizados Y SUFICIENTE SALDO
            Tarjeta tarjeta = new Tarjeta(50000, "NORMAL003");
            Colectivo colectivo = new Colectivo("144");
            
            DateTime fecha = new DateTime(2024, 1, 20, 14, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);

            // Simular 59 viajes previos
            tarjeta.SetViajesParaTesting(59, fecha.AddMinutes(-10));

            // Viaje 60 - debería tener 25% de descuento
            Boleto boleto60 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto60, "El boleto 60 no debería ser null");
            Assert.AreEqual((int)(1580 * 0.75), boleto60.Monto, "Viaje 60 debería tener 25% de descuento");
            Assert.AreEqual(1185, boleto60.Monto); // 1580 * 0.75 = 1185
            Assert.AreEqual(60, tarjeta.ViajesEsteMes);

            // Viaje 61 también con 25% de descuento
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(1));
            Boleto boleto61 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto61);
            Assert.AreEqual(1185, boleto61.Monto, "Viaje 61 debería tener 25% de descuento");
            Assert.AreEqual(61, tarjeta.ViajesEsteMes);
        }

        [Test]
        public void TestViajes81EnAdelante_TarifaNormal()
        {
            // Configurar tarjeta con 80 viajes ya realizados Y SUFICIENTE SALDO
            Tarjeta tarjeta = new Tarjeta(50000, "NORMAL004");
            Colectivo colectivo = new Colectivo("142");
            
            DateTime fecha = new DateTime(2024, 1, 25, 16, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);

            // Simular 80 viajes previos
            tarjeta.SetViajesParaTesting(80, fecha.AddMinutes(-10));

            // Viaje 81 - debería volver a tarifa normal
            Boleto boleto81 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto81, "El boleto 81 no debería ser null");
            Assert.AreEqual(1580, boleto81.Monto, "Viaje 81 debería ser tarifa normal");
            Assert.AreEqual(81, tarjeta.ViajesEsteMes);

            // Viaje 82 también tarifa normal
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(1));
            Boleto boleto82 = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(1580, boleto82.Monto, "Viaje 82 debería ser tarifa normal");
            Assert.AreEqual(82, tarjeta.ViajesEsteMes);
        }

        [Test]
        public void TestReinicioContadorCambioDeMes()
        {
            // Configurar tarjeta con muchos viajes en enero
            Tarjeta tarjeta = new Tarjeta(50000, "NORMAL005");
            Colectivo colectivo = new Colectivo("K");
            
            DateTime finEnero = new DateTime(2024, 1, 31, 23, 50, 0);
            DateTimeProvider.SetDateTimeProvider(() => finEnero);

            // Simular 40 viajes en enero
            tarjeta.SetViajesParaTesting(40, finEnero.AddMinutes(-10));

            // Último viaje en enero (con descuento 20%)
            Boleto boletoEnero = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boletoEnero);
            Assert.AreEqual(1264, boletoEnero.Monto, "Último viaje de enero debería tener 20% de descuento");
            Assert.AreEqual(41, tarjeta.ViajesEsteMes);

            // Primer viaje en febrero (debería reiniciar contador - tarifa normal)
            DateTime inicioFebrero = new DateTime(2024, 2, 1, 0, 10, 0);
            DateTimeProvider.SetDateTimeProvider(() => inicioFebrero);
            
            Boleto boletoFebrero = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boletoFebrero);
            Assert.AreEqual(1580, boletoFebrero.Monto, "Primer viaje de febrero debería ser tarifa normal");
            Assert.AreEqual(1, tarjeta.ViajesEsteMes, "Contador debería reiniciarse en nuevo mes");
        }

        [Test]
        public void TestSoloTarjetasNormalesAplicanDescuento()
        {
            DateTime fecha = new DateTime(2024, 1, 15, 10, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);
            Colectivo colectivo = new Colectivo("142");

            // Medio Boleto - no aplica descuento frecuente
            MedioBoletoEstudiantil medioBoleto = new MedioBoletoEstudiantil(10000, "MB001");
            medioBoleto.SetViajesParaTesting(40, fecha.AddMinutes(-10)); // 40 viajes este mes
            
            Boleto boletoMB = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(boletoMB);
            Assert.AreEqual(790, boletoMB.Monto, "Medio boleto debería ser 790 (mitad de 1580)");
            // No aplica el 20% de descuento aunque tenga 40 viajes

            // Boleto Gratuito - no aplica descuento frecuente
            BoletoGratuitoEstudiantil boletoGratuito = new BoletoGratuitoEstudiantil(10000, "BG001");
            boletoGratuito.SetViajesParaTesting(40, fecha.AddMinutes(-10)); // 40 viajes este mes
            
            // Hacer dos viajes gratuitos primero
            colectivo.PagarCon(boletoGratuito);
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(1));
            colectivo.PagarCon(boletoGratuito);
            
            // Tercer viaje (debería ser tarifa completa, sin descuento frecuente)
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(2));
            Boleto boletoBG = colectivo.PagarCon(boletoGratuito);
            Assert.IsNotNull(boletoBG);
            Assert.AreEqual(1580, boletoBG.Monto, "Boleto gratuito en tercer viaje debería ser tarifa completa sin descuento frecuente");

            // Franquicia Completa - no aplica descuento frecuente
            FranquiciaCompleta franquicia = new FranquiciaCompleta(0, "FC001");
            franquicia.SetViajesParaTesting(40, fecha.AddMinutes(-10)); // 40 viajes este mes
            
            Boleto boletoFC = colectivo.PagarCon(franquicia);
            Assert.IsNotNull(boletoFC);
            Assert.AreEqual(0, boletoFC.Monto, "Franquicia completa siempre es gratuita");
        }

        [Test]
        public void TestCalculoDescuentoExacto()
        {
            Tarjeta tarjeta = new Tarjeta(10000, "NORMAL007");
            
            // Viaje 35 (dentro del rango 30-59)
            tarjeta.SetViajesParaTesting(35, DateTimeProvider.Now);
            int descuento35 = tarjeta.ObtenerDescuentoAplicado(1580);
            Assert.AreEqual(316, descuento35); // 1580 * 0.2 = 316
            
            int monto35 = tarjeta.CalcularMontoConDescuentoFrecuente(1580);
            Assert.AreEqual(1264, monto35); // 1580 - 316 = 1264

            // Viaje 65 (dentro del rango 60-80)
            tarjeta.SetViajesParaTesting(65, DateTimeProvider.Now);
            int descuento65 = tarjeta.ObtenerDescuentoAplicado(1580);
            Assert.AreEqual(395, descuento65); // 1580 * 0.25 = 395
            
            int monto65 = tarjeta.CalcularMontoConDescuentoFrecuente(1580);
            Assert.AreEqual(1185, monto65); // 1580 - 395 = 1185

            // Viaje 85 (fuera del rango de descuento)
            tarjeta.SetViajesParaTesting(85, DateTimeProvider.Now);
            int descuento85 = tarjeta.ObtenerDescuentoAplicado(1580);
            Assert.AreEqual(0, descuento85);
            
            int monto85 = tarjeta.CalcularMontoConDescuentoFrecuente(1580);
            Assert.AreEqual(1580, monto85);
        }

        [Test]
        public void TestTransicionEntreRangos()
        {
            Tarjeta tarjeta = new Tarjeta(50000, "NORMAL008");
            Colectivo colectivo = new Colectivo("K");
            
            DateTime fecha = new DateTime(2024, 1, 15, 10, 0, 0);
            DateTimeProvider.SetDateTimeProvider(() => fecha);

            // Viaje 29 (último sin descuento)
            tarjeta.SetViajesParaTesting(28, fecha.AddMinutes(-10));
            Boleto boleto29 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto29);
            Assert.AreEqual(1580, boleto29.Monto, "Viaje 29 debería ser tarifa normal");
            Console.WriteLine(boleto29);

            // Viaje 30 (primero con 20% de descuento)
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(1));
            tarjeta.SetViajesParaTesting(29, fecha.AddMinutes(1));
            Boleto boleto30 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto30);
            Assert.AreEqual(1264, boleto30.Monto, "Viaje 30 debería tener 20% de descuento");

            // Viaje 59 (último con 20% de descuento)
            tarjeta.SetViajesParaTesting(58, fecha.AddMinutes(2));
            Boleto boleto59 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto59);
            Assert.AreEqual(1264, boleto59.Monto, "Viaje 59 debería tener 20% de descuento");

            // Viaje 60 (primero con 25% de descuento)
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(3));
            tarjeta.SetViajesParaTesting(59, fecha.AddMinutes(3));
            Boleto boleto60 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto60);
            Assert.AreEqual(1185, boleto60.Monto, "Viaje 60 debería tener 25% de descuento");

            // Viaje 80 (último con 25% de descuento)
            tarjeta.SetViajesParaTesting(79, fecha.AddMinutes(4));
            Boleto boleto80 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto80);
            Assert.AreEqual(1185, boleto80.Monto, "Viaje 80 debería tener 25% de descuento");

            // Viaje 81 (vuelve a tarifa normal)
            DateTimeProvider.SetDateTimeProvider(() => fecha.AddMinutes(5));
            tarjeta.SetViajesParaTesting(80, fecha.AddMinutes(5));
            Boleto boleto81 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto81);
            Assert.AreEqual(1580, boleto81.Monto, "Viaje 81 debería ser tarifa normal");
        }
    }
}
