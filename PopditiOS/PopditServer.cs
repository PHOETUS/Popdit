using Foundation;

namespace PopditiOS
{
    public static class PopditServer
    {
        // Set the stage here:
        static Stage ServerStage = PopditServer.Stage.Production;

        static NSUrl WebRootProd = new NSUrl("https://prod.popdit.com");
        static NSUrl PopRootProd = new NSUrl("https://pop.popdit.com");
        static NSUrl WebRootStage = new NSUrl("https://stage.popdit.com");
        static NSUrl PopRootStage = new NSUrl("https://pop-stage.popdit.com");
        static NSUrl WebRootDev = new NSUrl("http://192.168.1.116:82");
        static NSUrl PopRootDev = new NSUrl("http://192.168.1.116:83");

        enum Stage { Development, Staging, Production }

        public static NSUrl WebRoot
        {
            get
            {
                if (ServerStage == PopditServer.Stage.Production) return WebRootProd;
                if (ServerStage == PopditServer.Stage.Staging) return WebRootStage;
                return WebRootDev;
            }
        }

        public static NSUrl PopRoot
        {
            get
            {
                if (ServerStage == PopditServer.Stage.Production) return PopRootProd;
                if (ServerStage == PopditServer.Stage.Staging) return PopRootStage;
                return PopRootDev;
            }
        }
    }
}