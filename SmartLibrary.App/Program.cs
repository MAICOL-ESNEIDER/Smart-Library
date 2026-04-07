using System;
using System.Linq;
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
                        ShowLoansMenu(); // COMMIT 4: ya es CRUD real
                        break;

                    case 4:
                        ShowSearchReportsMenu(); // commit 5
                        break;

                    case 5:
                        ShowPersistenceMenu(); // commit 5
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

            // NOTA: Tu modelo no tiene FechaLimite, así que usamos FechaDevolucion como "fecha estimada/límite" mientras está Activo.
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

        // ===================== LIBROS (CRUD REAL - COMMIT 2) =====================
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
                        RegisterBook();
                        break;
                    case 2:
                        ShowListBooksMenu();
                        break;
                    case 3:
                        ViewBookDetail();
                        break;
                    case 4:
                        UpdateBookMenu();
                        break;
                    case 5:
                        DeleteBook();
                        break;
                    case 0:
                        back = true;
                        break;
                }

                Console.WriteLine();
            }
        }

        static void RegisterBook()
        {
            Console.WriteLine("--- Registrar libro ---");

            int id = ReadInt("ID del libro: ");

            // Validar duplicado
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
                    case 1:
                        ListBooksAll();
                        break;
                    case 2:
                        ListBooksAvailable();
                        break;
                    case 3:
                        ListBooksBorrowed();
                        break;
                    case 0:
                        back = true;
                        break;
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

            var all = _libroService.ObtenerTodos();
            var available = all.Where(l => l.Disponible).ToList();

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

            var all = _libroService.ObtenerTodos();
            var borrowed = all.Where(l => !l.Disponible).ToList();

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
                        string nuevoTitulo = ReadText("Nuevo título: ");
                        libro.Titulo = nuevoTitulo;
                        Console.WriteLine("[OK] Título actualizado.");
                        break;

                    case 2:
                        string nuevoAutor = ReadText("Nuevo autor: ");
                        libro.Autor = nuevoAutor;
                        Console.WriteLine("[OK] Autor actualizado.");
                        break;

                    case 3:
                        int nuevoAnio = ReadInt("Nuevo año: ");
                        string nuevaCategoria = ReadText("Nueva categoría: ");
                        libro.Anio = nuevoAnio;
                        libro.Categoria = nuevaCategoria;
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

            // Regla: NO permitir eliminar si está prestado (préstamo activo)
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

        // ===================== USUARIOS (CRUD REAL - COMMIT 3) =====================
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
                        RegisterUser();
                        break;
                    case 2:
                        ListUsers();
                        break;
                    case 3:
                        ViewUserDetail();
                        break;
                    case 4:
                        UpdateUserMenu();
                        break;
                    case 5:
                        DeleteUser();
                        break;
                    case 0:
                        back = true;
                        break;
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
                        string nuevoNombre = ReadText("Nuevo nombre: ");
                        usuario.Nombre = nuevoNombre;
                        Console.WriteLine("[OK] Nombre actualizado.");
                        break;

                    case 2:
                        string nuevoContacto = ReadText("Nuevo contacto: ");
                        usuario.Contacto = nuevoContacto;
                        Console.WriteLine("[OK] Contacto actualizado.");
                        break;

                    case 3:
                        usuario.Activo = !usuario.Activo;
                        Console.WriteLine(usuario.Activo
                            ? "[OK] Usuario activado."
                            : "[OK] Usuario desactivado.");
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

            // Regla: NO permitir eliminar si tiene préstamos activos
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

        // ===================== PRÉSTAMOS (CRUD REAL - COMMIT 4) =====================
        static void ShowLoansMenu()
        {
            bool back = false;

            while (!back)
            {
                // Antes de mostrar listados, actualizamos vencidos en base a "fecha estimada/límite"
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
                    case 1:
                        CreateLoan();
                        break;
                    case 2:
                        ShowListLoansMenu();
                        break;
                    case 3:
                        ViewLoanDetail();
                        break;
                    case 4:
                        RegisterReturn();
                        break;
                    case 5:
                        DeleteLoan();
                        break;
                    case 0:
                        back = true;
                        break;
                }

                Console.WriteLine();
            }
        }

        // Marca préstamos vencidos si siguen Activo y la fecha estimada (FechaDevolucion) ya pasó
        static void UpdateOverdueStatuses()
        {
            var all = _prestamoService.ObtenerTodos();
            foreach (var p in all)
            {
                if (p.Estado == EstadoPrestamo.Activo && p.FechaDevolucion.HasValue && p.FechaDevolucion.Value < DateTime.Now)
                {
                    p.Estado = EstadoPrestamo.Vencido;
                }
            }
        }

        static void CreateLoan()
        {
            Console.WriteLine("--- Crear préstamo ---");

            int userId = ReadInt("ID del usuario: ");
            var usuario = _usuarioService.BuscarPorId(userId);
            if (usuario == null)
            {
                Console.WriteLine("[ERROR] Usuario no existe.");
                return;
            }
            if (!usuario.Activo)
            {
                Console.WriteLine("[ERROR] Usuario inactivo. No puede crear préstamos.");
                return;
            }

            int bookId = ReadInt("ID del libro: ");
            var libro = _libroService.BuscarPorId(bookId);
            if (libro == null)
            {
                Console.WriteLine("[ERROR] Libro no existe.");
                return;
            }
            if (!libro.Disponible)
            {
                Console.WriteLine("[ERROR] Libro no disponible. Ya está prestado.");
                return;
            }

            int dias = ReadInt("Días de préstamo (ej: 7): ");
            if (dias <= 0)
            {
                Console.WriteLine("[ERROR] Los días deben ser mayores a 0.");
                return;
            }

            // ID automático
            int nextId = _prestamoService.ObtenerTodos().Count == 0
                ? 1
                : _prestamoService.ObtenerTodos().Max(p => p.Id) + 1;

            var fechaPrestamo = DateTime.Now;
            var fechaEstimadaDevolucion = DateTime.Now.AddDays(dias);

            // Usamos constructor con "fechaDevolucion" como fecha estimada/límite
            var prestamo = new Prestamo(nextId, libro, usuario, fechaPrestamo, fechaEstimadaDevolucion);
            prestamo.Estado = EstadoPrestamo.Activo;

            // Marcar libro como no disponible
            libro.Disponible = false;

            _prestamoService.AgregarPrestamo(prestamo);

            Console.WriteLine("[OK] Préstamo creado correctamente.");
            Console.WriteLine(prestamo.DetalleCompleto());
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
                    case 1:
                        ListLoansAll();
                        break;
                    case 2:
                        ListLoansActive();
                        break;
                    case 3:
                        ListLoansClosed();
                        break;
                    case 0:
                        back = true;
                        break;
                }

                Console.WriteLine();
            }
        }

        static void ListLoansAll()
        {
            Console.WriteLine("--- TODOS LOS PRÉSTAMOS ---");

            var all = _prestamoService.ObtenerTodos();
            if (all.Count == 0)
            {
                Console.WriteLine("[INFO] No hay préstamos registrados.");
                return;
            }

            foreach (var p in all)
                Console.WriteLine(p.DetalleCompleto());
        }

        static void ListLoansActive()
        {
            Console.WriteLine("--- PRÉSTAMOS ACTIVOS ---");

            var active = _prestamoService.BuscarPorEstado(EstadoPrestamo.Activo);
            if (active.Count == 0)
            {
                Console.WriteLine("[INFO] No hay préstamos activos.");
                return;
            }

            foreach (var p in active)
                Console.WriteLine(p.DetalleCompleto());
        }

        static void ListLoansClosed()
        {
            Console.WriteLine("--- PRÉSTAMOS CERRADOS (DEVUELTO / VENCIDO) ---");

            var all = _prestamoService.ObtenerTodos();
            var closed = all.Where(p => p.Estado != EstadoPrestamo.Activo).ToList();

            if (closed.Count == 0)
            {
                Console.WriteLine("[INFO] No hay préstamos cerrados.");
                return;
            }

            foreach (var p in closed)
                Console.WriteLine(p.DetalleCompleto());
        }

        static void ViewLoanDetail()
        {
            Console.WriteLine("--- Ver detalle de préstamo ---");
            int id = ReadInt("Ingrese ID del préstamo: ");

            var prestamo = _prestamoService.BuscarPorId(id);
            if (prestamo == null)
            {
                Console.WriteLine("[INFO] Préstamo no encontrado.");
                return;
            }

            Console.WriteLine(prestamo.DetalleCompleto());
            Console.WriteLine($"Días transcurridos: {prestamo.DiasTranscurridos()}");
            Console.WriteLine($"Estado: {prestamo.Estado}");
        }

        static void RegisterReturn()
        {
            Console.WriteLine("--- Registrar devolución ---");
            int id = ReadInt("Ingrese ID del préstamo: ");

            var prestamo = _prestamoService.BuscarPorId(id);
            if (prestamo == null)
            {
                Console.WriteLine("[INFO] Préstamo no encontrado.");
                return;
            }

            if (prestamo.Estado != EstadoPrestamo.Activo)
            {
                Console.WriteLine("[INFO] El préstamo no está activo. Estado actual: " + prestamo.Estado);
                return;
            }

            bool confirm = ConfirmYesNo("¿Confirmar devolución? (S/N): ");
            if (!confirm)
            {
                Console.WriteLine("[INFO] Operación cancelada.");
                return;
            }

            // Marcar devuelto
            prestamo.Estado = EstadoPrestamo.Devuelto;

            // FechaDevolucion se usa como fecha estimada mientras está activo.
            // Al devolver, la sobreescribimos con la fecha real de devolución.
            prestamo.FechaDevolucion = DateTime.Now;

            // Marcar libro disponible
            if (prestamo.Libro != null)
                prestamo.Libro.Disponible = true;

            Console.WriteLine("[OK] Devolución registrada correctamente.");
            Console.WriteLine(prestamo.DetalleCompleto());
        }

        static void DeleteLoan()
        {
            Console.WriteLine("--- Eliminar préstamo ---");
            int id = ReadInt("Ingrese ID del préstamo a eliminar: ");

            var prestamo = _prestamoService.BuscarPorId(id);
            if (prestamo == null)
            {
                Console.WriteLine("[INFO] Préstamo no encontrado.");
                return;
            }

            // Regla sugerida: no borrar préstamos activos (para no perder trazabilidad)
            if (prestamo.Estado == EstadoPrestamo.Activo)
            {
                Console.WriteLine("[ERROR] No se puede eliminar un préstamo activo. Registre devolución primero.");
                return;
            }

            bool confirm = ConfirmYesNo($"¿Seguro que desea eliminar el préstamo #{prestamo.Id}? (S/N): ");
            if (!confirm)
            {
                Console.WriteLine("[INFO] Operación cancelada.");
                return;
            }

            _prestamoService.EliminarPrestamo(prestamo);
            Console.WriteLine("[OK] Préstamo eliminado correctamente.");
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
