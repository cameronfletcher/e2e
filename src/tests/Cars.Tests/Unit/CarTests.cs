namespace Cars.Tests.Model
{
    using System;
    using Cars.Model;
    using Xunit;

    // TODO (Cameron): Get car state and use that for assertions.
    public class CarTests
    {
        [Fact]
        public void CanRegisterCar()
        {
            // arrange
            var registration = "abc";

            // act
            var car = new Car(registration);

            // assert
            var memento = car.GetState(); // NOTE (Cameron): State assertions are only possible because of internal scope visibility inside of test library.
            Assert.Equal(registration, memento.Registration);
            Assert.Equal(0, memento.TotalDistanceTravelled);
            Assert.Equal(false, memento.IsDestroyed);
        }

        [Fact]
        public void CanDriveCar()
        {
            // arrange
            var registration = "abc";
            var distance = 10;
            var car = new Car(registration);

            // act
            car.Drive(distance);

            // assert
            var memento = car.GetState();
            Assert.Equal(registration, memento.Registration);
            Assert.Equal(distance, memento.TotalDistanceTravelled);
            Assert.Equal(false, memento.IsDestroyed);
        }

        [Fact]
        public void CanDriveCarFurther()
        {
            // arrange
            var registration = "abc";
            var firstDistance = 10;
            var secondDistance = 15;
            var car = new Car(registration);

            // act
            car.Drive(firstDistance);
            car.Drive(secondDistance);

            // assert
            var memento = car.GetState();
            Assert.Equal(registration, memento.Registration);
            Assert.Equal(firstDistance + secondDistance, memento.TotalDistanceTravelled);
            Assert.Equal(false, memento.IsDestroyed);
        }

        [Fact]
        public void CannotDriveCarNegativeDistance()
        {
            // arrange
            var registration = "abc";
            var car = new Car(registration);

            // act
            Action action = () => car.Drive(-10);

            // assert
            Assert.Throws<BusinessException>(action);
        }

        [Fact]
        public void CanScrapCar()
        {
            // arrange
            var registration = "abc";
            var car = new Car(registration);

            // act
            car.Scrap();

            // assert
            var memento = car.GetState();
            Assert.Equal(registration, memento.Registration);
            Assert.Equal(0, memento.TotalDistanceTravelled);
            Assert.Equal(true, memento.IsDestroyed);
        }

        [Fact]
        public void CannotScrapCarTwice()
        {
            // arrange
            var registration = "abc";
            var car = new Car(registration);


            // act
            car.Scrap();
            Action action = () => car.Scrap();

            // assert
            Assert.Throws<BusinessException>(action);
        }

        [Fact]
        public void CannotDriveScrappedCar()
        {
            // arrange
            var registration = "abc";
            var car = new Car(registration);
            car.Scrap();

            // act
            Action action = () => car.Drive(10);

            // assert
            Assert.Throws<BusinessException>(action);
        }
    }
}
