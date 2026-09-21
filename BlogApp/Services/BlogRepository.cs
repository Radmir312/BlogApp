using BlogApp.Models;

namespace BlogApp.Services
{
    public class BlogRepository
    {
        private static readonly List<Post> _posts = new();
        private static readonly List<Comment> _comments = new();
        private static int _nextPostId = 1;
        private static int _nextCommentId = 1;

        static BlogRepository()
        {
            // === ЗАПОЛНЯЕМ ПОСТАМИ ПРО DOTU 2 ===
            AddPost(new Post
            {
                Title = "Патч 7.35: что изменилось в мете?",
                Content = "Вышел долгожданный патч 7.35. Ослаблены некоторые популярные герои, " +
                          "например, Muerta и Ringmaster. Усилены предметы для саппортов, " +
                          "а также переработана система нейтральных предметов. " +
                          "Ожидается, что мета сместится в сторону поздней игры.",
                PublishedAt = new DateTime(2026, 9, 15)
            });

            AddPost(new Post
            {
                Title = "Гайд на Pudge: как попадать хуки",
                Content = "Pudge — один из самых популярных героев в Dota 2. " +
                          "Секрет хорошего хука — предсказание движения врага. " +
                          "Используйте Blink Dagger для внезапных инициаций, " +
                          "а также комбинируйте Hook с Rot и Dismember. " +
                          "Не забывайте про Aghanim's Scepter — он даёт дополнительный хук.",
                PublishedAt = new DateTime(2026, 9, 16)
            });

            AddPost(new Post
            {
                Title = "The International 2026: итоги группового этапа",
                Content = "Групповой этап TI 2026 завершён. Team Spirit и Gaimin Gladiators " +
                          "показали доминирующую игру и вышли в верхнюю сетку. " +
                          "Китайские команды, к сожалению, выступили слабее ожидаемого. " +
                          "Плей-офф обещает быть жарким!",
                PublishedAt = new DateTime(2026, 9, 17)
            });

            AddPost(new Post
            {
                Title = "Лучшие герои для игры в мид в текущей мете",
                Content = "В текущей мете на миде доминируют: Invoker, Puck, Storm Spirit " +
                          "и Ember Spirit. Эти герои обладают высоким потенциалом " +
                          "для гангов и могут быстро набирать силу. " +
                          "Не забывайте про Zeus — он отлично работает против " +
                          "мобильных героев благодаря своему ультимейту.",
                PublishedAt = new DateTime(2026, 9, 18)
            });

            AddPost(new Post
            {
                Title = "Как правильно ставить варды: советы саппортам",
                Content = "Варды — это глаза вашей команды. Правильно поставленный вард " +
                          "может предотвратить ганг и спасти игру. " +
                          "Ставьте агрессивные варды у вражеского леса, " +
                          "чтобы контролировать передвижения врага. " +
                          "Не забывайте про Sentry — они помогут обнаружить невидимок.",
                PublishedAt = new DateTime(2026, 9, 19)
            });

            AddPost(new Post
            {
                Title = "Обзор нового героя: Ringmaster",
                Content = "Ringmaster — новый герой поддержки, добавленный в патче 7.34. " +
                          "Его способности позволяют контролировать зону и наносить " +
                          "урон по площади. Ультимейт создаёт иллюзию, " +
                          "которая пугает врагов и заставляет их отступать. " +
                          "Ringmaster отлично подходит для агрессивного стиля игры.",
                PublishedAt = new DateTime(2026, 9, 20)
            });

            AddPost(new Post
            {
                Title = "Топ-5 ошибок новичков в Dota 2",
                Content = "1. Игра без вардов — вы слепы. " +
                          "2. Фарм в одиночку без карты — вас убьют. " +
                          "3. Игнорирование рун — потеря бонусов. " +
                          "4. Покупка неправильных предметов — читайте гайды. " +
                          "5. Токсичность — она мешает команде побеждать.",
                PublishedAt = new DateTime(2026, 9, 21)
            });

            // === КОММЕНТАРИИ ===
            AddComment(new Comment { PostId = 1, Author = "DotaFan", Text = "Патч реально изменил всё, спасибо за обзор!" });
            AddComment(new Comment { PostId = 1, Author = "ProPlayer", Text = "Ждём, как про-игроки адаптируются." });
            AddComment(new Comment { PostId = 2, Author = "PudgeLover", Text = "Хуки — это искусство!" });
            AddComment(new Comment { PostId = 3, Author = "TI_Fan", Text = "Team Spirit снова лучшие!" });
        }

        // --- ПОСТЫ ---
        public PagedResult<Post> GetPosts(string? search, int page, int pageSize = 5)
        {
            var query = _posts.AsQueryable();

            // Поиск по заголовку или тексту
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(p =>
                    p.Title.ToLower().Contains(search) ||
                    p.Content.ToLower().Contains(search));
            }

            var ordered = query.OrderByDescending(p => p.PublishedAt).ToList();
            var totalItems = ordered.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var items = ordered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<Post>
            {
                Items = items,
                PageNumber = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                SearchQuery = search
            };
        }

        public Post? GetPostById(int id)
        {
            var post = _posts.FirstOrDefault(p => p.Id == id);
            if (post != null)
            {
                post.Comments = _comments
                    .Where(c => c.PostId == id)
                    .OrderBy(c => c.CreatedAt)
                    .ToList();
            }
            return post;
        }

        public static void AddPost(Post post)
        {
            post.Id = _nextPostId++;
            if (post.PublishedAt == default)
                post.PublishedAt = DateTime.Now;
            _posts.Add(post);
        }

        // --- КОММЕНТАРИИ ---
        public static void AddComment(Comment comment)
        {
            comment.Id = _nextCommentId++;
            comment.CreatedAt = DateTime.Now;
            _comments.Add(comment);
        }

        // --- ЛАЙКИ ---
        public bool LikePost(int id)
        {
            var post = _posts.FirstOrDefault(p => p.Id == id);
            if (post == null) return false;
            post.Likes++;
            return true;
        }

        public bool LikeComment(int id)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == id);
            if (comment == null) return false;
            comment.Likes++;
            return true;
        }
    }
}