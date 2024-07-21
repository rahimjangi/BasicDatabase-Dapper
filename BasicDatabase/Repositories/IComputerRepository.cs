namespace BasicDatabase.Repositories;

public interface IComputerRepository
{
    int CreateComputer(Computer computer, string identifier);
    Computer GetComputerById(int computerId, string identifier);
    void UpdateComputer(Computer computer, string identifier);
    void DeleteComputer(int computerId, string identifier);
}
