using KaiokenMod.FormLoader;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Armor {

    [AutoloadEquip(EquipType.Body)]
    public class TLOWGi : ModItem {

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("The Lord of Worlds' Shirt");
            Tooltip.SetDefault("There's some hidden power here...");
        }

        public override void SetDefaults() {
            Item.width = 28;
            Item.height = 18;
            Item.value = 18000;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 40;
        }

        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ItemID.FragmentSolar, 60).AddIngredient(ItemID.FragmentVortex, 60).AddIngredient(ItemID.FragmentNebula, 60).AddIngredient(ItemID.FragmentStardust, 60).
                AddIngredient(ItemID.LunarBar, 40).AddIngredient(ItemID.Robe).Register();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) {
            return head.type == ItemID.None && legs.type == ModContent.ItemType<TLOWPants>();
        }

        public override void UpdateArmorSet(Player player) {
            player.setBonus = "Gain greater control of the potential of Kaio-ken.\nWhen in Kaio-ken:\n +59% Max Strain\n +20% Damage\n +5% Crit Chance";

            if (player.HasBuff(FormRegister.KaiFormInstances["Kaioken"].BuffType)) {
                player.GetDamage(DamageClass.Generic) += 0.2f;
                player.GetCritChance(DamageClass.Generic) += 0.05f;
            }
        }
    }
}