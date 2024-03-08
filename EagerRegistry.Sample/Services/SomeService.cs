using System;

namespace EagerRegistry.Sample.Services;

public interface ISomeService
{
	void DoSomething();
}

[Singleton]
public class SomeService : ISomeService
{
	public void DoSomething()
	{
	}
}

internal class IndependentService
{
	public void DoSomething()
	{
	}
}