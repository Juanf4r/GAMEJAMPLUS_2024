namespace ProjectDawn.SplitScreen
{
    public interface ISplitScreen
    {
        bool IsCreated { get; }
        void CreateScreens(in Translating translating, ref ScreenRegions screenRegions);
        void DrawDelaunayDual();
        void DrawRegions(BlendShape blendShape);
    }
}
