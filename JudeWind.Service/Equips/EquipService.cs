using Common.Extension;
using DataAcxess.LogContext;
using DataAcxess.ProjectContext;
using DataAcxess.Repository;
using GreenUtility;
using GreenUtility.Equip;
using GreenUtility.Extension;
using JudeWind.Model.Base;
using JudeWind.Model.Equips;
using JudeWind.Service.Extension;
using MigraDoc.DocumentObjectModel;
using static GreenUtility.RPGSetting;

namespace JudeWind.Service.Equips
{
    public class EquipService(ProjectContext projectContext, DecoratorRepository decorator, EquipRepository equip) : BaseService(projectContext)
    {
        private readonly EquipRepository _equipRepository = equip;
        private readonly DecoratorRepository _decoratorRepository = decorator;
        readonly string[] suitTypes = Enum.GetNames(typeof(EquipRepository.SuitEquipType))
            .Where(e => e.ToEnum<EquipRepository.SuitEquipType>() != EquipRepository.SuitEquipType.None).ToArray();

        #region methods
        #region <-- Box -->
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

        #region <-- Suit -->
        public List<BaseEquip> SuitEquipBag(SuitEquipInput input) => _equipRepository.GetSuit(input.SuitType);

        public byte[] ExportSuitExcel(SuitEquipInput input)
        {
            var _suit = _equipRepository.GetSuit(input.SuitType);
            return ExportHelper.ExportExcel(GetSimpleExcel(_suit));
        }

        public Stream ExportSuitOds(SuitEquipInput input)
        {
            var _suit = _equipRepository.GetSuit(input.SuitType);
            OdsBuildInfo buildInfo = new()
            {
                SheetName = input.SuitType.ToString(),
                ColumnsSetting = GetOdsColumnsSetting(),
                Datas = GetOdsData(_suit)
            };
            return ExportHelper.ExportOds(buildInfo);
        }

        public byte[] ExportSuitPdf(SuitEquipInput input)
        {
            var _suit = _equipRepository.GetSuit(input.SuitType);
            PdfBuildInfo buildInfo = new()
            {
                Orientation = Orientation.Landscape,
                NomalStyle = GetNomalStyle(),
                Tables = [
                    (0, GetPdfTableBuildInfo(_suit))
                ]
            };
            return ExportHelper.ExportPdf(buildInfo);
        }

        public byte[] ExportAllSuitExcel()
        {
            List<BaseEquip> _suits = [];
            foreach (var name in suitTypes) _suits.AddRange(_equipRepository.GetSuit(name.ToEnum<EquipRepository.SuitEquipType>()));
            return ExportHelper.ExportExcel(GetSimpleExcel(_suits));
        }

        public Stream ExportAllSuitOds(bool isSpirt = false)
        {
            var os = GetOdsColumnsSetting();
            if (isSpirt)
            {
                List<OdsBuildInfo> buildInfoList = [];
                foreach (var name in suitTypes)
                {
                    var _suit = _equipRepository.GetSuit(name.ToEnum<EquipRepository.SuitEquipType>());
                    buildInfoList.Add(new()
                    {
                        SheetName = name,
                        ColumnsSetting = os,
                        Datas = GetOdsData(_suit)
                    });
                }
                return ExportHelper.ExportOds([.. buildInfoList]);
            }
            else
            {
                List<BaseEquip> _suits = [];
                foreach (var name in suitTypes) _suits.AddRange(_equipRepository.GetSuit(name.ToEnum<EquipRepository.SuitEquipType>()));
                OdsBuildInfo buildInfo = new()
                {
                    ColumnsSetting = os,
                    Datas = GetOdsData(_suits)
                };
                return ExportHelper.ExportOds(buildInfo);
            }
        }

        public byte[] ExportAllSuitPdf()
        {
            List<BaseEquip> _suits = [];
            foreach (var name in suitTypes) _suits.AddRange(_equipRepository.GetSuit(name.ToEnum<EquipRepository.SuitEquipType>()));
            PdfBuildInfo buildInfo = new()
            {
                Orientation = Orientation.Landscape,
                NomalStyle = GetNomalStyle(),
                Tables = [
                    (0, GetPdfTableBuildInfo(_suits))
                ]
            };
            return ExportHelper.ExportPdf(buildInfo);
        }
        #endregion


        #endregion

        #region private method
        #region <-- Boxing -->
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

