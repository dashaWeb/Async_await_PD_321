using Microsoft.EntityFrameworkCore;

namespace DataCar
{
    public class CarQueries
    {
        private CarDbContext ctx = new CarDbContext();
        private Random rand = new Random();
        public Task<IEnumerable<Car>> GetAllCars()
        {
            return Task.Run(() =>
            {
                Thread.Sleep(2000);
                return (IEnumerable<Car>)ctx.Cars.ToList();
            });
        }
        public async Task<IEnumerable<Car>> GetFilteredCars()
        {
            Thread.Sleep(2000);
            IQueryable<Car> query = ctx.Cars.Where(c => c.Make.StartsWith("A")).OrderBy(c => c.ModelYear);
            return await query.ToListAsync();
        }

        public async Task<Car> GetRandomCar()
        {
            var cars = await ctx.Cars.ToListAsync();
            Thread.Sleep(2000);
            return cars[rand.Next(cars.Count)];
        }
        public async Task CreateRandomData(int count)
        {
            List<Car> cars = new List<Car>();

            for (int i = 0; i < count; i++)
            {
                var car = await GetRandomCar();
                car.Id = 0;
                /*var car2 = new Car()
                {
                    Make = car.Make,
                    ModelYear = car.ModelYear,
                    Model = car.Model,
                };*/
                cars.Add(car);
            }
            ctx.Cars.AddRange(cars);

            await ctx.SaveChangesAsync();
        }
        public async Task<int> GetCarsCount()
        {
            return await ctx.Cars.CountAsync();
        }

    }
}
