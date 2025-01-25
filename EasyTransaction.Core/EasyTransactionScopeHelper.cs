using System;
using System.Threading.Tasks;
using System.Transactions;

namespace EasyTransaction.Core
{
    /// <summary>
    /// Helper class to execute code within a transaction scope
    /// </summary>
    public static class EasyTransactionScopeHelper
    {
        /// <summary>
        /// Execute the action within a transaction scope
        /// </summary>
        /// <param name="action"></param>
        public static void Execute(Action action)
        {
            Execute(action, TransactionScopeOption.Required, default);
        }

        /// <summary>
        /// Execute the action within a transaction scope
        /// </summary>
        /// <param name="action"></param>
        /// <param name="option"></param>
        public static void Execute(Action action, TransactionScopeOption option)
        {
            Execute(action, option, default);
        }

        /// <summary>
        /// Execute the action within a transaction scope
        /// </summary>
        /// <param name="action"></param>
        /// <param name="scopeOption"></param>
        /// <param name="transactionOptions"></param>
        /// <exception cref="EasyTransactionException"></exception>
        public static void Execute(Action action, TransactionScopeOption scopeOption, TransactionOptions transactionOptions)
        {
            Execute(() =>
            {
                action();
                return true;
            }, scopeOption, transactionOptions);
        }

        /// <summary>
        /// Execute the function within a transaction scope and return the result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T Execute<T>(Func<T> func)
        {
            return Execute(func, TransactionScopeOption.Required, default);
        }

        /// <summary>
        /// Execute the function within a transaction scope and return the result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static T Execute<T>(Func<T> func, TransactionScopeOption option)
        {
            return Execute(func, option, default);
        }

        /// <summary>
        /// Execute the function within a transaction scope and return the result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="scopeOption"></param>
        /// <param name="transactionOptions"></param>
        /// <returns></returns>
        /// <exception cref="EasyTransactionException"></exception>
        public static T Execute<T>(Func<T> func, TransactionScopeOption scopeOption, TransactionOptions transactionOptions)
        {
            using (var scope = new TransactionScope(scopeOption, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    T result = func();
                    scope.Complete();
                    return result;
                }
                catch (Exception e)
                {
                    throw new EasyTransactionException("Transaction Execute failed", e);
                }
            }
        }

        /// <summary>
        /// Execute the function within a transaction scope and return the result
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Task ExecuteAsync(Func<Task> func)
        {
            return ExecuteAsync(func, TransactionScopeOption.Required, default);
        }

        /// <summary>
        /// Execute the function within a transaction scope and return the result
        /// </summary>
        /// <param name="func"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static Task ExecuteAsync<T>(Func<Task> func, TransactionScopeOption option)
        {
            return ExecuteAsync(func, option, default);
        }

        /// <summary>
        /// Execute the function within a transaction scope and return the result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="scopeOption"></param>
        /// <param name="transactionOptions"></param>
        /// <returns></returns>
        /// <exception cref="EasyTransactionException"></exception>
        public static async Task ExecuteAsync(Func<Task> func, TransactionScopeOption scopeOption, TransactionOptions transactionOptions)
        {
            using (var scope = new TransactionScope(scopeOption, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await func();
                    scope.Complete();
                }
                catch (Exception e)
                {
                    throw new EasyTransactionException("Transaction ExecuteAsync failed", e);
                }
            }
        }
    }
}
