using GreenUtility;
using GreenUtility.Extension;
using static GreenUtility.RPGSetting;

namespace DataAcxess.Repository
{
    public class DecoratorRepository
    {
        readonly DecoratorItem[] StatusList = [new("淬毒的") { UnhealthyStatus = UnhealthyStatus.Poison, Note = "25% 中毒" }
            , new("灼熱的") { UnhealthyStatus = UnhealthyStatus.Burned, Note = "20% 燒傷" }
            , new("寒冷的"){ UnhealthyStatus = UnhealthyStatus.Freeze, Note = "20% 冰凍" }
            , new("混亂的"){ UnhealthyStatus = UnhealthyStatus.Confusion, Note = "30% 混亂" }
            , new("麻痺的"){ UnhealthyStatus = UnhealthyStatus.Paralysis, Note = "30% 麻痺" } ];
        readonly DecoratorItem[] ElementList = [new("清水的") { Element = Element.Water, Note = "水附加" }
            , new("赤紅的") { Element = Element.Fire, Note = "火附加" }
            , new("翠綠的"){ Element = Element.Wind, Note = "風附加" }
            , new("神聖的"){ Element = Element.Light, Note = "光附加" }
            , new("黑暗的"){ Element = Element.Dark, Note = "暗附加" } ];
        readonly DecoratorItem[] GreatElementList = [new("大地的") { GreatElement = GreatElement.Earth, Note = "大地附加" }
            , new("森林的") { GreatElement = GreatElement.Plant, Note = "植物附加" }
            , new("腐蝕的"){ GreatElement = GreatElement.Decay, Note = "腐蝕附加" }
            , new("靜電的"){ GreatElement = GreatElement.Electric, Note = "電磁附加" } ];
        readonly DecoratorItem[] PhysicList = [new(" +打擊") { Physic = PhysicType.Hit, Note = "打擊加乘" }
            , new(" +劈砍") { Physic = PhysicType.Slash, Note = "劈砍加乘" }
            , new(" +穿刺"){ Physic = PhysicType.Pierce, Note = "穿刺加乘" } ];
        public enum DecoratorType { Status, Element, GreatElement, Physic }

        public DecoratorItem GetDecoratorItem(DecoratorType dtype) => dtype switch
        {
            DecoratorType.Status => StatusList[Utility.RandomInt(StatusList.Length)],
            DecoratorType.Element => ElementList[Utility.RandomInt(ElementList.Length)],
            DecoratorType.GreatElement => GreatElementList[Utility.RandomInt(GreatElementList.Length)],
            DecoratorType.Physic => PhysicList[Utility.RandomInt(PhysicList.Length)],
            _ => new(string.Empty)
        };
    }
}
