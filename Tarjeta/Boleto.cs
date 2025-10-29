using System;

namespace Tarjeta
{
    public class Boleto
    {
        private int monto;
        private string linea;
        private DateTime fecha;
        private int saldoRestante;

        public Boleto(int monto, string linea, int saldoRestante)
        {
            this.monto = monto;
            this.linea = linea;
            this.fecha = DateTime.Now;
            this.saldoRestante = saldoRestante;
        }

        public int Monto
        {
            get { return monto; }
        }

        public string Linea
        {
            get { return linea; }
        }

        public DateTime Fecha
        {
            get { return fecha; }
        }

        public int SaldoRestante
        {
            get { return saldoRestante; }
        }

        public override string ToString()
        {
            return $"Boleto - Línea: {linea}, Monto: ${monto}, Fecha: {fecha:dd/MM/yyyy HH:mm}, Saldo restante: ${saldoRestante}";
        }
    }
}