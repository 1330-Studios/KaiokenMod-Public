using System;
using System.Collections.Generic;
using System.Linq;

using Terraria.ModLoader;

namespace KaiokenMod.FormLoader;

internal class FormRegister : ILoadable {
    public static IDictionary<string, KaiForm> KaiFormInstances;

    /// <summary>
    /// Finds all Kai Form classes in the mod passed along
    /// </summary>
    /// <param name="mod">Terraria Mod to search for Kai Forms</param>
    public static void Load(Mod mod) {
        KaiFormInstances = new Dictionary<string, KaiForm>();

        foreach (var type in mod.Code.DefinedTypes.Where(a => !a.IsInterface && !a.IsAbstract && a.IsAssignableTo(typeof(KaiForm)))) {
            var inst = Activator.CreateInstance(type) as KaiForm;
            inst?.Load();
            KaiFormInstances[type.Name] = inst;
        }
    }

    public static void Unload(Mod mod) {
        if (KaiFormInstances is not null)
            foreach (var form in KaiFormInstances)
                form.Value?.Unload();

        KaiFormInstances = null;
    }
}