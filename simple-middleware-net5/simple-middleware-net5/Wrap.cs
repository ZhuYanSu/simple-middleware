using System;

namespace simple_middleware
{
    internal class Wrap : Pipe
    {
        public Wrap(Action<string> action) : base(action)
        {
        }
        public override void Handle(string msg)
        {
            Console.WriteLine("start");
            _action(msg);
            Console.WriteLine("end");
        }
    }

}
