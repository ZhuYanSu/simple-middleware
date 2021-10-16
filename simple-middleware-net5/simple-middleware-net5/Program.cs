using System;
using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("xUnitTestProject")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace simple_middleware
{
    static class Program
    {
        static void Main(string[] args)
        {
            Action<string> mainAction = (msg) => { Console.WriteLine(msg); };
            PipeBuilder builder = new PipeBuilder(mainAction);
            Action<string> entry = builder
                .AddPipe(typeof(Try))
                .AddPipe(typeof(Wrap))
                .BuildRecursive();
            
            entry("HELLO WORLD");

            /*
             Result:
            trying
            start
            HELLO WORLD
            end
             */
        }
    }


}
