using System;
using System.Collections.Generic;

namespace Tarjeta
{
    public class Tarjeta
{
    private int saldo;
    private const int LIMITE_SALDO = 40000;
    private const int LIMITE_NEGATIVO = -1200;

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

        // Verificar si el descuento mantiene el saldo dentro del límite negativo
        if (saldo - montoReal >= LIMITE_NEGATIVO)
        {
            saldo -= montoReal;
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
