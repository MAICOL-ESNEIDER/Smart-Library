using System;
using System.Collections.Generic;
using SmartLibrary.App.Models;

namespace SmartLibrary.App.Services
{
    public class UsuarioService
    {
        private readonly List<Usuario> usuarios = new List<Usuario>();

        public void AgregarUsuario(Usuario usuario)
        {
            usuarios.Add(usuario);
        }

        public void EliminarUsuario(Usuario usuario)
        {
            usuarios.Remove(usuario);
        }

        public List<Usuario> ObtenerTodos()
        {
            return usuarios;
        }

        // BÚSQUEDAS (EV08)
        // "Documento/ID" en tu modelo equivale a Id (int)
        public Usuario? BuscarPorId(int id)
        {
            return usuarios.Find(u => u.Id == id);
        }

        public List<Usuario> BuscarPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return new List<Usuario>();

            return usuarios.FindAll(u =>
                u.Nombre != null &&
                u.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
