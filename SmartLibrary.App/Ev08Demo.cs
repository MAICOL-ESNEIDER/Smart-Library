using System;
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

            Console.WriteLine($"Libros cargados (seed): {libroService.ObtenerTodos().Count}");
            Console.WriteLine($"Usuarios cargados (seed): {usuarioService.ObtenerTodos().Count}");
            Console.WriteLine($"Préstamos cargados (seed): {prestamoService.ObtenerTodos().Count}");

            Console.WriteLine();
            Console.WriteLine("---- MUESTRA RÁPIDA (resúmenes) ----");
            Console.WriteLine("Libros:");
            foreach (var l in libroService.ObtenerTodos())
                Console.WriteLine(" - " + l.ResumenCorto());

            Console.WriteLine("Usuarios:");
            foreach (var u in usuarioService.ObtenerTodos())
                Console.WriteLine(" - " + u.ResumenCorto());

            Console.WriteLine("Préstamos:");
            foreach (var p in prestamoService.ObtenerTodos())
                Console.WriteLine(" - " + p.ResumenCorto());

            // =========================================================
            // COMMIT 3: BÚSQUEDAS + ORDENACIÓN (EV08 Puntos 4 y 5)
            // =========================================================
            Console.WriteLine();
            Console.WriteLine("===== (COMMIT 3) BÚSQUEDAS =====");

            // --- Libros: buscar por Id, título, autor
            Console.WriteLine("Buscar libro por Id = 1:");
            var libroId1 = libroService.BuscarPorId(1);
            Console.WriteLine(libroId1 != null ? "  OK -> " + libroId1.DetalleCompleto() : "  No encontrado");

            Console.WriteLine("Buscar libros por título contiene 'coronel':");
            var porTitulo = libroService.BuscarPorTitulo("coronel");
            foreach (var l in porTitulo)
                Console.WriteLine("  - " + l.DetalleCompleto());

            Console.WriteLine("Buscar libros por autor contiene 'García':");
            var porAutor = libroService.BuscarPorAutor("García");
            foreach (var l in porAutor)
                Console.WriteLine("  - " + l.DetalleCompleto());

            // --- Usuarios: buscar por Id / nombre
            Console.WriteLine();
            Console.WriteLine("Buscar usuario por Id = 2:");
            var usuarioId2 = usuarioService.BuscarPorId(2);
            Console.WriteLine(usuarioId2 != null ? "  OK -> " + usuarioId2.DetalleCompleto() : "  No encontrado");

            Console.WriteLine("Buscar usuarios por nombre contiene 'Ana':");
            var usuariosAna = usuarioService.BuscarPorNombre("Ana");
            foreach (var u in usuariosAna)
                Console.WriteLine("  - " + u.DetalleCompleto());

            // --- Préstamos: buscar por Id / estado
            Console.WriteLine();
            Console.WriteLine("Buscar préstamo por Id = 1:");
            var prestamoId1 = prestamoService.BuscarPorId(1);
            Console.WriteLine(prestamoId1 != null ? "  OK -> " + prestamoId1.DetalleCompleto() : "  No encontrado");

            Console.WriteLine("Buscar préstamos por estado Activo:");
            var prestamosActivos = prestamoService.BuscarPorEstado(EstadoPrestamo.Activo);
            foreach (var p in prestamosActivos)
                Console.WriteLine("  - " + p.DetalleCompleto());

            Console.WriteLine();
            Console.WriteLine("===== (COMMIT 3) ORDENACIÓN =====");

            // --- Libros: ordenar por título / año
            Console.WriteLine("Libros ordenados por Título:");
            var librosPorTitulo = libroService.OrdenarPorTitulo();
            foreach (var l in librosPorTitulo)
                Console.WriteLine("  - " + l.ResumenCorto());

            Console.WriteLine("Libros ordenados por Año:");
            var librosPorAnio = libroService.OrdenarPorAnio();
            foreach (var l in librosPorAnio)
                Console.WriteLine("  - " + l.ResumenCorto());

            // --- Usuarios: ordenar por nombre
            Console.WriteLine();
            Console.WriteLine("Usuarios ordenados por Nombre:");
            var usuariosOrdenados = usuarioService.OrdenarPorNombre();
            foreach (var u in usuariosOrdenados)
                Console.WriteLine("  - " + u.ResumenCorto());

            // --- Préstamos: ordenar por fecha de préstamo
            Console.WriteLine();
            Console.WriteLine("Préstamos ordenados por FechaPrestamo (ASC):");
            var prestamosAsc = prestamoService.OrdenarPorFechaPrestamoAsc();
            foreach (var p in prestamosAsc)
                Console.WriteLine("  - " + p.ResumenCorto());

            Console.WriteLine("Préstamos ordenados por FechaPrestamo (DESC):");
            var prestamosDesc = prestamoService.OrdenarPorFechaPrestamoDesc();
            foreach (var p in prestamosDesc)
                Console.WriteLine("  - " + p.ResumenCorto());

            Console.WriteLine();
            Console.WriteLine("===== FIN EV08 DEMO (commit 3) =====");
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
