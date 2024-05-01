using System.Threading.Tasks;

public interface IAsyncCommand
{
    public Task<bool> Execute();
}
