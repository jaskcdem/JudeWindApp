using DataAcxess.Extension;
using GreenUtility;
using GreenUtility.Equip;
using System.ComponentModel;
using static GreenUtility.RPGSetting;

namespace DataAcxess.Repository
{
    public class EquipRepository : ISampleRepository
    {
        #region public member
        /// <summary> 套裝類型 </summary>
        public enum SuitEquipType
        {
            /// <summary> 無 </summary>
            [Description("無")]
            None,
            /// <summary> 盜賊 </summary>
            [Description("盜賊")]
            Thief,
            /// <summary> 戰士 </summary>
            [Description("戰士")]
            Warrior,
            /// <summary> 鬥士 </summary>
            [Description("鬥士")]
            Fighter,
            /// <summary> 野蠻人 </summary>
            [Description("野蠻人")]
            Rober,
            /// <summary> 衛兵 </summary>
            [Description("衛兵")]
            Guardian,
            /// <summary> 步兵 </summary>
            [Description("步兵")]
            Solider,
            /// <summary> 神官 </summary>
            [Description("神官")]
            Priest,
            /// <summary> 女巫 </summary>
            [Description("女巫")]
            Witch,
            /// <summary> 遊俠 </summary>
            [Description("遊俠")]
            Ranger,
            /// <summary> 女戰士 </summary>
            [Description("女戰士")]
            Warrioress,
            /// <summary> 龍騎兵 </summary>
            [Description("龍騎兵")]
            DrangerKight,
            /// <summary> 騎士 </summary>
            [Description("騎士")]
            Kight,
            /// <summary> 破壞者 </summary>
            [Description("破壞者")]
            Destorier,
            /// <summary> 功夫 </summary>
            [Description("功夫")]
            MartialArtist,
            /// <summary> 主教 </summary>
            [Description("主教")]
            Curch,
            /// <summary> 魔法使 </summary>
            [Description("魔法使")]
            Magician,
            /// <summary> 魔戰士 </summary>
            [Description("魔戰士")]
            MagicianWarrior,
            /// <summary> 黃金 </summary>
            [Description("黃金")]
            Golden,
            /// <summary> 狂戰士 </summary>
            [Description("狂戰士")]
            Berserker,
            /// <summary> 美麗 </summary>
            [Description("美麗")]
            Beauty,
            /// <summary> 死神 </summary>
            [Description("死神")]
            Death,
            /// <summary> 武士 </summary>
            [Description("武士")]
            Samurai,
            /// <summary> 勇者 </summary>
            [Description("勇者")]
            Breave,
            /// <summary> 愛麗絲 </summary>
            [Description("愛麗絲")]
            Alice,
            /// <summary> 魅魔 </summary>
            [Description("魅魔")]
            Succubus,
        }
        /// <summary> 稀有度 </summary>
        public enum ItemLevel
        {
            /// <summary> 一般 </summary>
            N,
            /// <summary> 稀有 </summary>
            R,
            /// <summary> 極稀有 </summary>
            SR,
            /// <summary> 史詩 </summary>
            ER,
            /// <summary> 傳說 </summary>
            LR,
            /// <summary> 幻想 </summary>
            UR,
        }
        #endregion

