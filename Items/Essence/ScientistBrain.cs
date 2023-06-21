using KaiokenMod.Rarity;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;


namespace KaiokenMod.Items.Essence;
internal class ScientistBrain : ModItem {
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("\"Goku... Once I've taken control of your body, I'll finally discard this ugly, metal shell. Then, I will fulfill my destiny, and become the greatest scientist the world has ever known!\"\n+ 25 Max Mana while in Kaio-ken.");
        DisplayName.SetDefault("Scientist's Brain");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Item.type] = 1;
    }

    public override void AddRecipes() {
        CreateRecipe().AddTile(TileID.TinkerersWorkbench).AddIngredient(ItemID.RottenChunk, 25).AddIngredient(ItemID.ManaCrystal, 2).AddIngredient(ItemID.DemoniteBar, 10).Register();
        CreateRecipe().AddTile(TileID.TinkerersWorkbench).AddIngredient(ItemID.Vertebrae, 25).AddIngredient(ItemID.ManaCrystal, 2).AddIngredient(ItemID.CrimtaneBar, 10).Register();
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