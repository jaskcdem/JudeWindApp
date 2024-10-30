using DataAcxess.Extension;
using GreenUtility;
using GreenUtility.Equip;
using GreenUtility.Interface;
using static GreenUtility.RPGSetting;

namespace DataAcxess.Repository
{
    public class EquipRepository : ISampleRepository
    {
        #region private members
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

        Dictionary<Weapon, Type> _wepDic = null!;
        internal Dictionary<Weapon, Type> WepDic => _wepDic ?? InitWepDic();
        Dictionary<Armor, Type> _armDic = null!;
        internal Dictionary<Armor, Type> ArmDic => _armDic ?? InitAmrDic();
        #endregion

        #region methods
        public IEquipItem GetRandomEquip(ShopType shopType)
        {
            IEquipItem item = shopType switch
            {
                ShopType.Weapon => CreateWeapon(WepDic.ElementAt(Utility.RandomInt(WepDic.Count))),
                ShopType.Armor => CreateArmor(ArmDic.ElementAt(Utility.RandomInt(ArmDic.Count))),
                _ => Utility.RandomInt(2) switch
                {
                    1 => CreateArmor(ArmDic.ElementAt(Utility.RandomInt(ArmDic.Count))),
                    _ => CreateWeapon(WepDic.ElementAt(Utility.RandomInt(WepDic.Count))),
                },
            };
            return item ?? throw new ArgumentNullException(message: "Undefind Equip Type", paramName: "GetRandomEquip");
        }
        public IEquipItem GetWeapon(Weapon etype) => CreateWeapon(new KeyValuePair<Weapon, Type>(etype, WepDic[etype]));
        public IEquipItem GetArmor(Armor etype) => CreateArmor(new KeyValuePair<Armor, Type>(etype, ArmDic[etype]));
        #endregion

        #region <-- Factory -->
        Dictionary<Weapon, Type> InitWepDic()
        {
            _wepDic = [];
            _wepDic.Add(Weapon.Sword, typeof(BaseSword));
            _wepDic.Add(Weapon.Naginata, typeof(BaseNaginata));
            _wepDic.Add(Weapon.Bow, typeof(BaseBow));
            return _wepDic;
        }
        Dictionary<Armor, Type> InitAmrDic()
        {
            _armDic = [];
            _armDic.Add(Armor.Hat, typeof(BaseHat));
            _armDic.Add(Armor.Robes, typeof(BaseRobe));
            _armDic.Add(Armor.Shoes, typeof(BaseShoes));
            _armDic.Add(Armor.Gloves, typeof(BaseGloves));
            return _armDic;
        }
        IEquipItem CreateWeapon(KeyValuePair<Weapon, Type> etype)
        {
            IEquipItem item = null!;
            short rank = this.GetRandomRank();
            var query = WepList.Where(w => w.wep == etype.Key);
            var (wep, defName, values) = query.ElementAt(Utility.RandomInt(query.Count()));
            switch (etype.Key)
            {
                case Weapon.Sword: item = new BaseSword(rank, defName); break;
                case Weapon.Naginata: item = new BaseNaginata(rank, defName); break;
                case Weapon.Bow: item = new BaseBow(rank, defName); break;
            }
            SetEquip(item, rank, values);
            item.Note = defName;
            return item;
        }
        IEquipItem CreateArmor(KeyValuePair<Armor, Type> etype)
        {
            IEquipItem item = null!;
            var query = ArmList.Where(w => w.amr == etype.Key);
            var (wep, defName, values) = query.ElementAt(Utility.RandomInt(query.Count()));
            short rank = this.GetRandomRank();
            switch (etype.Key)
            {
                case Armor.Hat: item = new BaseHat(rank, defName); break;
                case Armor.Robes: item = new BaseRobe(rank, defName); break;
                case Armor.Shoes: item = new BaseShoes(rank, defName); break;
                case Armor.Gloves: item = new BaseGloves(rank, defName); break;
            }
            SetEquip(item, rank, values);
            item.Note = defName;
            return item;
        }

        static void SetEquip(IEquipItem item, short rank, (CharParameter para, int basePoint, int growPoint)[] values)
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
