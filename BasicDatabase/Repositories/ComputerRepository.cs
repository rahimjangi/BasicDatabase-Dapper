using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BasicDatabase.Repositories;

public class ComputerRepository : IComputerRepository
{
    private readonly string _connectionString;
    private readonly ILogger<ComputerRepository> _logger;

    public ComputerRepository(string connectionString, ILogger<ComputerRepository> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    private void LogError(Exception ex, string procedure, string identifier)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@ErrorMessage", ex.Message);
            parameters.Add("@ErrorProcedure", procedure);
            parameters.Add("@ErrorLine", 0); // Line number is not available in .NET exception
            parameters.Add("@Identifier", identifier);

            connection.Execute("INSERT INTO [LearnDB].[ErrorLogs] (ErrorMessage, ErrorProcedure, ErrorLine, Identifier) VALUES (@ErrorMessage, @ErrorProcedure, @ErrorLine, @Identifier)", parameters);
        }
    }

    public int CreateComputer(Computer computer, string identifier)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Motherboard", computer.Motherboard);
                parameters.Add("@CPUcores", computer.CPUcores);
                parameters.Add("@HasWiFi", computer.HasWiFi);
                parameters.Add("@HasLTE", computer.HasLTE);
                parameters.Add("@ReleaseDate", computer.ReleaseDate);
                parameters.Add("@Price", computer.Price);
                parameters.Add("@VideoCard", computer.VideoCard);
                parameters.Add("@Identifier", identifier);

                return connection.ExecuteScalar<int>("[LearnDB].[CreateComputer]", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating computer");
            LogError(ex, "CreateComputer", identifier);
            throw;
        }
    }

    public Computer GetComputerById(int computerId, string identifier)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { ComputerId = computerId, Identifier = identifier };
                return connection.QuerySingleOrDefault<Computer>("[LearnDB].[GetComputerById]", parameters, commandType: CommandType.StoredProcedure);
                //return connection.QuerySingleOrDefault<Computer>("INVALID SQL COMMAND", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting computer by ID");
            LogError(ex, "GetComputerById", identifier);
            throw;
        }
    }

    public void UpdateComputer(Computer computer, string identifier)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@ComputerId", computer.ComputerId);
                parameters.Add("@Motherboard", computer.Motherboard);
                parameters.Add("@CPUcores", computer.CPUcores);
                parameters.Add("@HasWiFi", computer.HasWiFi);
                parameters.Add("@HasLTE", computer.HasLTE);
                parameters.Add("@ReleaseDate", computer.ReleaseDate);
                parameters.Add("@Price", computer.Price);
                parameters.Add("@VideoCard", computer.VideoCard);
                parameters.Add("@Identifier", identifier);

                connection.Execute("[LearnDB].[UpdateComputer]", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating computer");
            LogError(ex, "UpdateComputer", identifier);
            throw;
        }
    }

    public void DeleteComputer(int computerId, string identifier)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new { ComputerId = computerId, Identifier = identifier };
                connection.Execute("[LearnDB].[DeleteComputer]", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting computer");
            LogError(ex, "DeleteComputer", identifier);
            throw;
        }
    }
}
