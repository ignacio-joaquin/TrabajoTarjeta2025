
using NUnit.Framework;
using Tarjeta;
using System;

namespace Tarjeta.Tests
{
    [TestFixture]
    public class FranquiciaHorariaTests
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
        public void TestMedioBoletoEstudiantil_FueraDeFranjaHoraria_Noche()
        {
            // Configurar fuera de franja horaria (noche)
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(5000, "MB001");
            Colectivo colectivo = new Colectivo("142");
            
            // Viernes a las 23:00 (fuera de franja)
            DateTime fueraFranjaNoche = new DateTime(2024, 1, 19, 23, 0, 0); // Viernes 23:00
            DateTimeProvider.SetDateTimeProvider(() => fueraFranjaNoche);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNull(boleto, "No debería permitir viaje fuera de franja horaria (noche)");
        }

        [Test]
        public void TestMedioBoletoEstudiantil_FueraDeFranjaHoraria_Madrugada()
        {
            // Configurar fuera de franja horaria (madrugada)
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(5000, "MB002");
            Colectivo colectivo = new Colectivo("K");
            
            // Lunes a las 5:00 (fuera de franja)
            DateTime fueraFranjaMadrugada = new DateTime(2024, 1, 15, 5, 0, 0); // Lunes 5:00
            DateTimeProvider.SetDateTimeProvider(() => fueraFranjaMadrugada);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNull(boleto, "No debería permitir viaje fuera de franja horaria (madrugada)");
        }

