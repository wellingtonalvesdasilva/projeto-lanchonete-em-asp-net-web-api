using System;
using System.Transactions;

namespace Arquitetura.Helper
{
    public static class TransactionHelper
    {
        public static void PersistirEmTransacao(object context, Action persistenciaComTransacaoDelegate)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                try
                {
                    persistenciaComTransacaoDelegate();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }
    }
}
