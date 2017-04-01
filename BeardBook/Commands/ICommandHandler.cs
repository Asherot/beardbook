namespace BeardBook.Commands
{
    public interface ICommandHandler<in TCommand>
    {
        void Handle(TCommand command);
    }
}
