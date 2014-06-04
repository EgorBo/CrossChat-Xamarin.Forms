using System.Linq;
using System.Reflection;

namespace Crosschat.Client.Model.Helpers
{
    public static class AutoMapper
    {
        public static TDest CopyPropertyValues<TDest>(object srcObject, TDest destObject)
        {
            var srcValuesMap = srcObject.GetType().GetTypeInfo().DeclaredProperties.ToDictionary(i => i.Name, i => i.GetValue(srcObject));
            foreach (var property in destObject.GetType().GetTypeInfo().DeclaredProperties)
            {
                object value = null;
                if (srcValuesMap.TryGetValue(property.Name, out value))
                {
                    property.SetValue(destObject, value);
                }
            }
            return destObject;
        }
    }
}
