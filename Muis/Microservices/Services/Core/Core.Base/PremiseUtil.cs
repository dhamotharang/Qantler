using System;
using System.Collections.Generic;
using System.Text;
using Core.Model;
using System.Globalization;

namespace Core.Base
{
  public static class PremiseUtil
  {
    public static string format(BasePremise prem)
    {
      var result = "";

      result += append(prem.Schedule);

      result += append(prem.BlockNo);

      result += append(prem.Address1);

      result += append(prem.Address2);

      string floorUnit = prem.FloorNo;

      if (prem.UnitNo != null)
      {
        if (floorUnit != null)
        {
          floorUnit += "-";
        }
        floorUnit += prem.UnitNo;
      }

      if (floorUnit != null)
      {
        if (!floorUnit.StartsWith("#"))
        {
          floorUnit = "#" + floorUnit;
        }
        result += append(floorUnit);
      }

      result += append(prem.BuildingName);

      result += append(prem.Province, ",");

      result += append(prem.Country, ",");

      result += append(prem.Postal);

      return result;
    }

    static string append(string val, string separator = null)
    {
      string str = "";

      if (val == null
        || val.Trim().Length == 0)
      {
        return null;
      }

      if (separator != null)
      {
        str += separator;
      }

      str += " " + val;

      return str;
    }
  }
}

