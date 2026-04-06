using System;
using System.Collections.Generic;
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
    }
}
