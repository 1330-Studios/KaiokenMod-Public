using KaiokenMod.Rarity;

using Microsoft.Xna.Framework;

using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Accessories;
internal class WorldKingSymbol : ModItem {
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("\"You still can't seem to deliver a good joke when the pressure's on.\"\n+ 70% Chance to not gain Strain when using Kaio-ken.");
        DisplayName.SetDefault("界王");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Item.type] = 1;
    }

    public override void AddRecipes() {
        CreateRecipe().AddTile(TileID.WorkBenches).AddIngredient(ItemID.BlackThread, 10).AddIngredient(ItemID.Silk, 50).AddIngredient(ItemID.SoulofMight, 5).AddIngredient(ItemID.HallowedBar, 15).Register();
    }

    public override void SetDefaults() {
        Item.color = new Color(0xC0, 0xC0, 0xCF);
        Item.width = 49;
        Item.height = 49;
        Item.value = 40000;
        Item.maxStack = 1;
        Item.rare = ModContent.RarityType<KaiokenRarity>();
        Item.defense = 0;
        Item.accessory = true;
    }
}
