using System.ComponentModel.DataAnnotations;

namespace MudGame.Models
{
    public class CharacterViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string Class { get; set; }

        [Range(1, 20)]
        public int Strength { get; set; } = 10;

        [Range(1, 20)]
        public int Intelligence { get; set; } = 10;

        [Range(1, 20)]
        public int Dexterity { get; set; } = 10;
    }
}