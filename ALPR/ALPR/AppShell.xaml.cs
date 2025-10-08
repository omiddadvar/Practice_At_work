using System.Diagnostics;

namespace ALPR;

//public partial class AppShell : Shell
//{
//    public AppShell()
//    {
//        InitializeComponent();
//    }
//}
public partial class AppShell : Shell
{
    public AppShell()
    {
        try
        {
            InitializeComponent();
            Debug.WriteLine("AppShell initialized successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"AppShell Constructor Error: {ex}");

            // Create a simple fallback shell
            CreateFallbackShell();
        }
    }

    private void CreateFallbackShell()
    {
        try
        {
            // Create a simple tab structure as fallback
            var flyoutItem = new FlyoutItem
            {
                Title = "ALPR",
                Items =
                {
                    new ShellContent
                    {
                        Content = new MainPage()
                    }
                }
            };

            Items.Add(flyoutItem);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Fallback shell creation failed: {ex}");
        }
    }
}