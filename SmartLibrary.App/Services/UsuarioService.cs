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
    }
}
