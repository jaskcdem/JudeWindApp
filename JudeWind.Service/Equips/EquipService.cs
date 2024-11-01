using DataAcxess.ProjectContext;
using DataAcxess.Repository;
using GreenUtility.Equip;
using GreenUtility.Extension;
using JudeWind.Model.Base;
using JudeWind.Model.Equips;
using JudeWind.Service.Extension;
using static GreenUtility.RPGSetting;

namespace JudeWind.Service.Equips
{
    public class EquipService(ProjectContext projectContext, DecoratorRepository decorator, EquipRepository equip) : BaseService(projectContext)
    {
        private readonly EquipRepository _equipRepository = equip;
        private readonly DecoratorRepository _decoratorRepository = decorator;

        #region methods
        /// <summary> 裝備箱 </summary>
        public EquipOutput RandomEquipBox(EquipRandomInput input) => input.Weapon.HasValue && !input.Armor.HasValue
                ? Boxing(input.Numbers, () => _equipRepository.GetWeapon(input.Weapon.Value))
                : !input.Weapon.HasValue && input.Armor.HasValue
                ? Boxing(input.Numbers, () => _equipRepository.GetArmor(input.Armor.Value))
                : Boxing(input.Numbers, () => _equipRepository.GetRandomEquip(ShopType.Equip));
        /// <summary> 武器箱 </summary>
        public EquipOutput RandomWeaponBox(EquipRandomInput input) => Boxing(input.Numbers, () => _equipRepository.GetRandomEquip(ShopType.Weapon));
        /// <summary> 防具箱 </summary>
        public EquipOutput RandomArmorBox(EquipRandomInput input) => Boxing(input.Numbers, () => _equipRepository.GetRandomEquip(ShopType.Armor));

        /// <summary> 彩蛋裝備箱 </summary>
        public DecoratorEquipOutput RandomDecEquipBox(DecoratorEquipInput input)
        {
            DecoratorEquipOutput _result = new();
            LimitDecorateCount(input);
            foreach (var _boxInfo in input.DecorateBox)
            {
                if (_boxInfo.Weapon.HasValue && !_boxInfo.Armor.HasValue)
                    DecorateBoxing(ref _result, _boxInfo, () => _equipRepository.GetWeapon(_boxInfo.Weapon.Value));
                else if (!_boxInfo.Weapon.HasValue && _boxInfo.Armor.HasValue)
                    DecorateBoxing(ref _result, _boxInfo, () => _equipRepository.GetArmor(_boxInfo.Armor.Value));
                else
                    DecorateBoxing(ref _result, _boxInfo, () => _equipRepository.GetRandomEquip(ShopType.Equip));
            }
            return _result;
        }
        /// <summary> 彩蛋武器箱 </summary>
        public DecoratorEquipOutput RandomDecWeaponBox(DecoratorEquipInput input)
        {
            DecoratorEquipOutput _result = new();
            LimitDecorateCount(input);
            foreach (var _boxInfo in input.DecorateBox)
            {
                DecorateBoxing(ref _result, _boxInfo, () => _equipRepository.GetRandomEquip(ShopType.Weapon));
            }
            return _result;
        }
        /// <summary> 彩蛋防具箱 </summary>
        public DecoratorEquipOutput RandomDecArmorBox(DecoratorEquipInput input)
        {
            DecoratorEquipOutput _result = new();
            LimitDecorateCount(input);
            foreach (var _boxInfo in input.DecorateBox)
            {
                DecorateBoxing(ref _result, _boxInfo, () => _equipRepository.GetRandomEquip(ShopType.Armor));
            }
            return _result;
        }
        #endregion

        #region private method
        /// <summary>  </summary>
        private static EquipOutput Boxing(int numbers, Func<BaseEquip> box)
        {
            EquipOutput _result = new();
            for (int i = 1; i <= numbers; i++)
                _result.Equips.Add(new() { Equip = box.Invoke() });
            return _result;
        }
        /// <summary>  </summary>
        private void DecorateBoxing(ref DecoratorEquipOutput result, DecoratorEquipBoxInfo boxInfo, Func<BaseEquip> box)
        {
            DecoratorBuilder builder;
            for (int i = 1; i <= boxInfo.Numbers; i++)
            {
                DecoratorEquipInfo _equip = new() { Equip = box.Invoke() };
                builder = CreateDecorateBuilder(_equip.Equip, boxInfo);
                _equip.Equip = (BaseEquip)builder.BuildEquip();
                _equip.UnhealthyStatuses = builder.GetUnhealthyStatuses();
                _equip.Elements = builder.GetElements();
                _equip.GreatElements = builder.GetGreatElements();
                _equip.PhysicTypes = builder.GetPhysics();
                result.Equips.Add(_equip);
            }
        }
        /// <summary>  </summary>
        private static void LimitDecorateCount(DecoratorEquipInput input)
        {
            foreach (var _boxInfo in input.DecorateBox)
                ServiceExtension.LimitDecorateCount(_boxInfo);
        }
        /// <summary>  </summary>
        private DecoratorBuilder CreateDecorateBuilder(BaseEquip equip, DecoratorEquipBoxInfo boxInfo)
        {
            DecoratorBuilder builder = new();
            builder.SetEquip(equip);
            ServiceExtension.ImportDecorateBuilderItem(builder, _decoratorRepository, boxInfo);
            return builder;
        }
        #endregion
    }
}
