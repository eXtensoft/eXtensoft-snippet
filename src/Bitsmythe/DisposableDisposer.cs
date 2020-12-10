using System;
using System.Collections.Generic;
using System.Text;

namespace Bitsmythe
{
    public class DisposableDisposer : IDisposable
    {
        private readonly IDisposable[] _Disposables;
        public DisposableDisposer(params IDisposable[] disposables)
        {
            _Disposables = disposables;
        }

        public void Dispose()
        {
            foreach (var disposable in _Disposables)
            {
                disposable.Dispose();
            }
        }

    }
}
