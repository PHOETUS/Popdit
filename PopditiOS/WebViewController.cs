using System;
using Foundation;
using UIKit;

namespace PopditiOS
{
    public partial class WebViewController : UIViewController
    {
        public LocationManager manager { get; set; }

        static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public WebViewController (IntPtr handle) : base (handle)
		{
            manager = new LocationManager();
            manager.Start();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Intercept URL loading to handle native calls from browser
			WebView.ShouldStartLoad += HandleShouldStartLoad;
            WebView.LoadRequest(new NSUrlRequest(new NSUrl("http://192.168.1.107:82/")));
            //WebView.LoadRequest(new NSUrlRequest(new NSUrl("http://stage.popdit.com/")));            
        }

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		bool HandleShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
		{
            //if (request.Url.AbsoluteString.Contains("popdit.com")
            if (request.Url.AbsoluteString.Contains("192.168.1"))
                return true;
            else
            {
                UIApplication.SharedApplication.OpenUrl(request.Url);
                return false;
            }
        }
	}
}