        [Test]
        public void TestMedioBoletoEstudiantil_DentroDeFranjaHoraria()
        {
            // Configurar dentro de franja horaria
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(5000, "MB003");
            Colectivo colectivo = new Colectivo("144");
            
            // Miércoles a las 14:00 (dentro de franja)
            DateTime dentroFranja = new DateTime(2024, 1, 17, 14, 0, 0); // Miércoles 14:00
            DateTimeProvider.SetDateTimeProvider(() => dentroFranja);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto, "Debería permitir viaje dentro de franja horaria");
            Assert.AreEqual(790, boleto.Monto, "Debería aplicar medio boleto");
        }

        [Test]
        public void TestBoletoGratuitoEstudiantil_FueraDeFranjaHoraria()
        {
            // Configurar fuera de franja horaria
            BoletoGratuitoEstudiantil tarjeta = new BoletoGratuitoEstudiantil(0, "BG001");
            Colectivo colectivo = new Colectivo("142");
            
            // Sábado a las 12:00 (fuera de franja - fin de semana)
            DateTime finDeSemana = new DateTime(2024, 1, 20, 12, 0, 0); // Sábado 12:00
            DateTimeProvider.SetDateTimeProvider(() => finDeSemana);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNull(boleto, "No debería permitir viaje en fin de semana");
        }

        [Test]
        public void TestFranquiciaCompleta_FueraDeFranjaHoraria()
        {
            // Configurar fuera de franja horaria
            FranquiciaCompleta tarjeta = new FranquiciaCompleta(0, "FC001");
            Colectivo colectivo = new Colectivo("142");
            
            // Viernes a las 22:30 (fuera de franja - justo después de las 22:00)
            DateTime fueraFranja = new DateTime(2024, 1, 19, 22, 30, 0); // Viernes 22:30
            DateTimeProvider.SetDateTimeProvider(() => fueraFranja);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNull(boleto, "No debería permitir viaje fuera de franja horaria");
        }

        [Test]
        public void TestTarjetaNormal_FueraDeFranjaHoraria_Permitido()
        {
            // Tarjeta normal debería funcionar en cualquier horario
            Tarjeta tarjeta = new Tarjeta(5000, "NORMAL001");
            Colectivo colectivo = new Colectivo("K");
            
            // Sábado a las 2:00 (fuera de franja para franquicias)
            DateTime fueraFranja = new DateTime(2024, 1, 20, 2, 0, 0); // Sábado 2:00
            DateTimeProvider.SetDateTimeProvider(() => fueraFranja);

            Boleto boleto = colectivo.PagarCon(tarjeta);
            
            Assert.IsNotNull(boleto, "Tarjeta normal debería funcionar en cualquier horario");
            Assert.AreEqual(1580, boleto.Monto, "Debería cobrar tarifa completa");
        }

        [Test]
        public void TestHorariosLimite_FranjaHoraria()
        {
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(5000, "MB004");
            Colectivo colectivo = new Colectivo("142");

            // Test límite inferior (6:00) - DEBERÍA funcionar
            DateTime limiteInferior = new DateTime(2024, 1, 15, 6, 0, 0); // Lunes 6:00
            DateTimeProvider.SetDateTimeProvider(() => limiteInferior);
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1, "Debería permitir viaje justo a las 6:00");

            // Test límite superior (21:59) - DEBERÍA funcionar
            DateTime limiteSuperior = new DateTime(2024, 1, 15, 21, 59, 0); // Lunes 21:59
            DateTimeProvider.SetDateTimeProvider(() => limiteSuperior);
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2, "Debería permitir viaje justo antes de las 22:00");

            // Test fuera de límite (22:00) - NO debería funcionar
            DateTime fueraLimite = new DateTime(2024, 1, 15, 22, 0, 0); // Lunes 22:00
            DateTimeProvider.SetDateTimeProvider(() => fueraLimite);
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto3, "No debería permitir viaje a las 22:00");
        }

        [Test]
        public void TestDiasDeSemana_FranjaHoraria()
        {
            MedioBoletoEstudiantil tarjeta = new MedioBoletoEstudiantil(5000, "MB005");
            Colectivo colectivo = new Colectivo("K");

            // Lunes - DEBERÍA funcionar
            DateTime lunes = new DateTime(2024, 1, 15, 12, 0, 0); // Lunes
            DateTimeProvider.SetDateTimeProvider(() => lunes);
            Boleto boletoLunes = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boletoLunes, "Debería permitir viaje el lunes");

            // Martes - DEBERÍA funcionar
            DateTime martes = new DateTime(2024, 1, 16, 12, 0, 0); // Martes
            DateTimeProvider.SetDateTimeProvider(() => martes);
            Boleto boletoMartes = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boletoMartes, "Debería permitir viaje el martes");

            // Miércoles - DEBERÍA funcionar
            DateTime miercoles = new DateTime(2024, 1, 17, 12, 0, 0); // Miércoles
            DateTimeProvider.SetDateTimeProvider(() => miercoles);
            Boleto boletoMiercoles = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boletoMiercoles, "Debería permitir viaje el miércoles");

            // Jueves - DEBERÍA funcionar
            DateTime jueves = new DateTime(2024, 1, 18, 12, 0, 0); // Jueves
            DateTimeProvider.SetDateTimeProvider(() => jueves);
            Boleto boletoJueves = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boletoJueves, "Debería permitir viaje el jueves");

            // Viernes - DEBERÍA funcionar
            DateTime viernes = new DateTime(2024, 1, 19, 12, 0, 0); // Viernes
            DateTimeProvider.SetDateTimeProvider(() => viernes);
            Boleto boletoViernes = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boletoViernes, "Debería permitir viaje el viernes");

            // Sábado - NO debería funcionar
            DateTime sabado = new DateTime(2024, 1, 20, 12, 0, 0); // Sábado
            DateTimeProvider.SetDateTimeProvider(() => sabado);
            Boleto boletoSabado = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boletoSabado, "No debería permitir viaje el sábado");

            // Domingo - NO debería funcionar
            DateTime domingo = new DateTime(2024, 1, 21, 12, 0, 0); // Domingo
            DateTimeProvider.SetDateTimeProvider(() => domingo);
            Boleto boletoDomingo = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boletoDomingo, "No debería permitir viaje el domingo");
        }

        [Test]
        public void TestTodasLasFranquicias_MismoComportamientoHorario()
        {
            Colectivo colectivo = new Colectivo("142");
            DateTime fueraFranja = new DateTime(2024, 1, 20, 12, 0, 0); // Sábado 12:00

            // Todas las franquicias deberían fallar fuera de franja
            MedioBoletoEstudiantil mb = new MedioBoletoEstudiantil(5000, "MB006");
            BoletoGratuitoEstudiantil bg = new BoletoGratuitoEstudiantil(0, "BG002");
            FranquiciaCompleta fc = new FranquiciaCompleta(0, "FC002");

            DateTimeProvider.SetDateTimeProvider(() => fueraFranja);

            Assert.IsNull(colectivo.PagarCon(mb), "Medio Boleto Estudiantil debería fallar");
            Assert.IsNull(colectivo.PagarCon(bg), "Boleto Gratuito debería fallar");
            Assert.IsNull(colectivo.PagarCon(fc), "Franquicia Completa debería fallar");
        }

        [Test]
        public void TestFranquiciaCompleta_Jubilados_Gratuita()
        {
            // La FranquiciaCompleta es para jubilados y es gratuita
            FranquiciaCompleta tarjetaJubilado = new FranquiciaCompleta(0, "JUBILADO001");
            Colectivo colectivo = new Colectivo("142");
            
            // Dentro de franja horaria
            DateTime dentroFranja = new DateTime(2024, 1, 15, 14, 0, 0); // Lunes 14:00
            DateTimeProvider.SetDateTimeProvider(() => dentroFranja);

            Boleto boleto = colectivo.PagarCon(tarjetaJubilado);
            
            Assert.IsNotNull(boleto, "Franquicia Completa (jubilados) debería funcionar en franja horaria");
            Assert.AreEqual(0, boleto.Monto, "Debería ser gratuito");
            Assert.AreEqual("Franquicia Completa", boleto.TipoTarjeta);
        }
    }
}
