using System.ComponentModel.DataAnnotations;

namespace ArenaApi
{
    public class FighterDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Questa proprietà è richiesta.")]
        [Range(1, 200)]
        public int Pv { get; set; }

        [Required(ErrorMessage = "Questa proprietà è richiesta.")]
        [Range(1, 100)]
        public int Speed { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required(ErrorMessage = "Questa proprietà è richiesta.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Questa proprietà è richiesta.")]
        [Range(1, 100)]
        public int Power { get; set; }

        [Required(ErrorMessage = "Questa proprietà è richiesta.")]
        public string Type { get; set; }
    }
}