using System;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PopditiOS
{
    public partial class WebViewController : UIViewController
    {
        public LocationManager Manager { get; set; }
        int Counter = 0;

        static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public WebViewController (IntPtr handle) : base (handle)
		{
            Manager = new LocationManager();
            Manager.Start();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Intercept URL loading to handle native calls from browser
			WebView.ShouldStartLoad += HandleShouldStartLoad;
            WebView.LoadError += HandleLoadError;
            WebView.LoadRequest(new NSUrlRequest(PopditServer.WebRoot));
        }

        private async void HandleLoadError(object sender, UIWebErrorArgs e)
        {
            Debug.WriteLine(">>>>> Load failed; retrying");
            // Send notification 10 seconds into every every 10 minutes.  This permits two failures withotu notification at the beginning.
            Counter = Counter % 120;  // Resets to 0 every 10 minutes.
            await Task.Delay(5000);  // 5 seconds.            
            if (Counter == 2) Manager.DisplayNotification("Connection lost", null, "Please connect to the Internet", "Connecting");
            // Increment counter.
            Counter++;
            // Try again.
            WebView.LoadRequest(new NSUrlRequest(PopditServer.WebRoot));
        }

		bool HandleShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
		{
            if (request.Url.AbsoluteString.Contains(PopditServer.WebRoot.AbsoluteString)) return true;
            else
            {
                UIApplication.SharedApplication.OpenUrl(request.Url);
                return false;
            }
        }
	}
}