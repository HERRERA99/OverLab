using System.Collections.ObjectModel;

namespace OverLab.Wpf.ViewModels;

public static class ObservableCollectionExtensions
{
    public static void Replace<T>(
        this ObservableCollection<T> collection,
        IEnumerable<T> values)
    {
        collection.Clear();
        foreach (var v in values)
            collection.Add(v);
    }
}