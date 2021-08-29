using System;

namespace simple_middleware
{
    internal class Try : Pipe
    {
        public Try(Action<string> action) : base(action)
        {

        }

        public override void Handle(string msg)
        {
            try
            {
                Console.WriteLine("trying");
                _action(msg);
            }
            catch (Exception ex)
            {
            }
        }
    }

}
