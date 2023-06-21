using System.Linq;
using System.Reflection;

using KaiokenMod.FormLoader;

using Terraria;
using Terraria.ModLoader;

namespace KaiokenMod.Utils.DBT;
internal class DBTCompat : ILoadable {
    public static Assembly DBT;
    public static bool IsLoaded;

    public static void Load(Mod mod) {
        IsLoaded = ModLoader.TryGetMod("DBZMODPORT", out var dbzmod);
        DBT = IsLoaded ? dbzmod.Code : null;
    }

    public static void Unload(Mod mod) {
        DBT = null;
        IsLoaded = false;
    }

    public static dynamic GetCurrentTransformation(Player player) {
        try {
            var TransformationHelperClazz = DBT.DefinedTypes.First(a => a.Name.Equals("TransformationHelper"));
            var currentForm = TransformationHelperClazz.Invoke<dynamic>("GetCurrentTransformation", player, false, false);
            return currentForm != null ? currentForm.menuId : 0;
        } catch {
            return 0; //dynamics are a bit weird.
        }
    }

    public static dynamic GetDBTPlayer(Player player) {
        var MyPlayerClazz = DBT.DefinedTypes.First(a => a.Name.Equals("MyPlayer"));
        return MyPlayerClazz.Invoke<dynamic>("ModPlayer", player);
    }

    public static void KaiokenUpdate(Player player, KaiForm kaiForm) {
        if (!IsLoaded)
            return;

        unsafe {
            GetDBTPlayer(player).KiDamage += kaiForm.GetDamage(player.GetModPlayer<KPlayer>().Data.Mastery);

            if (KaiokenServerConfig.Instance.AllowStacking)
                return;

            var TransformationHelperClazz = DBT.DefinedTypes.First(a => a.Name.Equals("TransformationHelper"));
            var IsAnyKaioken = TransformationHelperClazz.Invoke<bool>("IsAnyKaioken", player);
            if (!IsAnyKaioken) return;
            var kaioken = TransformationHelperClazz.Invoke<dynamic>("GetCurrentTransformation", player, false, true);
            TransformationHelperClazz.Invoke("RemoveTransformation", new object[] { player, kaioken });
        }
    }

    public static bool IsCharging(Player player) {
        if (!IsLoaded)
            return false;

        var dbtPlayer = GetDBTPlayer(player);
        var MyPlayerClazz = DBT.DefinedTypes.First(a => a.Name.Equals("MyPlayer"));
        return (bool)MyPlayerClazz.GetField("isCharging", ReflectionHelpers.ALL)!.GetValue(dbtPlayer)!;
    }

    public static float GetRotation(Player player) {
        if (!IsLoaded)
            return 0;

        var AuraAnimationsClazz = DBT.DefinedTypes.First(a => a.Name.Equals("AuraAnimations"));
        var chargeAura_AuraAnimationInfo = AuraAnimationsClazz.GetField("createChargeAura", ReflectionHelpers.ALL)?.GetValue(null);

        var DBTPlayer = GetDBTPlayer(player);

        return chargeAura_AuraAnimationInfo.Invoke<dynamic>("GetAuraRotationAndPosition", new object[] { DBTPlayer }).Item1;
    }
}