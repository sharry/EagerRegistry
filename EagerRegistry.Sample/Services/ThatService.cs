namespace EagerRegistry.Sample.Services;

public interface IThatService { void DoThat(); }
public interface IThatOtherService { void DoThatOtherThing(); }
public class ThatService : IThatService, IThatOtherService
{
	public void DoThat() {}
	public void DoThatOtherThing() {}
}