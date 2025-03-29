﻿using System.ComponentModel.DataAnnotations;

namespace ApiBlog.Models.DTO
{
    public class PostCrearDTO
    {
        [Required(ErrorMessage = "El Titulo es Obligatorio")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "La Descripcion es Obligatoria")]
        public string Descripcion { get; set; }
        public string RutaImagen { get; set; }
        [Required(ErrorMessage = "Las Etiquetas son Obligatorias")]
        public string Etiquetas { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
