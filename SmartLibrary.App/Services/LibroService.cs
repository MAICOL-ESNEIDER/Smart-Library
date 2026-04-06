using System;
using System.Collections.Generic;
using SmartLibrary.App.Models;

namespace SmartLibrary.App.Services
{
    public class LibroService
    {
        private List<Libro> libros = new List<Libro>();

        public void AgregarLibro(Libro libro)
        {
            libros.Add(libro);
        }

        public void EliminarLibro(Libro libro)
        {
            libros.Remove(libro);
        }

        public List<Libro> ObtenerTodos()
        {
            return libros;
        }
    }
}
