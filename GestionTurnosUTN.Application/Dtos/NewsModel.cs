using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTurnosUTN.Application.Dtos
{
    public record NewsModel
    {
        public record RequestNewsModel(string title, string description);
        public record ResponseNewsModel(string title, string description, DateTime datePost);
    }
}
