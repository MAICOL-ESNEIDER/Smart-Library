namespace SmartLibrary.App.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Contacto { get; set; } = null!;
        public bool Activo { get; set; } = true;

        // Constructor vacío
        public Usuario() { }

        // Constructor completo
        public Usuario(int id, string nombre, string contacto)
        {
            Id = id;
            Nombre = nombre;
            Contacto = contacto;
            Activo = true;
        }

        // Métodos
        public string ResumenCorto() => $"{Id} - {Nombre}";
        public string DetalleCompleto() => $"[{Id}] {Nombre}, Contacto: {Contacto}, Activo: {Activo}";

        // Override
        public override string ToString() => ResumenCorto();
    }
}
