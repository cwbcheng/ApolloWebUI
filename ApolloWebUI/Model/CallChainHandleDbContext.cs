using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Model
{
    public class CallChainHandleDbContext : DbContext
    {
        public CallChainHandleDbContext()
        {

        }

        public CallChainHandleDbContext(DbContextOptions<CallChainHandleDbContext> options) : base(options)
        {

        }

        public DbSet<CallChainHandleModel> CallChainHandles { get; set; }
    }
}