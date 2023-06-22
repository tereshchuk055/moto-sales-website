using MotoShop.Data;
using Microsoft.Data.SqlClient;

namespace MotoShop.Repository
{
    public class Repository
    {
        protected readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }
    }
}
