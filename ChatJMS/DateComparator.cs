using System.Collections.Generic;
using ChatJMS.Models;

namespace ChatJMS
{
    internal class DateComparator : IComparer<Conversation>
    {
        public int Compare(Conversation x, Conversation y)
        {
            return x.GetLastInteraction().CompareTo(y.GetLastInteraction());
        }
    }
}
