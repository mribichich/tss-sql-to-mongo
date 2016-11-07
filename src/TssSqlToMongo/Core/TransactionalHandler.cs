namespace TssSqlToMongo.Core
{
    using TssSqlToMongo.ReadModel.Handlers;

    public class TransactionalHandler<T> : IHandler<T>
        where T : IMessage
    {
        private readonly ISession session;
        private readonly IHandler<T> next;

        public TransactionalHandler(ISession session, IHandler<T> next)
        {
            this.session = session;
            this.next = next;
        }

        public void Handle(T message)
        {
            this.next.Handle(message);

            this.session.Commit();
        }
    }
}