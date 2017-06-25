namespace Cars.Persistence
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Cars.Model;

    public interface ICarRepository
    {
        Task Save(Car car);

        Task<Car> Load(string registration);

        // NOTE (Cameron): Read-model related members follow...
        Task<(string registration, int totalDistanceTravelled)?> GetCar(string registration);

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Under consideration: https://github.com/dotnet/csharplang/issues/43")]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not appropriate.")]
        Task<List<(string registration, int totalDistanceTravelled)>> GetCars();
    }
}
