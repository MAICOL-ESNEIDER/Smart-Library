using System;
using SmartLibrary.App.Models;
using SmartLibrary.App.Services;

namespace SmartLibrary.App
{
    class Program
    {
        // Services (capa de negocio en memoria)
        private static readonly LibroService _libroService = new LibroService();
        private static readonly UsuarioService _usuarioService = new UsuarioService();
        private static readonly PrestamoService _prestamoService = new PrestamoService();

        static void Main(string[] args)
        {
            // Ejecutar demo EV08 con:
            // dotnet run --project .\SmartLibrary.App\SmartLibrary.App.csproj -- --ev08
            if (args.Length > 0 && args[0].Equals("--ev08", StringComparison.OrdinalIgnoreCase))
            {
                Ev08Demo.Run();
                return;
            }

            // Seed inicial (datos de prueba para que el menú tenga con qué trabajar)
            SeedDataIfEmpty();

            bool running = true;
            while (running)
            {
                ShowMainMenu();
                int option = ReadOption(1, 6);

                switch (option)
                {
                    case 1:
                        ShowBooksMenu();
                        break;

                    case 2:
                        ShowUsersMenu();
                        break;

                    case 3:
                        ShowLoansMenu();
                        break;

                    case 4:
                        ShowSearchReportsMenu(); // En commit 5 se vuelve real
                        break;

                    case 5:
                        ShowPersistenceMenu(); // En commit 5 se vuelve real
                        break;

                    case 6:
                        running = !ConfirmExitAndSave();
                        break;
                }

                Console.WriteLine();
            }
        }

        // ===================== SEED =====================
        static void SeedDataIfEmpty()
        {
            // Si ya hay datos, no volvemos a sembrar
            if (_libroService.TotalLibros() > 0 || _usuarioService.TotalUsuarios() > 0 || _prestamoService.TotalPrestamos() > 0)
                return;

            // 2 libros
            var libro1 = new Libro(1, "Cien años de soledad", "Gabriel García Márquez", 1967, "Novela");
            var libro2 = new Libro(2, "El coronel no tiene quien le escriba", "Gabriel García Márquez", 1961, "Novela");

            _libroService.AgregarLibro(libro1);
            _libroService.AgregarLibro(libro2);

            // 2 usuarios
            var usuario1 = new Usuario(1, "Maicol Posada", "maicol@email.com");
            var usuario2 = new Usuario(2, "Ana Pérez", "ana@email.com");

            _usuarioService.AgregarUsuario(usuario1);
            _usuarioService.AgregarUsuario(usuario2);

            // 1 préstamo (activo) -> libro1 queda no disponible
            libro1.Disponible = false;

            // NOTA: Tu modelo no tiene FechaLimite, así que usamos FechaDevolucion como "fecha estimada/límite" en la simulación.
            var prestamo1 = new Prestamo(1, libro1, usuario1, DateTime.Now.AddDays(-3), DateTime.Now.AddDays(4));
            prestamo1.Estado = EstadoPrestamo.Activo;

            _prestamoService.AgregarPrestamo(prestamo1);
        }

        // ===================== MENÚ PRINCIPAL =====================
        static void ShowMainMenu()
        {
            Console.WriteLine("===== SMART LIBRARY =====");
            Console.WriteLine("1. Libros");
            Console.WriteLine("2. Usuarios");
            Console.WriteLine("3. Préstamos");
            Console.WriteLine("4. Búsquedas y reportes");
            Console.WriteLine("5. Guardar / Cargar datos");
            Console.WriteLine("6. Salir");
            Console.WriteLine("=========================");
        }

        static int ReadOption(int min, int max)
        {
            while (true)
            {
                Console.Write("Seleccione una opción: ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int option) && option >= min && option <= max)
                    return option;

                Console.WriteLine("Opción inválida. Intente nuevamente.");
            }
        }

        static int ReadInt(string label)
        {
            while (true)
            {
                Console.Write(label);
                var input = Console.ReadLine();

                if (int.TryParse(input, out int value))
                    return value;

                Console.WriteLine("Entrada inválida. Debe ser un número.");
            }
        }

        static string ReadText(string label)
        {
            while (true)
            {
                Console.Write(label);
                var input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("Entrada inválida. No puede estar vacía.");
            }
        }

        static bool ConfirmYesNo(string label)
        {
            while (true)
            {
                Console.Write(label);
                string? ans = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(ans))
                    continue;

                ans = ans.Trim().ToUpper();

                if (ans == "S") return true;
                if (ans == "N") return false;

                Console.WriteLine("Respuesta inválida. Use S/N.");
            }
        }

