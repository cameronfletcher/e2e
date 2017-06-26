namespace Cars.Model
{
    using static System.FormattableString;

    /// <summary>
    /// Represents a car.
    /// </summary>
    public partial class Car
    {
        private int totalDistanceTravelled;
        private bool isDestroyed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Car"/> class.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <exception cref="BusinessException">
        /// Thrown if an attempt is made to instantate a car with a registration that exceeds more than 50 characters in length.
        /// </exception>
        public Car(string registration)
        {
            Guard.Against.Null(() => registration);

            if (registration.Length == 0)
            {
                throw new BusinessException("A car registration cannot be blank.");
            }

            if (registration.Length > 50)
            {
                throw new BusinessException("A car registration cannot exceed more than 50 characters in length.");
            }

            this.Registration = registration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Car"/> class.
        /// </summary>
        //// NOTE (Cameron): Designed for use by the repository to instantiate an uninitialized instance of a car -or- by any future implementation that inherits from a type
        //// of Car in order for that implementation to provide the same functionality to the repository.
        protected internal Car()
        {
        }

        /// <summary>
        /// Gets the car registration.
        /// </summary>
        /// <value>The car registration.</value>
        public string Registration { get; private set; }

        /// <summary>
        /// Marks the car as having been driven the specified distance.
        /// </summary>
        /// <param name="distance">The distance.</param>
        /// <exception cref="BusinessException">
        /// Thrown if the lifecycle of the car has ended
        /// - or -
        /// if an attempt is made to mark the car as having been driven a negative distance.
        /// </exception>
        public void Drive(int distance)
        {
            this.ThrowIfLifecycleEnded();

            if (distance < 0)
            {
                throw new BusinessException("A car cannot be driven a negative distance.");
            }

            this.totalDistanceTravelled += distance;
        }

        /// <summary>
        /// Marks the car as having been scrapped.
        /// </summary>
        /// <exception cref="BusinessException">Thrown if the lifecycle of the car has ended.</exception>
        public void Scrap()
        {
            this.ThrowIfLifecycleEnded();

            this.isDestroyed = true;
        }

        protected void ThrowIfLifecycleEnded()
        {
            if (this.isDestroyed)
            {
                throw new BusinessException(Invariant($"Cannot apply changes to '{this.Registration}' because that '{this.GetType().Name}' no longer exists in the system."));
            }
        }
    }
}
