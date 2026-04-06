using System;
using System.Collections.Generic;
using System.Linq;
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

        // ORDENACIÓN (EV08)
        public List<Usuario> OrdenarPorNombre()
        {
            return usuarios.OrderBy(u => u.Nombre).ToList();
        }

        // KPIs (EV08 - OBLIGATORIO)
        public int TotalUsuarios()
        {
            return usuarios.Count;
        }

        public int UsuariosActivos()
        {
            return usuarios.Count(u => u.Activo);
        }

        public int UsuariosInactivos()
        {
            return usuarios.Count(u => !u.Activo);
        }
    }
}
