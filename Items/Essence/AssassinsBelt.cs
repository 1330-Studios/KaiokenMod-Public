using KaiokenMod.Rarity;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Essence;
internal class AssassinsBelt : ModItem {
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("\"You showing your cards tells me that you're young.\"\n+ 7% Dodge Chance while in Kaio-ken.");
        DisplayName.SetDefault("Assassin's Belt");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Item.type] = 1;
    }

    public override void AddRecipes() {
        CreateRecipe().AddTile(TileID.MythrilAnvil).AddIngredient(ItemID.CobaltBar, 25).AddIngredient(ItemID.Topaz, 5).Register();
        CreateRecipe().AddTile(TileID.MythrilAnvil).AddIngredient(ItemID.PalladiumBar, 25).AddIngredient(ItemID.Topaz, 5).Register();
    }

    public override void SetDefaults() {
        Item.color = Color.White;
        Item.width = 42;
        Item.height = 18;
        Item.value = 250000;
        Item.maxStack = 1;
        Item.rare = ModContent.RarityType<KaiokenRarity>();
        Item.defense = 0;
        Item.accessory = true;
    }
}