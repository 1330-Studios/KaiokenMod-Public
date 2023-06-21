using KaiokenMod.Rarity;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Essence;
internal class FruitOfMight : ModItem {
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("\"Kakarot, you are a disgrace to your people! Thousands of years of evolution, and you cast it aside for a multitude of weaklings!\"\n+ 100 Max Health while in Kaio-ken.");
        DisplayName.SetDefault("Fruit of the Tree of Might");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Item.type] = 1;
    }

    public override void AddRecipes() {
        CreateRecipe().AddTile(TileID.TinkerersWorkbench).AddIngredient(ItemID.Apple, 5).AddIngredient(ItemID.Lemon, 5).AddIngredient(ItemID.Peach, 5).AddIngredient(ItemID.LifeCrystal, 2).Register();
    }

    public override void SetDefaults() {
        Item.color = Color.White;
        Item.width = 46;
        Item.height = 47;
        Item.value = 150000;
        Item.maxStack = 1;
        Item.rare = ModContent.RarityType<KaiokenRarity>();
        Item.defense = 0;
        Item.accessory = true;
    }
}