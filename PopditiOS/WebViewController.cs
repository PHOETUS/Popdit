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


            /*
			// If the URL is not our own custom scheme, just let the webView load the URL as usual
			const string scheme = "hybrid:";

			if (request.Url.Scheme != scheme.Replace (":", ""))
				return true;

			// This handler will treat everything between the protocol and "?"
			// as the method name.  The querystring has all of the parameters.
			var resources = request.Url.ResourceSpecifier.Split ('?');
			var method = resources [0];
			var parameters = System.Web.HttpUtility.ParseQueryString (resources [1]);

			if (method == "UpdateLabel") {
				var textbox = parameters ["textbox"];

				// Add some text to our string here so that we know something
				// happened on the native part of the round trip.
				var prepended = string.Format ("C# says: {0}", textbox);

				// Build some javascript using the C#-modified result
				var js = string.Format ("SetLabelText('{0}');", prepended);

				webView.EvaluateJavascript (js);
			}

			return false;
            */
        }
	}
}

