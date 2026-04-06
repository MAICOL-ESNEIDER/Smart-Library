namespace SmartLibrary.App.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public int Anio { get; set; }
        public string Categoria { get; set; } = null!;
        public bool Disponible { get; set; } = true;

        // Constructor vacío
        public Libro() { }

        // Constructor completo
        public Libro(int id, string titulo, string autor, int anio, string categoria)
        {
            Id = id;
            Titulo = titulo;
            Autor = autor;
            Anio = anio;
            Categoria = categoria;
            Disponible = true;
        }

        // Métodos
        public string ResumenCorto() => $"{Id} - {Titulo} ({Autor})";
        public string DetalleCompleto() => $"[{Id}] {Titulo}, {Autor}, {Anio}, {Categoria}, Disponible: {Disponible}";

        // Override
        public override string ToString() => ResumenCorto();
    }
}
