using BlogApp.Models;
using BlogApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly BlogRepository _repository;

        public PostsController(BlogRepository repository)
        {
            _repository = repository;
        }

        // GET: /Posts?search=...&page=1
        public IActionResult Index(string? search, int page = 1)
        {
            var result = _repository.GetPosts(search, page, pageSize: 5);
            return View(result);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post post)
        {
            if (!ModelState.IsValid) return View(post);
            BlogRepository.AddPost(post);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var post = _repository.GetPostById(id);
            if (post == null) return NotFound();
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment(int postId, Comment newComment)
        {
            var post = _repository.GetPostById(postId);
            if (post == null) return NotFound();

            if (!ModelState.IsValid) return View("Details", post);

            newComment.PostId = postId;
            BlogRepository.AddComment(newComment);
            return RedirectToAction(nameof(Details), new { id = postId });
        }

        // === AJAX: ЛАЙК ПОСТА ===
        [HttpPost]
        public IActionResult LikePost([FromBody] int id)
        {
            var success = _repository.LikePost(id);
            if (!success) return NotFound();

            var post = _repository.GetPostById(id);
            return Json(new { likes = post!.Likes });
        }

        // === AJAX: ЛАЙК КОММЕНТАРИЯ ===
        [HttpPost]
        public IActionResult LikeComment([FromBody] int id)
        {
            var success = _repository.LikeComment(id);
            if (!success) return NotFound();

            // Находим комментарий, чтобы вернуть его лайки
            var comment = _repository.GetPostById(1)?.Comments.FirstOrDefault(); // временно
            // Проще: находим через отдельный метод
            var allPosts = _repository.GetPosts(null, 1, 1000).Items;
            var foundComment = allPosts
                .SelectMany(p => _repository.GetPostById(p.Id)?.Comments ?? new List<Comment>())
                .FirstOrDefault(c => c.Id == id);

            return Json(new { likes = foundComment?.Likes ?? 0 });
        }
    }
}