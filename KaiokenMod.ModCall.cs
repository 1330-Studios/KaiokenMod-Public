using System;
using System.Runtime.InteropServices;

using Terraria;

namespace KaiokenMod;

public partial class KaiokenMod {
    private static IntPtr? _pointer;

    /// <summary>
    /// Allows interoperability with other mods through the Mod.Call API. This is done by sending and receiving pointers to a KPlayer_Data-esque struct with a float, double, and bool and LayoutKind being LayoutKind.Sequential.
    /// </summary>
    /// <param name="args">args[0] = <c>Boolean</c> operation (True = Read, False = Write),
    /// args[1] = <c>Boolean</c> Player.whoAmI,
    /// args[2] = <c>IntPtr</c> Pointer to KPlayer_Data-esque struct (Only required on Write)</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">"args" is null</exception>
    /// <exception cref="ArgumentException">"args" Length is 0</exception>
    public override object Call(params object[] args) {
        if (args is null)
            throw new ArgumentNullException(nameof(args), "Arguments cannot be null!");
        if (args.Length == 0)
            throw new ArgumentException("Arguments cannot be empty!");

        var op = (bool)args[0];
        var playerId = (int)args[1];
        var player = playerId == -1 ? Main.LocalPlayer : Main.player[playerId];
        var kPlayer = player.GetModPlayer<KPlayer>();

        if (op) {
            var data = kPlayer.Data;
            if (_pointer.HasValue) Marshal.FreeHGlobal(_pointer.Value);

            var tmpPtr = Marshal.AllocHGlobal(Marshal.SizeOf(data));
            Marshal.StructureToPtr(data, tmpPtr, true);
            _pointer = tmpPtr;
            return _pointer;
        } else {
            var setPtr = (IntPtr)args[2];
            var data = Marshal.PtrToStructure<KPlayer_Data>(setPtr);

            kPlayer.Data.Formed = data.Formed;
            kPlayer.Data.SetStrain(data.Strain, player);
            kPlayer.Data.SetMastery(data.Mastery);
        }

        return null;
    }
}