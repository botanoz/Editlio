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
    public class RefreshTokenRepository : EfRepositoryBase<RefreshToken, int, ApplicationDbContext>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
