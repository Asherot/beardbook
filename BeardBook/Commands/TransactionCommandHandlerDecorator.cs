using System.Transactions;

namespace BeardBook.Commands
{
    public class TransactionCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private readonly ICommandHandler<TCommand> _decorated;

        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> decorated)
        {
            _decorated = decorated;
        }

        public void Handle(TCommand command)
        {
            using (var transactionScope = new TransactionScope())
            {
                _decorated.Handle(command);

                transactionScope.Complete();
            }
        }
    }
}
