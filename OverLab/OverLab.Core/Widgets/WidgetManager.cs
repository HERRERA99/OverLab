namespace OverLab.Core.Widgets;

public class WidgetManager
{
    private readonly Dictionary<string, IWidget> _widgets = new();

    public void RegisterWidget(string id, IWidget widget)
    {
        _widgets[id] = widget;
    }

    public T GetWidget<T>(string id) where T : IWidget
    {
        return (T)_widgets[id];
    }

    public IEnumerable<IWidget> GetAllWidgets() => _widgets.Values;
}