namespace Cars.Tests.Integration
{
    using System.Threading.Tasks;
    using Cars.Model;
    using Cars.Persistence;
    using Cars.Persistence.SqlServer;
    using Cars.Tests.Sdk;
    using FluentAssertions;
    using Xunit;

    public class CarRepositoryTests : Integration.Database
    {
        public CarRepositoryTests(SqlServerFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task CanPersistAndReconstituteCar()
        {
            // arrange
            var repository = new CarRepository(this.ConnectionString, "dbo");

            // act
            var car = new Car("test01");
            await repository.Save(car);
            var sameCar = await repository.Load(car.Registration);

            // assert
            car.GetState().ShouldBeEquivalentTo(sameCar.GetState());
        }

        [Fact]
        public async Task CanPersistScrappedCar()
        {
            // arrange
            var repository = new CarRepository(this.ConnectionString, "dbo");

            // act
            var car = new Car("test02");
            car.Drive(10);
            car.Scrap();
            await repository.Save(car);

            // assert
            await Assert.ThrowsAsync<AggregateRootNotFoundException>(async () => await repository.Load(car.Registration));
        }

        [Fact]
        public async Task CanPersistMoreThenOnceAndReconstituteCar()
        {
            // arrange
            var repository = new CarRepository(this.ConnectionString, "dbo");

            // act
            var car = new Car("test03");
            car.Drive(10);
            await repository.Save(car);
            car.Drive(5);
            await repository.Save(car);
            var sameCar = await repository.Load(car.Registration);

            // assert
            car.GetState().ShouldBeEquivalentTo(sameCar.GetState());
        }

        [Fact]
        public async Task CanPersistMoreThanOnceAndScrapCar()
        {
            // arrange
            var repository = new CarRepository(this.ConnectionString, "dbo");

            // act
            var car = new Car("test04");
            car.Drive(10);
            await repository.Save(car);
            car.Scrap();
            await repository.Save(car);

            // assert
            await Assert.ThrowsAsync<AggregateRootNotFoundException>(async () => await repository.Load(car.Registration));
        }

        [Fact]
        public async Task CannotPersistCarWithInvalidState()
        {
            // arrange
            var repository = new CarRepository(this.ConnectionString, "dbo");

            // act
            var originalCar = new Car("test05");
            originalCar.Drive(10);
            await repository.Save(originalCar);
            var car = await repository.Load(originalCar.Registration);
            car.Drive(5);
            await repository.Save(car);

            // assert
            await Assert.ThrowsAsync<ConcurrencyException>(async () => await repository.Save(originalCar));
        }

        [Fact]
        public async Task CannotPersistCarThatHasBeenScrappedWithInvalidState()
        {
            // arrange
            var repository = new CarRepository(this.ConnectionString, "dbo");

            // act
            var originalCar = new Car("test06");
            originalCar.Drive(10);
            await repository.Save(originalCar);
            var car = await repository.Load(originalCar.Registration);
            car.Scrap();
            await repository.Save(car);

            // assert
            await Assert.ThrowsAsync<ConcurrencyException>(async () => await repository.Save(originalCar));
        }

        [Fact]
        public async Task CannotPersistCarThatAlreadyExists()
        {
            // arrange
            var repository = new CarRepository(this.ConnectionString, "dbo");

            // act
            var car = new Car("test07");
            var sameCar = new Car("test07");
            await repository.Save(car);

            // assert
            await Assert.ThrowsAsync<ConcurrencyException>(async () => await repository.Save(sameCar));
        }
    }
}
