using System;
using System.Collections.Generic; // necesario para List<>
using SmartLibrary.App.Models;

namespace SmartLibrary.App
{
    class Program
    {
        static void Main(string[] args)
        {
            // === LISTAS DE PRUEBA ===
            var libros = new List<Libro>
            {
                new Libro(1, "Cien años de soledad", "Gabriel García Márquez", 1967, "Novela"),
                new Libro(2, "El coronel no tiene quien le escriba", "Gabriel García Márquez", 1961, "Novela")
            };

            var usuarios = new List<Usuario>
            {
                new Usuario(1, "Maicol Posada", "maicol@email.com"),
                new Usuario(2, "Ana Pérez", "ana@email.com")
            };

            var prestamos = new List<Prestamo>
            {
                new Prestamo(1, libros[0], usuarios[0], DateTime.Now, DateTime.Now.AddDays(7))
            };

            // === BLOQUE DE PRUEBAS DE MODELOS ===
            Console.WriteLine("---- PRUEBAS DE MODELOS ----");
            Console.WriteLine(libros[0].DetalleCompleto());
            Console.WriteLine(usuarios[0].DetalleCompleto());
            Console.WriteLine(prestamos[0].DetalleCompleto());
            Console.WriteLine();

            // === MENÚ PRINCIPAL ===
            bool running = true;

            while (running)
            {
                ShowMainMenu();
                int option = ReadOption(1, 6);

                switch (option)
                {
                    case 1: ShowBooksMenu(libros); break;
                    case 2: ShowUsersMenu(usuarios); break;
                    case 3: ShowLoansMenu(prestamos); break;
                    case 4: Console.WriteLine("Funcionalidad de búsquedas en desarrollo."); break;
                    case 5: Console.WriteLine("Funcionalidad de guardar/cargar en desarrollo."); break;
                    case 6: running = ExitApplication(); break;
                }

                Console.WriteLine();
            }
        }

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

                Console.WriteLine("Opción inválida.");
            }
        }

        // ===================== LIBROS =====================
      static void ShowBooksMenu(List<Libro> libros)
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
            case 1: Console.WriteLine("Simulación: registrar libro."); break;
            case 2:
                Console.WriteLine("=== LISTA DE LIBROS ===");
                foreach (var libro in libros)
                {
                    Console.WriteLine(libro.DetalleCompleto());
                }
                break;
            case 3: Console.WriteLine("Simulación: ver detalle de libro."); break;
            case 4: UpdateBookMenu(); break;
            case 5: DeleteBook(); break;
            case 0: back = true; break;
        }
        Console.WriteLine();
    }
}

       static void ShowListBooksMenu(List<Libro> libros)
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
                Console.WriteLine("=== TODOS LOS LIBROS ===");
                foreach (var libro in libros)
                {
                    Console.WriteLine(libro.DetalleCompleto());
                }
                break;
            case 2: Console.WriteLine("Simulación: mostrar libros disponibles."); break;
            case 3: Console.WriteLine("Simulación: mostrar libros prestados."); break;
            case 0: back = true; break;
        }
        Console.WriteLine();
    }
}

        static void UpdateBookMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== ACTUALIZAR LIBRO ===");
                Console.WriteLine("1. Editar título");
                Console.WriteLine("2. Editar autor");
                Console.WriteLine("3. Editar año/categoría");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 3);
                switch (option)
                {
                    case 1: Console.WriteLine("Simulación: editar título."); break;
                    case 2: Console.WriteLine("Simulación: editar autor."); break;
                    case 3: Console.WriteLine("Simulación: editar año/categoría."); break;
                    case 0: back = true; break;
                }
                Console.WriteLine();
            }
        }

        static void DeleteBook()
        {
            Console.WriteLine("Simulación: eliminar libro (validar que no esté prestado).");
        }

        // ===================== USUARIOS =====================
        static void ShowUsersMenu(List<Usuario> usuarios)
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
            case 1: Console.WriteLine("Simulación: registrar usuario."); break;
            case 2:
                Console.WriteLine("=== LISTA DE USUARIOS ===");
                foreach (var usuario in usuarios)
                {
                    Console.WriteLine(usuario.DetalleCompleto());
                }
                break;
            case 3: Console.WriteLine("Simulación: ver detalle de usuario."); break;
            case 4: UpdateUserMenu(); break;
            case 5: DeleteUser(); break;
            case 0: back = true; break;
        }
        Console.WriteLine();
    }
}

        static void UpdateUserMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== ACTUALIZAR USUARIO ===");
                Console.WriteLine("1. Editar nombre");
                Console.WriteLine("2. Editar contacto");
                Console.WriteLine("3. Activar/Desactivar usuario");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 3);
                switch (option)
                {
                    case 1: Console.WriteLine("Simulación: editar nombre."); break;
                    case 2: Console.WriteLine("Simulación: editar contacto."); break;
                    case 3: Console.WriteLine("Simulación: activar/desactivar usuario."); break;
                    case 0: back = true; break;
                }
                Console.WriteLine();
            }
        }

        static void DeleteUser()
        {
            Console.WriteLine("Simulación: eliminar usuario (validar que no tenga préstamos activos).");
        }
        // ===================== PRÉSTAMOS =====================
        static void ShowLoansMenu(List<Prestamo> prestamos)
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
            case 1: Console.WriteLine("Simulación: crear préstamo (validaciones)."); break;
            case 2: ShowListLoansMenu(prestamos); break;
            case 3: Console.WriteLine("Simulación: ver detalle de préstamo."); break;
            case 4: RegisterReturn(); break;
            case 5: DeleteLoan(); break;
            case 0: back = true; break;
        }
        Console.WriteLine();
    }
}

