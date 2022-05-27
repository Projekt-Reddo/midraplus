using BoardService.Models;

namespace BoardService.Data
{
    public interface IBoardRepo : IRepository<Board> { }

    public class BoardRepo : Repository<Board>, IBoardRepo
    {
        public BoardRepo(IMongoContext context) : base(context) { }
    }
}