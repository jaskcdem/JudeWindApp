using DataAcxess.Extension;
using GreenUtility;
using GreenUtility.Equip;
using static GreenUtility.RPGSetting;

namespace DataAcxess.Repository
{
    public class EquipRepository : ISampleRepository
    {
        #region private members
        readonly string[] weaponTypes = Enum.GetNames(typeof(Weapon)),
            armorType = Enum.GetNames(typeof(Armor));

        readonly List<(Weapon wep, string defName, (CharParameter para, int basePoint, int growPoint)[] values)> WepList
           = [(Weapon.Sword, "魔法騎士劍", [(CharParameter.Atk, 5, 2), (CharParameter.Mat, 7, 6)])
                ,(Weapon.Sword, "皇家騎士劍", [(CharParameter.Atk, 8, 5)])
                ,(Weapon.Sword, "碧水晶之劍", [(CharParameter.Atk, 16, 4), (CharParameter.Mdf, 2 , 1)])
                ,(Weapon.Sword, "金水晶之劍", [(CharParameter.Atk, 16, 4), (CharParameter.Def, 2 , 1)])
                ,(Weapon.Sword, "白薔薇之劍", [(CharParameter.Atk, 16, 4), (CharParameter.Mhp, 20 , 10)])
                ,(Weapon.Sword, "藍薔薇之劍", [(CharParameter.Atk, 16, 4), (CharParameter.Mmp, 20 , 10)])
                ,(Weapon.Naginata, "驅魔薙刀", [(CharParameter.Atk, 6, 5), (CharParameter.Agi, 1, 1)])
                ,(Weapon.Naginata, "水縹", [(CharParameter.Atk, 10, 3)])
                ,(Weapon.Naginata, "迦具土", [(CharParameter.Atk, 12, 3), (CharParameter.Mat, 4, 1)])
                ,(Weapon.Bow, "獵弓", [(CharParameter.Atk, 10, 3)])
                ,(Weapon.Bow, "龍骨弓", [(CharParameter.Atk, 13, 8)])
                ,(Weapon.Bow, "詩歌之弓", [(CharParameter.Atk, 12, 3)])
                ,(Weapon.Bow, "煉鋼弓", [(CharParameter.Atk, 17, 6)])
               ];
        readonly List<(Armor amr, string defName, (CharParameter para, int basePoint, int growPoint)[] values)> ArmList
            = [(Armor.Shoes, "跑鞋", [(CharParameter.Agi, 10, 2)])
                ,(Armor.Shoes, "獸毛鞋", [(CharParameter.Agi, 8, 3), (CharParameter.Def, 1, 1)])
                ,(Armor.Shoes, "肉球皮鞋", [(CharParameter.Agi, 8, 4), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Gloves, "雪乃手套", [(CharParameter.Mhp, 80, 20), (CharParameter.Mmp, 100, 25)])
                ,(Armor.Gloves, "狩獵手套", [(CharParameter.Mhp, 50, 25)])
                ,(Armor.Gloves, "猛鷲手套", [(CharParameter.Def, 5, 9), (CharParameter.Mdf, 0, 7)])
                ,(Armor.Hat, "貝雷帽", [(CharParameter.Mhp, 120, 20), (CharParameter.Mmp, 60, 10)])
                ,(Armor.Hat, "蝴蝶緞帶", [(CharParameter.Mhp, 60, 20)])
                ,(Armor.Hat, "羽毛帽", [(CharParameter.Mmp, 70, 15)])
                ,(Armor.Robes, "元素袍", [(CharParameter.Def, 5, 3), (CharParameter.Mdf, 9, 6)])
                ,(Armor.Robes, "女巫袍", [(CharParameter.Def, 8, 5), (CharParameter.Mdf, 0, 1)])
                ,(Armor.Robes, "棉布長袍", [(CharParameter.Def, 9, 5), (CharParameter.Mdf, 14, 8)])
                ,(Armor.Robes, "百花袍", [(CharParameter.Def, 16, 4), (CharParameter.Mdf, 0, 2)])
                ];
        #endregion

        #region methods
        public BaseEquip GetRandomEquip(ShopType shopType) => shopType switch
        {
            ShopType.Weapon => CreateWeapon(weaponTypes[Utility.RandomInt(1, weaponTypes.Length)].ToEnum<Weapon>()),
            ShopType.Armor => CreateArmor(armorType[Utility.RandomInt(1, armorType.Length)].ToEnum<Armor>()),
            _ => Utility.RandomInt(2) switch
            {
                1 => CreateArmor(armorType[Utility.RandomInt(1, armorType.Length)].ToEnum<Armor>()),
                _ => CreateWeapon(weaponTypes[Utility.RandomInt(1, weaponTypes.Length)].ToEnum<Weapon>()),
            },
        };
        public BaseEquip GetWeapon(Weapon etype) => CreateWeapon(etype);
        public BaseEquip GetArmor(Armor etype) => CreateArmor(etype);
        #endregion

        #region <-- Factory -->
        BaseEquip CreateWeapon(Weapon etype)
        {
            short rank = this.GetRandomRank();
            var query = WepList.Where(w => w.wep == etype);
            if (!query.Any()) return new BaseEquip(rank);
            var (wep, defName, values) = query.ElementAt(Utility.RandomInt(query.Count()));
            BaseEquip item = etype switch
            {
                Weapon.Sword => new BaseSword(rank, defName),
                Weapon.Katana => new BaseKatana(rank, defName),
                Weapon.Rapier => new BaseRapier(rank, defName),
                Weapon.Dagger => new BaseDagger(rank, defName),
                Weapon.GreatSword => new BaseGreatSword(rank, defName),
                Weapon.Naginata => new BaseNaginata(rank, defName),
                Weapon.Dual_wield => new BaseDualwield(rank, defName),
                Weapon.Axe => new BaseAxe(rank, defName),
                Weapon.Hammer => new BaseHammer(rank, defName),
                Weapon.Stick => new BaseStick(rank, defName),
                Weapon.Mace => new BaseMace(rank, defName),
                Weapon.Halberd => new BaseHalberd(rank, defName),
                Weapon.Spear => new BaseSpear(rank, defName),
                Weapon.Bow => new BaseBow(rank, defName),
                Weapon.Crossbow => new BaseCrossbow(rank, defName),
                Weapon.Gun => new BaseGun(rank, defName),
                Weapon.Staff => new BaseStaff(rank, defName),
                Weapon.Knuckles => new BaseKnuckles(rank, defName),
                Weapon.Claw => new BaseClaw(rank, defName),
                Weapon.Scythe => new BaseScythe(rank, defName),
                Weapon.Whip => new BaseWhip(rank, defName),
                _ => throw new ArgumentNullException(nameof(etype), "Undifind Equip type"),
            };
            SetEquip(item, rank, values);
            item.Note = defName;
            return item;
        }
        BaseEquip CreateArmor(Armor etype)
        {
            short rank = this.GetRandomRank();
            var query = ArmList.Where(w => w.amr == etype);
            if (!query.Any()) return new BaseEquip(rank);
            var (wep, defName, values) = query.ElementAt(Utility.RandomInt(query.Count()));
            BaseEquip item = etype switch
            {
                Armor.Clothes => new BaseClothes(rank, defName),
                Armor.Robes => new BaseRobe(rank, defName),
                Armor.LightArmor => new BaseLightArmor(rank, defName),
                Armor.HeavyArmor => new BaseHeavyArmor(rank, defName),
                Armor.SmallSheld => new BaseSmallSheld(rank, defName),
                Armor.BigSheld => new BaseBigSheld(rank, defName),
                Armor.KitSheld => new BaseKitSheld(rank, defName),
                Armor.Hat => new BaseHat(rank, defName),
                Armor.Helmet => new BaseHelmet(rank, defName),
                Armor.Gloves => new BaseGloves(rank, defName),
                Armor.Shoes => new BaseShoes(rank, defName),
                _ => throw new ArgumentNullException(nameof(etype), "Undifind Equip type"),
            };
            SetEquip(item, rank, values);
            item.Note = defName;
            return item;
        }

        static void SetEquip(BaseEquip item, short rank, (CharParameter para, int basePoint, int growPoint)[] values)
        {
            if (item == null) return;
            foreach (var (para, basePoint, growPoint) in values)
            {
                switch (para)
                {
                    case CharParameter.Mhp: item.Mhp = basePoint + rank * growPoint; break;
                    case CharParameter.Mmp: item.Mmp = basePoint + rank * growPoint; break;
                    case CharParameter.Atk: item.Atk = basePoint + rank * growPoint; break;
                    case CharParameter.Def: item.Def = basePoint + rank * growPoint; break;
                    case CharParameter.Mat: item.Mat = basePoint + rank * growPoint; break;
                    case CharParameter.Mdf: item.Mdf = basePoint + rank * growPoint; break;
                    case CharParameter.Agi: item.Agi = basePoint + rank * growPoint; break;
                }
            }
        }
        #endregion
    }
}
