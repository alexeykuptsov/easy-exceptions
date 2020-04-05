using System;
using System.Collections.Generic;

namespace EasyExceptions.Utils
{
    public class DisposablesHolder : IDisposable
    {
        private readonly List<IDisposable> myDisposables = new List<IDisposable>();

        public void Using(IDisposable disposable)
        {
            myDisposables.Add(disposable);
        }
        
        public void Dispose()
        {
            foreach (var disposable in myDisposables)
            {
                disposable.Dispose();
            }
        }
    }
}