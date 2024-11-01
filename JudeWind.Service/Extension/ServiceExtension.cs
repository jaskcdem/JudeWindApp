using DataAcxess.Repository;
using GreenUtility.Extension;
using JudeWind.Model.Base;

namespace JudeWind.Service.Extension
{
    internal static class ServiceExtension
    {
        internal const int StatusDecMax = 1, ElementDecMax = 1, GreateElementDecMax = 1, PhysicDecMax = 1, TotalDecMax = 3;

        internal static void LimitDecorateCount(in DecoratorBoxInfo boxInfo)
        {
            boxInfo.StatusCount = Math.Clamp(boxInfo.StatusCount, 0, StatusDecMax);
            boxInfo.ElementCount = Math.Clamp(boxInfo.ElementCount, 0, ElementDecMax);
            boxInfo.PhysicCount = boxInfo.StatusCount + boxInfo.ElementCount < TotalDecMax
               ? Math.Clamp(boxInfo.PhysicCount, 0, PhysicDecMax) : 0;
            boxInfo.GreatElementCount = boxInfo.StatusCount + boxInfo.PhysicCount < TotalDecMax && boxInfo.ElementCount <= 0
               ? Math.Clamp(boxInfo.GreatElementCount, 0, GreateElementDecMax) : 0;
        }

        internal static void ImportDecorateBuilderItem(in DecoratorBuilder builder, in DecoratorRepository _repository, in DecoratorBoxInfo boxInfo)
        {
            for (int j = 1; j <= boxInfo.GreatElementCount; j++)
                builder.AddPrev(_repository.GetDecoratorItem(DecoratorRepository.DecoratorType.GreatElement));
            for (int j = 1; j <= boxInfo.ElementCount; j++)
                builder.AddPrev(_repository.GetDecoratorItem(DecoratorRepository.DecoratorType.Element));
            for (int j = 1; j <= boxInfo.StatusCount; j++)
                builder.AddPrev(_repository.GetDecoratorItem(DecoratorRepository.DecoratorType.Status));
            for (int j = 1; j <= boxInfo.PhysicCount; j++)
                builder.AddNext(_repository.GetDecoratorItem(DecoratorRepository.DecoratorType.Physic));
        }
    }
}
