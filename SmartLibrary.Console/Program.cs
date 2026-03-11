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
                ShowMenu();

                int option = ReadMenuOption();

                switch (option)
                {
                    case 1:
                        Console.WriteLine("Add Book - Coming soon...");
                        break;

                    case 2:
                        Console.WriteLine("View Books - Coming soon...");
                        break;

                    case 3:
                        Console.WriteLine("Exit selected.");
                        running = false;
                        break;
                }

                Console.WriteLine();
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("===== SMART LIBRARY =====");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. View Books");
            Console.WriteLine("3. Exit");
            Console.WriteLine("=========================");
        }

        static int ReadMenuOption()
        {
            while (true)
            {
                Console.Write("Select an option: ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int option) && option >= 1 && option <= 3)
                {
                    return option;
                }

                Console.WriteLine("Invalid option. Please enter a number between 1 and 3.");
            }
        }
    }
}