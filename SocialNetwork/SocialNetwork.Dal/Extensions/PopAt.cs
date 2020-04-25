using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Dal.Extensions
{
    public static class PopAtExtension
    {
        public static T PopAt<T>(this List<T> list, int index)
        {
            T r = list[index];
            list.RemoveAt(index);
            return r;
        }
    }
}
