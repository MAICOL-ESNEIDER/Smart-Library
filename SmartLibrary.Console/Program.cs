using System;

namespace SmartLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                ShowMainMenu();
                int option = ReadOption(1, 6);

                switch (option)
                {
                    case 1: ShowBooksMenu(); break;
                    case 2: ShowUsersMenu(); break;
                    case 3: ShowLoansMenu(); break;
                    case 4: ShowSearchMenu(); break;
                    case 5: ShowDataMenu(); break;
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
                    case 1: Console.WriteLine("Simulación: registrar libro."); break;
                    case 2: ShowListBooksMenu(); break;
                    case 3: Console.WriteLine("Simulación: ver detalle de libro."); break;
                    case 4: UpdateBookMenu(); break;
                    case 5: DeleteBook(); break;
                    case 0: back = true; break;
                }
                Console.WriteLine();
            }
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
                    case 1: Console.WriteLine("Simulación: mostrar todos los libros."); break;
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
                    case 1: Console.WriteLine("Simulación: registrar usuario."); break;
                    case 2: Console.WriteLine("Simulación: listar usuarios."); break;
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
                    case 1: Console.WriteLine("Simulación: crear préstamo (validaciones)."); break;
                    case 2: ShowListLoansMenu(); break;
                    case 3: Console.WriteLine("Simulación: ver detalle de préstamo."); break;
                    case 4: RegisterReturn(); break;
                    case 5: DeleteLoan(); break;
                    case 0: back = true; break;
                }
                Console.WriteLine();
            }
        }

        static void ShowListLoansMenu()
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
                    case 1: Console.WriteLine("Simulación: mostrar todos los préstamos."); break;
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
        // ===================== DATOS =====================
        static void ShowDataMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== GUARDAR / CARGAR DATOS ===");
                Console.WriteLine("1. Guardar datos");
                Console.WriteLine("2. Cargar datos");
                Console.WriteLine("0. Volver");

                int option = ReadOption(0, 2);
                switch (option)
                {
                    case 1: Console.WriteLine("Simulación: guardar datos."); break;
                    case 2: Console.WriteLine("Simulación: cargar datos."); break;
                    case 0: back = true; break;
                }
                Console.WriteLine();
            }
        }

        // ===================== SALIDA =====================
        static bool ExitApplication()
        {
            Console.Write("¿Guardar antes de salir? (S/N): ");
            string? answer = Console.ReadLine();

            if (answer?.ToUpper() == "S")
            {
                Console.WriteLine("Guardando datos...");
            }

            Console.WriteLine("Saliendo del sistema...");
            return false;
        }
    }
}