        #region private members
        readonly string[] weaponTypes = Enum.GetNames(typeof(Weapon)),
            armorType = Enum.GetNames(typeof(Armor));
        readonly Dictionary<ItemLevel, int> RarePoint = new()
        {
            { ItemLevel.N, 5 }, { ItemLevel.R, 7 }, { ItemLevel.SR, 9 }, { ItemLevel.ER, 11 }, { ItemLevel.LR, 13 }, { ItemLevel.UR, int.MaxValue }
        };
        readonly List<(Weapon wep, string defName, (CharParameter para, int basePoint, int growPoint)[] values)> WepList
           = [(Weapon.Sword, "N-衛兵之劍", [(CharParameter.Atk, 4, 1), (CharParameter.Def, 1, 1)])
                ,(Weapon.Sword, "R-女戰士西洋劍", [(CharParameter.Atk, 4, 1), (CharParameter.Mat, 2, 1), (CharParameter.Agi, 1, 1)])
                ,(Weapon.Sword, "R-騎士劍", [(CharParameter.Atk, 5, 1), (CharParameter.Mat, 2, 1)])
                ,(Weapon.Sword, "SR-魔法騎士劍", [(CharParameter.Atk, 2, 2), (CharParameter.Mat, 16, 2)])
                ,(Weapon.Sword, "SR-皇家騎士劍", [(CharParameter.Atk, 16, 2), (CharParameter.Mat, 2, 2)])
                ,(Weapon.Sword, "ER-勇者之劍", [(CharParameter.Atk, 11, 1)])
                //,(Weapon.Sword, "LR-", [])
                ,(Weapon.Katana, "SR-魔戰士魔法刀", [(CharParameter.Atk, 3, 1), (CharParameter.Mat, 6, 1)])
                ,(Weapon.Katana, "ER-武士刀", [(CharParameter.Mhp, 24, 8), (CharParameter.Atk, 6, 1), (CharParameter.Mat, 2, 1)])
                //,(Weapon.Katana, "ER-", [])
                //,(Weapon.Katana, "LR-", [])
                //,(Weapon.Rapier, "N-", [])
                //,(Weapon.Rapier, "R-", [])
                //,(Weapon.Rapier, "SR-", [])
                //,(Weapon.Rapier, "ER-", [])
                //,(Weapon.Rapier, "LR-", [])
                ,(Weapon.Dagger, "N-盜賊小刀", [(CharParameter.Atk, 3, 1), (CharParameter.Agi, 2, 1)])
                ,(Weapon.Dagger, "N-遊俠短劍", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 2, 1)])
                //,(Weapon.Dagger, "R-", [])
                //,(Weapon.Dagger, "SR-", [])
                //,(Weapon.Dagger, "ER-", [])
                //,(Weapon.Dagger, "LR-", [])
                ,(Weapon.GreatSword, "N-戰士鬥劍", [(CharParameter.Atk, 3, 1), (CharParameter.Mat, 2, 1)])
                //,(Weapon.GreatSword, "R-", [])
                ,(Weapon.GreatSword, "SR-黃金劍", [(CharParameter.Atk, 5, 1), (CharParameter.Mat, 4, 1)])
                //,(Weapon.GreatSword, "ER-", [])
                //,(Weapon.GreatSword, "LR-", [])
                ,(Weapon.Naginata, "N-驅魔薙刀", [(CharParameter.Atk, 8, 2), (CharParameter.Agi, 2, 2)])
                ,(Weapon.Naginata, "R-水縹", [(CharParameter.Atk, 21, 3)])
                ,(Weapon.Naginata, "R-迦具土", [(CharParameter.Atk, 9, 3), (CharParameter.Mat, 12, 3)])
                //,(Weapon.Naginata, "SR-", [])
                //,(Weapon.Naginata, "ER-", [])
                //,(Weapon.Naginata, "LR-", [])
                //,(Weapon.Dual_wield, "N-", [])
                //,(Weapon.Dual_wield, "R-", [])
                //,(Weapon.Dual_wield, "SR-", [])
                //,(Weapon.Dual_wield, "ER-", [])
                //,(Weapon.Dual_wield, "LR-", [])
                ,(Weapon.Axe, "N-野蠻人斧頭", [(CharParameter.Atk, 5, 1)])
                ,(Weapon.Axe, "R-龍騎兵之斧", [(CharParameter.Atk, 6, 1), (CharParameter.Mat, 1, 1)])
                ,(Weapon.Axe, "SR-狂戰士之斧", [(CharParameter.Atk, 9, 1)])
                //,(Weapon.Axe, "ER-", [])
                //,(Weapon.Axe, "LR-", [])
                ,(Weapon.Hammer, "N-鬥士戰錘", [(CharParameter.Atk, 5, 1)])
                ,(Weapon.Hammer, "R-破壞者之錘", [(CharParameter.Atk, 7, 1)])
                //,(Weapon.Hammer, "SR-", [])
                //,(Weapon.Hammer, "ER-", [])
                //,(Weapon.Hammer, "LR-", [])
                //,(Weapon.Stick, "N-", [])
                //,(Weapon.Stick, "R-", [])
                ,(Weapon.Stick, "SR-美麗指揮棒", [(CharParameter.Mat, 9, 1)])
                //,(Weapon.Stick, "ER-", [])
                //,(Weapon.Stick, "LR-", [])
                ,(Weapon.Mace, "N-神官錘矛", [(CharParameter.Atk, 1, 1), (CharParameter.Mat, 4, 1)])
                //,(Weapon.Mace, "R-", [])
                //,(Weapon.Mace, "SR-", [])
                //,(Weapon.Mace, "ER-", [])
                //,(Weapon.Mace, "LR-", [])
                //,(Weapon.Halberd, "N-", [])
                //,(Weapon.Halberd, "R-", [])
                //,(Weapon.Halberd, "SR-", [])
                //,(Weapon.Halberd, "ER-", [])
                //,(Weapon.Halberd, "LR-", [])
                ,(Weapon.Spear, "N-步兵之槍", [(CharParameter.Atk, 3, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 1, 1)])
                ,(Weapon.Spear, "N-修女十字槍", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 2, 1)])
                //,(Weapon.Spear, "R-", [])
                //,(Weapon.Spear, "SR-", [])
                //,(Weapon.Spear, "ER-", [])
                //,(Weapon.Spear, "LR-", [])
                ,(Weapon.Bow, "N-獵弓", [(CharParameter.Atk, 10, 2)])
                ,(Weapon.Bow, "R-龍骨弓", [(CharParameter.Atk, 14, 2)])
                ,(Weapon.Bow, "R-詩歌之弓", [(CharParameter.Atk, 4, 2), (CharParameter.Mat, 10, 2)])
                ,(Weapon.Bow, "SR-煉鋼弓", [(CharParameter.Atk, 8, 2), (CharParameter.Mat, 8, 2), (CharParameter.Agi, 3, 1)])
                //,(Weapon.Bow, "ER-", [])
                //,(Weapon.Bow, "LR-", [])
                //,(Weapon.Crossbow, "N-", [])
                //,(Weapon.Crossbow, "R-", [])
                //,(Weapon.Crossbow, "SR-", [])
                //,(Weapon.Crossbow, "ER-", [])
                //,(Weapon.Crossbow, "LR-", [])
                //,(Weapon.Gun, "N-", [])
                //,(Weapon.Gun, "R-", [])
                //,(Weapon.Gun, "SR-", [])
                //,(Weapon.Gun, "ER-", [])
                //,(Weapon.Gun, "LR-", [])
                ,(Weapon.Staff, "N-女巫法杖", [(CharParameter.Mat, 5, 1)])
                ,(Weapon.Staff, "R-主教法杖", [(CharParameter.Atk, 2, 1), (CharParameter.Mat, 5, 1)])
                ,(Weapon.Staff, "R-魔法使之杖", [(CharParameter.Atk, 1, 1), (CharParameter.Mat, 6, 1)])
                //,(Weapon.Staff, "SR-", [])
                //,(Weapon.Staff, "ER-", [])
                //,(Weapon.Staff, "LR-", [])
                //,(Weapon.Knuckles, "N-", [])
                //,(Weapon.Knuckles, "R-", [])
                //,(Weapon.Knuckles, "SR-", [])
                //,(Weapon.Knuckles, "ER-", [])
                //,(Weapon.Knuckles, "LR-", [])
                //,(Weapon.Claw, "N-", [])
                ,(Weapon.Claw, "R-功夫爪", [(CharParameter.Atk, 4, 1), (CharParameter.Def, 1, 1), (CharParameter.Agi, 2, 1)])
                //,(Weapon.Claw, "SR-", [])
                //,(Weapon.Claw, "ER-", [])
                //,(Weapon.Claw, "LR-", [])
                //,(Weapon.Scythe, "N-", [])
                //,(Weapon.Scythe, "R-", [])
                ,(Weapon.Scythe, "SR-死神鐮刀", [(CharParameter.Atk, 5, 1), (CharParameter.Mat, 2, 1), (CharParameter.Agi, 2, 1)])
                //,(Weapon.Scythe, "ER-", [])
                //,(Weapon.Scythe, "LR-", [])
                //,(Weapon.Whip, "N-", [])
                //,(Weapon.Whip, "R-", [])
                //,(Weapon.Whip, "SR-", [])
                ,(Weapon.Whip, "ER-愛麗絲之鞭", [(CharParameter.Mmp, 12, 6), (CharParameter.Mat, 7, 1), (CharParameter.Agi, 2, 1)])
                ,(Weapon.Whip, "LR-魅魔之尾", [(CharParameter.Mhp, 16, 8), (CharParameter.Mmp, 12, 6), (CharParameter.Atk, 9, 1)])
               ];
        readonly List<(Armor arm, string defName, (CharParameter para, int basePoint, int growPoint)[] values)> ArmList
            = [(Armor.Clothes, "N-步兵服", [(CharParameter.Def, 2, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.Clothes, "N-神官服", [(CharParameter.Def, 2, 1), (CharParameter.Mat, 2, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Clothes, "N-遊俠外套", [(CharParameter.Def, 3, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.Clothes, "R-龍騎兵內襯衣", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 3, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.Clothes, "R-功夫服", [(CharParameter.Def, 2, 1), (CharParameter.Mat, 2, 1), (CharParameter.Mdf, 2, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.Clothes, "SR-魔戰士魔裝", [(CharParameter.Def, 4, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 4, 1)])
                ,(Armor.Clothes, "SR-狂戰士襯衣", [(CharParameter.Atk, 3, 1), (CharParameter.Def, 4, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.Clothes, "SR-死神的外套", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 4, 1), (CharParameter.Mat, 2, 1), (CharParameter.Agi, 2, 1)])
                ,(Armor.Clothes, "SR-美麗外套", [(CharParameter.Def, 1, 1), (CharParameter.Mat, 5, 1), (CharParameter.Mdf, 3, 1)])
                ,(Armor.Clothes, "ER-愛麗絲禮服", [(CharParameter.Mmp, 18, 6), (CharParameter.Def, 4, 1), (CharParameter.Mdf, 4, 1)])
                ,(Armor.Clothes, "LR-魅魔服", [(CharParameter.Mhp, 16, 8), (CharParameter.Mmp, 12, 6), (CharParameter.Def, 9, 1)])
                ,(Armor.Robes, "N-女巫長袍", [(CharParameter.Def, 1, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 3, 1)])
                ,(Armor.Robes, "R-主教長袍", [(CharParameter.Def, 2, 1), (CharParameter.Mat, 3, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.Robes, "R-魔法使長袍", [(CharParameter.Def, 2, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 4, 1)])
                ,(Armor.Robes, "SR-元素袍", [(CharParameter.Mmp, 30, 6), (CharParameter.Def, 4, 2), (CharParameter.Mdf, 4, 2)])
                ,(Armor.Robes, "SR-棉布長袍", [(CharParameter.Mhp, 40, 8), (CharParameter.Def, 4, 2), (CharParameter.Mdf, 4, 2)])
                ,(Armor.Robes, "ER-百花袍", [(CharParameter.Mhp, 40, 8), (CharParameter.Mmp, 30, 6), (CharParameter.Def, 0, 2), (CharParameter.Mdf, 0, 2), (CharParameter.Agi, 6, 2)])
                //,(Armor.Robes, "LR-", [])
                ,(Armor.LightArmor, "N-盜賊胸甲", [(CharParameter.Def, 3, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.LightArmor, "N-衛兵鎧甲", [(CharParameter.Mat, 2, 1), (CharParameter.Mdf, 2, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.LightArmor, "N-戰士鎖子甲", [(CharParameter.Def, 2, 1), (CharParameter.Mdf, 3, 1)])
                ,(Armor.LightArmor, "R-女戰士盔甲", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 3, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.LightArmor, "R-功夫套環", [(CharParameter.Atk, 3, 1), (CharParameter.Def, 2, 1), (CharParameter.Mdf, 2, 1)])
                //,(Armor.LightArmor, "SR-", [])
                ,(Armor.LightArmor, "ER-武士甲冑", [(CharParameter.Mhp, 16, 8), (CharParameter.Atk, 3, 1), (CharParameter.Def, 3, 1), (CharParameter.Mdf, 3, 1)])
                //,(Armor.LightArmor, "LR-", [])
                ,(Armor.HeavyArmor, "N-野蠻人鎧甲", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 2, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.HeavyArmor, "N-鬥士板甲", [(CharParameter.Def, 2, 1), (CharParameter.Mdf, 2, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.HeavyArmor, "R-破壞者鎧甲", [(CharParameter.Def, 7, 1)])
                ,(Armor.HeavyArmor, "R-騎士板甲", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 4, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.HeavyArmor, "SR-黃金盔甲", [(CharParameter.Def, 7, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.HeavyArmor, "ER-勇者鎧甲", [(CharParameter.Def, 5, 1), (CharParameter.Mdf, 6, 1)])
                //,(Armor.HeavyArmor, "LR-", [])
                ,(Armor.SmallSheld, "N-盜賊盾牌", [(CharParameter.Def, 1, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 2, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.SmallSheld, "N-步兵圓盾", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 2, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.SmallSheld, "N-鬥士盾牌", [(CharParameter.Def, 3, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.SmallSheld, "N-神官盾", [(CharParameter.Mat, 2, 1), (CharParameter.Mdf, 3, 1)])
                ,(Armor.SmallSheld, "N-女巫之書", [(CharParameter.Atk, 3, 1), (CharParameter.Mat, 2, 1)])
                ,(Armor.SmallSheld, "N-遊俠之盾", [(CharParameter.Def, 1, 1), (CharParameter.Agi, 4, 1)])
                ,(Armor.SmallSheld, "R-女戰士之盾", [(CharParameter.Def, 3, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 2, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.SmallSheld, "R-主教護符盾", [(CharParameter.Mdf, 7, 1)])
                ,(Armor.SmallSheld, "R-魔法使之書", [(CharParameter.Mat, 7, 1)])
                ,(Armor.SmallSheld, "SR-死神之羽", [(CharParameter.Atk, 2, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 4, 1), (CharParameter.Agi, 2, 1)])
                ,(Armor.SmallSheld, "SR-美麗提包", [(CharParameter.Def, 2, 1), (CharParameter.Mdf, 7, 1)])
                ,(Armor.SmallSheld, "ER-武士小太刀", [(CharParameter.Mhp, 32, 8), (CharParameter.Atk, 4, 1), (CharParameter.Def, 3, 1)])
                ,(Armor.SmallSheld, "ER-愛麗絲長襪", [(CharParameter.Mmp, 18, 6), (CharParameter.Def, 3, 1), (CharParameter.Mdf, 3, 1), (CharParameter.Agi, 2, 1)])
                //,(Armor.SmallSheld, "LR-", [])
                ,(Armor.BigSheld, "N-衛兵盾牌", [(CharParameter.Def, 5, 1)])
                ,(Armor.BigSheld, "N-戰士鐵盾", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.BigSheld, "R-破壞者之盾", [(CharParameter.Mdf, 7, 1)])
                ,(Armor.BigSheld, "SR-狂戰士護盾", [(CharParameter.Atk, 3, 1), (CharParameter.Def, 3, 1), (CharParameter.Mdf, 3, 1)])
                ,(Armor.BigSheld, "SR-黃金盾", [(CharParameter.Def, 9, 1)])
                //,(Armor.BigSheld, "ER-", [])
                //,(Armor.BigSheld, "LR-", [])
                ,(Armor.KitSheld, "N-野蠻人盾牌", [(CharParameter.Atk, 3, 1), (CharParameter.Def, 1, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.KitSheld, "R-龍騎兵護盾", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 3, 1), (CharParameter.Mdf, 3, 1)])
                ,(Armor.KitSheld, "R-騎士之盾", [(CharParameter.Def, 7, 1)])
                ,(Armor.KitSheld, "SR-魔戰士鏡盾", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 4, 1), (CharParameter.Mdf, 4, 1)])
                ,(Armor.KitSheld, "ER-勇者之盾", [(CharParameter.Mdf, 11, 1)])
                ,(Armor.KitSheld, "LR-魅魔之翼", [(CharParameter.Mhp, 16, 8), (CharParameter.Mmp, 12, 6), (CharParameter.Mat, 9, 1)])
                ,(Armor.Hat, "N-盜賊帽子", [(CharParameter.Def, 1, 1), (CharParameter.Mat, 2, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Hat, "N-戰士鏈甲兜帽", [(CharParameter.Def, 2, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.Hat, "N-神官帽子", [(CharParameter.Mdf, 5, 1)])
                ,(Armor.Hat, "N-女巫帽", [(CharParameter.Atk, 1, 1), (CharParameter.Mat, 4, 1)])
                ,(Armor.Hat, "N-遊俠帽子", [(CharParameter.Def, 1, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 3, 1)])
                ,(Armor.Hat, "R-女戰士頭飾", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 2, 1), (CharParameter.Mat, 2, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.Hat, "R-功夫頭巾", [(CharParameter.Def, 2, 1), (CharParameter.Mdf, 2, 1), (CharParameter.Agi, 3, 1)])
                ,(Armor.Hat, "R-主教頭飾", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 2, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 2, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.Hat, "R-魔法使高帽", [(CharParameter.Mat, 5, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.Hat, "SR-狂戰士面具", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 2, 1), (CharParameter.Mat, 2, 1), (CharParameter.Mdf, 2, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.Hat, "SR-黃金面具", [(CharParameter.Atk, 3, 1), (CharParameter.Def, 3, 1), (CharParameter.Mdf, 3, 1)])
                ,(Armor.Hat, "SR-美麗兜帽", [(CharParameter.Mat, 6, 1), (CharParameter.Mdf, 3, 1)])
                ,(Armor.Hat, "ER-愛麗絲緞帶", [(CharParameter.Mmp, 18, 6), (CharParameter.Def, 2, 1), (CharParameter.Mat, 4, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.Hat, "LR-魅魔之角", [(CharParameter.Mhp, 16, 8), (CharParameter.Mmp, 12, 6), (CharParameter.Mdf, 9, 1)])
                ,(Armor.Hat, "N-貝雷帽", [(CharParameter.Mhp, 16, 8), (CharParameter.Mmp, 12, 6), (CharParameter.Def, 1, 1), (CharParameter.Mdf, 0, 1)])
                ,(Armor.Hat, "N-蝴蝶緞帶", [(CharParameter.Mhp, 40, 8), (CharParameter.Mdf, 0, 1)])
                ,(Armor.Hat, "N-羽毛帽", [(CharParameter.Mmp, 30, 6), (CharParameter.Def, 0, 1)])
                ,(Armor.Helmet, "N-步兵頭盔", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 2, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Helmet, "N-野蠻人護額", [(CharParameter.Def, 3, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Helmet, "N-鬥士頭盔", [(CharParameter.Def, 2, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 2, 1)])
                ,(Armor.Helmet, "N-衛兵鐵盔", [(CharParameter.Def, 2, 1), (CharParameter.Mdf, 3, 1)])
                ,(Armor.Helmet, "R-龍騎兵頭盔", [(CharParameter.Def, 3, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 3, 1)])
                ,(Armor.Helmet, "R-破壞者頭盔", [(CharParameter.Def, 5, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.Helmet, "R-騎士頭盔", [(CharParameter.Def, 3, 1), (CharParameter.Mdf, 3, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.Helmet, "SR-魔戰士頭冠", [(CharParameter.Atk, 4, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 4, 1)])
                ,(Armor.Helmet, "SR-死神的冠冕", [(CharParameter.Atk, 1, 1), (CharParameter.Mat, 3, 1), (CharParameter.Mdf, 3, 1), (CharParameter.Agi, 2, 1)])
                ,(Armor.Helmet, "ER-武士頭盔", [(CharParameter.Mhp, 8, 8), (CharParameter.Atk, 4, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 4, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Helmet, "ER-勇者頭盔", [(CharParameter.Def, 7, 1), (CharParameter.Mdf, 4, 1)])
                //,(Armor.Helmet, "LR-", [])
                ,(Armor.Gloves, "N-盜賊手套", [(CharParameter.Atk, 2, 1), (CharParameter.Mat, 2, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.Gloves, "N-步兵手套", [(CharParameter.Atk, 3, 1), (CharParameter.Def, 2, 1)])
                ,(Armor.Gloves, "N-野蠻人臂甲", [(CharParameter.Atk, 3, 1), (CharParameter.Def, 1, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Gloves, "N-鬥士手甲", [(CharParameter.Atk, 3, 1), (CharParameter.Def, 1, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Gloves, "N-衛兵臂甲", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 2, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.Gloves, "N-戰士鏈甲手套", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 2, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Gloves, "N-神官手套", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.Gloves, "N-女巫手套", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 2, 1)])
                ,(Armor.Gloves, "N-遊俠手套", [(CharParameter.Atk, 2, 1), (CharParameter.Mat, 2, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.Gloves, "R-女戰士手套", [(CharParameter.Atk, 3, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 2, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Gloves, "R-龍騎兵手甲", [(CharParameter.Atk, 4, 1), (CharParameter.Mat, 3, 1)])
                ,(Armor.Gloves, "R-破壞者手甲", [(CharParameter.Def, 2, 1), (CharParameter.Mdf, 5, 1)])
                ,(Armor.Gloves, "R-騎士臂鎧", [(CharParameter.Atk, 3, 1), (CharParameter.Def, 2, 1), (CharParameter.Mat, 2, 1)])
                ,(Armor.Gloves, "R-功夫綁手帶", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 2, 1), (CharParameter.Mat, 1, 1), (CharParameter.Agi, 2, 1)])
                ,(Armor.Gloves, "R-主教手套", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 2, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.Gloves, "R-魔法使手套", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 2, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.Gloves, "SR-魔戰士護手", [(CharParameter.Atk, 4, 1), (CharParameter.Mat, 4, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Gloves, "SR-狂戰士之腕", [(CharParameter.Atk, 6, 1), (CharParameter.Def, 3, 1)])
                ,(Armor.Gloves, "SR-死神之手", [(CharParameter.Atk, 3, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 2, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 2, 1)])
                ,(Armor.Gloves, "SR-黃金之手", [(CharParameter.Atk, 3, 1), (CharParameter.Mdf, 3, 1), (CharParameter.Agi, 3, 1)])
                ,(Armor.Gloves, "SR-美麗手甲", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 2, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 3, 1)])
                ,(Armor.Gloves, "ER-武士之鏝", [(CharParameter.Mhp, 24, 8), (CharParameter.Atk, 2, 1), (CharParameter.Def, 2, 1), (CharParameter.Mat, 2, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.Gloves, "ER-勇者臂甲", [(CharParameter.Atk, 5, 1), (CharParameter.Def, 5, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Gloves, "ER-愛麗絲長手套", [(CharParameter.Mat, 11, 1)])
                ,(Armor.Gloves, "LR-魅魔之戒", [(CharParameter.Mhp, 16, 8), (CharParameter.Mmp, 12, 6), (CharParameter.Atk, 2, 1), (CharParameter.Def, 2, 1), (CharParameter.Mat, 2, 1), (CharParameter.Mdf, 2, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.Gloves, "UR-雪乃手套", [(CharParameter.Mhp, 80, 10), (CharParameter.Mmp, 60, 10)])
                ,(Armor.Gloves, "UR-狩獵手套", [(CharParameter.Atk, 5, 5), (CharParameter.Mat, 0, 7)])
                ,(Armor.Gloves, "UR-猛鷲手套", [(CharParameter.Def, 5, 5), (CharParameter.Mdf, 0, 7)])
                ,(Armor.Shoes, "N-盜賊靴子", [(CharParameter.Def, 2, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.Shoes, "N-步兵靴子", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 1, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.Shoes, "N-野蠻人長靴", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Shoes, "N-鬥士脛甲", [(CharParameter.Def, 1, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 3, 1)])
                ,(Armor.Shoes, "N-衛兵鐵靴", [(CharParameter.Agi, 5, 1)])
                ,(Armor.Shoes, "N-戰士靴子", [(CharParameter.Atk, 2, 1), (CharParameter.Agi, 3, 1)])
                ,(Armor.Shoes, "N-神官靴子", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 2, 1), (CharParameter.Mat, 2, 1)])
                ,(Armor.Shoes, "N-女巫鞋", [(CharParameter.Def, 2, 1), (CharParameter.Mat, 2, 1), (CharParameter.Mdf, 1, 1)])
                ,(Armor.Shoes, "N-遊俠長靴", [(CharParameter.Atk, 1, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 3, 1)])
                ,(Armor.Shoes, "R-女戰士長靴", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 2, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 1, 1)])
                ,(Armor.Shoes, "R-龍騎兵長靴", [(CharParameter.Atk, 2, 1), (CharParameter.Mat, 2, 1), (CharParameter.Agi, 3, 1)])
                ,(Armor.Shoes, "R-破壞者足當", [(CharParameter.Agi, 7, 1)])
                ,(Armor.Shoes, "R-騎士護腿", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 3, 1)])
                ,(Armor.Shoes, "R-功夫鞋", [(CharParameter.Atk, 2, 1), (CharParameter.Mdf, 3, 1), (CharParameter.Agi, 2, 1)])
                ,(Armor.Shoes, "R-主教靴子", [(CharParameter.Def, 2, 1), (CharParameter.Mdf, 2, 1), (CharParameter.Agi, 3, 1)])
                ,(Armor.Shoes, "R-魔法使短靴", [(CharParameter.Def, 2, 1), (CharParameter.Mdf, 3, 1), (CharParameter.Agi, 2, 1)])
                ,(Armor.Shoes, "SR-魔戰士靴子", [(CharParameter.Atk, 2, 1), (CharParameter.Mat, 2, 1), (CharParameter.Agi, 5, 1)])
                ,(Armor.Shoes, "SR-狂戰士長靴", [(CharParameter.Atk, 4, 1), (CharParameter.Def, 2, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 2, 1)])
                ,(Armor.Shoes, "SR-死神之腳", [(CharParameter.Atk, 2, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 3, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 2, 1)])
                ,(Armor.Shoes, "SR-黃金護腿", [(CharParameter.Def, 3, 1), (CharParameter.Mat, 3, 1), (CharParameter.Agi, 3, 1)])
                ,(Armor.Shoes, "SR-美麗長靴", [(CharParameter.Atk, 1, 1), (CharParameter.Def, 1, 1), (CharParameter.Mat, 1, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 5, 1)])
                ,(Armor.Shoes, "ER-武士足袋", [(CharParameter.Mhp, 16, 8), (CharParameter.Agi, 9, 1)])
                ,(Armor.Shoes, "ER-勇者之靴", [(CharParameter.Atk, 3, 1), (CharParameter.Def, 4, 1), (CharParameter.Mdf, 2, 1), (CharParameter.Agi, 2, 1)])
                ,(Armor.Shoes, "ER-愛麗絲皮鞋", [(CharParameter.Mmp, 12, 6), (CharParameter.Atk, 2, 1), (CharParameter.Def, 2, 1), (CharParameter.Mdf, 5, 1)])
                ,(Armor.Shoes, "LR-魅魔之靴", [(CharParameter.Mhp, 16, 8), (CharParameter.Mmp, 12, 6), (CharParameter.Agi, 9, 1)])
                ,(Armor.Shoes, "UR-雪乃長靴", [(CharParameter.Def, 1, 1), (CharParameter.Mdf, 1, 1), (CharParameter.Agi, 6, 3)])
                ,(Armor.Shoes, "UR-獸毛鞋", [(CharParameter.Def, 1, 1), (CharParameter.Agi, 9, 3)])
                ,(Armor.Shoes, "UR-肉球皮鞋", [(CharParameter.Mdf, 1, 1), (CharParameter.Agi, 8, 4), ])
                ];

        readonly List<(SuitEquipType type, string[] names)> SuitEquipList = [
            (SuitEquipType.Thief, ["N-盜賊小刀", "N-盜賊胸甲", "N-盜賊盾牌", "N-盜賊帽子", "N-盜賊手套", "N-盜賊靴子"]),
            (SuitEquipType.Warrior, ["N-戰士鬥劍", "N-戰士鎖子甲", "N-戰士鐵盾", "N-戰士鏈甲兜帽", "N-戰士鏈甲手套", "N-戰士靴子"]),
            (SuitEquipType.Rober, ["N-野蠻人斧頭", "N-野蠻人鎧甲", "N-野蠻人盾牌", "N-野蠻人護額", "N-野蠻人臂甲", "N-野蠻人長靴"]),
            (SuitEquipType.Fighter, ["N-鬥士戰錘", "N-鬥士板甲", "N-鬥士盾牌", "N-鬥士頭盔", "N-鬥士手甲", "N-鬥士脛甲"]),
            (SuitEquipType.Guardian, ["N-衛兵之劍", "N-衛兵鎧甲", "N-衛兵盾牌", "N-衛兵鐵盔", "N-衛兵臂甲", "N-衛兵鐵靴"]),
            (SuitEquipType.Solider, ["N-步兵之槍", "N-步兵服", "N-步兵圓盾", "N-步兵頭盔", "N-步兵手套", "N-步兵靴子"]),
            (SuitEquipType.Priest, ["N-神官錘矛", "N-神官服", "N-神官盾", "N-神官帽子", "N-神官手套", "N-神官靴子"]),
            (SuitEquipType.Witch, ["N-女巫法杖", "N-女巫長袍", "N-女巫之書", "N-女巫帽", "N-女巫手套", "N-女巫鞋"]),
            (SuitEquipType.Ranger, ["N-遊俠短劍", "N-遊俠外套", "N-遊俠之盾", "N-遊俠帽子", "N-遊俠手套", "N-遊俠長靴"]),
            (SuitEquipType.Warrioress, ["R-女戰士西洋劍", "R-女戰士盔甲", "R-女戰士之盾", "R-女戰士頭飾", "R-女戰士手套", "R-女戰士長靴"]),
            (SuitEquipType.DrangerKight, ["R-龍騎兵之斧", "R-龍騎兵內襯衣", "R-龍騎兵護盾", "R-龍騎兵頭盔", "R-龍騎兵手甲", "R-龍騎兵長靴"]),
            (SuitEquipType.Kight, ["R-騎士劍", "R-騎士板甲", "R-騎士之盾", "R-騎士頭盔", "R-騎士臂鎧", "R-騎士護腿"]),
            (SuitEquipType.Destorier, ["R-破壞者之錘", "R-破壞者鎧甲", "R-破壞者之盾", "R-破壞者頭盔", "R-破壞者手甲", "R-破壞者足當"]),
            (SuitEquipType.MartialArtist, ["R-功夫爪", "R-功夫服", "R-功夫套環", "R-功夫頭巾", "R-功夫綁手帶", "R-功夫鞋"]),
            (SuitEquipType.Curch, ["R-主教法杖", "R-主教長袍", "R-主教護符盾", "R-主教頭飾", "R-主教手套", "R-主教靴子"]),
            (SuitEquipType.Magician, ["R-魔法使之杖", "R-魔法使長袍", "R-魔法使之書", "R-魔法使高帽", "R-魔法使手套", "R-魔法使短靴"]),
            (SuitEquipType.MagicianWarrior, ["SR-魔戰士魔法刀", "SR-魔戰士魔裝", "SR-魔戰士鏡盾", "SR-魔戰士頭冠", "SR-魔戰士護手", "SR-魔戰士靴子"]),
            (SuitEquipType.Golden, ["SR-黃金劍", "SR-黃金盔甲", "SR-黃金盾", "SR-黃金面具", "SR-黃金之手", "SR-黃金護腿"]),
            (SuitEquipType.Berserker, ["SR-狂戰士之斧", "SR-狂戰士襯衣", "SR-狂戰士護盾", "SR-狂戰士面具", "SR-狂戰士之腕", "SR-狂戰士長靴"]),
            (SuitEquipType.Beauty, ["SR-美麗指揮棒", "SR-美麗外套", "SR-美麗提包", "SR-美麗兜帽", "SR-美麗手甲", "SR-美麗長靴"]),
            (SuitEquipType.Death, ["SR-死神鐮刀", "SR-死神的外套", "SR-死神之羽", "SR-死神的冠冕", "SR-死神之手", "SR-死神之腳"]),
            (SuitEquipType.Samurai, ["ER-武士刀", "ER-武士甲冑", "ER-武士小太刀", "ER-武士頭盔", "ER-武士之鏝", "ER-武士足袋"]),
            (SuitEquipType.Breave, ["ER-勇者之劍", "ER-勇者鎧甲", "ER-勇者之盾", "ER-勇者頭盔", "ER-勇者臂甲", "ER-勇者之靴"]),
            (SuitEquipType.Alice, ["ER-愛麗絲之鞭", "ER-愛麗絲禮服", "ER-愛麗絲長襪", "ER-愛麗絲緞帶", "ER-愛麗絲長手套", "ER-愛麗絲皮鞋"]),
            (SuitEquipType.Succubus, ["LR-魅魔之尾", "LR-魅魔服", "LR-魅魔之翼", "LR-魅魔之角", "LR-魅魔之戒", "LR-魅魔之靴"]),
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

        public List<BaseEquip> GetSuit(SuitEquipType sType)
        {
            var equips = CreateSuit(sType);
            if (equips.Count != 0)
            {
                BaseEquip total = new(equips.First().Rank, $"{sType.GetEnumDescription()}總和")
                {
                    Mhp = equips.Sum(x => x.Mhp),
                    Mmp = equips.Sum(x => x.Mmp),
                    Atk = equips.Sum(x => x.Atk),
                    Def = equips.Sum(x => x.Def),
                    Mat = equips.Sum(x => x.Mat),
                    Mdf = equips.Sum(x => x.Mdf),
                    Agi = equips.Sum(x => x.Agi)
                };
                equips.Add(total);
            }
            return equips;
        }
        #endregion

        #region <-- Factory -->
        #region Create
        BaseEquip CreateWeapon(Weapon etype)
        {
            short rank = this.GetRandomRank();
            var query = WepList.Where(w => w.wep == etype);
            if (!query.Any()) return new BaseEquip(rank);
            var (wep, defName, values) = query.ElementAt(Utility.RandomInt(query.Count()));
            defName = GetNameWithoutRank(defName);
            BaseEquip item = InitWeapon(wep, rank, defName);
            SetEquip(item, rank, values);
            item.Note = defName;
            return item;
        }
        BaseEquip CreateArmor(Armor etype)
        {
            short rank = this.GetRandomRank();
            var query = ArmList.Where(w => w.arm == etype);
            if (!query.Any()) return new BaseEquip(rank);
            var (arm, defName, values) = query.ElementAt(Utility.RandomInt(query.Count()));
            defName = GetNameWithoutRank(defName);
            BaseEquip item = InitArmor(arm, rank, defName);
            SetEquip(item, rank, values);
            item.Note = defName;
            return item;
        }
        List<BaseEquip> CreateSuit(SuitEquipType sType)
        {
            List<BaseEquip> equips = [];
            if (sType == SuitEquipType.None) return equips;
            var (type, names) = SuitEquipList.FirstOrDefault(s => s.type == sType);
            short rank = this.GetRandomRank();
            var wepQuery = WepList.Where(w => w.defName == names[0]);
            if (wepQuery.Any())
            {
                var (wep, defName, values) = wepQuery.ElementAt(Utility.RandomInt(wepQuery.Count()));
                defName = GetNameWithoutRank(defName);
                BaseEquip item = InitWeapon(wep, rank, defName);
                SetEquip(item, rank, values);
                item.Note = defName;
                equips.Add(item);
            }
            for (int i = 1; i < names.Length; i++)
            {
                var armQuery = ArmList.Where(a => a.defName == names[i]);
                if (armQuery.Any())
                {
                    var (arm, defName, values) = armQuery.ElementAt(Utility.RandomInt(armQuery.Count()));
                    defName = GetNameWithoutRank(defName);
                    BaseEquip item = InitArmor(arm, rank, defName);
                    SetEquip(item, rank, values);
                    item.Note = defName;
                    equips.Add(item);
                }
            }
            return equips;
        }
        #endregion

        #region Init
        private static BaseEquip InitWeapon(Weapon etype, short rank, string defName) => etype switch
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
        private static BaseEquip InitArmor(Armor etype, short rank, string defName) => etype switch
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
        #endregion

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
        /// <summary> Spilt and get name from fromat '{Rank}-{Name}' </summary>
        private static string GetNameWithoutRank(string name) => name.Split('-').Length > 1 ? name.Split('-')[1] : name;
        #endregion

        private int CountTotalPoint((int basePoint, int growPoint)[] values) => values.Sum(x => x.growPoint != 0 ? x.basePoint / x.growPoint : 0);
    }
}
