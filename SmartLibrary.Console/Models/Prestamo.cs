namespace SmartLibrary.Console.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public Libro Libro { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public EstadoPrestamo Estado { get; set; } = EstadoPrestamo.Activo;

        // Constructor vacío
        public Prestamo() { }

        // Constructor completo
        public Prestamo(int id, Libro libro, Usuario usuario, DateTime fechaPrestamo, DateTime fechaDevolucion)
        {
            Id = id;
            Libro = libro;
            Usuario = usuario;
            FechaPrestamo = fechaPrestamo;
            FechaDevolucion = fechaDevolucion;
            Estado = EstadoPrestamo.Activo;
        }
    }
}