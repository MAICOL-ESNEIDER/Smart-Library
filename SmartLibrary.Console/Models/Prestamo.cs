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

        public Prestamo() { }

        public Prestamo(int id, Libro libro, Usuario usuario, DateTime fechaPrestamo, DateTime fechaDevolucion)
        {
            Id = id;
            Libro = libro;
            Usuario = usuario;
            FechaPrestamo = fechaPrestamo;
            FechaDevolucion = fechaDevolucion;
            Estado = EstadoPrestamo.Activo;
        }

        // Métodos
        public string ResumenCorto() => $"{Id} - {Libro?.Titulo} ({Usuario?.Nombre})";
        public string DetalleCompleto() =>
            $"[{Id}] Libro: {Libro?.Titulo}, Usuario: {Usuario?.Nombre}, Prestado: {FechaPrestamo:d}, Devolución: {FechaDevolucion:d}, Estado: {Estado}";
    }
}