static void ShowListLoansMenu(List<Prestamo> prestamos)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine("=== LISTAR PRÉSTAMOS ===");
        Console.WriteLine("1. Listar todos");
        Console.WriteLine("2. Listar activos");
        Console.WriteLine("3. Listar cerrados");
        Console.WriteLine("0. Volver");

        int option = ReadOption(0, 3);
        switch (option)
        {
            case 1:
                Console.WriteLine("=== TODOS LOS PRÉSTAMOS ===");
                foreach (var prestamo in prestamos)
                {
                    Console.WriteLine(prestamo.DetalleCompleto());
                }
                break;
            case 2: Console.WriteLine("Simulación: mostrar préstamos activos."); break;
            case 3: Console.WriteLine("Simulación: mostrar préstamos cerrados."); break;
            case 0: back = true; break;
        }
        Console.WriteLine();
    }
}

        static void RegisterReturn()
        {
            Console.WriteLine("Simulación: registrar devolución (validar estado del préstamo).");
        }

        static void DeleteLoan()
        {
            Console.WriteLine("Simulación: eliminar préstamo (validar reglas de negocio).");
        }

        // ===================== BÚSQUEDAS Y REPORTES =====================
        static void ShowSearchMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== MENÚ BÚSQUEDAS Y REPORTES ===");
                Console.WriteLine("1. Buscar libros por título/autor");
                Console.WriteLine("2. Buscar usuarios por nombre");
                Console.WriteLine("3. Reporte de préstamos activos");
                Console.WriteLine("4. Reporte de préstamos vencidos");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 4);
                switch (option)
                {
                    case 1: Console.WriteLine("Simulación: buscar libros por título/autor."); break;
                    case 2: Console.WriteLine("Simulación: buscar usuarios por nombre."); break;
                    case 3: Console.WriteLine("Simulación: reporte de préstamos activos."); break;
                    case 4: Console.WriteLine("Simulación: reporte de préstamos vencidos."); break;
                    case 0: back = true; break;
                }
                Console.WriteLine();
            }
        }
        // ===================== PERSISTENCIA =====================
        static void ShowDataMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== MENÚ PERSISTENCIA ===");
                Console.WriteLine("1. Guardar datos");
                Console.WriteLine("2. Cargar datos");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 2);
                switch (option)
                {
                    case 1: SaveData(); break;
                    case 2: LoadData(); break;
                    case 0: back = true; break;
                }
                Console.WriteLine();
            }
        }
                static void SaveData()
        {
            Console.WriteLine("[INFO] Guardando datos en archivo... (simulación)");
            Console.WriteLine("[OK] Datos guardados correctamente.");
        }

        static void LoadData()
        {
            Console.WriteLine("[INFO] Cargando datos desde archivo... (simulación)");
            Console.WriteLine("[OK] Datos cargados correctamente.");
        }



              // ===================== SALIDA =====================
        static bool ExitApplication()
        {
            Console.Write("¿Desea guardar los datos antes de salir? (S/N): ");
            string? answer = Console.ReadLine();

            if (answer?.ToUpper() == "S")
            {
                Console.WriteLine("[INFO] Guardando datos... (simulación)");
                Console.WriteLine("[OK] Datos guardados correctamente.");
            }
            else
            {
                Console.WriteLine("[INFO] No se guardaron cambios.");
            }

            Console.WriteLine("[SYSTEM] Cerrando aplicación. ¡Hasta pronto!");
            return false;
        }
    }
}





