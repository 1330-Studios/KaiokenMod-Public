using KaiokenMod.Rarity;

using Microsoft.Xna.Framework;

using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Essence;
internal class RoyalTail : ModItem {
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("\"Now I'll show you the wall that you can never scale with \"hard work\" alone...!\"\n+ 5% Damage Reduction while in Kaio-ken.");
        DisplayName.SetDefault("Royal Tail");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Item.type] = 1;
    }

    public override void AddRecipes() {
        CreateRecipe().AddTile(TileID.TinkerersWorkbench).AddIngredient(ItemID.ShadowScale, 20).AddIngredient(ItemID.IronBar, 5).Register();
        CreateRecipe().AddTile(TileID.TinkerersWorkbench).AddIngredient(ItemID.TissueSample, 20).AddIngredient(ItemID.IronBar, 5).Register();
    }

    public override void SetDefaults() {
        Item.color = Color.White;
        Item.width = 46;
        Item.height = 46;
        Item.value = 75000;
        Item.maxStack = 1;
        Item.rare = ModContent.RarityType<KaiokenRarity>();
        Item.defense = 0;
        Item.accessory = true;
    }
}