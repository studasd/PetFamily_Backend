namespace PetFamily.Core.Messaging;

public interface IMessageQueue<TMessage>
{
	Task WriteAsync(TMessage files, CancellationToken token);

	Task<TMessage> ReadAsync(CancellationToken token);
}
