# 🗂️ LexNote — Kurumsal Not Uygulaması

LexNote, ASP.NET Core MVC ile geliştirilmiş, kurum içi not alma ve içerik düzenleme sistemidir.  
Kategorili, etiket destekli, zengin içerikli notları kolayca arayabilir, düzenleyebilir ve modal üzerinden detaylarını görüntüleyebilirsiniz.

---

## 🚀 Özellikler

- ✅ Not ekleme / silme / düzenleme (CRUD)
- ✅ Kategori desteği
- ✅ Etiketleme ve etiket filtreleme
- ✅ Gelişmiş içerik düzenleme (Markdown destekli)
- ✅ Modal popup ile hızlı önizleme
- ✅ Arama (başlık + içerik + etiket içinde)

---

## 🧱 Kullanılan Teknolojiler

- ASP.NET Core MVC
- Entity Framework Core
- Bootstrap 5
- Marked.js (Markdown to HTML converter)

---

## 📁 Notum Modeli

```csharp
public class Notum
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Baslik { get; set; }
    [Required]
    public string Icerik { get; set; } // Markdown içeriği
    public string? Etiketler { get; set; } // Virgülle ayrılmış etiket listesi
    public int? KategoriId { get; set; }
    public Kategori? Kategori { get; set; }
    public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
}
```

---

## 🔍 Arama ve Etiket Filtreleme (NotumController.cs)

```csharp
public IActionResult Index(string? q, string? etiket)
{
    var notlar = _context.Notlar.Include(x => x.Kategori).ToList();

    if (!string.IsNullOrEmpty(q))
    {
        q = q.ToLower();
        notlar = notlar
            .Where(n => (n.Baslik?.ToLower().Contains(q) ?? false) ||
                        (n.Icerik?.ToLower().Contains(q) ?? false) ||
                        (n.Etiketler?.ToLower().Contains(q) ?? false))
            .ToList();
    }

    if (!string.IsNullOrEmpty(etiket))
    {
        notlar = notlar
            .Where(n => n.Etiketler != null &&
                        n.Etiketler.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(e => e.Trim().ToLower())
                                   .Contains(etiket.ToLower()))
            .ToList();
    }

    ViewBag.Arama = q;
    ViewBag.Etiket = etiket;

    return View(notlar);
}
```

---

## 🖥️ Notların Listelendiği Sayfa (Index.cshtml)

- Tabloda her not için `modal` tetikleyici link
- Modal içinde Markdown içeriği render edilir

### 📌 Modal İçeriği (Markdown Dönüşümü)

```html
<script src="https://cdn.jsdelivr.net/npm/marked/marked.min.js"></script>
```

```html
<!-- Modal içi -->
<div id="icerik@not.Id" class="markdown-content" data-md="@not.Icerik.Replace(""", "&quot;")"></div>
```

```html
<script>
    document.addEventListener("DOMContentLoaded", function () {
        const mdBlocks = document.querySelectorAll(".markdown-content");
        mdBlocks.forEach(div => {
            const raw = div.dataset.md.replaceAll("\n", "\n");
            const html = marked.parse(raw);
            div.innerHTML = html;
        });
    });
</script>
```

---

## 🧪 İçerik Format Örneği (Markdown)

```markdown
# Başlık

**Kalın yazı**  
*İtalik yazı*

- Madde 1
- Madde 2
```

---

## 📦 Kurulum & Kullanım

1. Projeyi klonla veya indir.
2. Veritabanı bağlantısını `appsettings.json` içinde ayarla.
3. Terminalden şu komutları çalıştır:

```bash
dotnet ef database update
dotnet run
```

4. Tarayıcıdan `https://localhost:5001` ile aç.

---

## 📌 Notlar

- `Etiketler` alanı virgülle ayrılır: `hukuk, mahkeme, tebligat`
- Markdown içeriği HTML olarak işlenir, XSS riski yoktur
- Bootstrap modal `id="modal@(not.Id)"` şeklinde tanımlanmalı

---

## ✨ Planlanan Geliştirmeler

- [ ] Markdown editörü (SimpleMDE, EasyMDE) ile içerik girme
- [ ] Kategoriye göre filtreleme
- [ ] Favori notlar
- [ ] PDF çıktısı alma

---

## 👨‍💻 Geliştiren

Efe Özgür — [LexNote Projesi](https://github.com/)
