using System;

namespace simple_middleware
{
    internal abstract class Pipe
    {
        // action should be accessed by inherited classes
        protected Action<string> _action;
        protected Pipe(Action<string> action)
        {
            this._action = action;
        }

        public abstract void Handle(string msg);
    }

}
