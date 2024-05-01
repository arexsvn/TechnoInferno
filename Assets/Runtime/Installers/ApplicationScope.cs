using VContainer;
using VContainer.Unity;

public class ApplicationScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameController>();

        builder.Register<UICreator>(Lifetime.Singleton);
        builder.Register<MainMenuController>(Lifetime.Singleton);
        builder.Register<UIController>(Lifetime.Singleton);
        builder.Register<HudController>(Lifetime.Singleton);
        builder.Register<TextOverlayController>(Lifetime.Singleton);
        builder.Register<LocaleManager>(Lifetime.Singleton);
        builder.Register<CharacterManager>(Lifetime.Singleton);
        builder.Register<JournalController>(Lifetime.Singleton);
        builder.Register<SaveStateController>(Lifetime.Singleton);
        builder.Register<InboxController>(Lifetime.Singleton);
        builder.Register<CameraController>(Lifetime.Singleton);
        builder.Register<ClockController>(Lifetime.Singleton);
        builder.Register<WebRequestService>(Lifetime.Singleton);
        builder.Register<BasicDialogController>(Lifetime.Singleton);
        builder.Register<InventoryController>(Lifetime.Singleton);

        // Interface implementations
        builder.Register<IAssetService, AddressablesAssetService>(Lifetime.Singleton).AsSelf();
        builder.Register<ISerializationOption, NewtonsoftJsonSerializationOption>(Lifetime.Singleton);
        builder.Register<IUITransitions, DOTweenUITransitions>(Lifetime.Singleton);
        builder.Register<ISceneController, SceneController>(Lifetime.Singleton);

        // Message Channels
        builder.Register<MessageChannel<ApplicationMessage>>(Lifetime.Singleton).AsImplementedInterfaces();

        // Feature-specific and special case installers
        new DynamicGameObjectInstaller().Install(builder);
        new GameScriptIntegration.GameScriptInstaller().Install(builder);
    }
}