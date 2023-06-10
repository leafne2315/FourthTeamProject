using System.ComponentModel;
using System.Reflection;

namespace FourthTeamProject.Controllers.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var fieldinfo = value.GetType().GetField(value.ToString());
            var descriptionAttribute = fieldinfo.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute != null ? descriptionAttribute.Description : value.ToString();
    }
    }
}
