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
        private int descuentoFrecuente;

        public Boleto(int monto, string linea, int saldoRestante, string tipoTarjeta, string idTarjeta, int montoTotalAbonado)
        {
            this.monto = monto;
            this.linea = linea;
            this.fecha = DateTimeProvider.Now;
            this.saldoRestante = saldoRestante;
            this.tipoTarjeta = tipoTarjeta;
            this.idTarjeta = idTarjeta;
            this.montoTotalAbonado = montoTotalAbonado;
            this.descuentoFrecuente = 0;
        }

        public Boleto(int monto, string linea, int saldoRestante, string tipoTarjeta, string idTarjeta, int montoTotalAbonado, DateTime fechaEspecifica)
        {
            this.monto = monto;
            this.linea = linea;
            this.fecha = fechaEspecifica;
            this.saldoRestante = saldoRestante;
            this.tipoTarjeta = tipoTarjeta;
            this.idTarjeta = idTarjeta;
            this.montoTotalAbonado = montoTotalAbonado;
            this.descuentoFrecuente = 0;
        }

        public Boleto(int monto, string linea, int saldoRestante, string tipoTarjeta, string idTarjeta, int montoTotalAbonado, int descuentoFrecuente)
        {
            this.monto = monto;
            this.linea = linea;
            this.fecha = DateTimeProvider.Now;
            this.saldoRestante = saldoRestante;
            this.tipoTarjeta = tipoTarjeta;
            this.idTarjeta = idTarjeta;
            this.montoTotalAbonado = montoTotalAbonado;
            this.descuentoFrecuente = descuentoFrecuente;
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

        public int DescuentoFrecuente
        {
            get { return descuentoFrecuente; }
        }

        public override string ToString()
        {
            string descuentoInfo = descuentoFrecuente > 0 ? $" [Descuento frecuente: -${descuentoFrecuente}]" : "";
            return $"Boleto - Línea: {linea}, Tipo: {tipoTarjeta}, ID: {idTarjeta}, " +
                   $"Monto: ${monto}{descuentoInfo}, " +
                   $"Total abonado: ${montoTotalAbonado}, " +
                   $"Fecha: {fecha:dd/MM/yyyy HH:mm}, Saldo restante: ${saldoRestante}";
        }
    }
}