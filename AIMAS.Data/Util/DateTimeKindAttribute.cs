using System;
using System.Linq;
using System.Reflection;

namespace AIMAS.Data.Util
{
  [AttributeUsage(AttributeTargets.Property)]
  public class DateTimeKindAttribute : Attribute
  {
    private readonly DateTimeKind _kind;

    public DateTimeKindAttribute(DateTimeKind kind)
    {
      _kind = kind;
    }

    public DateTimeKind Kind
    {
      get { return _kind; }
    }

    public static void Apply(object entity)
    {
      if (entity == null)
        return;

      var properties = entity.GetType().GetProperties()
          .Where(x => x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?));

      foreach (var property in properties)
      {
        var attr = property.GetCustomAttribute<DateTimeKindAttribute>();
        if (attr == null)
          continue;

        var dt = property.PropertyType == typeof(DateTime?)
            ? (DateTime?)property.GetValue(entity)
            : (DateTime)property.GetValue(entity);

        if (dt == null)
          continue;

        if (dt.Value.Kind == DateTimeKind.Local && attr.Kind == DateTimeKind.Utc)
          dt = dt.Value.ToUniversalTime();

        if (dt.Value.Kind == DateTimeKind.Utc && attr.Kind == DateTimeKind.Local)
          dt = dt.Value.ToLocalTime();

        property.SetValue(entity, DateTime.SpecifyKind(dt.Value, attr.Kind));
      }
    }
  }
}
