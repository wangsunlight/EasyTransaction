using AspectCore.DynamicProxy;
using EasyTransaction.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Transactions;

namespace EsayTransaction.Aspnet
{
    /// <summary>
    /// 事务特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class EasyTransactionAttribute : AbstractInterceptorAttribute, IAsyncActionFilter
    {
        private readonly TransactionScopeOption scopeOption;
        private readonly TransactionOptions transactionOptions;

        public EasyTransactionAttribute()
        {
            this.scopeOption = TransactionScopeOption.Required;
            this.transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout
            };
        }

        public EasyTransactionAttribute(TransactionScopeOption scopeOption, TransactionOptions transactionOptions = default)
        {
            this.scopeOption = scopeOption;
            this.transactionOptions = transactionOptions == default ? new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout
            } : transactionOptions;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                await EasyTransactionScopeHelper.ExecuteAsync(() => next(context), scopeOption, transactionOptions);
            }
            catch (EasyTransactionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new EasyTransactionException("Transaction failed", e);
            }
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                await EasyTransactionScopeHelper.ExecuteAsync(async () =>
                {
                    var result = await next();
                    if (result.Exception != null)
                    {
                        throw result.Exception;
                    }
                }, scopeOption, transactionOptions);
            }
            catch (EasyTransactionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new EasyTransactionException("Transaction failed", e);
            }
        }
    }
}
