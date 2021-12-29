using System;
using System.Collections.Generic;

namespace EasyExceptions.Utils
{
    public class DisposablesHolder : IDisposable
    {
        private readonly List<IDisposable> myDisposables = new List<IDisposable>();

        public void Register(IDisposable disposable)
        {
            myDisposables.Add(disposable);
        }
        
        public void Dispose()
        {
            var disposablesToEnumerate = new List<IDisposable>(myDisposables);
            disposablesToEnumerate.Reverse();
            foreach (var disposable in disposablesToEnumerate)
            {
                disposable.Dispose();
            }
        }
    }
}