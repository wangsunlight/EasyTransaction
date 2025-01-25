using System;
using System.Collections.Generic;
using System.Text;

namespace EasyTransaction.Core
{
    public class EasyTransactionException : Exception
    {
        public EasyTransactionException() { }

        public EasyTransactionException(string message) : base(message) { }

        public EasyTransactionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
