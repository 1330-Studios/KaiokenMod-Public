using Microsoft.Xna.Framework.Input;

using Terraria.ModLoader;

namespace KaiokenMod.Utils;

public sealed class KaiokenKeybinds : ILoadable {
    public static ModKeybind Toggle { get; private set; }

    public static void Load(Mod mod) {
        Toggle = KeybindLoader.RegisterKeybind(mod, "Toggle Kaio-ken", Keys.U);
    }

    public static void Unload(Mod mod) {
        Toggle = null;
    }
}