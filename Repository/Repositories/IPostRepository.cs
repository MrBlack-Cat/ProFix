using Domain.Entities;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories;

public interface IPostRepository : IRepository<Post>
{
    Task<IEnumerable<Post>> GetByServiceProviderIdAsync(int serviceProviderProfileId);
    //Task DeleteAsync(int id, string deletedReason, int deletedBy);
}
