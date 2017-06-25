namespace Cars.Persistence.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Cars.Model;

    public sealed class CarRepository : ICarRepository
    {
        private const string DatabaseName = "Cars";

        private readonly string connectionString;
        private readonly string schema;

        public CarRepository(string connectionString)
            : this(connectionString, "dbo")
        {
        }

        public CarRepository(string connectionString, string schema)
        {
            Guard.Against.Null(() => connectionString);
            Guard.Against.Null(() => schema);

            this.connectionString = connectionString;
            this.schema = schema;
        }

        public async Task Save(Car car)
        {
            Guard.Against.Null(() => car);

            var memento = car.GetState();

            using (var connection = this.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = $"{this.schema}.SaveCar";
                command.Parameters.Add("@Registration", SqlDbType.VarChar, 50).Value = memento.Registration;
                command.Parameters.Add("@TotalDistanceTravelled", SqlDbType.Int).Value = memento.TotalDistanceTravelled;
                command.Parameters.Add("@IsDestroyed", SqlDbType.Bit).Value = memento.IsDestroyed;
                command.Parameters.Add("@__State", SqlDbType.VarChar, 8).Value = (object)memento.State ?? DBNull.Value;
                command.Parameters.Add("@__StateOut", SqlDbType.VarChar, 8).Direction = ParameterDirection.Output;

                await connection.OpenAsync().ConfigureAwait(false);

                try
                {
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                catch (SqlException ex) when (ex.Errors.Cast<SqlError>().Any(sqlError => sqlError.Number == 50409))
                {
                    if (memento.State == null)
                    {
                        throw new ConcurrencyException("Car already exists.", ex);
                    }
                    else
                    {
                        throw new ConcurrencyException(ex);
                    }
                }

                memento.State = Convert.ToString(command.Parameters["@__StateOut"].Value);

                // HACK (Cameron): This is a round-about way to set the current 'state' value (post-persistence) of the in-memory car and should be changed... somehow.
                car.SetState(memento);
            }
        }

        public async Task<Car> Load(string registration)
        {
            Guard.Against.Null(() => registration);

            using (var connection = this.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = $"{this.schema}.LoadCar";
                command.Parameters.Add("@Registration", SqlDbType.VarChar, 50).Value = registration;

                await connection.OpenAsync().ConfigureAwait(false);

                using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (!await reader.ReadAsync().ConfigureAwait(false))
                    {
                        throw new AggregateRootNotFoundException($"Cannot find any car with registration '{registration}'.");
                    }

                    var car = new Car();

                    car.SetState(
                        new Car.Memento
                        {
                            Registration = reader.GetString(0),
                            TotalDistanceTravelled = reader.GetInt32(1),
                            State = reader.GetString(2),
                        });

                    return car;
                }
            }
        }

        public async Task<List<(string registration, int totalDistanceTravelled)>> GetCars()
        {
            using (var connection = this.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = $"{this.schema}.GetCars";

                await connection.OpenAsync().ConfigureAwait(false);

                using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    var cars = new List<(string, int)>();

                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        cars.Add((reader.GetString(0), reader.GetInt32(1)));
                    }

                    return cars;
                }
            }
        }

        public async Task<(string registration, int totalDistanceTravelled)?> GetCar(string registration)
        {
            using (var connection = this.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = $"{this.schema}.GetCar";
                command.Parameters.Add("@Registration", SqlDbType.VarChar, 50).Value = registration;

                await connection.OpenAsync().ConfigureAwait(false);

                using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    return (!await reader.ReadAsync().ConfigureAwait(false))
                        ? ((string, int)?)null
                        : (reader.GetString(0), reader.GetInt32(1));
                }
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed by caller.")]
        private SqlConnection GetConnection() => new SqlConnection(this.connectionString).WithInitializedSchema(this.schema, DatabaseName);
    }
}
