using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Armor {

    [AutoloadEquip(EquipType.Legs)]
    public class TLOWPants : ModItem {

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("The Lord of Worlds' Pants");
            Tooltip.SetDefault("There's some hidden power here...");
        }

        public override void SetDefaults() {
            Item.width = 28;
            Item.height = 18;
            Item.value = 11000;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 36;
        }

        public override void AddRecipes() {
            CreateRecipe().AddIngredient(ItemID.FragmentSolar, 60).AddIngredient(ItemID.FragmentVortex, 60).AddIngredient(ItemID.FragmentNebula, 60).AddIngredient(ItemID.FragmentStardust, 60).
                AddIngredient(ItemID.LunarBar, 40).AddIngredient(ItemID.Silk, 50).Register();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) {
            return head.type == ItemID.None && body.type == ModContent.ItemType<TLOWGi>();
        }

        public override void UpdateArmorSet(Player player) {
            player.setBonus = "Gain greater control of the potential of Kaio-ken.";
        }
    }
}