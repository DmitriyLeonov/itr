using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace itr.Models
{
    public class ArticleRating
    {
        public int Id { get; set; }
        public int articleId { get; set; }

        public string UserName { get; set; }
        public double Rating { get; set; }
    }
}
