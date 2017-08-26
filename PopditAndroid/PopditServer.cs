using Java.Net;

namespace PopditAndroid
{
    public static class PopditServer
    {
        static bool Production = true;

        static URL WebRootProd = new URL("https://stage.popdit.com");
        static URL PopRootProd = new URL("https://pop-stage.popdit.com");
        static URL WebRootDev = new URL("http://192.168.1.116:82");
        static URL PopRootDev = new URL("http://192.168.1.116:83");

        public static URL WebRoot
        {
            get { if (Production) return WebRootProd; else return WebRootDev; }
        }

        public static URL PopRoot
        {
            get { if (Production) return PopRootProd; else return PopRootDev; }
        }
    }
}