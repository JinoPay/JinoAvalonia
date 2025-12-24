using System.Threading.Tasks;
using AvaloniaApplication1.Services;
using UIKit;

namespace AvaloniaApplication1.iOS.Services;

public class iOSToastService : IToastService
{
    public Task ShowAsync(string title, string message, ToastType type = ToastType.Information, int durationMs = 3000)
    {
        UIApplication.SharedApplication.InvokeOnMainThread(() =>
        {
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            var rootViewController = GetRootViewController();
            rootViewController?.PresentViewController(alert, true, null);

            Task.Delay(durationMs).ContinueWith(_ =>
            {
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    alert.DismissViewController(true, null);
                });
            });
        });

        return Task.CompletedTask;
    }

    private static UIViewController? GetRootViewController()
    {
        // Use modern scene-based API for iOS 13+
        var scenes = UIApplication.SharedApplication.ConnectedScenes;
        foreach (var scene in scenes)
        {
            if (scene is UIWindowScene windowScene)
            {
                foreach (var window in windowScene.Windows)
                {
                    if (window.IsKeyWindow)
                    {
                        return window.RootViewController;
                    }
                }
            }
        }
        return null;
    }
}
