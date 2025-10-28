using System;

namespace TarjetaSube
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Tests Manuales - Sistema SUBE ===\n");
            
            int testsPasados = 0;
            int testsFallados = 0;

            // Test 1: Constructor con saldo inicial
            Console.WriteLine("Test 1: Constructor con saldo inicial");
            Tarjeta t1 = new Tarjeta(5000);
            if (t1.Saldo == 5000)
            {
                Console.WriteLine("✓ PASS");
                testsPasados++;
            }
            else
            {
                Console.WriteLine("✗ FAIL");
                testsFallados++;
            }

            // Test 2: Cargar monto válido (2000)
            Console.WriteLine("\nTest 2: Cargar $2000");
            Tarjeta t2 = new Tarjeta();
            bool resultado = t2.Cargar(2000);
            if (resultado && t2.Saldo == 2000)
            {
                Console.WriteLine("✓ PASS");
                testsPasados++;
            }
            else
            {
                Console.WriteLine("✗ FAIL");
                testsFallados++;
            }

            // Test 3: Cargar monto inválido
            Console.WriteLine("\nTest 3: Cargar monto inválido ($1500)");
            Tarjeta t3 = new Tarjeta();
            bool resultado2 = t3.Cargar(1500);
            if (!resultado2 && t3.Saldo == 0)
            {
                Console.WriteLine("✓ PASS");
                testsPasados++;
            }
            else
            {
                Console.WriteLine("✗ FAIL");
                testsFallados++;
            }

            // Test 4: Cargar superando límite
            Console.WriteLine("\nTest 4: Cargar superando límite de $40000");
            Tarjeta t4 = new Tarjeta(35000);
            bool resultado3 = t4.Cargar(10000);
            if (!resultado3 && t4.Saldo == 35000)
            {
                Console.WriteLine("✓ PASS");
                testsPasados++;
            }
            else
            {
                Console.WriteLine("✗ FAIL");
                testsFallados++;
            }

            // Test 5: Pagar con colectivo - saldo suficiente
            Console.WriteLine("\nTest 5: Pagar con colectivo (saldo suficiente)");
            Tarjeta t5 = new Tarjeta(5000);
            Colectivo colectivo = new Colectivo("K");
            Boleto boleto = colectivo.PagarCon(t5);
            if (boleto != null && t5.Saldo == 3420 && boleto.Monto == 1580)
            {
                Console.WriteLine("✓ PASS");
                testsPasados++;
            }
            else
            {
                Console.WriteLine("✗ FAIL");
                testsFallados++;
            }

            // Test 6: Pagar con colectivo - saldo insuficiente
            Console.WriteLine("\nTest 6: Pagar con colectivo (saldo insuficiente)");
            Tarjeta t6 = new Tarjeta(1000);
            Boleto boleto2 = colectivo.PagarCon(t6);
            if (boleto2 == null && t6.Saldo == 1000)
            {
                Console.WriteLine("✓ PASS");
                testsPasados++;
            }
            else
            {
                Console.WriteLine("✗ FAIL");
                testsFallados++;
            }

            // Test 7: Cargar todos los montos válidos
            Console.WriteLine("\nTest 7: Cargar todos los montos válidos");
            int[] montosValidos = { 2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000 };
            bool todosOk = true;
            foreach (int monto in montosValidos)
            {
                Tarjeta temp = new Tarjeta();
                if (!temp.Cargar(monto) || temp.Saldo != monto)
                {
                    todosOk = false;
                    Console.WriteLine($"  ✗ Fallo con monto ${monto}");
                    break;
                }
            }
            if (todosOk)
            {
                Console.WriteLine("✓ PASS - Todos los montos válidos funcionan");
                testsPasados++;
            }
            else
            {
                Console.WriteLine("✗ FAIL");
                testsFallados++;
            }

            // Test 8: Boleto contiene información correcta
            Console.WriteLine("\nTest 8: Boleto contiene información correcta");
            Tarjeta t8 = new Tarjeta(5000);
            Colectivo col8 = new Colectivo("142");
            Boleto bol8 = col8.PagarCon(t8);
            if (bol8 != null && bol8.Linea == "142" && bol8.SaldoRestante == 3420 && bol8.Fecha != null)
            {
                Console.WriteLine("✓ PASS");
                testsPasados++;
            }
            else
            {
                Console.WriteLine("✗ FAIL");
                testsFallados++;
            }

            // Resumen
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine($"Tests pasados: {testsPasados}");
            Console.WriteLine($"Tests fallados: {testsFallados}");
            Console.WriteLine($"Total: {testsPasados + testsFallados}");
            Console.WriteLine(new string('=', 50));

            // Demostración del sistema
            Console.WriteLine("\n=== Demostración del Sistema ===\n");
            
            Tarjeta miTarjeta = new Tarjeta();
            Console.WriteLine($"Saldo inicial: ${miTarjeta.Saldo}");
            
            miTarjeta.Cargar(5000);
            Console.WriteLine($"Después de cargar $5000: ${miTarjeta.Saldo}");
            
            Colectivo lineaK = new Colectivo("K");
            Boleto miBoletro = lineaK.PagarCon(miTarjeta);
            
            if (miBoletro != null)
            {
                Console.WriteLine($"\n{miBoletro}");
            }
        }
    }
}