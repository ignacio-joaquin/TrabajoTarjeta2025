using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Tarjeta
    {
        private int saldo;
        private const int LIMITE_SALDO = 40000;
        private static readonly HashSet<int> CargasAceptadas = new HashSet<int> 
        { 
            2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000 
        };

        public Tarjeta(int saldoInicial = 0)
        {
            this.saldo = saldoInicial;
        }

        public int Saldo
        {
            get { return saldo; }
        }
        
        public bool Cargar(int importe)
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

        public bool Descontar(int monto)
        {
            if (saldo < monto)
            {
                return false;
            }

            saldo -= monto;
            return true;
        }
    }
}