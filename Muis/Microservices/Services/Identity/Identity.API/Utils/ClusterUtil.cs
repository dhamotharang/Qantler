using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Utils
{
  public class ClusterUtil
  {
    readonly static string[] Colors = new string[]
    {
      "red",
      "red",
      "purple",
      "deep-purple",
      "indigo",
      "blue",
      "light-blue",
      "cyan",
      "teal",
      "green",
      "light-green",
      "lime",
      "yellow",
      "amber",
      "orange",
      "deep-orange",
      "brown",
      "grey",
      "blue-grey"
    };

    public static string GetNextColor(int lastIndex)
    {
      return ClusterUtil.Colors[lastIndex % ClusterUtil.Colors.Length];
    }
  }
}
