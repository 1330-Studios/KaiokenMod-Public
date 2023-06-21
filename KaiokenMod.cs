using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using KaiokenMod.FormLoader;
using KaiokenMod.Forms;
using KaiokenMod.Utils;
using KaiokenMod.Utils.DBT;

using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace KaiokenMod;

public partial class KaiokenMod : Mod {
    public static KaiokenMod Instance;

    public override void Load() {
        Instance = this;

        KaiokenKeybinds.Load(this);
        FormRegister.Load(this);
        DBTCompat.Load(this);

        Task.Run(RebuildLoop);
    }

    /// <summary>
    /// Rebuilds the Kaio-ken form, based on config, on loop.
    /// </summary>
    [DoesNotReturn]
    public static async Task RebuildLoop() {
        var i = 0;
        while (true) {
            await Task.Delay(i < 10 ? 5000 : i < 200 ? 15000 : 30000);
            try {
                Kaioken.Rebuild();
            } catch {
                // ignored
            }

            i = ++i;
        }
    }

    public override void Unload() {
        KaiokenKeybinds.Unload(this);
        FormRegister.Unload(this);
        DBTCompat.Unload(this);

        KaiokenConfig.Unload();
        KaiokenServerConfig.Unload();

        Instance = null;
    }

    /// <summary>
    /// Saves config
    /// </summary>
    /// <param name="cfg"></param>
    internal static void SaveConfig(KaiokenConfig cfg) => typeof(ConfigManager).Invoke("Save", cfg);
}