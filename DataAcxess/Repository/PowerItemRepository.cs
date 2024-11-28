using DataAcxess.DataEnums;
using DataAcxess.Extension;
using DataAcxess.WebContext;
using GreenUtility;
using GreenUtility.Item;
using static GreenUtility.RPGSetting;

namespace DataAcxess.Repository
{
    public class PowerItemRepository : ISampleRepository
    {
        #region private members
        readonly string[] parameterTypes = Enum.GetNames(typeof(ParameterType)), itemEffects = Enum.GetNames(typeof(ItemEffect))
            , itemEffectParameter = Enum.GetNames(typeof(ItemEffectParameter));
        readonly List<(string defName, PowerItemSetting[] values)> ItemList = [
            ("靛色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Atk
            }]),
            ("白色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Def
            }]),
            ("橙色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Mat
            }]),
            ("青色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Mdf
            }]),
            ("綠色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Agi
            }]),
            ("金色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 50,
                GrowValue = 50,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Mhp
            }]),
            ("銀色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 25,
                GrowValue = 25,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("棕色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Atk
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Def
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Mat
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Mdf
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Agi
            }]),
            ("灰色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 50,
                GrowValue = 50,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Mhp
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 25,
                GrowValue = 25,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("黑色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Atk
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Def
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Mat
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Mdf
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Agi
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 50,
                GrowValue = 50,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Mhp
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 25,
                GrowValue = 25,
                TrackEffect = ItemEffect.PowerUp,
                TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("紅色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 200,
                GrowValue = 100,
                TrackEffect = ItemEffect.Healing,
                TrackParameter = ItemEffectParameter.Hp
            }]),
            ("藍色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 100,
                GrowValue = 50,
                TrackEffect = ItemEffect.Healing,
                TrackParameter = ItemEffectParameter.Mp
            }]),
            ("紫色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 200,
                GrowValue = 100,
                TrackEffect = ItemEffect.Healing,
                TrackParameter = ItemEffectParameter.Hp
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 100,
                GrowValue = 50,
                TrackEffect = ItemEffect.Healing,
                TrackParameter = ItemEffectParameter.Mp
            }]),
            ("黃色藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 500,
                GrowValue = 100,
                TrackEffect = ItemEffect.Healing,
                TrackParameter = ItemEffectParameter.Hp
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                TrackEffect = ItemEffect.Others,
            }]),
            ("大力丸", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Atk
            }]),
            ("鐵壁丸", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Def
            }]),
            ("魔法丸", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Mat
            }]),
            ("結界丸", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Mdf
            }]),
            ("奔馳丸", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Agi
            }]),
            ("增血丸", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 20,
                GrowValue = 1.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Mhp
            }]),
            ("增魔丸", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 20,
                GrowValue = 1.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("勇士丸", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Atk
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Precent,
            BaseValue = 10,
            GrowValue = 0.5m,
            TrackEffect = ItemEffect.TimePowerUp,
            TrackParameter = ItemEffectParameter.Def
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Precent,
            BaseValue = 10,
            GrowValue = 0.5m,
            TrackEffect = ItemEffect.TimePowerUp,
            TrackParameter = ItemEffectParameter.Mat
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Precent,
            BaseValue = 10,
            GrowValue = 0.5m,
            TrackEffect = ItemEffect.TimePowerUp,
            TrackParameter = ItemEffectParameter.Mdf
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Precent,
            BaseValue = 10,
            GrowValue = 0.5m,
            TrackEffect = ItemEffect.TimePowerUp,
            TrackParameter = ItemEffectParameter.Agi
            }]),
            ("衛兵丸", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 20,
                GrowValue = 1.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Mhp
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 20,
                GrowValue = 1.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("神力丸", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Atk
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Def
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Mat
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Mdf
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Agi
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 20,
                GrowValue = 1.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Mhp
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 20,
                GrowValue = 1.5m,
                TrackEffect = ItemEffect.TimePowerUp,
                TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("核心之母", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 25,
                GrowValue = 5,
                TrackEffect = ItemEffect.Healing,
                TrackParameter = ItemEffectParameter.Hp
            }]),
            ("魔力羽毛", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 25,
                GrowValue = 1,
                TrackEffect = ItemEffect.Healing,
                TrackParameter = ItemEffectParameter.Mp
            }]),
            ("黏性果凍", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 25,
                GrowValue = 5,
                TrackEffect = ItemEffect.Healing,
                TrackParameter = ItemEffectParameter.Hp
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 25,
                GrowValue = 1,
                TrackEffect = ItemEffect.Healing,
                TrackParameter = ItemEffectParameter.Mp
            }]),
            ("洩力藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Atk
            }]),
            ("破甲藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Def
            }]),
            ("洩魔藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Mat
            }]),
            ("破結藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Mdf
            }]),
            ("遲鈍藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Agi
            }]),
            ("毒藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 50,
                GrowValue = 50,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Mhp
            }]),
            ("嘔吐藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 25,
                GrowValue = 25,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("虛脫藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Atk
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Point,
            BaseValue = 1,
            GrowValue = 1,
            TrackEffect = ItemEffect.PowerDown,
            TrackParameter = ItemEffectParameter.Def
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Point,
            BaseValue = 1,
            GrowValue = 1,
            TrackEffect = ItemEffect.PowerDown,
            TrackParameter = ItemEffectParameter.Mat
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Point,
            BaseValue = 1,
            GrowValue = 1,
            TrackEffect = ItemEffect.PowerDown,
            TrackParameter = ItemEffectParameter.Mdf
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Point,
            BaseValue = 1,
            GrowValue = 1,
            TrackEffect = ItemEffect.PowerDown,
            TrackParameter = ItemEffectParameter.Agi
            }]),
            ("癱瘓藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 50,
                GrowValue = 50,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Mhp
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Point,
            BaseValue = 25,
            GrowValue = 25,
            TrackEffect = ItemEffect.PowerDown,
            TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("疫病藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Atk
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Def
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Mat
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Mdf
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 1,
                GrowValue = 1,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Agi
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 50,
                GrowValue = 50,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Mhp
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 25,
                GrowValue = 25,
                TrackEffect = ItemEffect.PowerDown,
                TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("爆炸藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 200,
                GrowValue = 100,
                TrackEffect = ItemEffect.Damage,
                TrackParameter = ItemEffectParameter.Hp
            }]),
            ("臭味藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 100,
                GrowValue = 50,
                TrackEffect = ItemEffect.Damage,
                TrackParameter = ItemEffectParameter.Mp
            }]),
            ("耗弱藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 200,
                GrowValue = 100,
                TrackEffect = ItemEffect.Damage,
                TrackParameter = ItemEffectParameter.Hp
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 100,
                GrowValue = 50,
                TrackEffect = ItemEffect.Damage,
                TrackParameter = ItemEffectParameter.Mp
            }]),
            ("惡作劇藥水", [new PowerItemSetting {
                ParameterType = ParameterType.Point,
                BaseValue = 500,
                GrowValue = 100,
                TrackEffect = ItemEffect.Damage,
                TrackParameter = ItemEffectParameter.Hp
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Point,
                TrackEffect = ItemEffect.Others,
            }]),
            ("洩力糖果", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Atk
            }]),
            ("卸甲糖果", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Def
            }]),
            ("洩魔糖果", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Mat
            }]),
            ("毀結糖果", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Mdf
            }]),
            ("遲鈍糖果", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Agi
            }]),
            ("彩虹糖果", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 20,
                GrowValue = 1.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Mhp
            }]),
            ("幻惑糖果", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 20,
                GrowValue = 1.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("亢奮糖果", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Atk
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Precent,
            BaseValue = 10,
            GrowValue = 0.5m,
            TrackEffect = ItemEffect.TimePowerDown,
            TrackParameter = ItemEffectParameter.Def
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Precent,
            BaseValue = 10,
            GrowValue = 0.5m,
            TrackEffect = ItemEffect.TimePowerDown,
            TrackParameter = ItemEffectParameter.Mat
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Precent,
            BaseValue = 10,
            GrowValue = 0.5m,
            TrackEffect = ItemEffect.TimePowerDown,
            TrackParameter = ItemEffectParameter.Mdf
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Precent,
            BaseValue = 10,
            GrowValue = 0.5m,
            TrackEffect = ItemEffect.TimePowerDown,
            TrackParameter = ItemEffectParameter.Agi
            }]),
            ("魔法糖果", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 20,
                GrowValue = 1.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Mhp
            },
            new PowerItemSetting {
            ParameterType = ParameterType.Precent,
            BaseValue = 20,
            GrowValue = 1.5m,
            TrackEffect = ItemEffect.TimePowerDown,
            TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("天界糖果", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Atk
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Def
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Mat
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Mdf
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 10,
                GrowValue = 0.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Agi
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 20,
                GrowValue = 1.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Mhp
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 20,
                GrowValue = 1.5m,
                TrackEffect = ItemEffect.TimePowerDown,
                TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("輪刃鏢", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 25,
                GrowValue = 5,
                TrackEffect = ItemEffect.Damage,
                TrackParameter = ItemEffectParameter.Hp
            }]),
            ("臭味蛋", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 25,
                GrowValue = 1,
                TrackEffect = ItemEffect.Damage,
                TrackParameter = ItemEffectParameter.Mp
            }]),
            ("黑心火鍋", [new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 25,
                GrowValue = 5,
                TrackEffect = ItemEffect.Damage,
                TrackParameter = ItemEffectParameter.Hp
            },
            new PowerItemSetting {
                ParameterType = ParameterType.Precent,
                BaseValue = 25,
                GrowValue = 1,
                TrackEffect = ItemEffect.Damage,
                TrackParameter = ItemEffectParameter.Mp
            }]),
            ];
        readonly List<(string defName, PowerItemSetting[] values)> CustomItemList = [
            ("攻勢卷軸", [new PowerItemSetting {
                BaseValue = 14,
                GrowValue = 7,
                TrackParameter = ItemEffectParameter.Atk
            }]),
            ("防禦卷軸", [new PowerItemSetting {
                BaseValue = 14,
                GrowValue = 7,
                TrackParameter = ItemEffectParameter.Def
            }]),
            ("神通卷軸", [new PowerItemSetting {
                BaseValue = 14,
                GrowValue = 7,
                TrackParameter = ItemEffectParameter.Mat
            }]),
            ("結界卷軸", [new PowerItemSetting {
                BaseValue = 14,
                GrowValue = 7,
                TrackParameter = ItemEffectParameter.Mdf
            }]),
            ("疾風卷軸", [new PowerItemSetting {
                BaseValue = 14,
                GrowValue = 7,
                TrackParameter = ItemEffectParameter.Agi
            }]),
            ("生命卷軸", [new PowerItemSetting {
                BaseValue = 210,
                GrowValue = 70,
                TrackParameter = ItemEffectParameter.Mhp
            }]),
            ("魔法卷軸", [new PowerItemSetting {
                BaseValue = 105,
                GrowValue = 35,
                TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("勇士卷軸", [new PowerItemSetting {
                BaseValue = 10,
                GrowValue = 5,
                TrackParameter = ItemEffectParameter.Atk
            },
            new PowerItemSetting {
                BaseValue = 10,
                GrowValue = 5,
                TrackParameter = ItemEffectParameter.Def
            },
            new PowerItemSetting {
                BaseValue = 10,
                GrowValue = 5,
                TrackParameter = ItemEffectParameter.Mat
            },
            new PowerItemSetting {
                BaseValue = 10,
                GrowValue = 5,
                TrackParameter = ItemEffectParameter.Mdf
            },
            new PowerItemSetting {
                BaseValue = 10,
                GrowValue = 5,
                TrackParameter = ItemEffectParameter.Agi
            }]),
            ("守衛卷軸", [new PowerItemSetting {
                BaseValue = 100,
                GrowValue = 50,
                TrackParameter = ItemEffectParameter.Mhp
            },
            new PowerItemSetting {
                BaseValue = 50,
                GrowValue = 25,
                TrackParameter = ItemEffectParameter.Mmp
            }]),
            ("神力卷軸", [new PowerItemSetting {
                BaseValue = 6,
                GrowValue = 3,
                TrackParameter = ItemEffectParameter.Atk
            },
            new PowerItemSetting {
                BaseValue = 6,
                GrowValue = 3,
                TrackParameter = ItemEffectParameter.Def
            },
            new PowerItemSetting {
                BaseValue = 6,
                GrowValue = 3,
                TrackParameter = ItemEffectParameter.Mat
            },
            new PowerItemSetting {
                BaseValue = 6,
                GrowValue = 3,
                TrackParameter = ItemEffectParameter.Mdf
            },
            new PowerItemSetting {
                BaseValue = 6,
                GrowValue = 3,
                TrackParameter = ItemEffectParameter.Agi
            },
            new PowerItemSetting {
                BaseValue = 80,
                GrowValue = 40,
                TrackParameter = ItemEffectParameter.Mhp
            },
            new PowerItemSetting {
                BaseValue = 40,
                GrowValue = 20,
                TrackParameter = ItemEffectParameter.Mmp
            }]),
            ];
        #endregion

        #region methods
        public BasePowerPotion GetFullRandomItem() => CreateItem();
        public BasePowerPotion GetRandomItemParameter() => CreateItem(pt: parameterTypes[Utility.RandomInt(parameterTypes.Length)].ToEnum<ParameterType>());
        public BasePowerPotion GetRandomItemEffect() => CreateItem(ie: itemEffects[Utility.RandomInt(itemEffects.Length)].ToEnum<ItemEffect>());
        public BasePowerPotion GetRandomItemEffectParameter() => CreateItem(iep: itemEffectParameter[Utility.RandomInt(itemEffectParameter.Length)].ToEnum<ItemEffectParameter>());
        public BasePowerPotion GetPowerItem(ParameterType? pt = null, ItemEffect? ie = null, ItemEffectParameter? iep = null) => CreateItem(pt, ie, iep);

        public BasePowerScroll GetFullRandomCustomPowerItem() => CreateCustomItem();
        public BasePowerScroll GetRandomCustomPowerItem() => CreateCustomItem(itemEffectParameter[Utility.RandomInt(itemEffectParameter.Length)].ToEnum<ItemEffectParameter>());
        public BasePowerScroll GetCustomPowerItem(ItemEffectParameter? iep = null) => CreateCustomItem(iep);
        #endregion

        #region <-- Factory -->
        private BasePowerPotion CreateItem(ParameterType? pt = null, ItemEffect? ie = null, ItemEffectParameter? iep = null)
        {
            short rank = this.GetRandomRank();
            var query = ItemList.AsEnumerable();
            if (pt != null) query = query.Where(i => i.values.Any(p => p.ParameterType == pt.Value));
            if (ie != null) query = query.Where(i => i.values.Any(p => p.TrackEffect == ie.Value));
            if (iep != null) query = query.Where(i => i.values.Any(p => p.TrackParameter == iep.Value));
            if (!query.Any()) return new BasePowerPotion(0, string.Empty);
            var (defName, values) = query.ElementAt(Utility.RandomInt(query.Count()));
            BasePowerPotion powerItem = new(rank, defName);
            SetItem(powerItem, rank, values);
            powerItem.Note = defName;
            return powerItem;
        }
        private BasePowerScroll CreateCustomItem(ItemEffectParameter? iep = null)
        {
            short rank = this.GetRandomRank();
            var query = CustomItemList.AsEnumerable();
            if (iep != null) query = query.Where(i => i.values.Any(p => p.TrackParameter == iep.Value));
            if (!query.Any()) return new BasePowerScroll(0, string.Empty);
            var (defName, values) = query.ElementAt(Utility.RandomInt(query.Count()));
            BasePowerScroll powerItem = new(rank, defName);
            SetItem(powerItem, rank, values);
            powerItem.Note = defName;
            return powerItem;
        }

        private static void SetItem(BasePowerPotion powerItem, short rank, PowerItemSetting[] values)
        {
            if (powerItem == null) return;
            foreach (var setting in values)
            {
                switch (setting.TrackParameter)
                {
                    case ItemEffectParameter.Hp:
                        switch (setting.ParameterType)
                        {
                            case ParameterType.Point: powerItem.HealingPoint += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                            case ParameterType.Precent: powerItem.HealingPrecent += setting.BaseValue + rank * setting.GrowValue; break;
                        }
                        break;
                    case ItemEffectParameter.Mp:
                        switch (setting.ParameterType)
                        {
                            case ParameterType.Point: powerItem.RecoverPoint += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                            case ParameterType.Precent: powerItem.RecoverPrecent += setting.BaseValue + rank * setting.GrowValue; break;
                        }
                        break;
                    case ItemEffectParameter.Mhp:
                        switch (setting.ParameterType)
                        {
                            case ParameterType.Point: powerItem.Mhp += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                            case ParameterType.Precent: powerItem.MhpPrecent += setting.BaseValue + rank * setting.GrowValue; break;
                        }
                        break;
                    case ItemEffectParameter.Mmp:
                        switch (setting.ParameterType)
                        {
                            case ParameterType.Point: powerItem.Mmp += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                            case ParameterType.Precent: powerItem.MmpPrecent += setting.BaseValue + rank * setting.GrowValue; break;
                        }
                        break;
                    case ItemEffectParameter.Atk:
                        switch (setting.ParameterType)
                        {
                            case ParameterType.Point: powerItem.Atk += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                            case ParameterType.Precent: powerItem.AtkPrecent += setting.BaseValue + rank * setting.GrowValue; break;
                        }
                        break;
                    case ItemEffectParameter.Def:
                        switch (setting.ParameterType)
                        {
                            case ParameterType.Point: powerItem.Def += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                            case ParameterType.Precent: powerItem.DefPrecent += setting.BaseValue + rank * setting.GrowValue; break;
                        }
                        break;
                    case ItemEffectParameter.Mat:
                        switch (setting.ParameterType)
                        {
                            case ParameterType.Point: powerItem.Mat += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                            case ParameterType.Precent: powerItem.MatPrecent += setting.BaseValue + rank * setting.GrowValue; break;
                        }
                        break;
                    case ItemEffectParameter.Mdf:
                        switch (setting.ParameterType)
                        {
                            case ParameterType.Point: powerItem.Mdf += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                            case ParameterType.Precent: powerItem.MdfPrecent += setting.BaseValue + rank * setting.GrowValue; break;
                        }
                        break;
                    case ItemEffectParameter.Agi:
                        switch (setting.ParameterType)
                        {
                            case ParameterType.Point: powerItem.Agi += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                            case ParameterType.Precent: powerItem.AgiPrecent += setting.BaseValue + rank * setting.GrowValue; break;
                        }
                        break;
                }
            }
        }
        private static void SetItem(BasePowerScroll powerItem, short rank, PowerItemSetting[] values)
        {
            if (powerItem == null) return;
            foreach (var setting in values)
            {
                switch (setting.TrackParameter)
                {
                    case ItemEffectParameter.Mhp: powerItem.Mhp += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                    case ItemEffectParameter.Mmp: powerItem.Mmp += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                    case ItemEffectParameter.Atk: powerItem.Atk += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                    case ItemEffectParameter.Def: powerItem.Def += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                    case ItemEffectParameter.Mat: powerItem.Mat += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                    case ItemEffectParameter.Mdf: powerItem.Mdf += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                    case ItemEffectParameter.Agi: powerItem.Agi += (int)setting.BaseValue + rank * (int)setting.GrowValue; break;
                }
            }
        }
        #endregion
    }
}
