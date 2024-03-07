using System;
using System.Collections.Generic;

class Proceso
{
    public int PID { get; set; }
    public int TiempoEjecucion { get; set; }
    public int TiempoEspera { get; set; }
    public int TiempoRetorno { get; set; }
}

class Program
{
    static void RoundRobin(List<Proceso> procesos, int quantum)
    {
        int tiempoTotal = 0;
        while (procesos.Count > 0)
        {
            for (int i = 0; i < procesos.Count; i++)
            {
                var proceso = procesos[i];
                if (proceso.TiempoEjecucion > 0)
                {
                    int tiempoEjecucion = Math.Min(quantum, proceso.TiempoEjecucion);
                    proceso.TiempoEjecucion -= tiempoEjecucion;
                    tiempoTotal += tiempoEjecucion;
                    proceso.TiempoRetorno = tiempoTotal;
                    foreach (var otroProceso in procesos)
                    {
                        if (otroProceso != proceso && otroProceso.TiempoEjecucion > 0)
                        {
                            otroProceso.TiempoEspera += tiempoEjecucion;
                        }
                    }
                    if (proceso.TiempoEjecucion <= 0)
                    {
                        procesos.RemoveAt(i);
                        i--; // Para compensar el cambio de tamaño de la lista
                    }
                }
            }
        }
    }

    static void Main(string[] args)
    {
        List<Proceso> procesos = new List<Proceso>();
        Console.Write("Ingrese la cantidad de procesos: ");
        int cantidadProcesos = int.Parse(Console.ReadLine());
        for (int i = 1; i <= cantidadProcesos; i++)
        {
            Console.Write($"Ingrese el tiempo de ejecución para el proceso {i}: ");
            int tiempoEjecucion = int.Parse(Console.ReadLine());
            procesos.Add(new Proceso { PID = i, TiempoEjecucion = tiempoEjecucion });
        }

        Console.Write("Ingrese el quantum: ");
        int quantum = int.Parse(Console.ReadLine());

        RoundRobin(procesos, quantum);

        double tiempoEsperaPromedio = procesos.Sum(p => p.TiempoEspera) / (double)cantidadProcesos;
        double tiempoRetornoPromedio = procesos.Sum(p => p.TiempoRetorno) / (double)cantidadProcesos;

        Console.WriteLine($"Tiempo de espera promedio: {tiempoEsperaPromedio}");
        Console.WriteLine($"Tiempo de retorno promedio: {tiempoRetornoPromedio}");
    }
}

