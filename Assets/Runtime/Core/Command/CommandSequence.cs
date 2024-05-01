using System;
using System.Collections.Generic;

namespace command
{
	public class CommandSequence : ICommand
	{
		public bool haltOnError = false;
		private bool _errorHit = false;
		private int _index = 0;
        public Action Completed { get; set; }
        public Action<string> Error { get; set; }
        readonly List<ICommand> _commands;

		public CommandSequence()
		{
			_commands = new List<ICommand>();
		}
			
		public virtual void Execute()
		{
            Reset();
			next();
		}

		public void Reset()
		{
			_index = 0;
			_errorHit = false;
		}

		protected virtual void Complete()
		{
            Completed?.Invoke();
		}
			
		protected virtual void next()
		{
			if (_index < _commands.Count)
			{
				if (!_errorHit || !haltOnError)
				{
					ICommand command = _commands[_index];
					_index++;
					command.Completed += (commandComplete);
					command.Error += (commandError);
					command.Execute();
				}
			} 
			else
			{
                Complete();
			}
		}

		private void commandComplete()
		{
			next();
		}
			
		private void commandError(string errorText)
		{
			_errorHit = true;
            Error?.Invoke(errorText);
		}

		public ICommand currentCommand
		{
			get
			{
				return _commands[_index];
			}
		}

		public ICommand previousCommand
		{
			get
			{
				return _commands[_index - 1];
			}
		}

		public ICommand add(ICommand command)
		{
			_commands.Add(command);

			return this;
		}

		public ICommand remove(ICommand command)
		{
			_commands.Remove(command);

			return this;
		}

		public ICommand pop()
		{
			_commands.RemoveAt(_commands.Count - 1);

			return this;
		}

		public int index
		{
			get
			{
				return _index;
			}
		}
	}
}