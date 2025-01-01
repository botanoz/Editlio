using Editlio.Infrastructure.Context;
using Editlio.Infrastructure.Repositories.Abstracts;
using Editlio.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Infrastructure.Repositories.Concretes
{
    public class PageRepository : EfRepositoryBase<Page, int, ApplicationDbContext>, IPageRepository
    {
        public PageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

}
