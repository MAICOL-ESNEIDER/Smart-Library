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
                int option = ReadOption();

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
                        ShowSearchMenu();
                        break;

                    case 5:
                        ShowDataMenu();
                        break;

                    case 6:
                        running = ExitApplication();
                        break;
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

        static int ReadOption()
        {
            while (true)
            {
                Console.Write("Seleccione una opción: ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int option) && option >= 1 && option <= 6)
                    return option;

                Console.WriteLine("Opción inválida.");
            }
        }

        static void ShowBooksMenu()
        {
            Console.WriteLine("=== MENÚ LIBROS ===");
            Console.WriteLine("1. Registrar libro");
            Console.WriteLine("2. Listar libros");
            Console.WriteLine("3. Ver detalle de libro");
            Console.WriteLine("4. Actualizar libro");
            Console.WriteLine("5. Eliminar libro");
            Console.WriteLine("0. Volver");

            Console.WriteLine("Funcionalidad en desarrollo...");
        }

        static void ShowUsersMenu()
        {
            Console.WriteLine("=== MENÚ USUARIOS ===");
            Console.WriteLine("1. Registrar usuario");
            Console.WriteLine("2. Listar usuarios");
            Console.WriteLine("3. Ver detalle de usuario");
            Console.WriteLine("4. Actualizar usuario");
            Console.WriteLine("5. Eliminar usuario");
            Console.WriteLine("0. Volver");

            Console.WriteLine("Funcionalidad en desarrollo...");
        }

        static void ShowLoansMenu()
        {
            Console.WriteLine("=== MENÚ PRÉSTAMOS ===");
            Console.WriteLine("1. Crear préstamo");
            Console.WriteLine("2. Listar préstamos");
            Console.WriteLine("3. Ver detalle de préstamo");
            Console.WriteLine("4. Registrar devolución");
            Console.WriteLine("5. Eliminar préstamo");
            Console.WriteLine("0. Volver");

            Console.WriteLine("Funcionalidad en desarrollo...");
        }

        static void ShowSearchMenu()
        {
            Console.WriteLine("=== BÚSQUEDAS Y REPORTES ===");
            Console.WriteLine("1. Buscar libro");
            Console.WriteLine("2. Buscar usuario");
            Console.WriteLine("3. Reportes");
            Console.WriteLine("0. Volver");

            Console.WriteLine("Funcionalidad en desarrollo...");
        }

        static void ShowDataMenu()
        {
            Console.WriteLine("=== GUARDAR / CARGAR DATOS ===");
            Console.WriteLine("1. Guardar datos");
            Console.WriteLine("2. Cargar datos");
            Console.WriteLine("3. Reiniciar datos");
            Console.WriteLine("0. Volver");

            Console.WriteLine("Funcionalidad en desarrollo...");
        }

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