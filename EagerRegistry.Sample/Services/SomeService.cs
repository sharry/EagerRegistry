namespace EagerRegistry.Sample.Services;

public interface ISomeService
{
	void DoSomething();
}

public interface ISomeOtherService
{
	void DoSomethingElse();
}

public class SomeService : ISomeService, ISomeOtherService
{
	public void DoSomething()
	{
	}

	public void DoSomethingElse()
	{
		throw new System.NotImplementedException();
	}
}

internal class IndependentService
{
	public void DoSomething()
	{
	}
}