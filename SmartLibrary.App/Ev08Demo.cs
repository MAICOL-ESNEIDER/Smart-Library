using System;
using SmartLibrary.App.Models;
using SmartLibrary.App.Services;

namespace SmartLibrary.App
{
    public static class Ev08Demo
    {
        // En el siguiente commit lo llamaremos desde Program.cs
        public static void Run()
        {
            // Placeholder intencional
            // En próximos commits: búsquedas, ordenaciones, KPIs y comparación Array vs List
        }

        // Crea servicios con datos de prueba (2 libros, 2 usuarios, 1 préstamo)
        public static (LibroService libroService, UsuarioService usuarioService, PrestamoService prestamoService) CreateServicesWithSeedData()
        {
            var libroService = new LibroService();
            var usuarioService = new UsuarioService();
            var prestamoService = new PrestamoService();

            // === Datos de prueba: Libros (2) ===
            var libro1 = new Libro(1, "Cien años de soledad", "Gabriel García Márquez", 1967, "Novela");
            var libro2 = new Libro(2, "El coronel no tiene quien le escriba", "Gabriel García Márquez", 1961, "Novela");

            libroService.AgregarLibro(libro1);
            libroService.AgregarLibro(libro2);

            // === Datos de prueba: Usuarios (2) ===
            var usuario1 = new Usuario(1, "Maicol Posada", "maicol@email.com");
            var usuario2 = new Usuario(2, "Ana Pérez", "ana@email.com");

            usuarioService.AgregarUsuario(usuario1);
            usuarioService.AgregarUsuario(usuario2);

            // === Datos de prueba: Préstamo (1) ===
            // Usamos el constructor sin fecha de devolución (queda null y estado Activo)
            var prestamo1 = new Prestamo(1, libro1, usuario1, DateTime.Now.AddDays(-3));
            prestamoService.AgregarPrestamo(prestamo1);

            return (libroService, usuarioService, prestamoService);
        }
    }
}
