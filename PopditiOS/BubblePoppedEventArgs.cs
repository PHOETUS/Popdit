using System;
using CoreLocation;

namespace PopditiOS
{
    public class BubblePoppedEventArgs : EventArgs
    {
        CLLocation location;

        public BubblePoppedEventArgs(CLLocation location)
        {
            this.location = location;
        }

        public CLLocation Location
        {
            get { return location; }
        }
    }
}
