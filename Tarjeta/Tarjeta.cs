using System;
using System.Collections.Generic;

namespace Tarjeta
{
    public class Tarjeta
    {
        private int saldo;
        private int saldoPendiente;
        private const int LIMITE_SALDO = 56000; // Cambiado de 40000 a 56000
        private const int LIMITE_NEGATIVO = -1200;
        private string id;

        private static readonly HashSet<int> CargasAceptadas = new HashSet<int>
        {
            2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000
        };

        public Tarjeta(int saldoInicial = 0)
        {
            this.saldo = saldoInicial;
            this.saldoPendiente = 0;
            this.id = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }

        public Tarjeta(int saldoInicial, string id)
        {
            this.saldo = saldoInicial;
            this.saldoPendiente = 0;
            this.id = id;
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

            // Calcular cuánto se puede acreditar ahora
            int espacioDisponible = LIMITE_SALDO - saldo;
            int montoAAcreditar = Math.Min(importe, espacioDisponible);
            int montoPendiente = importe - montoAAcreditar;

            // Acreditar lo que se pueda
            saldo += montoAAcreditar;
            
            // Guardar el excedente como pendiente
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
                
                // Después de descontar, intentar acreditar saldo pendiente
                AcreditarCarga();
                
                return true;
            }

            return false;
        }

        public virtual int CalcularMontoPasaje(int tarifaBase)
        {
            return tarifaBase;
        }

        public virtual int CalcularMontoRealAPagar(int tarifaBase, out bool tuvoRecargo)
        {
            tuvoRecargo = false;
            return CalcularMontoPasaje(tarifaBase);
        }
    }
}