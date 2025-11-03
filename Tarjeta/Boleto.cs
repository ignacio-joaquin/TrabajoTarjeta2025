
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
        private bool esTrasbordo;

        public Boleto(int monto, string linea, int saldoRestante, string tipoTarjeta, string idTarjeta, int montoTotalAbonado, int descuentoFrecuente, bool esTrasbordo)
        {
            this.monto = monto;
            this.linea = linea;
            this.fecha = DateTimeProvider.Now;
            this.saldoRestante = saldoRestante;
            this.tipoTarjeta = tipoTarjeta;
            this.idTarjeta = idTarjeta;
            this.montoTotalAbonado = montoTotalAbonado;
            this.descuentoFrecuente = descuentoFrecuente;
            this.esTrasbordo = esTrasbordo;
        }

        public Boleto(int monto, string linea, int saldoRestante, string tipoTarjeta, string idTarjeta, int montoTotalAbonado, int descuentoFrecuente, bool esTrasbordo, DateTime fechaEspecifica)
        {
            this.monto = monto;
            this.linea = linea;
            this.fecha = fechaEspecifica;
            this.saldoRestante = saldoRestante;
            this.tipoTarjeta = tipoTarjeta;
            this.idTarjeta = idTarjeta;
            this.montoTotalAbonado = montoTotalAbonado;
            this.descuentoFrecuente = descuentoFrecuente;
            this.esTrasbordo = esTrasbordo;
        }

        // Constructor simplificado para tests existentes
        public Boleto(int monto, string linea, int saldoRestante, string tipoTarjeta, string idTarjeta, int montoTotalAbonado, int descuentoFrecuente)
            : this(monto, linea, saldoRestante, tipoTarjeta, idTarjeta, montoTotalAbonado, descuentoFrecuente, false)
        {
        }

        public Boleto(int monto, string linea, int saldoRestante, string tipoTarjeta, string idTarjeta, int montoTotalAbonado, DateTime fechaEspecifica)
            : this(monto, linea, saldoRestante, tipoTarjeta, idTarjeta, montoTotalAbonado, 0, false, fechaEspecifica)
        {
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

        public bool EsTrasbordo
        {
            get { return esTrasbordo; }
        }

        public override string ToString()
        {
            string descuentoInfo = descuentoFrecuente > 0 ? $" [Descuento frecuente: -${descuentoFrecuente}]" : "";
            string trasbordoInfo = esTrasbordo ? " [TRASBORDO GRATUITO]" : "";
            return $"Boleto - Línea: {linea}, Tipo: {tipoTarjeta}, ID: {idTarjeta}, " +
                   $"Monto: ${monto}{descuentoInfo}{trasbordoInfo}, " +
                   $"Total abonado: ${montoTotalAbonado}, " +
                   $"Fecha: {fecha:dd/MM/yyyy HH:mm}, Saldo restante: ${saldoRestante}";
        }
    }
}