        #region <-- Export setting/data -->
        private static List<Dictionary<string, object>> GetSimpleExcel(List<BaseEquip> data)
         => [
             ..data.Select(j => new Dictionary<string, object>
                {
                    {"名稱", j.Name},
                    {"品級", j.Rank},
                    {"最大血量", j.Mhp},
                    {"最大魔力", j.Mmp},
                    {"攻擊力", j.Atk},
                    {"防禦力", j.Def},
                    {"魔法攻擊", j.Mat},
                    {"魔法防禦", j.Mdf},
                    {"敏捷", j.Agi},
                    {"售價", j.Price},
                    {"備註", j.Note ?? string.Empty},
                    {"類別", j.GetType() }
                }),
         ];
        private static List<Dictionary<string, object>> GetSimpleExcel(List<SuitEquipLog> data)
         => [
             ..data.Select(j => new Dictionary<string, object>
                {
                    {"名稱", j.Name},
                    {"品級", j.Rank},
                    {"最大血量", j.Mhp},
                    {"最大魔力", j.Mmp},
                    {"攻擊力", j.Atk},
                    {"防禦力", j.Def},
                    {"魔法攻擊", j.Mat},
                    {"魔法防禦", j.Mdf},
                    {"敏捷", j.Agi},
                    {"稀有度", j.Level},
                    {"總點數", j.TotalPoint},
                    {"比較值", j.Compared},
                    {"備註", j.Note ?? string.Empty},
                }),
         ];
        private static List<(int index, string name, double width)> GetOdsColumnsSetting()
            => [
                (0, "名稱", 15), (1, "品級", 0), (2, "最大血量", 0), (3, "最大魔力", 0),
                (4, "攻擊力", 0), (5, "防禦力", 0), (6, "魔法攻擊", 0), (7, "魔法防禦", 0),
                (8, "敏捷", 0), (9, "售價", 20), (10, "備註", 25), (11, "類別", 15)
            ];
        private static IEnumerable<List<(int index, object value)>> GetOdsData(List<BaseEquip> data)
            => data.Select(x => new List<(int index, object value)>
            {
                (0, x.Name), (1, x.Rank), (2, x.Mhp), (3, x.Mmp),
                (4, x.Atk), (5, x.Def), (6, x.Mat), (7, x.Mdf),
                (8, x.Agi), (9, x.Price), (10, x.Note ?? string.Empty), (11, x.GetType().Name)
            });
        private static Style GetNomalStyle()
        {
            Style style = new();
            style.Font.Size = 14;
            style.Font.Bold = false;
            style.Font.Name = "標楷體";
            style.Font.Color = Colors.Black;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            return style;
        }
        private static PdfTableBuildInfo GetPdfTableBuildInfo(List<BaseEquip> data) => new()
        {
            ColumnsSetting = [
                new PdfTableColumnBuildInfo { Index = 0, Name = "名稱", Width = 1.5 },
                new PdfTableColumnBuildInfo { Index = 1, Name = "品級", Width = 2 },
                new PdfTableColumnBuildInfo { Index = 2, Name = "最大血量", Width = 2 },
                new PdfTableColumnBuildInfo { Index = 3, Name = "最大魔力", Width = 2 },
                new PdfTableColumnBuildInfo { Index = 4, Name = "攻擊力", Width = 2 },
                new PdfTableColumnBuildInfo { Index = 5, Name = "防禦力", Width = 2 },
                new PdfTableColumnBuildInfo { Index = 6, Name = "魔法攻擊", Width = 2 },
                new PdfTableColumnBuildInfo { Index = 7, Name = "魔法防禦", Width = 2 },
                new PdfTableColumnBuildInfo { Index = 8, Name = "敏捷", Width = 2 },
                new PdfTableColumnBuildInfo { Index = 9, Name = "售價", Width = 2 },
                new PdfTableColumnBuildInfo { Index = 10, Name = "備註", Width = 2.5 },
                new PdfTableColumnBuildInfo { Index = 11, Name = "類別", Width = 3.2 },
            ],
            Datas = data.Select(x => new List<PdfTableCellBuildInfo>
            {
                new() { Index = 0, Value = x.Name },
                new() { Index = 1, Value = x.Rank },
                new() { Index = 2, Value = x.Mhp },
                new() { Index = 3, Value = x.Mmp },
                new() { Index = 4, Value = x.Atk },
                new() { Index = 5, Value = x.Def },
                new() { Index = 6, Value = x.Mat },
                new() { Index = 7, Value = x.Mdf },
                new() { Index = 8, Value = x.Agi },
                new() { Index = 9, Value = x.Price },
                new() { Index = 10, Value = x.Note ?? string.Empty },
                new() { Index = 11, Value = x.GetType().Name },
            })
        };
        #endregion
        #endregion
    }
}
