using GestionTurnosUTN.Application.Interfaces;
using GestionTurnosUTN.Application.Dtos;
using GestionTurnosUTN.Domain.Entities;
using GestionTurnosUTN.Domain.Interfaces;

namespace GestionTurnosUTN.Application.Services
{
    public class NewsManagementService : INewsService
    {
        private readonly IRepository _repository;

        public NewsManagementService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateNewsAsync(NewsModel.RequestNewsModel request, Guid workerId)
        {
            var news = new News(request.title, request.description, DateTime.Now, workerId);

            news.Status = NewsStatus.POSTED;

            await _repository.Add(news);
        }

        public async Task UpdateNewsAsync(Guid newsId, NewsModel.RequestNewsModel request)
        {
            var news = await _repository.GetById<News>(newsId)
                ?? throw new Exception($"No se encontró la noticia con Id {newsId}");

            news.Title = request.title;
            news.Description = request.description;

            await _repository.Update(news);
        }

        public async Task DeleteNewsAsync(Guid newsId)
        {
            var news = await _repository.GetById<News>(newsId)
                ?? throw new Exception($"No se encontró la noticia con Id {newsId}");

            news.IsActive = false;

            await _repository.Update(news); // No es delete físico, es cambio de estado
        }

        public async Task<IEnumerable<NewsModel.ResponseNewsModel>> GetAllNewsAsync()
        {
            var newsList = await _repository.GetAll<News>()
                ?? Enumerable.Empty<News>();

            return newsList
                .Where(n => n.IsActive)
                .Select(n => new NewsModel.ResponseNewsModel(n.Title, n.Description, n.DatePost));
        }
}
}
