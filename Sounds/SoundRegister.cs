using System.Collections.Generic;

using KaiokenMod.Utils;

using Microsoft.Xna.Framework;

using Terraria.Audio;

namespace KaiokenMod.Sounds;
internal class SoundRegister {
    public static readonly Dictionary<string, SoundStyle> Styles = new() {
        {"KaiForms_SE_00", new SoundStyle("KaiokenMod/Sounds/KaiForms_SE_00")},
        {"KaiForms_SE_01", new SoundStyle("KaiokenMod/Sounds/KaiForms_SE_01")},
        {"KaiForms_SE_02", new SoundStyle("KaiokenMod/Sounds/KaiForms_SE_02")},
        {"KaiForms_SE_03", new SoundStyle("KaiokenMod/Sounds/KaiForms_SE_03")},
        {"KaiForms_SE_04", new SoundStyle("KaiokenMod/Sounds/KaiForms_SE_04", DONTMUTE)},
    };
    private const SoundType DONTMUTE = (SoundType)4;

    internal static void Play(string name, Vector2 position, bool isFormSound = true) {
        if (Styles[name].Type != DONTMUTE)
            if (isFormSound && !KaiokenConfig.Instance.PlayTransformationNoises)
                return;

        SoundEngine.PlaySound(Styles[name], position);
    }
}