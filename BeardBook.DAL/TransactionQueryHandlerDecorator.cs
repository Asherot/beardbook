using System.Transactions;

namespace BeardBook.DAL
{
    public class TransactionQueryHandlerDecorator<TQuery, TResult>
        : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _decorated;

        public TransactionQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated)
        {
            _decorated = decorated;
        }

        public TResult Handle(TQuery query)
        {
            TResult result;

            using (var transactionScope = new TransactionScope())
            {
                result = _decorated.Handle(query);

                transactionScope.Complete();
            }

            return result;
        }
    }
}
