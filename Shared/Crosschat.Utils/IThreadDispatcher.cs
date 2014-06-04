using System;

namespace Crosschat.Utils
{
    public interface IThreadDispatcher
    {
        void Dispatch(Action action);
        void Dispatch<T>(Action<T> action, T state);
        void DispatchIfNeeded(Action action);
    }
}
