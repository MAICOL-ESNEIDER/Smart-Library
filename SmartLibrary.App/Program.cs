using System;
using System.Collections.Generic;
using System.Linq;
using SmartLibrary.App.Models;
using SmartLibrary.App.Services;

namespace SmartLibrary.App
{
    class Program
    {
        // Services en memoria (no readonly para poder reiniciar/cargar)
        private static LibroService _libroService = new LibroService();
        private static UsuarioService _usuarioService = new UsuarioService();
        private static PrestamoService _prestamoService = new PrestamoService();

        // "Persistencia" simulada (snapshots en memoria)
        private static List<Libro>? _savedLibros;
        private static List<Usuario>? _savedUsuarios;
        private static List<Prestamo>? _savedPrestamos;

        static void Main(string[] args)
        {
            // Ejecutar demo EV08 con:
            // dotnet run --project .\SmartLibrary.App\SmartLibrary.App.csproj -- --ev08
            if (args.Length > 0 && args[0].Equals("--ev08", StringComparison.OrdinalIgnoreCase))
            {
                Ev08Demo.Run();
                return;
            }

            SeedDataIfEmpty();

            bool running = true;
            while (running)
            {
                ShowMainMenu();
                int option = ReadOption(1, 8);

                switch (option)
                {
                    case 1: ShowBooksMenu(); break;
                    case 2: ShowUsersMenu(); break;
                    case 3: ShowLoansMenu(); break;
                    case 4: ShowSearchReportsMenu(); break;
                    case 5: ShowPersistenceMenu(); break;
                    case 6: ProbarServicios(); break;          // NUEVO
                    case 7: CompararArrayVsList(); break;      // NUEVO
                    case 8: running = !ConfirmExitAndSave(); break;
                }

                Console.WriteLine();
            }
        }

        // ===================== SEED =====================
        static void SeedDataIfEmpty()
        {
            if (_libroService.TotalLibros() > 0 || _usuarioService.TotalUsuarios() > 0 || _prestamoService.TotalPrestamos() > 0)
                return;

            var libro1 = new Libro(1, "Cien años de soledad", "Gabriel García Márquez", 1967, "Novela");
            var libro2 = new Libro(2, "El coronel no tiene quien le escriba", "Gabriel García Márquez", 1961, "Novela");

            _libroService.AgregarLibro(libro1);
            _libroService.AgregarLibro(libro2);

            var usuario1 = new Usuario(1, "Maicol Posada", "maicol@email.com");
            var usuario2 = new Usuario(2, "Ana Pérez", "ana@email.com");

            _usuarioService.AgregarUsuario(usuario1);
            _usuarioService.AgregarUsuario(usuario2);

            // 1 préstamo seed activo: libro1 no disponible
            libro1.Disponible = false;

            // NOTA: tu modelo no tiene FechaLimite => usamos FechaDevolucion como "fecha estimada/límite" mientras está activo
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
            Console.WriteLine("6. Probar Services");
            Console.WriteLine("7. Comparar Arrays vs List");
            Console.WriteLine("8. Salir");
            Console.WriteLine("=========================");
        }

        // ===================== HELPERS INPUT =====================
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

        static void Pause()
        {
            Console.WriteLine("\nPresione una tecla para continuar...");
            Console.ReadKey();
        }

        // ===================== LIBROS =====================
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
                    case 1: RegisterBook(); break;
                    case 2: ShowListBooksMenu(); break;
                    case 3: ViewBookDetail(); break;
                    case 4: UpdateBookMenu(); break;
                    case 5: DeleteBook(); break;
                    case 0: back = true; break;
                }

