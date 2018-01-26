using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
    public static class EventIconAgent
    {
        public static string BuidlerIcon(int id)
        {
            return BuidlerIcon(id, false);
        }

        public static string BuidlerIcon(int id, bool isBig)
        {
            if (isBig)
                return string.Format("/Images/EventIcons/event_icon_{0}.png", id);
            return string.Format("/Images/EventIcons/event_icon_{0}s.png", id);
        }
    }
}
