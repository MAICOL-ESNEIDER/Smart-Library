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
    }
}