                Console.WriteLine();
            }
        }

        static void RegisterBook()
        {
            Console.WriteLine("--- Registrar libro ---");

            int id = ReadInt("ID del libro: ");
            var existing = _libroService.BuscarPorId(id);
            if (existing != null)
            {
                Console.WriteLine("[ERROR] Ya existe un libro con ese ID.");
                return;
            }

            string titulo = ReadText("Título: ");
            string autor = ReadText("Autor: ");
            int anio = ReadInt("Año: ");
            string categoria = ReadText("Categoría: ");

            var libro = new Libro(id, titulo, autor, anio, categoria);
            _libroService.AgregarLibro(libro);

            Console.WriteLine("[OK] Libro registrado correctamente.");
            Console.WriteLine(libro.DetalleCompleto());
        }

        static void ShowListBooksMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== LISTAR LIBROS ===");
                Console.WriteLine("1. Listar todos");
                Console.WriteLine("2. Listar disponibles");
                Console.WriteLine("3. Listar prestados");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 3);

                switch (option)
                {
                    case 1: ListBooksAll(); break;
                    case 2: ListBooksAvailable(); break;
                    case 3: ListBooksBorrowed(); break;
                    case 0: back = true; break;
                }

                Console.WriteLine();
            }
        }

        static void ListBooksAll()
        {
            Console.WriteLine("--- TODOS LOS LIBROS ---");
            var all = _libroService.ObtenerTodos();

            if (all.Count == 0)
            {
                Console.WriteLine("[INFO] No hay libros registrados.");
                return;
            }

            foreach (var libro in all)
                Console.WriteLine(libro.DetalleCompleto());
        }

        static void ListBooksAvailable()
        {
            Console.WriteLine("--- LIBROS DISPONIBLES ---");
            var available = _libroService.ObtenerTodos().Where(l => l.Disponible).ToList();

            if (available.Count == 0)
            {
                Console.WriteLine("[INFO] No hay libros disponibles.");
                return;
            }

            foreach (var libro in available)
                Console.WriteLine(libro.DetalleCompleto());
        }

        static void ListBooksBorrowed()
        {
            Console.WriteLine("--- LIBROS PRESTADOS ---");
            var borrowed = _libroService.ObtenerTodos().Where(l => !l.Disponible).ToList();

            if (borrowed.Count == 0)
            {
                Console.WriteLine("[INFO] No hay libros prestados.");
                return;
            }

            foreach (var libro in borrowed)
                Console.WriteLine(libro.DetalleCompleto());
        }

        static void ViewBookDetail()
        {
            Console.WriteLine("--- Ver detalle de libro ---");
            int id = ReadInt("Ingrese ID del libro: ");

            var libro = _libroService.BuscarPorId(id);
            if (libro == null)
            {
                Console.WriteLine("[INFO] Libro no encontrado.");
                return;
            }

            Console.WriteLine(libro.DetalleCompleto());
        }

        static void UpdateBookMenu()
        {
            Console.WriteLine("--- Actualizar libro ---");
            int id = ReadInt("Ingrese ID del libro a actualizar: ");

            var libro = _libroService.BuscarPorId(id);
            if (libro == null)
            {
                Console.WriteLine("[INFO] Libro no encontrado.");
                return;
            }

            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== ACTUALIZAR LIBRO ===");
                Console.WriteLine("1. Editar título");
                Console.WriteLine("2. Editar autor");
                Console.WriteLine("3. Editar año / categoría");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 3);

                switch (option)
                {
                    case 1:
                        libro.Titulo = ReadText("Nuevo título: ");
                        Console.WriteLine("[OK] Título actualizado.");
                        break;

                    case 2:
                        libro.Autor = ReadText("Nuevo autor: ");
                        Console.WriteLine("[OK] Autor actualizado.");
                        break;

                    case 3:
                        libro.Anio = ReadInt("Nuevo año: ");
                        libro.Categoria = ReadText("Nueva categoría: ");
                        Console.WriteLine("[OK] Año y categoría actualizados.");
                        break;

                    case 0:
                        back = true;
                        break;
                }

                Console.WriteLine();
            }

            Console.WriteLine("Libro actualizado:");
            Console.WriteLine(libro.DetalleCompleto());
        }

        static void DeleteBook()
        {
            Console.WriteLine("--- Eliminar libro ---");
            int id = ReadInt("Ingrese ID del libro a eliminar: ");

            var libro = _libroService.BuscarPorId(id);
            if (libro == null)
            {
                Console.WriteLine("[INFO] Libro no encontrado.");
                return;
            }

            bool enPrestamoActivo = _prestamoService.ObtenerTodos()
                .Any(p => p.Estado == EstadoPrestamo.Activo && p.Libro != null && p.Libro.Id == id);

            if (enPrestamoActivo || !libro.Disponible)
            {
                Console.WriteLine("[ERROR] No se puede eliminar: el libro está prestado (préstamo activo).");
                return;
            }

            bool confirm = ConfirmYesNo($"¿Seguro que desea eliminar '{libro.Titulo}'? (S/N): ");
            if (!confirm)
            {
                Console.WriteLine("[INFO] Operación cancelada.");
                return;
            }

            _libroService.EliminarLibro(libro);
            Console.WriteLine("[OK] Libro eliminado correctamente.");
        }

        // ===================== USUARIOS =====================
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
                    case 1: RegisterUser(); break;
                    case 2: ListUsers(); break;
                    case 3: ViewUserDetail(); break;
                    case 4: UpdateUserMenu(); break;
                    case 5: DeleteUser(); break;
                    case 0: back = true; break;
                }

                Console.WriteLine();
            }
        }

        static void RegisterUser()
        {
            Console.WriteLine("--- Registrar usuario ---");

            int id = ReadInt("ID del usuario: ");
            var existing = _usuarioService.BuscarPorId(id);
            if (existing != null)
            {
                Console.WriteLine("[ERROR] Ya existe un usuario con ese ID.");
                return;
            }

            string nombre = ReadText("Nombre: ");
            string contacto = ReadText("Contacto (email/teléfono): ");

            var usuario = new Usuario(id, nombre, contacto);
            _usuarioService.AgregarUsuario(usuario);

            Console.WriteLine("[OK] Usuario registrado correctamente.");
            Console.WriteLine(usuario.DetalleCompleto());
        }

        static void ListUsers()
        {
            Console.WriteLine("--- LISTA DE USUARIOS ---");
            var all = _usuarioService.ObtenerTodos();

            if (all.Count == 0)
            {
                Console.WriteLine("[INFO] No hay usuarios registrados.");
                return;
            }

            foreach (var u in all)
                Console.WriteLine(u.DetalleCompleto());
        }

        static void ViewUserDetail()
        {
            Console.WriteLine("--- Ver detalle de usuario ---");
            int id = ReadInt("Ingrese ID del usuario: ");

            var usuario = _usuarioService.BuscarPorId(id);
            if (usuario == null)
            {
                Console.WriteLine("[INFO] Usuario no encontrado.");
                return;
            }

            Console.WriteLine(usuario.DetalleCompleto());
        }

        static void UpdateUserMenu()
        {
            Console.WriteLine("--- Actualizar usuario ---");
            int id = ReadInt("Ingrese ID del usuario a actualizar: ");

            var usuario = _usuarioService.BuscarPorId(id);
            if (usuario == null)
            {
                Console.WriteLine("[INFO] Usuario no encontrado.");
                return;
            }

            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== ACTUALIZAR USUARIO ===");
                Console.WriteLine("1. Editar nombre");
                Console.WriteLine("2. Editar contacto");
                Console.WriteLine("3. Activar / desactivar");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 3);

                switch (option)
                {
                    case 1:
                        usuario.Nombre = ReadText("Nuevo nombre: ");
                        Console.WriteLine("[OK] Nombre actualizado.");
                        break;

                    case 2:
                        usuario.Contacto = ReadText("Nuevo contacto: ");
                        Console.WriteLine("[OK] Contacto actualizado.");
                        break;

                    case 3:
                        usuario.Activo = !usuario.Activo;
                        Console.WriteLine(usuario.Activo ? "[OK] Usuario activado." : "[OK] Usuario desactivado.");
                        break;

                    case 0:
                        back = true;
                        break;
                }

                Console.WriteLine();
            }

            Console.WriteLine("Usuario actualizado:");
            Console.WriteLine(usuario.DetalleCompleto());
        }

        static void DeleteUser()
        {
            Console.WriteLine("--- Eliminar usuario ---");
            int id = ReadInt("Ingrese ID del usuario a eliminar: ");

            var usuario = _usuarioService.BuscarPorId(id);
            if (usuario == null)
            {
                Console.WriteLine("[INFO] Usuario no encontrado.");
                return;
            }

            bool tienePrestamoActivo = _prestamoService.ObtenerTodos()
                .Any(p => p.Estado == EstadoPrestamo.Activo && p.Usuario != null && p.Usuario.Id == id);

            if (tienePrestamoActivo)
            {
                Console.WriteLine("[ERROR] No se puede eliminar: el usuario tiene préstamos activos.");
                return;
            }

            bool confirm = ConfirmYesNo($"¿Seguro que desea eliminar a '{usuario.Nombre}'? (S/N): ");
            if (!confirm)
            {
                Console.WriteLine("[INFO] Operación cancelada.");
                return;
            }

            _usuarioService.EliminarUsuario(usuario);
            Console.WriteLine("[OK] Usuario eliminado correctamente.");
        }

        // ====== PARTE 2 CONTINÚA DESDE AQUÍ ======
        // ===================== PRÉSTAMOS =====================
        static void ShowLoansMenu()
        {
            bool back = false;
            while (!back)
            {
                UpdateOverdueStatuses();

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
                    case 1: CreateLoan(); break;
                    case 2: ShowListLoansMenu(); break;
                    case 3: ViewLoanDetail(); break;
                    case 4: RegisterReturn(); break;
                    case 5: DeleteLoan(); break;
                    case 0: back = true; break;
                }

                Console.WriteLine();
            }
        }

        // Marca vencidos: mientras esté Activo y su "fecha estimada/límite" (FechaDevolucion) ya pasó
        static void UpdateOverdueStatuses()
        {
            var all = _prestamoService.ObtenerTodos();
            foreach (var p in all)
            {
                if (p.Estado == EstadoPrestamo.Activo && p.FechaDevolucion.HasValue && p.FechaDevolucion.Value < DateTime.Now)
                    p.Estado = EstadoPrestamo.Vencido;
            }
        }

        static void CreateLoan()
        {
            Console.WriteLine("--- Crear préstamo ---");

            int userId = ReadInt("ID del usuario: ");
            var usuario = _usuarioService.BuscarPorId(userId);
            if (usuario == null) { Console.WriteLine("[ERROR] Usuario no existe."); Pause(); return; }
            if (!usuario.Activo) { Console.WriteLine("[ERROR] Usuario inactivo. No puede crear préstamos."); Pause(); return; }

            int bookId = ReadInt("ID del libro: ");
            var libro = _libroService.BuscarPorId(bookId);
            if (libro == null) { Console.WriteLine("[ERROR] Libro no existe."); Pause(); return; }
            if (!libro.Disponible) { Console.WriteLine("[ERROR] Libro no disponible. Ya está prestado."); Pause(); return; }

            int dias = ReadInt("Días de préstamo (ej: 7): ");
            if (dias <= 0) { Console.WriteLine("[ERROR] Los días deben ser mayores a 0."); Pause(); return; }

            int nextId = _prestamoService.ObtenerTodos().Count == 0
                ? 1
                : _prestamoService.ObtenerTodos().Max(p => p.Id) + 1;

            var fechaPrestamo = DateTime.Now;
            var fechaEstimada = DateTime.Now.AddDays(dias);

            // Usamos constructor con "fechaDevolucion" como fecha estimada/límite (simulación)
            var prestamo = new Prestamo(nextId, libro, usuario, fechaPrestamo, fechaEstimada);
            prestamo.Estado = EstadoPrestamo.Activo;

            // Marcar libro no disponible
            libro.Disponible = false;

            _prestamoService.AgregarPrestamo(prestamo);

            Console.WriteLine("[OK] Préstamo creado correctamente.");
            Console.WriteLine(prestamo.DetalleCompleto());
            Pause();
        }

        static void ShowListLoansMenu()
        {
            bool back = false;
            while (!back)
            {
                UpdateOverdueStatuses();

                Console.WriteLine("=== LISTAR PRÉSTAMOS ===");
                Console.WriteLine("1. Listar todos");
                Console.WriteLine("2. Listar activos");
                Console.WriteLine("3. Listar cerrados (devueltos/vencidos)");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 3);

                switch (option)
                {
                    case 1: ListLoansAll(); break;
                    case 2: ListLoansActive(); break;
                    case 3: ListLoansClosed(); break;
                    case 0: back = true; break;
                }

                Console.WriteLine();
            }
        }

        static void ListLoansAll()
        {
            Console.WriteLine("--- TODOS LOS PRÉSTAMOS ---");
            var all = _prestamoService.ObtenerTodos();
            if (all.Count == 0) { Console.WriteLine("[INFO] No hay préstamos registrados."); return; }
            foreach (var p in all) Console.WriteLine(p.DetalleCompleto());
            Pause();
        }

        static void ListLoansActive()
        {
            Console.WriteLine("--- PRÉSTAMOS ACTIVOS ---");
            var active = _prestamoService.BuscarPorEstado(EstadoPrestamo.Activo);
            if (active.Count == 0) { Console.WriteLine("[INFO] No hay préstamos activos."); Pause(); return; }
            foreach (var p in active) Console.WriteLine(p.DetalleCompleto());
            Pause();
        }

        static void ListLoansClosed()
        {
            Console.WriteLine("--- PRÉSTAMOS CERRADOS (DEVUELTO / VENCIDO) ---");
            var closed = _prestamoService.ObtenerTodos().Where(p => p.Estado != EstadoPrestamo.Activo).ToList();
            if (closed.Count == 0) { Console.WriteLine("[INFO] No hay préstamos cerrados."); Pause(); return; }
            foreach (var p in closed) Console.WriteLine(p.DetalleCompleto());
            Pause();
        }

        static void ViewLoanDetail()
        {
            Console.WriteLine("--- Ver detalle de préstamo ---");
            int id = ReadInt("Ingrese ID del préstamo: ");

            var prestamo = _prestamoService.BuscarPorId(id);
            if (prestamo == null) { Console.WriteLine("[INFO] Préstamo no encontrado."); Pause(); return; }

            UpdateOverdueStatuses();
            Console.WriteLine(prestamo.DetalleCompleto());
            Console.WriteLine($"Días transcurridos: {prestamo.DiasTranscurridos()}");
            Console.WriteLine($"¿Está vencido?: {prestamo.EstaVencido()}");
            Console.WriteLine($"Estado: {prestamo.Estado}");
            Pause();
        }

        static void RegisterReturn()
        {
            Console.WriteLine("--- Registrar devolución ---");
            int id = ReadInt("Ingrese ID del préstamo: ");

            var prestamo = _prestamoService.BuscarPorId(id);
            if (prestamo == null) { Console.WriteLine("[INFO] Préstamo no encontrado."); Pause(); return; }

            UpdateOverdueStatuses();

            if (prestamo.Estado != EstadoPrestamo.Activo)
            {
                Console.WriteLine("[INFO] El préstamo no está activo. Estado actual: " + prestamo.Estado);
                Pause();
                return;
            }

            bool confirm = ConfirmYesNo("¿Confirmar devolución? (S/N): ");
            if (!confirm) { Console.WriteLine("[INFO] Operación cancelada."); Pause(); return; }

            prestamo.Estado = EstadoPrestamo.Devuelto;
            prestamo.FechaDevolucion = DateTime.Now;

            if (prestamo.Libro != null)
                prestamo.Libro.Disponible = true;

            Console.WriteLine("[OK] Devolución registrada correctamente.");
            Console.WriteLine(prestamo.DetalleCompleto());
            Pause();
        }

        static void DeleteLoan()
        {
            Console.WriteLine("--- Eliminar préstamo ---");
            int id = ReadInt("Ingrese ID del préstamo a eliminar: ");

            var prestamo = _prestamoService.BuscarPorId(id);
            if (prestamo == null) { Console.WriteLine("[INFO] Préstamo no encontrado."); Pause(); return; }

            UpdateOverdueStatuses();

            if (prestamo.Estado == EstadoPrestamo.Activo)
            {
                Console.WriteLine("[ERROR] No se puede eliminar un préstamo activo. Registre devolución primero.");
                Pause();
                return;
            }

            bool confirm = ConfirmYesNo($"¿Seguro que desea eliminar el préstamo #{prestamo.Id}? (S/N): ");
            if (!confirm) { Console.WriteLine("[INFO] Operación cancelada."); Pause(); return; }

            // Asegurar disponibilidad del libro
            if (prestamo.Libro != null)
                prestamo.Libro.Disponible = true;

            _prestamoService.EliminarPrestamo(prestamo);
            Console.WriteLine("[OK] Préstamo eliminado correctamente.");
            Pause();
        }

        // ===================== BÚSQUEDAS Y REPORTES =====================
        static void ShowSearchReportsMenu()
        {
            bool back = false;
            while (!back)
            {
                UpdateOverdueStatuses();

                Console.WriteLine("=== MENÚ BÚSQUEDAS Y REPORTES ===");
                Console.WriteLine("1. Buscar libro (id/título/autor/categoría)");
                Console.WriteLine("2. Buscar usuario (id/nombre)");
                Console.WriteLine("3. Reportes");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 3);

                switch (option)
                {
                    case 1: SearchBookMenu(); break;
                    case 2: SearchUserMenu(); break;
                    case 3: ReportsMenu(); break;
                    case 0: back = true; break;
                }

                Console.WriteLine();
            }
        }

        static void SearchBookMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("--- Buscar libro ---");
                Console.WriteLine("1. Por ID");
                Console.WriteLine("2. Por título");
                Console.WriteLine("3. Por autor");
                Console.WriteLine("4. Por categoría");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 4);

                switch (option)
                {
                    case 1:
                        int id = ReadInt("ID: ");
                        var b = _libroService.BuscarPorId(id);
                        Console.WriteLine(b != null ? b.DetalleCompleto() : "[INFO] No encontrado.");
                        Pause();
                        break;

                    case 2:
                        string t = ReadText("Texto en título: ");
                        var byTitle = _libroService.BuscarPorTitulo(t);
                        if (byTitle.Count == 0) Console.WriteLine("[INFO] Sin resultados.");
                        else byTitle.ForEach(x => Console.WriteLine(x.DetalleCompleto()));
                        Pause();
                        break;

                    case 3:
                        string a = ReadText("Texto en autor: ");
                        var byAuthor = _libroService.BuscarPorAutor(a);
                        if (byAuthor.Count == 0) Console.WriteLine("[INFO] Sin resultados.");
                        else byAuthor.ForEach(x => Console.WriteLine(x.DetalleCompleto()));
                        Pause();
                        break;

                    case 4:
                        string c = ReadText("Categoría: ");
                        var byCat = _libroService.ObtenerTodos()
                            .Where(x => x.Categoria != null && x.Categoria.Contains(c, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        if (byCat.Count == 0) Console.WriteLine("[INFO] Sin resultados.");
                        else byCat.ForEach(x => Console.WriteLine(x.DetalleCompleto()));
                        Pause();
                        break;

                    case 0:
                        back = true;
                        break;
                }
                Console.WriteLine();
            }
        }

        static void SearchUserMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("--- Buscar usuario ---");
                Console.WriteLine("1. Por ID");
                Console.WriteLine("2. Por nombre");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 2);

                switch (option)
                {
                    case 1:
                        int id = ReadInt("ID: ");
                        var u = _usuarioService.BuscarPorId(id);
                        Console.WriteLine(u != null ? u.DetalleCompleto() : "[INFO] No encontrado.");
                        Pause();
                        break;

                    case 2:
                        string n = ReadText("Texto en nombre: ");
                        var byName = _usuarioService.BuscarPorNombre(n);
                        if (byName.Count == 0) Console.WriteLine("[INFO] Sin resultados.");
                        else byName.ForEach(x => Console.WriteLine(x.DetalleCompleto()));
                        Pause();
                        break;

                    case 0:
                        back = true;
                        break;
                }
                Console.WriteLine();
            }
        }

        static void ReportsMenu()
        {
            bool back = false;
            while (!back)
            {
                UpdateOverdueStatuses();

                Console.WriteLine("=== REPORTES ===");
                Console.WriteLine("1. Reporte por usuario");
                Console.WriteLine("2. Reporte por libro");
                Console.WriteLine("3. Reporte vencidos");
                Console.WriteLine("4. Resumen (KPIs)");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 4);

                switch (option)
                {
                    case 1: ReportByUser(); break;
                    case 2: ReportByBook(); break;
                    case 3: ReportOverdue(); break;
                    case 4: ReportSummary(); break;
                    case 0: back = true; break;
                }

                Console.WriteLine();
            }
        }

        static void ReportByUser()
        {
            Console.WriteLine("--- Reporte por usuario ---");
            int id = ReadInt("ID usuario: ");

            var user = _usuarioService.BuscarPorId(id);
            if (user == null) { Console.WriteLine("[INFO] Usuario no encontrado."); Pause(); return; }

            var loans = _prestamoService.ObtenerTodos()
                .Where(p => p.Usuario != null && p.Usuario.Id == id)
                .ToList();

            Console.WriteLine(user.DetalleCompleto());
            if (loans.Count == 0) { Console.WriteLine("[INFO] No tiene préstamos."); Pause(); return; }

            loans.ForEach(p => Console.WriteLine(p.DetalleCompleto()));
            Pause();
        }

        static void ReportByBook()
        {
            Console.WriteLine("--- Reporte por libro ---");
            int id = ReadInt("ID libro: ");

            var book = _libroService.BuscarPorId(id);
            if (book == null) { Console.WriteLine("[INFO] Libro no encontrado."); Pause(); return; }

            var loans = _prestamoService.ObtenerTodos()
                .Where(p => p.Libro != null && p.Libro.Id == id)
                .ToList();

            Console.WriteLine(book.DetalleCompleto());
            if (loans.Count == 0) { Console.WriteLine("[INFO] No tiene préstamos."); Pause(); return; }

            loans.ForEach(p => Console.WriteLine(p.DetalleCompleto()));
            Pause();
        }

        static void ReportOverdue()
        {
            Console.WriteLine("--- Reporte préstamos vencidos ---");
            var overdue = _prestamoService.BuscarPorEstado(EstadoPrestamo.Vencido);

            if (overdue.Count == 0)
            {
                Console.WriteLine("[INFO] No hay préstamos vencidos.");
                Pause();
                return;
            }

            overdue.ForEach(p => Console.WriteLine(p.DetalleCompleto()));
            Pause();
        }

        static void ReportSummary()
        {
            Console.WriteLine("--- Resumen (KPIs) ---");

            Console.WriteLine($"Libros: Total={_libroService.TotalLibros()}, Disponibles={_libroService.LibrosDisponibles()}, Prestados={_libroService.LibrosPrestados()}");
            Console.WriteLine($"Usuarios: Total={_usuarioService.TotalUsuarios()}, Activos={_usuarioService.UsuariosActivos()}, Inactivos={_usuarioService.UsuariosInactivos()}");
            Console.WriteLine($"Préstamos: Total={_prestamoService.TotalPrestamos()}, Activos={_prestamoService.PrestamosActivos()}, Devueltos={_prestamoService.PrestamosDevueltos()}, Vencidos={_prestamoService.PrestamosVencidos()}");
            Console.WriteLine($"Promedio días préstamo: {_prestamoService.PromedioDiasPrestamo():0.00}");
            Pause();
        }

        // ===================== PERSISTENCIA =====================
        static void ShowPersistenceMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== MENÚ PERSISTENCIA ===");
                Console.WriteLine("1. Guardar datos (simulación)");
                Console.WriteLine("2. Cargar datos (simulación)");
                Console.WriteLine("3. Reiniciar datos (S/N)");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 3);

                switch (option)
                {
                    case 1: SaveData(); break;
                    case 2: LoadData(); break;
                    case 3: ResetData(); break;
                    case 0: back = true; break;
                }

                Console.WriteLine();
            }
        }

        static void SaveData()
        {
            _savedLibros = new List<Libro>(_libroService.ObtenerTodos());
            _savedUsuarios = new List<Usuario>(_usuarioService.ObtenerTodos());
            _savedPrestamos = new List<Prestamo>(_prestamoService.ObtenerTodos());

            Console.WriteLine("[INFO] Guardando datos... (simulación en memoria)");
            Console.WriteLine($"[OK] Guardado: Libros={_savedLibros.Count}, Usuarios={_savedUsuarios.Count}, Préstamos={_savedPrestamos.Count}");
            Pause();
        }

        static void LoadData()
        {
            if (_savedLibros == null || _savedUsuarios == null || _savedPrestamos == null)
            {
                Console.WriteLine("[INFO] No hay datos guardados para cargar (simulación).");
                Pause();
                return;
            }

            Console.WriteLine("[INFO] Cargando datos... (simulación en memoria)");

            _libroService = new LibroService();
            _usuarioService = new UsuarioService();
            _prestamoService = new PrestamoService();

            foreach (var b in _savedLibros) _libroService.AgregarLibro(b);
            foreach (var u in _savedUsuarios) _usuarioService.AgregarUsuario(u);
            foreach (var p in _savedPrestamos) _prestamoService.AgregarPrestamo(p);

            UpdateOverdueStatuses();
            Console.WriteLine("[OK] Datos cargados correctamente (simulación).");
            Pause();
        }

        static void ResetData()
        {
            bool confirm = ConfirmYesNo("¿Seguro que desea REINICIAR los datos? (S/N): ");
            if (!confirm)
            {
                Console.WriteLine("[INFO] Reinicio cancelado.");
                Pause();
                return;
            }

            _libroService = new LibroService();
            _usuarioService = new UsuarioService();
            _prestamoService = new PrestamoService();

            Console.WriteLine("[OK] Datos reiniciados (simulación).");
            Pause();
        }

        // ===================== OPCIÓN 6: PROBAR SERVICES =====================
        static void ProbarServicios()
        {
            Console.WriteLine("=== PRUEBA SERVICES ===");

            Console.WriteLine($"Libros: Total={_libroService.TotalLibros()} Disponibles={_libroService.LibrosDisponibles()} Prestados={_libroService.LibrosPrestados()}");
            Console.WriteLine($"Usuarios: Total={_usuarioService.TotalUsuarios()} Activos={_usuarioService.UsuariosActivos()} Inactivos={_usuarioService.UsuariosInactivos()}");
            Console.WriteLine($"Préstamos: Total={_prestamoService.TotalPrestamos()} Activos={_prestamoService.PrestamosActivos()} Devueltos={_prestamoService.PrestamosDevueltos()} Vencidos={_prestamoService.PrestamosVencidos()}");
            Console.WriteLine($"Promedio días préstamo: {_prestamoService.PromedioDiasPrestamo():0.00}");
            Console.WriteLine();

            Console.WriteLine("Libros ordenados por título:");
            foreach (var l in _libroService.OrdenarPorTitulo())
                Console.WriteLine(" - " + l.ResumenCorto());

            Console.WriteLine("Usuarios ordenados por nombre:");
            foreach (var u in _usuarioService.OrdenarPorNombre())
                Console.WriteLine(" - " + u.ResumenCorto());

            Console.WriteLine("Préstamos ordenados por fecha préstamo (ASC):");
            foreach (var p in _prestamoService.OrdenarPorFechaPrestamoAsc())
                Console.WriteLine(" - " + p.ResumenCorto());

            Pause();
        }

        // ===================== OPCIÓN 7: COMPARAR ARRAYS vs LIST =====================
        static void CompararArrayVsList()
        {
            Console.WriteLine("=== COMPARACIÓN ARRAY vs LIST ===");

            string[] arrayNombres = new string[2];
            arrayNombres[0] = "Ana";
            arrayNombres[1] = "Juan";

            Console.WriteLine($"ARRAY tamaño fijo = {arrayNombres.Length}");
            Console.WriteLine($"ARRAY[0]={arrayNombres[0]}, ARRAY[1]={arrayNombres[1]}");

            Console.WriteLine("Para agregar un 3er elemento en ARRAY: crear nuevo y copiar.");
            string[] nuevoArray = new string[arrayNombres.Length + 1];
            for (int i = 0; i < arrayNombres.Length; i++)
                nuevoArray[i] = arrayNombres[i];
            nuevoArray[2] = "Carlos";
            Console.WriteLine($"Nuevo ARRAY tamaño = {nuevoArray.Length}, nuevoArray[2]={nuevoArray[2]}");

            var listNombres = new List<string>();
            listNombres.Add("Ana");
            listNombres.Add("Juan");
            Console.WriteLine($"LIST Count = {listNombres.Count}");

            listNombres.Add("Carlos");
            Console.WriteLine($"LIST Count después de Add = {listNombres.Count}");

            listNombres.Remove("Ana");
            Console.WriteLine($"LIST Count después de Remove = {listNombres.Count}");

            Console.WriteLine("Conclusión: ARRAY fijo / LIST dinámico.");
            Pause();
        }

        // ===================== SALIDA =====================
        static bool ConfirmExitAndSave()
        {
            bool save = ConfirmYesNo("¿Desea guardar los datos antes de salir? (S/N): ");

            if (save)
            {
                SaveData();
                Console.WriteLine("[OK] Guardado antes de salir.");
            }
            else
            {
                Console.WriteLine("[INFO] No se guardaron cambios.");
            }

            Console.WriteLine("[SYSTEM] Cerrando aplicación. ¡Hasta pronto!");
            Pause();
            return true;
        }
    }
}
