using System;
using System.Collections.Generic;

namespace Tarjeta
{
    public class Tarjeta
    {
        private int saldo;
        private int saldoPendiente;
        private const int LIMITE_SALDO = 56000;
        private const int LIMITE_NEGATIVO = -1200;
        private string id;
        private int viajesEsteMes;
        private DateTime ultimoViajeMes;

        private static readonly HashSet<int> CargasAceptadas = new HashSet<int>
        {
            2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000
        };

        public Tarjeta(int saldoInicial = 0)
        {
            this.saldo = saldoInicial;
            this.saldoPendiente = 0;
            this.id = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            this.viajesEsteMes = 0;
            this.ultimoViajeMes = DateTime.MinValue;
        }

        public Tarjeta(int saldoInicial, string id)
        {
            this.saldo = saldoInicial;
            this.saldoPendiente = 0;
            this.id = id;
            this.viajesEsteMes = 0;
            this.ultimoViajeMes = DateTime.MinValue;
        }

        public int Saldo
        {
            get { return saldo; }
        }

        public int SaldoPendiente
        {
            get { return saldoPendiente; }
        }

        public string Id
        {
            get { return id; }
        }

        public int ViajesEsteMes
        {
            get 
            { 
                VerificarCambioDeMes();
                return viajesEsteMes; 
            }
        }

        public virtual string TipoTarjeta
        {
            get { return "Normal"; }
        }

        public virtual bool Cargar(int importe)
        {
            if (!CargasAceptadas.Contains(importe))
            {
                return false;
            }

            int espacioDisponible = LIMITE_SALDO - saldo;
            int montoAAcreditar = Math.Min(importe, espacioDisponible);
            int montoPendiente = importe - montoAAcreditar;

            saldo += montoAAcreditar;
            
            if (montoPendiente > 0)
            {
                saldoPendiente += montoPendiente;
            }

            return true;
        }

        public void AcreditarCarga()
        {
            if (saldoPendiente > 0)
            {
                int espacioDisponible = LIMITE_SALDO - saldo;
                int montoAAcreditar = Math.Min(saldoPendiente, espacioDisponible);
                
                saldo += montoAAcreditar;
                saldoPendiente -= montoAAcreditar;
            }
        }

        public virtual bool Descontar(int monto)
        {
            if (saldo - monto >= LIMITE_NEGATIVO)
            {
                saldo -= monto;
                
                AcreditarCarga();
                
                return true;
            }

            return false;
        }

        public virtual int CalcularMontoPasaje(int tarifaBase)
        {
            return tarifaBase;
        }

        public virtual int CalcularMontoRealAPagar(int tarifaBase)
        {
            int montoBase = CalcularMontoPasaje(tarifaBase);
            return CalcularMontoConDescuentoFrecuente(montoBase);
        }

        public virtual int CalcularMontoConDescuentoFrecuente(int tarifaBase)
        {
            VerificarCambioDeMes();

            viajesEsteMes++; // Contar el viaje actual
            
            // Solo aplica para tarjetas normales (no para las especiales)
            if (this is MedioBoletoEstudiantil || this is BoletoGratuitoEstudiantil || this is FranquiciaCompleta)
            {
            viajesEsteMes--; // Revertir el conteo del viaje actual

                return tarifaBase;
            }

            // Aplicar descuentos según cantidad de viajes
            if (viajesEsteMes >= 30 && viajesEsteMes <= 59)
            {
                // 20% de descuento
            viajesEsteMes--; // Revertir el conteo del viaje actual
             
                return (int)(tarifaBase * 0.8);
            }
            else if (viajesEsteMes >= 60 && viajesEsteMes <= 80)
            {
                // 25% de descuento
            viajesEsteMes--; // Revertir el conteo del viaje actual
                
                return (int)(tarifaBase * 0.75);
            }
            else
            {
                // Viajes 1-29 y 81+ : tarifa normal
            viajesEsteMes--; // Revertir el conteo del viaje actual

                return tarifaBase;
            }

        }

        public void RegistrarViaje()
        {
            VerificarCambioDeMes();
            viajesEsteMes++;
            ultimoViajeMes = DateTimeProvider.Now;
        }

        public int ObtenerDescuentoAplicado(int tarifaBase)
        {
            int montoConDescuento = CalcularMontoConDescuentoFrecuente(tarifaBase);
            return tarifaBase - montoConDescuento;
        }

        private void VerificarCambioDeMes()
        {
            DateTime ahora = DateTimeProvider.Now;
            
            // Si es el primer viaje o cambió el mes, reiniciar contador
            if (ultimoViajeMes == DateTime.MinValue || 
                ultimoViajeMes.Month != ahora.Month || 
                ultimoViajeMes.Year != ahora.Year)
            {
                viajesEsteMes = 0;
                ultimoViajeMes = ahora;
            }
        }

        // Método para testing - permite establecer viajes específicos
        public void SetViajesParaTesting(int viajes, DateTime ultimoViaje)
        {
            this.viajesEsteMes = viajes;
            this.ultimoViajeMes = ultimoViaje;
        }
    }
}