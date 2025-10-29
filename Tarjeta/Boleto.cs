using System;

namespace Tarjeta
{
    public class Boleto
    {
        private int monto;
        private string linea;
        private DateTime fecha;
        private int saldoRestante;
        private string tipoTarjeta;
        private string idTarjeta;
        private int montoTotalAbonado;
        private bool tuvoRecargo;

        public Boleto(int monto, string linea, int saldoRestante, string tipoTarjeta, string idTarjeta, int montoTotalAbonado, bool tuvoRecargo)
        {
            this.monto = monto;
            this.linea = linea;
            this.fecha = DateTimeProvider.Now;
            this.saldoRestante = saldoRestante;
            this.tipoTarjeta = tipoTarjeta;
            this.idTarjeta = idTarjeta;
            this.montoTotalAbonado = montoTotalAbonado;
            this.tuvoRecargo = tuvoRecargo;
        }

        public Boleto(int monto, string linea, int saldoRestante, string tipoTarjeta, string idTarjeta, int montoTotalAbonado, bool tuvoRecargo, DateTime fechaEspecifica)
        {
            this.monto = monto;
            this.linea = linea;
            this.fecha = fechaEspecifica;
            this.saldoRestante = saldoRestante;
            this.tipoTarjeta = tipoTarjeta;
            this.idTarjeta = idTarjeta;
            this.montoTotalAbonado = montoTotalAbonado;
            this.tuvoRecargo = tuvoRecargo;
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

        public string TipoTarjeta
        {
            get { return tipoTarjeta; }
        }

        public string IdTarjeta
        {
            get { return idTarjeta; }
        }

        public int MontoTotalAbonado
        {
            get { return montoTotalAbonado; }
        }

        public bool TuvoRecargo
        {
            get { return tuvoRecargo; }
        }

        public override string ToString()
        {
            string recargoInfo = tuvoRecargo ? $" (incluye recargo por saldo negativo)" : "";
            return $"Boleto - Línea: {linea}, Tipo: {tipoTarjeta}, ID: {idTarjeta}, " +
                   $"Monto: ${monto}{recargoInfo}, Total abonado: ${montoTotalAbonado}, " +
                   $"Fecha: {fecha:dd/MM/yyyy HH:mm}, Saldo restante: ${saldoRestante}";
        }
    }
}