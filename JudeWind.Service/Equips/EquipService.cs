using DataAcxess.ProjectContext;
using DataAcxess.Repository;
using GreenUtility.Extension;
using GreenUtility.Interface;
using JudeWind.Model.Equips;
using static GreenUtility.RPGSetting;

namespace JudeWind.Service.Equips
{
    public class EquipService(ProjectContext projectContext, DecoratorRepository decorator, EquipRepository equip) : BaseService(projectContext)
    {
        private readonly EquipRepository _equipRepository = equip;
        private readonly DecoratorRepository _decoratorRepository = decorator;
        const int StatusDecMax = 1, ElementDecMax = 1, GreateElementDecMax = 1, PhysicDecMax = 1, TotalDecMax = 3;

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
        private static EquipOutput Boxing(int numbers, Func<IEquipItem> box)
        {
            EquipOutput _result = new();
            for (int i = 1; i <= numbers; i++)
                _result.Equips.Add(new() { Equip = box.Invoke() });
            return _result;
        }
        /// <summary>  </summary>
        private void DecorateBoxing(ref DecoratorEquipOutput result, DecoratorInfo boxInfo, Func<IEquipItem> box)
        {
            DecoratorBuilder builder;
            for (int i = 1; i <= boxInfo.Numbers; i++)
            {
                DecoratorEquipInfo _equip = new() { Equip = box.Invoke() };
                builder = CreateDecorateBuilder(_equip.Equip, boxInfo);
                _equip.Equip = builder.BuildEquip();
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
            {
                _boxInfo.StatusCount = Math.Clamp(_boxInfo.StatusCount, 0, StatusDecMax);
                _boxInfo.ElementCount = Math.Clamp(_boxInfo.ElementCount, 0, ElementDecMax);
                _boxInfo.PhysicCount = _boxInfo.StatusCount + _boxInfo.ElementCount < TotalDecMax
                   ? Math.Clamp(_boxInfo.PhysicCount, 0, PhysicDecMax) : 0;
                _boxInfo.GreatElementCount = _boxInfo.StatusCount + _boxInfo.PhysicCount < TotalDecMax && _boxInfo.ElementCount <= 0
                   ? Math.Clamp(_boxInfo.GreatElementCount, 0, GreateElementDecMax) : 0;
            }
        }
        /// <summary>  </summary>
        private DecoratorBuilder CreateDecorateBuilder(IEquipItem equip, DecoratorInfo boxInfo)
        {
            DecoratorBuilder builder = new();
            builder.SetEquip(equip);
            for (int j = 1; j <= boxInfo.GreatElementCount; j++)
                builder.AddPrev(_decoratorRepository.GetDecoratorItem(DecoratorRepository.DecoratorType.GreatElement));
            for (int j = 1; j <= boxInfo.ElementCount; j++)
                builder.AddPrev(_decoratorRepository.GetDecoratorItem(DecoratorRepository.DecoratorType.Element));
            for (int j = 1; j <= boxInfo.StatusCount; j++)
                builder.AddPrev(_decoratorRepository.GetDecoratorItem(DecoratorRepository.DecoratorType.Status));
            for (int j = 1; j <= boxInfo.PhysicCount; j++)
                builder.AddNext(_decoratorRepository.GetDecoratorItem(DecoratorRepository.DecoratorType.Physic));
            return builder;
        }
        #endregion
    }
}
