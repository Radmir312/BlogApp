using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите заголовок")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите текст публикации")]
        [Display(Name = "Содержимое")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Дата публикации")]
        public DateTime PublishedAt { get; set; } = DateTime.Now;

        // НОВОЕ: счетчик лайков
        public int Likes { get; set; } = 0;

        public List<Comment> Comments { get; set; } = new();
    }
}