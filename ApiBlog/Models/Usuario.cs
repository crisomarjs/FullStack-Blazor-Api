﻿using System.ComponentModel.DataAnnotations;

namespace ApiBlog.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NombreUsuario { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
