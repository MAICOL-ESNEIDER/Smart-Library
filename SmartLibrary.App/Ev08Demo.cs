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
            // BÚSQUEDAS + ORDENACIÓN (EV08 Puntos 4 y 5)
            // =========================================================
            Console.WriteLine();
            Console.WriteLine("===== (COMMIT 3) BÚSQUEDAS =====");

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

            Console.WriteLine();
            Console.WriteLine("Buscar usuario por Id = 2:");
            var usuarioId2 = usuarioService.BuscarPorId(2);
            Console.WriteLine(usuarioId2 != null ? "  OK -> " + usuarioId2.DetalleCompleto() : "  No encontrado");

            Console.WriteLine("Buscar usuarios por nombre contiene 'Ana':");
            var usuariosAna = usuarioService.BuscarPorNombre("Ana");
            foreach (var u in usuariosAna)
                Console.WriteLine("  - " + u.DetalleCompleto());

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

            Console.WriteLine("Libros ordenados por Título:");
            var librosPorTitulo = libroService.OrdenarPorTitulo();
            foreach (var l in librosPorTitulo)
                Console.WriteLine("  - " + l.ResumenCorto());

            Console.WriteLine("Libros ordenados por Año:");
            var librosPorAnio = libroService.OrdenarPorAnio();
            foreach (var l in librosPorAnio)
                Console.WriteLine("  - " + l.ResumenCorto());

            Console.WriteLine();
            Console.WriteLine("Usuarios ordenados por Nombre:");
            var usuariosOrdenados = usuarioService.OrdenarPorNombre();
            foreach (var u in usuariosOrdenados)
                Console.WriteLine("  - " + u.ResumenCorto());

            Console.WriteLine();
            Console.WriteLine("Préstamos ordenados por FechaPrestamo (ASC):");
            var prestamosAsc = prestamoService.OrdenarPorFechaPrestamoAsc();
            foreach (var p in prestamosAsc)
                Console.WriteLine("  - " + p.ResumenCorto());

            Console.WriteLine("Préstamos ordenados por FechaPrestamo (DESC):");
            var prestamosDesc = prestamoService.OrdenarPorFechaPrestamoDesc();
            foreach (var p in prestamosDesc)
                Console.WriteLine("  - " + p.ResumenCorto());

            // =========================================================
            // KPIs + ESTADÍSTICAS (EV08 Punto 6 - OBLIGATORIO)
            // =========================================================
            Console.WriteLine();
            Console.WriteLine("===== (COMMIT 4) KPIs Y ESTADÍSTICAS =====");

            Console.WriteLine("---- KPIs Libros ----");
            Console.WriteLine($"Total libros: {libroService.TotalLibros()}");
            Console.WriteLine($"Disponibles: {libroService.LibrosDisponibles()}");
            Console.WriteLine($"Prestados: {libroService.LibrosPrestados()}");
            
            Console.WriteLine();
            Console.WriteLine("---- KPIs Usuarios ----");
            Console.WriteLine($"Total usuarios: {usuarioService.TotalUsuarios()}");
            Console.WriteLine($"Activos: {usuarioService.UsuariosActivos()}");
            Console.WriteLine($"Inactivos: {usuarioService.UsuariosInactivos()}");

            Console.WriteLine();
            Console.WriteLine("---- KPIs Préstamos ----");
            Console.WriteLine($"Total préstamos: {prestamoService.TotalPrestamos()}");
            Console.WriteLine($"Activos: {prestamoService.PrestamosActivos()}");
            Console.WriteLine($"Devueltos: {prestamoService.PrestamosDevueltos()}");
            Console.WriteLine($"Vencidos: {prestamoService.PrestamosVencidos()}");
            Console.WriteLine($"Promedio días de préstamo: {prestamoService.PromedioDiasPrestamo():0.00}");

            // =========================================================
            // COMMIT 5: COMPARACIÓN ARRAY vs LIST (EV08 - OBLIGATORIO)
            // =========================================================
            Console.WriteLine();
            Console.WriteLine("===== (COMMIT 5) COMPARACIÓN ARRAY vs LIST =====");
            CompareArrayVsList();

            Console.WriteLine();
            Console.WriteLine("===== FIN EV08 DEMO (commit 5) =====");
            Console.WriteLine();
        }

        // Comparación obligatoria Array vs List (en consola)
        private static void CompareArrayVsList()
        {
            Console.WriteLine("Ejemplo con strings:");

            // ARRAY: tamaño fijo
            string[] nombresArray = new string[2];
            nombresArray[0] = "Ana";
            nombresArray[1] = "Maicol";

            Console.WriteLine($"ARRAY tamaño fijo = {nombresArray.Length}");
            Console.WriteLine($"ARRAY[0]={nombresArray[0]}, ARRAY[1]={nombresArray[1]}");

            Console.WriteLine();
            Console.WriteLine("Si quiero agregar un tercer elemento en ARRAY, no puedo directamente.");
            Console.WriteLine("Tengo que crear un nuevo array más grande y copiar manualmente:");

            string[] nuevoArray = new string[nombresArray.Length + 1];
            for (int i = 0; i < nombresArray.Length; i++)
            {
                nuevoArray[i] = nombresArray[i];
            }
            nuevoArray[2] = "Carlos";

            Console.WriteLine($"Nuevo ARRAY tamaño = {nuevoArray.Length}");
            Console.WriteLine($"Nuevo ARRAY[2]={nuevoArray[2]}");

            Console.WriteLine();
            Console.WriteLine("LIST: tamaño dinámico (crece y se reduce fácil)");

            var nombresList = new System.Collections.Generic.List<string>();
            nombresList.Add("Ana");
            nombresList.Add("Maicol");

            Console.WriteLine($"LIST Count = {nombresList.Count}");
            Console.WriteLine($"LIST[0]={nombresList[0]}, LIST[1]={nombresList[1]}");

            Console.WriteLine("Agregando 'Carlos' con Add():");
            nombresList.Add("Carlos");
            Console.WriteLine($"LIST Count = {nombresList.Count}");
            Console.WriteLine($"LIST[2]={nombresList[2]}");

            Console.WriteLine("Eliminando 'Ana' con Remove():");
            nombresList.Remove("Ana");
            Console.WriteLine($"LIST Count = {nombresList.Count}");

            Console.WriteLine();
            Console.WriteLine("Conclusión:");
            Console.WriteLine("- ARRAY: tamaño fijo, acceso rápido por índice, para crecer toca copiar a otro array.");
            Console.WriteLine("- LIST: tamaño dinámico, permite Add/Remove fácil, ideal para colecciones que cambian.");
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
            var prestamo1 = new Prestamo(1, libro1, usuario1, DateTime.Now.AddDays(-3));
            prestamoService.AgregarPrestamo(prestamo1);

            return (libroService, usuarioService, prestamoService);
        }
    }
}
