using RecoilReworkServer.Models;
using SPTarkov.Common.Extensions;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Utils.Logger;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;

namespace RecoilReworkServer.Helpers
{
    [Injectable]
    public class ItemHelper(SptLogger<ItemHelper> logger)
    {
        public void AddRecoilModifierData(TemplateItem itemClone, RecoilModifierData? recoilModifierData)
        {
            if (recoilModifierData is null) return;

            Dictionary<string, object?> props = recoilModifierData.GetAllPropertiesAsDictionary();

            foreach (KeyValuePair<string, object?> prop in props)
            {
                if (prop.Value != null)
                {
                    itemClone.AddToExtensionData(prop.Key, prop.Value);
                }
            }
        }
        
        public void UpdateBaseItem(TemplateItem itemClone, TemplateItemProperties? overrideProperties)
        {
            if (overrideProperties is null || itemClone?.Properties is null) return;

            var target = itemClone.Properties;
            var targetType = target.GetType();

            foreach (var member in overrideProperties.GetType().GetMembers())
            {
                var value = member.MemberType switch
                {
                    MemberTypes.Property => ((PropertyInfo)member).GetValue(overrideProperties),
                    MemberTypes.Field => ((FieldInfo)member).GetValue(overrideProperties),
                    _ => null,
                };

                if (value is null)
                {
                    continue;
                }

                var targetMember = targetType.GetMember(member.Name).FirstOrDefault();
                if (targetMember is null)
                {
                    continue;
                }

                switch (targetMember.MemberType)
                {
                    case MemberTypes.Property:
                        var prop = (PropertyInfo)targetMember;
                        if (prop.CanWrite)
                        {
                            prop.SetValue(target, value);
                        }

                        break;

                    case MemberTypes.Field:
                        var field = (FieldInfo)targetMember;
                        if (!field.IsInitOnly)
                        {
                            field.SetValue(target, value);
                        }

                        break;
                }
            }
        }
    }
}
