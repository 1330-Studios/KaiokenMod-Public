using Terraria.ModLoader;

namespace KaiokenMod.Utils;

public interface ILoadable {
#pragma warning disable CA2252

    public static abstract void Load(Mod mod);

    public static abstract void Unload(Mod mod);

#pragma warning restore CA2252
}