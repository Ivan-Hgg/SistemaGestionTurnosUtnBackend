using GestionTurnosUTN.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Interfaces
{
    public interface INewsService
    {
        Task CreateNewsAsync(NewsModel.RequestNewsModel request, Guid workerId);
        Task UpdateNewsAsync(Guid newsId, NewsModel.RequestNewsModel request);  
        Task DeleteNewsAsync(Guid newsId);                                       
        Task<IEnumerable<NewsModel.ResponseNewsModel>> GetAllNewsAsync();
    }
}
