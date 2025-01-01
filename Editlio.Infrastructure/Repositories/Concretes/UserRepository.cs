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
    public class UserRepository : EfRepositoryBase<User, int, ApplicationDbContext>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
