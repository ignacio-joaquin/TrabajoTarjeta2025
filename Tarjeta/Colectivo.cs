using System;

namespace Tarjeta
{
    public class Colectivo
    {
        private const int TARIFA_BASICA = 1580;
        private string linea;

        public Colectivo(string linea)
        {
            this.linea = linea;
        }

        public string Linea
        {
            get { return linea; }
        }

        public Boleto PagarCon(Tarjeta tarjeta)
        {
            if (tarjeta == null)
            {
                return null;
            }

            // Validaciones específicas para Medio Boleto
            if (tarjeta is MedioBoletoEstudiantil medioBoleto)
            {
                if (!medioBoleto.PuedeViajar())
                {
                    return null; // No pasaron 5 minutos desde el último viaje
                }
            }

            // Calcular el monto base según el tipo de tarjeta
            int montoBase = tarjeta.CalcularMontoPasaje(TARIFA_BASICA);
            
            // Aplicar descuento por uso frecuente
            int montoAPagar = tarjeta.CalcularMontoConDescuentoFrecuente(montoBase);
            
            int saldoAnterior = tarjeta.Saldo;
            
            int montoTotalAbonado = montoAPagar;
            if (saldoAnterior < 0 && montoAPagar > 0)
            {
                montoTotalAbonado = montoAPagar - saldoAnterior;
            }

            if (!tarjeta.Descontar(montoAPagar))
            {
                return null;
            }

            // Registrar el viaje para todos los tipos de tarjeta
            tarjeta.RegistrarViaje();

            // Registrar el viaje específico para Medio Boleto (para control de 5 minutos)
            if (tarjeta is MedioBoletoEstudiantil medioBoletoRegistro)
            {
                medioBoletoRegistro.RegistrarViaje();
            }

            // Registrar el viaje específico para Boleto Gratuito (para control de 2 viajes gratis)
            if (tarjeta is BoletoGratuitoEstudiantil boletoGratuitoRegistro)
            {
                boletoGratuitoRegistro.RegistrarViaje();
            }

            // Calcular el descuento frecuente aplicado
            int descuentoFrecuente = montoBase - montoAPagar;

            return new Boleto(montoAPagar, this.linea, tarjeta.Saldo, 
                            tarjeta.TipoTarjeta, tarjeta.Id, montoTotalAbonado, descuentoFrecuente);
        }
    }
}