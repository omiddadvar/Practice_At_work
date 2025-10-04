
using CommunityToolkit.Mvvm.ComponentModel;

namespace Maui_Practice.ViewModels;
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;
}
