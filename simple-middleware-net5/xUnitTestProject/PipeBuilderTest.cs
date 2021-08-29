using System;
using Xunit;
using simple_middleware;
using System.IO;
using Moq;

namespace xUnitTestProject
{
    public class PipeBuilderTest: ConsoleTestBase, IDisposable
    {
        private PipeBuilder builder;
        private Action<string> mainAction;
        private string msg;
        private bool disposedValue;

        public PipeBuilderTest(): base("HELLO WORLD")
        {
            mainAction = (msg) => { Console.WriteLine(msg); };
            builder = new PipeBuilder(mainAction);
            msg = _setInMsg;
        }

        [Fact]
        public void CreatePipelineRecursive_NoPipeAdded_ReturnMainAction()
        {
            // arrange
            string expected = msg;
            // action
            Action<string> entry = builder.Build();
            entry(msg);

            // assert
            Assert.Equal(expected, _input.ReadToEnd());
        }

        [Fact]
        public void AddPipe_AddNonPipeType_IgnoreIt()
        {
            Action<string> entry = builder.AddPipe(typeof(string)).Build();
            entry(msg);
            string expected = msg;
            // assert
            Assert.Equal(expected, _input.ReadToEnd());

        }


        [Fact]
        public void AddPipe_AddAPipeType_WrapWithPipe()
        {
            // var fakePipe = new Mock<Try>();
            Action<string> entry = builder.AddPipe(typeof(Try)).Build();
            entry(msg);

            // change SetIn to look for wrapped message
            string expected = "trying\\n" + msg;
            _input = new StringReader(expected);

            // assert
            Assert.Equal(expected, _input.ReadToEnd());

        }

        [Fact]
        public void AddPipe_AddAPipeType_WrapWithMultiplePipes()
        {
            // var fakePipe = new Mock<Try>();
            Action<string> entry = builder.AddPipe(typeof(Try)).AddPipe(typeof(Wrap)).Build();
            entry(msg);

            // change SetIn to look for wrapped message
            string expected = "trying\\nstart\\n" + msg + "\\nend";
            _input = new StringReader(expected);

            // assert
            Assert.Equal(expected, _input.ReadToEnd());

        }

        [Fact]
        public void AddPipe_AddNoPipe_ReturnMainAction()
        {
            // var fakePipe = new Mock<Try>();
            Action<string> entry = builder.Build();
            entry(msg);

            // assert
            Assert.Same(entry, mainAction);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~PipeBuilderTest()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
