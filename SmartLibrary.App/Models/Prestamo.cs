using System;

namespace SmartLibrary.App.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public Libro Libro { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
        public DateTime FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public EstadoPrestamo Estado { get; set; } = EstadoPrestamo.Activo;

        // Constructor vacío
        public Prestamo() { }

        // Constructor completo
        public Prestamo(int id, Libro libro, Usuario usuario, DateTime fechaPrestamo)
        {
            Id = id;
            Libro = libro;
            Usuario = usuario;
            FechaPrestamo = fechaPrestamo;
            FechaDevolucion = null; // condición de la guía
            Estado = EstadoPrestamo.Activo;
        }

        // Métodos de validación
        public bool EstaVencido()
        {
            return Estado == EstadoPrestamo.Activo 
                   && FechaDevolucion.HasValue 
                   && FechaDevolucion.Value < DateTime.Now;
        }

        public int DiasTranscurridos()
        {
            return (DateTime.Now - FechaPrestamo).Days;
        }

        // Métodos de presentación
        public string ResumenCorto() => $"{Id} - {Libro?.Titulo} ({Usuario?.Nombre})";

        public string DetalleCompleto() =>
            $"[{Id}] Libro: {Libro?.Titulo}, Usuario: {Usuario?.Nombre}, Prestado: {FechaPrestamo:d}, " +
            $"Devolución: {(FechaDevolucion.HasValue ? FechaDevolucion.Value.ToString("d") : "No definida")}, Estado: {Estado}";

        // Override
        public override string ToString() => ResumenCorto();
    }
}