using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите ваше имя")]
        [Display(Name = "Имя")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите текст комментария")]
        [Display(Name = "Текст")]
        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int PostId { get; set; }

        // НОВОЕ: счетчик лайков
        public int Likes { get; set; } = 0;
    }
}