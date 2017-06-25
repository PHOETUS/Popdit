using CoreLocation;

namespace PopditMobile.iOS
{
    class PopditMobileClient
    {
        // Zone refresh radius in meters - refresh the bubble catalog when you move outside this area.
        private int RefreshRadius = 3000;
        // Zone radius in meters - get the catalog of bubbles that overlap this area.
        private int ZoneRadius = 4500;
        // Location manager object.
        private CLLocationManager LocMgr = new CLLocationManager();

        public PopditMobileClient()
        {
            if (CLLocationManager.IsMonitoringAvailable(typeof(CLCircularRegion)))
            {
                // Initialize.
                Initialize();
                // Set the location fence to detect movement and add a handler for when you pop it.

                // Get the bubble catalog and add a handler for when you pop them.

            }
            else { } // Notify user that geofencing is not possible.
        }

        private void Initialize()
        {
            // Get permission to access user's location in the background
            LocMgr.RequestAlwaysAuthorization();
            // Get permission to access user's location when the app is in use.
            LocMgr.RequestWhenInUseAuthorization();
            // Set pop handlers.
            LocMgr.RegionEntered += LocMgr_RegionEntered;
            LocMgr.RegionLeft += LocMgr_RegionLeft;
        }

        private void LocMgr_RegionEntered(object sender, CLRegionEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void LocMgr_RegionLeft(object sender, CLRegionEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void GetBubbles() { }
  
        private void HandlePop() { }

        private void HandleMove() { }

        void Dummy()
        {
            /*
            var LocMgr = new CLLocationManager();
            LocMgr.RequestAlwaysAuthorization(); //to access user's location in the background
            LocMgr.RequestWhenInUseAuthorization(); //to access user's location when the app is in use.

            CLCircularRegion region = new CLCircularRegion(new CLLocationCoordinate2D(+37.29430502, -122.09423697), 10129.46, "Cupertino");

            if (CLLocationManager.IsMonitoringAvailable(typeof(CLCircularRegion)))
            {
                LocMgr.DidStartMonitoringForRegion += (o, e) => { Console.WriteLine("Now monitoring region {0}", e.Region.ToString()); };
                LocMgr.RegionEntered += (o, e) => { Console.WriteLine("Just entered " + e.Region.ToString()); };
                LocMgr.RegionLeft += (o, e) => { Console.WriteLine("Just left " + e.Region.ToString()); };
                LocMgr.StartMonitoring(region);
            }
            else { Console.WriteLine("This app requires region monitoring, which is unavailable on this device"); }
            */
        }
    }
}
