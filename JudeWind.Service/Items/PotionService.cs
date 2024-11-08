using DataAcxess.ProjectContext;
using DataAcxess.Repository;
using GreenUtility.Extension;
using GreenUtility.Interface;
using GreenUtility.Potion;
using JudeWind.Model.Base;
using JudeWind.Model.Items;
using JudeWind.Service.Extension;

namespace JudeWind.Service.Items
{
    public class PotionService(ProjectContext projectContext, DecoratorRepository decorator, PotionRepository potion) : BaseService(projectContext)
    {
        private readonly PotionRepository _potionRepository = potion;
        private readonly DecoratorRepository _decoratorRepository = decorator;

        #region methods
        /// <summary> 藥水箱 </summary>
        public ClassicPotionOutput RandomClassicPotionBox(ClassicPotionRandomInput input) => input.Potion.HasValue
            ? Boxing(input.Numbers, () => _potionRepository.GetRandomPotion(input.Potion.Value))
            : Boxing(input.Numbers, _potionRepository.GetFullRandomPotion);

        /// <summary> 彩蛋藥水箱 </summary>
        public DecoratorClassicPotionOutput RandomDecClassicPotionBox(DecoratorClassicPotionInput input)
        {
            DecoratorClassicPotionOutput _result = new();
            LimitDecorateCount(input);
            foreach (var _boxInfo in input.DecorateBox)
            {
                if (_boxInfo.Potion.HasValue)
                    DecorateBoxing(ref _result, _boxInfo, () => _potionRepository.GetRandomPotion(_boxInfo.Potion.Value));
                else
                    DecorateBoxing(ref _result, _boxInfo, _potionRepository.GetFullRandomPotion);
            }
            return _result;
        }
        #endregion

        #region private method
        /// <summary>  </summary>
        private static ClassicPotionOutput Boxing(int numbers, Func<BasePotion> box)
        {
            ClassicPotionOutput _result = new();
            for (int i = 1; i <= numbers; i++)
                _result.Potions.Add(new() { Potion = box.Invoke() });
            return _result;
        }
        /// <summary>  </summary>
        private void DecorateBoxing(ref DecoratorClassicPotionOutput result, DecoratorClassicPotionBoxInfo boxInfo, Func<BasePotion> box)
        {
            DecoratorBuilder builder;
            for (int i = 1; i <= boxInfo.Numbers; i++)
            {
                DecoratorClassicPotionInfo _potion = new() { Potion = box.Invoke() };
                builder = CreateDecorateBuilder(_potion.Potion, boxInfo);
                _potion.Potion = (BasePotion)builder.BuildPotion();
                _potion.UnhealthyStatuses = builder.GetUnhealthyStatuses();
                _potion.Elements = builder.GetElements();
                _potion.GreatElements = builder.GetGreatElements();
                _potion.PhysicTypes = builder.GetPhysics();
                result.Potions.Add(_potion);
            }
        }
        /// <summary>  </summary>
        private static void LimitDecorateCount(DecoratorClassicPotionInput input)
        {
            foreach (var _boxInfo in input.DecorateBox)
                ServiceExtension.LimitDecorateCount(_boxInfo);
        }
        /// <summary>  </summary>
        private DecoratorBuilder CreateDecorateBuilder(IPotion potion, DecoratorClassicPotionBoxInfo boxInfo)
        {
            DecoratorBuilder builder = new();
            builder.SetPotion(potion);
            ServiceExtension.ImportDecorateBuilderItem(builder, _decoratorRepository, boxInfo);
            return builder;
        }
        #endregion
    }
}
