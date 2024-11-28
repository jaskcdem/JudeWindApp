using DataAcxess.DataEnums;
using static GreenUtility.RPGSetting;

namespace DataAcxess.WebContext
{
    /// <summary> 道具設定 </summary>
    public class PowerItemSetting
    {
        /// <summary> 加成類型 </summary>
        public ParameterType ParameterType { get; set; }
        /// <summary> 基礎值 </summary>
        public decimal BaseValue { get; set; }
        /// <summary> 成長值 </summary>
        public decimal GrowValue { get; set; }
        /// <summary> 道具效果 </summary>
        public ItemEffect TrackEffect { get; set; }
        /// <summary> 道具效果參數 </summary>
        public ItemEffectParameter TrackParameter { get; set; }
        /// <summary> 點數 </summary>
        public decimal Point => BaseValue / GrowValue;

        public (ItemEffect Effect, ItemEffectParameter Parameter) GetTrack() => (TrackEffect, TrackParameter);
    }
}
