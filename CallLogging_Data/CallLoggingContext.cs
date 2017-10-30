using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallLogging_Data
{
    public class CallLoggingContext : DbContext
    {
        public DbSet<MessageCompany> MessageCompanies { get; set; }
        public DbSet<MessageRecord> MessageRecords { get; set; }
    }
}
