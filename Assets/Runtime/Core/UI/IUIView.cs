
public interface IUIView
{
    void Show(bool animate = true);
    void Hide(bool animate = true);
    bool IsFullScreen { get; }
}
