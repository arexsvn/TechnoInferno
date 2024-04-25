// GENERATED CODE - DO NOT EDIT BY HAND


namespace GameScript
{
    public static class RoutineInitializer
    {
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize()
        {
            RoutineDirectory.Directory = new System.Action<RunnerContext>[18];
            RoutineDirectory.Directory[0] = (RunnerContext ctx) =>
            {
                uint seq = ctx.SequenceNumber;
                NodeMethods.SaveEvent("AskForHelp");
            };
            RoutineDirectory.Directory[1] = (RunnerContext ctx) =>
            {
                uint seq = ctx.SequenceNumber;
                NodeMethods.AddItem("coffee");
            };
            RoutineDirectory.Directory[2] = (RunnerContext ctx) =>
            {
                ctx.SetConditionResult(!NodeMethods.HasItem("coffee"));
            };
            RoutineDirectory.Directory[3] = (RunnerContext ctx) =>
            {
                ctx.SetConditionResult(!NodeMethods.CheckEvent("AskedForCoffee"));
            };
            RoutineDirectory.Directory[4] = (RunnerContext ctx) =>
            {
                uint seq = ctx.SequenceNumber;
                NodeMethods.SaveEvent("AskedForCoffee");
            };
            RoutineDirectory.Directory[5] = (RunnerContext ctx) =>
            {
                ctx.SetConditionResult(NodeMethods.CheckEvent("AskedForCoffee")&&!NodeMethods.CheckEvent("GaveCoffee"));
            };
            RoutineDirectory.Directory[6] = (RunnerContext ctx) =>
            {
                ctx.SetConditionResult(!NodeMethods.HasItem("coffee"));
            };
            RoutineDirectory.Directory[7] = (RunnerContext ctx) =>
            {
                ctx.SetConditionResult(NodeMethods.HasItem("coffee"));
            };
            RoutineDirectory.Directory[8] = (RunnerContext ctx) =>
            {
                uint seq = ctx.SequenceNumber;
                NodeMethods.RemoveItem("coffee");NodeMethods.SaveEvent("GaveCoffee");
            };
            RoutineDirectory.Directory[9] = (RunnerContext ctx) =>
            {
                ctx.SetConditionResult(NodeMethods.HasItem("coffee"));
            };
            RoutineDirectory.Directory[10] = (RunnerContext ctx) =>
            {
                ctx.SetConditionResult(NodeMethods.CheckEvent("GaveCoffee"));
            };
            RoutineDirectory.Directory[11] = (RunnerContext ctx) =>
            {
                ctx.SetConditionResult(!NodeMethods.CheckEvent("GotRaise"));
            };
            RoutineDirectory.Directory[12] = (RunnerContext ctx) =>
            {
                ctx.SetConditionResult(!NodeMethods.CheckEvent("GaveCoffee"));
            };
            RoutineDirectory.Directory[13] = (RunnerContext ctx) =>
            {
                ctx.SetConditionResult(NodeMethods.CheckEvent("GotRaise"));
            };
            RoutineDirectory.Directory[14] = (RunnerContext ctx) =>
            {
                uint seq = ctx.SequenceNumber;
                NodeMethods.SaveEvent("GotRaise");
            };
            RoutineDirectory.Directory[15] = (RunnerContext ctx) =>
            {
                ctx.SetConditionResult(NodeMethods.CheckEvent("GaveCoffee"));
            };
            RoutineDirectory.Directory[16] = (RunnerContext ctx) =>
            {
            };
            RoutineDirectory.Directory[17] = (RunnerContext ctx) =>
            {
                ctx.SetConditionResult(true);
            };
        }
    }
}
