using Foundation;

namespace PopditiOS
{
    public static class PopditServer
    {
        static bool Production = true;

        static NSUrl WebRootProd = new NSUrl("https://stage.popdit.com");
        static NSUrl PopRootProd = new NSUrl("https://pop-stage.popdit.com");
        static NSUrl WebRootDev = new NSUrl("http://192.168.1.116:82");
        static NSUrl PopRootDev = new NSUrl("http://192.168.1.116:83");

        public static NSUrl WebRoot
        {
            get { if (Production) return WebRootProd; else return WebRootDev; }
        }

        public static NSUrl PopRoot
        {
            get { if (Production) return PopRootProd; else return PopRootDev; }
        }
    }
}