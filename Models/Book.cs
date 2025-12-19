using gerenciadorLivraria.Enums;

namespace gerenciadorLivraria.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public Genre Genre { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }


        public Book(
            string title,
            string author,
            Genre genre,
            decimal price,
            int stock)
        {
            if (string.IsNullOrWhiteSpace(title) || title.Length < 2 || title.Length > 120)
                throw new ArgumentException("Título inválido");

            if (string.IsNullOrWhiteSpace(author) || author.Length < 2 || author.Length > 120)
                throw new ArgumentException("Autor inválido");

            if (price < 0)
                throw new ArgumentException("Preço não pode ser negativo");

            if (stock < 0)
                throw new ArgumentException("Estoque não pode ser negativo");

            Id = Guid.NewGuid();
            Title = title;
            Author = author;
            Genre = genre;
            Price = price;
            Stock = stock;
        }

        // Construtor vazio para o EF Core
        protected Book() { }

        public void Update(
            string title,
            string author,
            Genre genre,
            decimal price,
            int stock)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Price = price;
            Stock = stock;
        }
    }
}
