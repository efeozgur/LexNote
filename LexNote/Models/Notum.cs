using System.ComponentModel.DataAnnotations;

namespace LexNote.Models
{
    public class Notum
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Baslik { get; set; }

        [Required]
        public string Icerik { get; set; }

        public string? Etiketler { get; set; }

        public int? KategoriId { get; set; }
        public Kategori? Kategori { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
    }
}