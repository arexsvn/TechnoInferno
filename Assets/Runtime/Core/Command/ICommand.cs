using System;

namespace command
{
	public interface ICommand
	{
		void Execute();
        Action Completed { get; set; }
        Action<string> Error { get; set; }
    }
}