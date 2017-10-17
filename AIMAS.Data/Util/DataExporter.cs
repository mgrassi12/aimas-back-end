using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AIMAS.Data.Util
{
  public class DataExporter
  {
    public static string CreateCSV<T>(List<T> list)
    {
      var csv = "";
      csv += CreateHeader(list);
      csv += CreateRows(list);
      return csv;
    }

    private static string CreateHeader<T>(List<T> list)
    {
      var headers = "";
      PropertyInfo[] properties = typeof(T).GetProperties();
      for (int i = 0; i < properties.Length - 1; i++)
      {
        headers += properties[i].Name + ",";
      }
      var lastProp = properties[properties.Length - 1].Name;
      headers += lastProp + Environment.NewLine;
      return headers;
    }

    private static string CreateRows<T>(List<T> list)
    {
      var rows = "";
      foreach (var item in list)
      {
        rows += CreateRow<T>(item);
      }
      return rows;
    }

    private static string CreateRow<T>(T item, string delim = ",", bool newLine = true)
    {
      var row = "";
      PropertyInfo[] properties = item.GetType().GetProperties();
      for (int i = 0; i < properties.Length - 1; i++)
      {
        var prop = properties[i];
        row += GeneratreProperty(prop, item, delim);
      }
      if (properties.Length != 0)
      {
        var lastProp = properties[properties.Length - 1];
        row += GeneratreProperty(lastProp, item, "");
      }
      else
      {
        row += item.ToString();
      }

      return row + (newLine ? Environment.NewLine : "");
    }

    private static string GeneratreProperty(PropertyInfo prop, object item, string delim)
    {
      var propStr = "";
      if (prop.GetValue(item) == null)
      {
        propStr += "";
      }
      else if (
        prop.PropertyType == typeof(DateTime)
        ||
        prop.PropertyType == typeof(string)
        )
      {
        propStr += prop.GetValue(item) + delim;
      }
      else if (prop.PropertyType == typeof(IList))
      {
        foreach (var subitem in (IList)prop.GetValue(item))
        {
          propStr += CreateRow(subitem, " & ", false);
        }
      }
      else if (prop.PropertyType.IsClass)
      {
        propStr += CreateRow(prop.GetValue(item), " & ", false);
      }      
      else
      {
        propStr += prop.GetValue(item) + delim;
      }

      return propStr;
    }

  }
}
