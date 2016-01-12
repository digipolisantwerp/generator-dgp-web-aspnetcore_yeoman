using System.ComponentModel.DataAnnotations;

namespace StarterKit.Entities
{
    public class CodeTabelBase : EntityBase
    {
        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(100)]
        public string Omschrijving { get; set; }
    }
}