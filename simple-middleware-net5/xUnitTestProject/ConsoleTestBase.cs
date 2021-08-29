using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTestProject
{
    public abstract class ConsoleTestBase
    {
        protected readonly string _setInMsg;
        protected TextReader _input;
        protected StringWriter _output;

        public ConsoleTestBase(string setInMsg)
        {
            _setInMsg = setInMsg;
            _input = new StringReader(setInMsg);
            _output  = new StringWriter();

            Console.SetIn(_input);
            Console.SetOut(_output);

        }
    }
}
