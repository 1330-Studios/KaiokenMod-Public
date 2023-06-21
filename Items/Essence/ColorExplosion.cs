using KaiokenMod.Rarity;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Essence;
internal class ColorExplosion : ModItem {
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("\"Well Saiyan, you are a truly fascinating creature. However, your passion is expended. You have nothing more to offer.\"\n+ 15% Crit Chance while in Kaio-ken.");
        DisplayName.SetDefault("Pride Trooper's Color Explosion");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Item.type] = 1;
    }

    public override void AddRecipes() {
        CreateRecipe().AddTile(TileID.MythrilAnvil).AddIngredient(ItemID.SoulofLight, 10).AddIngredient(ItemID.SoulofNight, 10).Register();
    }

    public override void SetDefaults() {
        Item.color = Color.White;
        Item.width = 44;
        Item.height = 41;
        Item.value = 500000;
        Item.maxStack = 1;
        Item.rare = ModContent.RarityType<KaiokenRarity>();
        Item.defense = 0;
        Item.accessory = true;
    }
}