using KaiokenMod.Rarity;

using Microsoft.Xna.Framework;

using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Accessories;
internal class EnlightenmentSymbol : ModItem {
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("\"Maybe even a lower-class outcast can surpass an elite if he puts his mind to it!\"\n+ 100% Kaio-ken Mastery Gain.\n+ 25% Strain Gain.");
        DisplayName.SetDefault("悟");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Item.type] = 1;
    }

    public override void AddRecipes() {
        CreateRecipe().AddTile(TileID.WorkBenches).AddIngredient(ItemID.Silk, 50).AddIngredient(ItemID.Obsidian, 50).AddIngredient(ItemID.HellstoneBar, 20).Register();
    }

    public override void SetDefaults() {
        Item.color = new Color(0xFF, 0xCE, 0xCF);
        Item.width = 41;
        Item.height = 41;
        Item.value = 40000;
        Item.maxStack = 1;
        Item.rare = ModContent.RarityType<KaiokenRarity>();
        Item.defense = 0;
        Item.accessory = true;
    }
}
