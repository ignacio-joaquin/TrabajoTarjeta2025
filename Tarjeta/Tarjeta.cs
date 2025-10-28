using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Tarjeta
    {
        private int saldo;
        private const int LIMITE_SALDO = 40000;
        private const int LIMITE_NEGATIVO = -1200;
        private int viajesPlus;
        private const int MAX_VIAJES_PLUS = 2;
        
        private static readonly HashSet<int> CargasAceptadas = new HashSet<int> 
        { 
            2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000 
        };

        public Tarjeta(int saldoInicial = 0)
        {
            this.saldo = saldoInicial;
            this.viajesPlus = 0;
        }

        public int Saldo
        {
            get { return saldo; }
        }
        
        public int ViajesPlus
        {
            get { return viajesPlus; }
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

            // Si hay viajes Plus pendientes, descontarlos primero
            if (viajesPlus > 0)
            {
                int montoViajesPlus = viajesPlus * 1580;
                if (importe >= montoViajesPlus)
                {
                    importe -= montoViajesPlus;
                    viajesPlus = 0;
                }
                else
                {
                    viajesPlus -= (int)Math.Ceiling((double)importe / 1580);
                    importe = 0;
                }
            }

            saldo += importe;
            return true;
        }

        public virtual bool Descontar(int monto)
        {
            // Verificar si es una franquicia completa (viaje gratuito)
            if (this is FranquiciaCompleta || this is BoletoGratuitoEstudiantil)
            {
                return true;
            }

            // Aplicar descuento para medio boleto
            int montoReal = monto;
            if (this is MedioBoletoEstudiantil)
            {
                montoReal = monto / 2;
            }

            if (saldo >= montoReal)
            {
                saldo -= montoReal;
                return true;
            }
            else if (saldo - montoReal >= LIMITE_NEGATIVO && viajesPlus < MAX_VIAJES_PLUS)
            {
                // Viaje Plus
                saldo -= montoReal;
                viajesPlus++;
                return true;
            }

            return false;
        }

        public virtual int CalcularMontoPasaje(int tarifaBase)
        {
            if (this is FranquiciaCompleta || this is BoletoGratuitoEstudiantil)
            {
                return 0;
            }
            else if (this is MedioBoletoEstudiantil)
            {
                return tarifaBase / 2;
            }
            else
            {
                return tarifaBase;
            }
        }
    }
}