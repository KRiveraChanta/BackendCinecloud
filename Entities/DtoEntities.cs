using System;
using System.Collections.Generic; // Para List<T>
using System.ComponentModel.DataAnnotations; // <-- ¡ESTE ES CLAVE!
using BackendCine.Entities.Models;

namespace BackendCine.Entities
{
    public class LoginParam
    {
        public string NomUsuario { get; set; }
        public string Password { get; set; }
    }

    public class PeliculaXCartelera
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public int Duracion { get; set; }
        public string Img { get; set; }
        public List<GeneroXVideo> Generos { get; set; }

    }

    public class PeliculaXSala
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public int Duracion { get; set; }
        public string Img { get; set; }
        public List<Genero> Generos { get; set; }
    }

    public class ListarPeliculasParam
    {
        public string? titulo { get; set; }
        public int? idGenero { get; set; }
    }

    public class ClienteRegistroDTO
    {
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public string? Genero { get; set; }

    }

    // DTO principal para el registro completo
    public class RegistroUsuarioDTO
    {
        [Required]
        public string NomUsuario { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int IdRol { get; set; } // Asume que el Rol de "Cliente" será un valor fijo (ej. 2)

        [Required]
        public ClienteRegistroDTO Cliente { get; set; }
    }

}
