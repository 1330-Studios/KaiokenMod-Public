using KaiokenMod.Rarity;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Essence;
internal class HaloOfLight : ModItem {
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("\"Mortals, you must be expunged. A pure and perfect multiverse. Made clean by Zamasu's hand!\"\n+ 25 Base Generic Damage while in Kaio-ken.");
        DisplayName.SetDefault("Halo of Light");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Item.type] = 1;
    }

    public override void AddRecipes() {
        CreateRecipe().AddTile(TileID.MythrilAnvil).AddIngredient(ItemID.SoulofLight, 30).Register();
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