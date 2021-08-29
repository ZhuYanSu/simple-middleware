using System;
using System.Collections.Generic;

namespace simple_middleware
{
    
    internal class PipeBuilder: IDisposable
    {
        private List<Type> _pipeTypes;
        private Action<string> _mainAction;
        private bool disposedValue;

        public PipeBuilder(Action<string> mainAction)
        {
            _pipeTypes = new List<Type>();
            _mainAction = mainAction;
        }

        public PipeBuilder AddPipe(Type pipeType)
        {
            // only type which is subclass of Pipe can be added
            if (pipeType.IsSubclassOf(typeof(Pipe))) {
                _pipeTypes.Add(pipeType);
            }

            return this;
        }

        public Action<string> CreatePipeline()
        {
            int pipeCnt = _pipeTypes.Count;
            if (pipeCnt < 1) { return _mainAction; }

            var pipe = (Pipe)Activator.CreateInstance(_pipeTypes[pipeCnt - 1], _mainAction);
            for (int i = pipeCnt - 2; i > -1; i--)
            {
                Action<string> handle = pipe.Handle;
                var pipe2 = (Pipe)Activator.CreateInstance(_pipeTypes[i], handle);
                pipe = pipe2;
            }
            return pipe.Handle;
        }

        public Action<string> CreatePipelineRecursive(int index)
        {
            // base case
            if (_pipeTypes.Count < 1)
            {
                return _mainAction;
            }
            if (index == _pipeTypes.Count - 1)
            {
                // the last pipe
                Pipe lastPipe = (Pipe)Activator.CreateInstance(_pipeTypes[index], _mainAction);
                return lastPipe.Handle;
            }
            else {
                // call recursively
                Action<string> previousHandler = CreatePipelineRecursive(index + 1);
                Pipe currentPipe = (Pipe)Activator.CreateInstance(_pipeTypes[index], previousHandler);
                return currentPipe.Handle;
            }
        }

        public Action<string> Build()
        {
            return CreatePipeline();
        }

        public Action<string> BuildRecursive()
        {
            
            return CreatePipelineRecursive(0);
        }

        protected virtual void Dispose(bool disposing) // protected virtual => derived classes can overrride this method
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
        // ~PipeBuilder()
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

        //~PipeBuilder() {
        //    ((IDisposable)this).Dispose();
        //}
    }

}
