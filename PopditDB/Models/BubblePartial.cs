using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace PopditDB.Models
{
    partial class Bubble : IEquatable<Bubble>
    {
        public void UpdateMaxMin()
        {
            const double metersPerDegree = 111195;
            double LatRadius = Radius.Meters / metersPerDegree;
            MaxLatitude = Latitude + LatRadius;
            MinLatitude = Latitude - LatRadius;
            double LongRadius = Radius.Meters / metersPerDegree * Math.Abs(Math.Cos(Math.PI / 180 * Latitude));
            MaxLongitude = Longitude + LongRadius;
            MinLongitude = Longitude - LongRadius;
        }

        public bool Equals(Bubble other)
        {
            return (this.Id == other.Id);
        }

        public bool IsFriendly(Profile p) // Does this belong to a friend of p?
        {
            bool friendlyBubble = false;
            foreach (Friendship f in p.Friendshipz)
            {
                if (f.ProfileIdOwned == this.ProfileId)
                {
                    friendlyBubble = true;
                    break;
                }
            }
            return friendlyBubble;
        }

        public bool IsFresh(Profile p, DateTime sinceWhen) // Has this profile popped this bubble since this time?
        {
            // Get a list of bubbles popped by this user since the specified time.
            var popped = p.Events.Where(e => e.Timestamp > sinceWhen).Select(e => e.Bubble);
            return !popped.Contains(this);
        }
    }
}