        // ===================== LIBROS (stub, commit 2 lo vuelve CRUD real) =====================
        static void ShowBooksMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== MENÚ LIBROS ===");
                Console.WriteLine("1. Registrar libro");
                Console.WriteLine("2. Listar libros");
                Console.WriteLine("3. Ver detalle de libro");
                Console.WriteLine("4. Actualizar libro");
                Console.WriteLine("5. Eliminar libro");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 5);

                switch (option)
                {
                    case 1:
                        Console.WriteLine("[PENDIENTE] Registrar libro (CRUD real se agrega en commit 2).");
                        break;
                    case 2:
                        Console.WriteLine("[PENDIENTE] Listar libros (CRUD real se agrega en commit 2).");
                        break;
                    case 3:
                        Console.WriteLine("[PENDIENTE] Ver detalle (CRUD real se agrega en commit 2).");
                        break;
                    case 4:
                        Console.WriteLine("[PENDIENTE] Actualizar libro (CRUD real se agrega en commit 2).");
                        break;
                    case 5:
                        Console.WriteLine("[PENDIENTE] Eliminar libro (reglas se agregan en commit 2).");
                        break;
                    case 0:
                        back = true;
                        break;
                }

                Console.WriteLine();
            }
        }

        // ===================== USUARIOS (stub, commit 3 lo vuelve CRUD real) =====================
        static void ShowUsersMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== MENÚ USUARIOS ===");
                Console.WriteLine("1. Registrar usuario");
                Console.WriteLine("2. Listar usuarios");
                Console.WriteLine("3. Ver detalle de usuario");
                Console.WriteLine("4. Actualizar usuario");
                Console.WriteLine("5. Eliminar usuario");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 5);

                switch (option)
                {
                    case 1:
                        Console.WriteLine("[PENDIENTE] Registrar usuario (CRUD real se agrega en commit 3).");
                        break;
                    case 2:
                        Console.WriteLine("[PENDIENTE] Listar usuarios (CRUD real se agrega en commit 3).");
                        break;
                    case 3:
                        Console.WriteLine("[PENDIENTE] Ver detalle (CRUD real se agrega en commit 3).");
                        break;
                    case 4:
                        Console.WriteLine("[PENDIENTE] Actualizar usuario (CRUD real se agrega en commit 3).");
                        break;
                    case 5:
                        Console.WriteLine("[PENDIENTE] Eliminar usuario (reglas se agregan en commit 3).");
                        break;
                    case 0:
                        back = true;
                        break;
                }

                Console.WriteLine();
            }
        }

        // ===================== PRÉSTAMOS (stub, commit 4 lo vuelve CRUD real) =====================
        static void ShowLoansMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== MENÚ PRÉSTAMOS ===");
                Console.WriteLine("1. Crear préstamo");
                Console.WriteLine("2. Listar préstamos");
                Console.WriteLine("3. Ver detalle de préstamo");
                Console.WriteLine("4. Registrar devolución");
                Console.WriteLine("5. Eliminar préstamo");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 5);

                switch (option)
                {
                    case 1:
                        Console.WriteLine("[PENDIENTE] Crear préstamo (CRUD real se agrega en commit 4).");
                        break;
                    case 2:
                        Console.WriteLine("[PENDIENTE] Listar préstamos (CRUD real se agrega en commit 4).");
                        break;
                    case 3:
                        Console.WriteLine("[PENDIENTE] Ver detalle (CRUD real se agrega en commit 4).");
                        break;
                    case 4:
                        Console.WriteLine("[PENDIENTE] Registrar devolución (CRUD real se agrega en commit 4).");
                        break;
                    case 5:
                        Console.WriteLine("[PENDIENTE] Eliminar préstamo (reglas se agregan en commit 4).");
                        break;
                    case 0:
                        back = true;
                        break;
                }

                Console.WriteLine();
            }
        }

        // ===================== BÚSQUEDAS/REPORTES (commit 5 lo vuelve real) =====================
        static void ShowSearchReportsMenu()
        {
            Console.WriteLine("[PENDIENTE] Búsquedas y reportes se implementan en commit 5.");
        }

        // ===================== PERSISTENCIA (commit 5 lo vuelve real) =====================
        static void ShowPersistenceMenu()
        {
            Console.WriteLine("[PENDIENTE] Guardar/Cargar/Reset se implementa en commit 5 (simulación en memoria).");
        }

        // ===================== SALIDA =====================
        static bool ConfirmExitAndSave()
        {
            bool save = ConfirmYesNo("¿Desea guardar los datos antes de salir? (S/N): ");

            if (save)
            {
                Console.WriteLine("[INFO] Guardando datos... (simulación)");
                Console.WriteLine("[OK] Datos guardados correctamente.");
            }
            else
            {
                Console.WriteLine("[INFO] No se guardaron cambios.");
            }

            Console.WriteLine("[SYSTEM] Cerrando aplicación. ¡Hasta pronto!");
            return true;
        }
    }
}
