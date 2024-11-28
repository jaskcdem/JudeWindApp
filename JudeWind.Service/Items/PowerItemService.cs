using DataAcxess.ProjectContext;
using DataAcxess.Repository;
using GreenUtility;
using GreenUtility.Item;
using GreenUtility.Potion;
using JudeWind.Model.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudeWind.Service.Items
{
    public class PowerItemService(ProjectContext projectContext, PowerItemRepository powerItem) : BaseService(projectContext)
    {
        private readonly PowerItemRepository _powerItemRepository = powerItem;

        #region method
        /// <summary> 道具箱 </summary>
        public List<PowerItemRandomOutput> RandomPowerItemBox(PowerItemRandomInput input)
        {
            if (input.IsItem) return Boxing(input.Numbers, _powerItemRepository.GetFullRandomItem);
            if (input.IsCustom) return Boxing(input.Numbers, _powerItemRepository.GetFullRandomCustomPowerItem);
            return [];
        }
        /// <summary> 小型道具箱 </summary>
        public List<PowerItemRandomOutput> RandomPowerItemSamllBox(int numbers) => Utility.RandomInt(4) switch
        {
            1 => Boxing(numbers, _powerItemRepository.GetRandomItemEffect),
            2 => Boxing(numbers, _powerItemRepository.GetRandomItemEffectParameter),
            3 => Boxing(numbers, _powerItemRepository.GetRandomCustomPowerItem),
            _ => Boxing(numbers, _powerItemRepository.GetRandomItemParameter),
        };
        /// <summary> 道具箱 </summary>
        public List<PowerItemRandomOutput> PowerItemBox(PowerItemInput input)
        {
            if (input.IsItem) return Boxing(input.Numbers, () => _powerItemRepository.GetPowerItem(input.ParameterType, input.ItemEffect, input.EffectParameter));
            if (input.IsCustom) return Boxing(input.Numbers, () => _powerItemRepository.GetCustomPowerItem(input.EffectParameter));
            return [];
        }
        #endregion

        #region private method
        /// <summary>  </summary>
        private static List<PowerItemRandomOutput> Boxing(int numbers, Func<BasePowerPotion> box)
        {
            List<PowerItemRandomOutput> _result = [];
            for (int i = 1; i <= numbers; i++)
                _result.Add(new() { Item = box.Invoke() });
            _result.RemoveAll(e => string.IsNullOrWhiteSpace(e.Item.Note));
            return _result;
        }
        /// <summary>  </summary>
        private static List<PowerItemRandomOutput> Boxing(int numbers, Func<BasePowerScroll> box)
        {
            List<PowerItemRandomOutput> _result = [];
            for (int i = 1; i <= numbers; i++)
                _result.Add(new() { Custom = box.Invoke() });
            _result.RemoveAll(e => string.IsNullOrWhiteSpace(e.Custom.Note));
            return _result;
        }
        #endregion
    }
}
