using System;

namespace xFrame.Core
{
    public interface IErrorHandler
    {
        void HandelError(Exception ex);
    }
}