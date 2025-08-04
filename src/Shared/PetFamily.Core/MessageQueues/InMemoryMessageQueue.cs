using PetFamily.Core.Messaging;
using System.Threading.Channels;

namespace PetFamily.Core.MessageQueues;

public class InMemoryMessageQueue<TMessage> : IMessageQueue<TMessage>
{
	private readonly Channel<TMessage> channel = Channel.CreateUnbounded<TMessage>();


	public async Task WriteAsync(TMessage files, CancellationToken token)
	{
		await channel.Writer.WriteAsync(files, token);
	}

	public async Task<TMessage> ReadAsync(CancellationToken token)
	{
		return await channel.Reader.ReadAsync(token);
	}
}
