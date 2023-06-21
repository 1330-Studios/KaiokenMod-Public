using KaiokenMod.Rarity;

using Microsoft.Xna.Framework;

using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Essence;
internal class CyberneticEnhancements : ModItem {
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("\"Not that you merit the attention, but I am going to kill you myself. You should feel honored.\"\n+ 5 Defense while in Kaio-ken.");
        DisplayName.SetDefault("Cybernetic Enhancements");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Item.type] = 1;
    }

    public override void AddRecipes() {
        CreateRecipe().AddTile(TileID.TinkerersWorkbench).AddIngredient(ItemID.IronBar, 15).AddIngredient(ItemID.Emerald, 5).Register();
    }

    public override void SetDefaults() {
        Item.color = Color.White;
        Item.width = 34;
        Item.height = 32;
        Item.value = 125000;
        Item.maxStack = 1;
        Item.rare = ModContent.RarityType<KaiokenRarity>();
        Item.defense = 0;
        Item.accessory = true;
    }
}