using Editlio.Infrastructure.Context;
using Editlio.Infrastructure.Repositories.Abstracts;

namespace Editlio.Infrastructure.Repositories.Concretes
{
    public class FileRepository : EfRepositoryBase<Shared.Entities.File, int, ApplicationDbContext>, IFileRepository
    {
        public FileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
