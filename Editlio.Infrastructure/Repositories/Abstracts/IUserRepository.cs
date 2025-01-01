using Editlio.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Infrastructure.Repositories.Abstracts
{
    public interface IUserRepository : IAsyncRepository<User, int>
    {
    }
}
