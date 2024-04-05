using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDBCar
{
    public class CarDbContext :DbContext
    {
        public CarDbContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<Car> Cars { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"data source=(localdb)\MSSQLLocalDB;
                                    initial catalog=CarDB;
                                    integrated security=True;
                                    Connect Timeout = 2;
                                    Encrypt = False;
                                    Trust Server Certificate = False;
                                    Application Intent = ReadWrite;
                                    Multi Subnet Failover = False");
        }
    }

    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        public DateTime ModelYear { get; set; }

        public override string ToString()
        {
            return $"{Make,-20} |  {Model,-20}  | {ModelYear.ToShortDateString(),-5}";
        }
    }
}
