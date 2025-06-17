namespace LexNote.Models
{
    public class Kategori
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public List<Notum>? Notum { get; set; }
    }
}