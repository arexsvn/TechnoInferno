using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class AsyncCommandSequence : IAsyncCommand
{
    public bool HaltOnError { get; set; } = true;
    public Action Completed { get; set; }
    public CancellationTokenSource Token => _cancellationTokenSource;
    public Action<IAsyncCommand> Error { get; set; }
    public IAsyncCommand CurrentCommand => _commands[_index];
    public IAsyncCommand PreviousCommand => _commands[_index - 1];
    public int Index => _index;
    private int _index = 0;
    readonly List<IAsyncCommand> _commands = new();
    readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public AsyncCommandSequence()
    {

    }

    public virtual async Task<bool> Execute()
    {
        Reset();
        return await Next();
    }

    public void Reset()
    {
        _index = 0;
    }

    public IAsyncCommand Add(IAsyncCommand command)
    {
        _commands.Add(command);

        return this;
    }

    public IAsyncCommand Remove(IAsyncCommand command)
    {
        _commands.Remove(command);

        return this;
    }

    public IAsyncCommand Pop()
    {
        _commands.RemoveAt(_commands.Count - 1);

        return this;
    }

    protected virtual async Task<bool> Next()
    {
        if (_index < _commands.Count)
        {
            IAsyncCommand command = _commands[_index];
            bool success = await command.Execute();
            if (!success && HaltOnError) 
            {
                Error.Invoke(command);
                return false;
            }

            _index++;
            await Next();
        }
        Completed?.Invoke();
        return true;
    }
}