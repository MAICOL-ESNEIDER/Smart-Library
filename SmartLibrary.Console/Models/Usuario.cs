namespace SmartLibrary.Console.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contacto { get; set; }
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
    }
}