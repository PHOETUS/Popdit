using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopditDB.Models
{
    partial class Profile
    {
        public ICollection<Friendship> Friendshipz
        {
            get { return Friendships1; }
            set { Friendships1 = value; }
        }
    }
}
