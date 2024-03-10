using Microsoft.Extensions.DependencyInjection;

namespace EagerRegistry.Sample;

public class SampleEntity
{
	SampleEntity()
	{
	}
	private int Id { get; } = 42;
	public string? Name { get; } = "Sample";
}
