using GameScript;
using VContainer;
using VContainer.Unity;

public class DynamicGameObjectInstaller : IInstaller
{
    public void Install(IContainerBuilder builder)
    {
        builder.RegisterComponentOnNewGameObject<CoroutineRunner>(Lifetime.Scoped, "CoroutineRunner");
        builder.RegisterComponentOnNewGameObject<ApplicationMonitor>(Lifetime.Scoped, "ApplicationMonitor");
        builder.RegisterComponentOnNewGameObject<AudioSources>(Lifetime.Scoped, "AudioSources");
        builder.RegisterComponentOnNewGameObject<Runner>(Lifetime.Scoped, "GamescriptRunner").DontDestroyOnLoad();
    }
}
