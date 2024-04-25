using GameScript;
using VContainer;
using VContainer.Unity;

namespace GameScriptIntegration
{
    public class GameScriptInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<IDialogueController, DialogueController>(Lifetime.Singleton);
            builder.Register<NodeMethods>(Lifetime.Singleton);
            builder.Register<Locale>(_ =>
            {
                Locale locale = new Locale();
                locale.Index = 0;
                locale.LocalizedName = new Localization();
                return locale;
            }, Lifetime.Scoped);
        }
    }
}
