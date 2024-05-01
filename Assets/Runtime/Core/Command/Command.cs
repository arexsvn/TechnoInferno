using System;

namespace command
{
	public class Command : ICommand
	{
        public Action Completed { get; set; }
        public Action<string> Error { get; set; }

        public Command()
		{

		}

		virtual public void Execute()
		{
			
		}
			
		virtual protected void Complete()
		{
            Completed?.Invoke();
		}
	}
}