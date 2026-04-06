using System;
using System.Linq;
using SmartLibrary.App.Models;
using SmartLibrary.App.Services;

namespace SmartLibrary.App
{
    public static class Ev08Demo
    {
        public static void Run()
        {
            Console.WriteLine();
            Console.WriteLine("===== EV08 DEMO (Services + List<T>) =====");

            var (libroService, usuarioService, prestamoService) = CreateServicesWithSeedData();

            // Salida básica (en próximos commits agregamos búsquedas/ordenación/KPIs detallados)
            Console.WriteLine($"Libros cargados (seed): {libroService.ObtenerTodos().Count}");
            Console.WriteLine($"Usuarios cargados (seed): {usuarioService.ObtenerTodos().Count}");
            Console.WriteLine($"Préstamos cargados (seed): {prestamoService.ObtenerTodos().Count}");

            Console.WriteLine();
            Console.WriteLine("---- MUESTRA RÁPIDA ----");
            Console.WriteLine("Libros:");
            foreach (var l in libroService.ObtenerTodos())
                Console.WriteLine(" - " + l.ResumenCorto());

            Console.WriteLine("Usuarios:");
            foreach (var u in usuarioService.ObtenerTodos())
                Console.WriteLine(" - " + u.ResumenCorto());

            Console.WriteLine("Préstamos:");
            foreach (var p in prestamoService.ObtenerTodos())
                Console.WriteLine(" - " + p.ResumenCorto());

            Console.WriteLine("===== FIN EV08 DEMO (commit 2) =====");
            Console.WriteLine();
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
            // Constructor sin fecha de devolución => FechaDevolucion null y Estado Activo
            var prestamo1 = new Prestamo(1, libro1, usuario1, DateTime.Now.AddDays(-3));
            prestamoService.AgregarPrestamo(prestamo1);

            return (libroService, usuarioService, prestamoService);
        }
    }
}
