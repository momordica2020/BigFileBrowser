using System.Configuration;
using System.Data;
using FileViewer.Views;
using System.Windows;

namespace FileViewer;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // 在这里注册依赖项
    }
}

