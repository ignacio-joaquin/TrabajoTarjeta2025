using System;
using System.Collections.Generic;

namespace Tarjeta
{
    public class Tarjeta
    {
        private int saldo;
        private const int LIMITE_SALDO = 40000;
        private const int LIMITE_NEGATIVO = -1200;
        private string id;

        private static readonly HashSet<int> CargasAceptadas = new HashSet<int>
        {
            2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000
        };

        public Tarjeta(int saldoInicial = 0)
        {
            this.saldo = saldoInicial;
            this.id = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }

        public Tarjeta(int saldoInicial, string id)
        {
            this.saldo = saldoInicial;
            this.id = id;
        }

        public int Saldo
        {
            get { return saldo; }
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

            if (saldo + importe > LIMITE_SALDO)
            {
                return false;
            }

            saldo += importe;
            return true;
        }

        public virtual bool Descontar(int monto)
        {
            if (saldo - monto >= LIMITE_NEGATIVO)
            {
                saldo -= monto;
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