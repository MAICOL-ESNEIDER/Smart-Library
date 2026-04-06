using System;
using System.Collections.Generic;
using System.Linq;
using SmartLibrary.App.Models;

namespace SmartLibrary.App.Services
{
    public class PrestamoService
    {
        private readonly List<Prestamo> prestamos = new List<Prestamo>();

        public void AgregarPrestamo(Prestamo prestamo)
        {
            prestamos.Add(prestamo);
        }

        public void EliminarPrestamo(Prestamo prestamo)
        {
            prestamos.Remove(prestamo);
        }

        public List<Prestamo> ObtenerTodos()
        {
            return prestamos;
        }

        // BÚSQUEDAS (EV08)
        public Prestamo? BuscarPorId(int id)
        {
            return prestamos.Find(p => p.Id == id);
        }

        public List<Prestamo> BuscarPorEstado(EstadoPrestamo estado)
        {
            return prestamos.FindAll(p => p.Estado == estado);
        }

        // ORDENACIÓN (EV08)
        // La guía menciona "fecha límite", pero tu modelo no la tiene.
        // Adaptación válida: ordenar por FechaPrestamo.
        public List<Prestamo> OrdenarPorFechaPrestamoAsc()
        {
            return prestamos.OrderBy(p => p.FechaPrestamo).ToList();
        }

        public List<Prestamo> OrdenarPorFechaPrestamoDesc()
        {
            return prestamos.OrderByDescending(p => p.FechaPrestamo).ToList();
        }

        // KPIs (EV08 - OBLIGATORIO)
        public int TotalPrestamos()
        {
            return prestamos.Count;
        }

        public int PrestamosActivos()
        {
            return prestamos.Count(p => p.Estado == EstadoPrestamo.Activo);
        }

        public int PrestamosDevueltos()
        {
            return prestamos.Count(p => p.Estado == EstadoPrestamo.Devuelto);
        }

        public int PrestamosVencidos()
        {
            return prestamos.Count(p => p.Estado == EstadoPrestamo.Vencido);
        }

        // Promedio de días de préstamo (EV08 - OBLIGATORIO)
        // Usamos el método DiasTranscurridos() del modelo.
        // Si no hay préstamos, devuelve 0 para evitar división por cero.
        public double PromedioDiasPrestamo()
        {
            if (prestamos.Count == 0) return 0;

            return prestamos.Average(p => p.DiasTranscurridos());
        }
    }
}
