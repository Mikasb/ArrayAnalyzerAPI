using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DanskeWebApp.Model;
using Microsoft.EntityFrameworkCore;

namespace DanskeWebApp.Database
{
    public class ArrayAnalysisDbContext : DbContext
    {
        public ArrayAnalysisDbContext(DbContextOptions<ArrayAnalysisDbContext> options) : base(options)
        {

        }

        public DbSet<AnalyzedArrays> AnalyzedArrays { get; set; }
    }
}
