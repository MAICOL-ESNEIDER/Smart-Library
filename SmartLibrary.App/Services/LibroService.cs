using System;
using System.Collections.Generic;
using SmartLibrary.App.Models;

namespace SmartLibrary.App.Services
{
    public class LibroService
    {
        private readonly List<Libro> libros = new List<Libro>();

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

        // BÚSQUEDAS (EV08)
        public Libro? BuscarPorId(int id)
        {
            return libros.Find(l => l.Id == id);
        }

        public List<Libro> BuscarPorTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo)) return new List<Libro>();

            return libros.FindAll(l =>
                l.Titulo != null &&
                l.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase)
            );
        }

        public List<Libro> BuscarPorAutor(string autor)
        {
            if (string.IsNullOrWhiteSpace(autor)) return new List<Libro>();

            return libros.FindAll(l =>
                l.Autor != null &&
                l.Autor.Contains(autor, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
