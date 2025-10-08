using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace ALPR;

public partial class App : Application
{
    public App()
    {
        try
        {
            InitializeComponent();

            // Set up global exception handling
            SetupExceptionHandling();

            Debug.WriteLine("App constructor completed successfully");
        }
        catch (Exception ex)
        {
            HandleCriticalException(ex, "App Constructor");
            // Even if constructor fails, we still need to set MainPage
            FallbackMainPage();
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        try
        {
            Debug.WriteLine("Creating main window...");
            return new Window(new AppShell());
        }
        catch (Exception ex)
        {
            HandleCriticalException(ex, "CreateWindow");

            // Fallback: Create a simple window with error info
            return CreateFallbackWindow(ex);
        }
    }

    private void SetupExceptionHandling()
    {
        // Handle exceptions from all .NET threads
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            Exception ex = (Exception)args.ExceptionObject;
            Debug.WriteLine($"[GLOBAL UNHANDLED] {ex}");

            LogToFile($"[GLOBAL UNHANDLED] {DateTime.Now}: {ex}");

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    if (Current?.MainPage != null)
                    {
                        await Current.MainPage.DisplayAlert(
                            "Critical Error",
                            $"A critical error occurred:\n{ex.GetType().Name}: {ex.Message}",
                            "OK"
                        );
                    }
                }
                catch
                {
                    // If we can't show alert, at least we logged it
                }
            });
        };

        // Handle exceptions from async/await tasks
        TaskScheduler.UnobservedTaskException += (sender, args) =>
        {
            Debug.WriteLine($"[UNOBSERVED TASK] {args.Exception}");
            LogToFile($"[UNOBSERVED TASK] {DateTime.Now}: {args.Exception}");
            args.SetObserved(); // Mark as handled to prevent app crash
        };

        // Handle Android runtime exceptions (if on Android)
#if ANDROID
        Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
        {
            Debug.WriteLine($"[ANDROID RUNTIME] {args.Exception}");
            LogToFile($"[ANDROID RUNTIME] {DateTime.Now}: {args.Exception}");
        };
#endif
    }

    protected override void OnStart()
    {
        try
        {
            base.OnStart();
            Debug.WriteLine("App OnStart completed successfully");
        }
        catch (Exception ex)
        {
            HandleCriticalException(ex, "OnStart");
        }
    }

    protected override void OnSleep()
    {
        try
        {
            base.OnSleep();
            Debug.WriteLine("App OnSleep completed successfully");
        }
        catch (Exception ex)
        {
            HandleCriticalException(ex, "OnSleep");
        }
    }

    protected override void OnResume()
    {
        try
        {
            base.OnResume();
            Debug.WriteLine("App OnResume completed successfully");
        }
        catch (Exception ex)
        {
            HandleCriticalException(ex, "OnResume");
        }
    }

    private void HandleCriticalException(Exception ex, string context)
    {
        string errorMessage = $"[CRITICAL] {context}: {ex}";
        Debug.WriteLine(errorMessage);
        LogToFile($"{DateTime.Now}: {errorMessage}");
    }

    private void LogToFile(string message)
    {
        try
        {
            string logPath = Path.Combine(FileSystem.AppDataDirectory, "alpr_crash_log.txt");
            File.AppendAllText(logPath, message + Environment.NewLine + Environment.NewLine);
        }
        catch (Exception logEx)
        {
            Debug.WriteLine($"[LOG FAILED] Could not write to log file: {logEx}");
        }
    }

    private void FallbackMainPage()
    {
        try
        {
            // Create a simple fallback main page if the regular one fails
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label { Text = "ALPR App", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                        new Label { Text = "App initialized in fallback mode due to startup error.", Margin = new Thickness(20) },
                        new Label { Text = "Check the debug output for details.", Margin = new Thickness(20) }
                    }
                }
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[FALLBACK FAILED] Could not create fallback page: {ex}");
        }
    }

    private Window CreateFallbackWindow(Exception ex)
    {
        try
        {
            var fallbackPage = new ContentPage
            {
                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Padding = 30,
                        Children =
                        {
                            new Label { Text = "⚠️ ALPR - Error", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                            new Label { Text = "The app encountered an error during startup:", FontSize = 16, Margin = new Thickness(0, 20, 0, 10) },
                            new Label { Text = ex.Message, FontSize = 14, TextColor = Colors.Red },
                            new Label { Text = "Stack Trace:", FontSize = 16, Margin = new Thickness(0, 20, 0, 10) },
                            new Label { Text = ex.StackTrace, FontSize = 10, FontFamily = "Courier New" }
                        }
                    }
                }
            };

            return new Window(fallbackPage);
        }
        catch (Exception fallbackEx)
        {
            Debug.WriteLine($"[FALLBACK WINDOW FAILED] {fallbackEx}");

            // Ultimate fallback - just return a basic window
            return new Window(new ContentPage
            {
                Content = new Label { Text = "ALPR App - Critical Error" }
            });
        }
    }
}