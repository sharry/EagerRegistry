namespace EagerRegistry.Sample.Services;

public interface ISomeService
{
	void DoSomething();
}

public interface ISomeOtherService
{
	void DoSomethingElse();
}
public interface ISomeThirdOtherService
{
	void DoSomethingCompletelyDifferent();
}

public class SomeService : ISomeService, ISomeOtherService, ISomeThirdOtherService
{
	public void DoSomething() {}

	public void DoSomethingElse() {}
	public void DoSomethingCompletelyDifferent()
	{
	}
}

internal class IndependentService
{
	public static void DoSomething() {}
	public void DoSomethingElse() {}
}