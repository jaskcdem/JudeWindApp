using Common;
using Common.Extension;
using DataAcxess.DataEnums;
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
        public List<EquipOutput> RandomEquipBox(EquipRandomInput input) => input.Weapon.HasValue && !input.Armor.HasValue
                ? Boxing(input.Numbers, () => _equipRepository.GetWeapon(input.Weapon.Value))
                : !input.Weapon.HasValue && input.Armor.HasValue
                ? Boxing(input.Numbers, () => _equipRepository.GetArmor(input.Armor.Value))
                : Boxing(input.Numbers, () => _equipRepository.GetRandomEquip(ShopType.Equip));
        /// <summary> 武器箱 </summary>
        public List<EquipOutput> RandomWeaponBox(EquipRandomInput input) => Boxing(input.Numbers, () => _equipRepository.GetRandomEquip(ShopType.Weapon));
        /// <summary> 防具箱 </summary>
        public List<EquipOutput> RandomArmorBox(EquipRandomInput input) => Boxing(input.Numbers, () => _equipRepository.GetRandomEquip(ShopType.Armor));

        /// <summary> 彩蛋裝備箱 </summary>
        public List<DecoratorEquipOutput> RandomDecEquipBox(DecoratorEquipInput input)
        {
            List<DecoratorEquipOutput> _result = [];
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
            _result.RemoveAll(CanRemove);
            return _result;
        }
        /// <summary> 彩蛋武器箱 </summary>
        public List<DecoratorEquipOutput> RandomDecWeaponBox(DecoratorEquipInput input)
        {
            List<DecoratorEquipOutput> _result = [];
            LimitDecorateCount(input);
            foreach (var _boxInfo in input.DecorateBox)
            {
                DecorateBoxing(ref _result, _boxInfo, () => _equipRepository.GetRandomEquip(ShopType.Weapon));
            }
            _result.RemoveAll(CanRemove);
            return _result;
        }
        /// <summary> 彩蛋防具箱 </summary>
        public List<DecoratorEquipOutput> RandomDecArmorBox(DecoratorEquipInput input)
        {
            List<DecoratorEquipOutput> _result = [];
            LimitDecorateCount(input);
            foreach (var _boxInfo in input.DecorateBox)
            {
                DecorateBoxing(ref _result, _boxInfo, () => _equipRepository.GetRandomEquip(ShopType.Armor));
            }
            _result.RemoveAll(CanRemove);
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

        public byte[] ExportSuitLogExcel()
        {
            List<SuitEquipLog> _suits = [];
            foreach (var name in suitTypes) _suits.AddRange(_equipRepository.CountSuitPoint(name.ToEnum<EquipRepository.SuitEquipType>()));
            return ExportHelper.ExportExcel(GetSimpleExcel(_suits));
        }
        public byte[] ExportSuitLogExcelEp()
        {
            List<SuitEquipLog> _suits = [];
            foreach (var name in suitTypes) _suits.AddRange(_equipRepository.CountSuitPoint(name.ToEnum<EquipRepository.SuitEquipType>()));

            (string header, int index, Func<SuitEquipLog, object> selector)[] columns = [
                ("名稱", 1, s => s.Name), ("品級", 2, s => s.Rank), ("最大血量", 3, s => s.Mhp), ("最大魔力", 4, s => s.Mmp),
                ("攻擊力", 5, s => s.Atk), ("防禦力", 6, s => s.Def), ("魔法攻擊", 7, s => s.Mat), ("魔法防禦", 8, s => s.Mdf),
                ("敏捷", 9, s => s.Agi), ("稀有度", 10, s => s.Level), ("總點數", 11, s => s.TotalPoint), ("比較值", 12, s => s.Compared),
                ("備註", 13, s => s.Note ?? string.Empty)
            ];
            EPExcelHelper<SuitEquipLog> helper = new();
            var result = helper.InitExport(("SuitLog", _suits, columns), ("SuitLog2", _suits, columns), ("SuitLog3", _suits, columns)).Export();
            return result ?? throw new NullReferenceException();
        }
        public byte[] ExportSuitLogExcelEpWithTitle()
        {
            List<SuitEquipLog> _suits = [];
            foreach (var name in suitTypes) _suits.AddRange(_equipRepository.CountSuitPoint(name.ToEnum<EquipRepository.SuitEquipType>()));

            (string header, int index, Func<SuitEquipLog, object> selector)[] columns = [
                ("名稱", 1, s => s.Name), ("品級", 2, s => s.Rank), ("最大血量", 3, s => s.Mhp), ("最大魔力", 4, s => s.Mmp),
                ("攻擊力", 5, s => s.Atk), ("防禦力", 6, s => s.Def), ("魔法攻擊", 7, s => s.Mat), ("魔法防禦", 8, s => s.Mdf),
                ("敏捷", 9, s => s.Agi), ("稀有度", 10, s => s.Level), ("總點數", 11, s => s.TotalPoint), ("比較值", 12, s => s.Compared),
                ("備註", 13, s => s.Note ?? string.Empty)
            ];
            ExcelCustomTitle[] titles = [
                new ExcelCustomTitle(){
                    HeaderRow = 1,
                    HeaderColumn = [("亞莉亞", new ExcelCustomStyle(){ BackgroungColor = System.Drawing.Color.OrangeRed}),
                        ("賽娜", new ExcelCustomStyle(){ BackgroungColor = System.Drawing.Color.AliceBlue}),
                        ("茱麗葉", new ExcelCustomStyle(){ BackgroungColor = System.Drawing.Color.LightGray}),
                        ("尤彌爾", new ExcelCustomStyle()),
                ]},
                new ExcelCustomTitle(){
                    HeaderRow = 2,
                    HeaderColumn =[("夏洛特", new ExcelCustomStyle(){ BackgroungColor = System.Drawing.Color.LightYellow}),
                        ("費南雪", new ExcelCustomStyle(){ BackgroungColor = System.Drawing.Color.Violet}),
                        ("休凱特", new ExcelCustomStyle(){ BackgroungColor = System.Drawing.Color.LimeGreen})
                ]},
                new ExcelCustomTitle(){
                    HeaderRow = 3,
                    HeaderColumn = [("甘納許", new ExcelCustomStyle(){ BackgroungColor = System.Drawing.Color.Purple}),
                        ("舒服蕾", new ExcelCustomStyle(){ BackgroungColor = System.Drawing.Color.Pink}),
                        ("克拉芙緹", new ExcelCustomStyle(){ BackgroungColor = System.Drawing.Color.Coral}),
                        ("@@@", new ExcelCustomStyle()),
                        ("XXX", new ExcelCustomStyle(){ FontColor = System.Drawing.Color.Green}),
                ]},
            ];

            EPExcelHelper<SuitEquipLog> helper = new();
            var result = helper.InitExport(("SuitLog", _suits, columns)).ExportWithTitle((0, titles));
            return result ?? throw new NullReferenceException();
        }
        #endregion

        #region <-- StoreBox -->
        public List<StoreEquipBoxOutput> StoreEquipBoxes(StoreEquipBoxInput input)
        {
            ArgumentNullException.ThrowIfNull(nameof(input.BoxInfos));
            if (input.BoxInfos.Count == 0) return [];
            Dictionary<ItemLevel, double> presentStage = BuildRateDic(input.BoxInfos);

            #region local func
            ItemLevel RandomLevel()
            {
                var dRes = GeneralTool.StageRandom(presentStage.Values.ToArray());
                var elemnet = presentStage.FirstOrDefault(x => x.Value == dRes);
                return elemnet.Key;
            }
            BaseEquip SpecifyWeapon(ItemLevel lev) => _equipRepository.GetSpecifyWeapon(input.Weapon, lev, input.SuitType);
            BaseEquip SpecifyArmor(ItemLevel lev) => _equipRepository.GetSpecifyArmor(input.Armor, lev, input.SuitType);
            #endregion
            if (input.IsWeaponBox)
                return StoreBoxing(input.Numbers, RandomLevel, SpecifyWeapon);
            if (input.IsArmorBox)
                return StoreBoxing(input.Numbers, RandomLevel, SpecifyArmor);
            return Utility.RandomInt(2) switch
            {
                1 => StoreBoxing(input.Numbers, RandomLevel, SpecifyWeapon),
                _ => StoreBoxing(input.Numbers, RandomLevel, SpecifyArmor)
            };
        }
        public List<StoreDecEquipBoxOutput> StoreDecEquipBoxes(StoreDecEquipBoxInput input)
        {
            ArgumentNullException.ThrowIfNull(nameof(input.BoxInfos));
            if (input.BoxInfos.Count == 0) return [];
            Dictionary<ItemLevel, double> presentStage = BuildRateDic(input.BoxInfos);

            #region local func
            ItemLevel RandomLevel()
            {
                var dRes = GeneralTool.StageRandom(presentStage.Values.ToArray());
                var elemnet = presentStage.FirstOrDefault(x => x.Value == dRes);
                return elemnet.Key;
            }
            BaseEquip SpecifyWeapon(ItemLevel lev) => _equipRepository.GetSpecifyWeapon(input.Weapon, lev, input.SuitType);
            BaseEquip SpecifyArmor(ItemLevel lev) => _equipRepository.GetSpecifyArmor(input.Armor, lev, input.SuitType);
            #endregion
            List<StoreDecEquipBoxOutput> _result = [];
            LimitDecorateCount(input);
            foreach (var _boxInfo in input.DecorateBox)
            {
                if (input.IsWeaponBox)
                {
                    StoreDecorateBoxing(ref _result, _boxInfo, RandomLevel, SpecifyWeapon);
                    continue;
                }
                if (input.IsArmorBox)
                {
                    StoreDecorateBoxing(ref _result, _boxInfo, RandomLevel, SpecifyArmor);
                    continue;
                }

                switch (Utility.RandomInt(2))
                {
                    case 1: StoreDecorateBoxing(ref _result, _boxInfo, RandomLevel, SpecifyWeapon); continue;
                    default: StoreDecorateBoxing(ref _result, _boxInfo, RandomLevel, SpecifyArmor); continue;
                }
            }
            _result.RemoveAll(e => e.TypeName == nameof(BaseEquip) && e.Equip.Rank <= 0);
            return _result;
        }
        #endregion
        #endregion

        #region private method
        #region <-- Boxing -->
        /// <summary>  </summary>
        /// <param name="numbers">total pickup numbers</param>
        /// <param name="box">create or find equip func</param>
        private static List<EquipOutput> Boxing(int numbers, Func<BaseEquip> box)
        {
            List<EquipOutput> _result = [];
            for (int i = 1; i <= numbers; i++)
                _result.Add(new() { Equip = box.Invoke() });
            _result.RemoveAll(e => e.TypeName == nameof(BaseEquip) && e.Equip.Rank <= 0);
            return _result;
        }
        /// <summary>  </summary>
        /// <param name="result">out put object</param>
        /// <param name="boxInfo">decorate info</param>
        /// <param name="box">create or find equip func</param>
        private void DecorateBoxing(ref List<DecoratorEquipOutput> result, DecoratorEquipBoxInfo boxInfo, Func<BaseEquip> box)
        {
            DecoratorBuilder builder;
            for (int i = 1; i <= boxInfo.Numbers; i++)
            {
                DecoratorEquipOutput _equip = new() { Equip = box.Invoke() };
                builder = CreateDecorateBuilder(_equip.Equip, boxInfo);
                _equip.Equip = (BaseEquip)builder.BuildEquip();
                _equip.UnhealthyStatuses = builder.GetUnhealthyStatuses();
                _equip.Elements = builder.GetElements();
                _equip.GreatElements = builder.GetGreatElements();
                _equip.PhysicTypes = builder.GetPhysics();
                result.Add(_equip);
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

        #region <-- StoreBox -->
        /// <summary>  </summary>
        /// <param name="numbers">total pickup numbers</param>
        /// <param name="box">create or find equip func</param>
        /// <param name="ranLev">itemLevel taker</param>
        private static List<StoreEquipBoxOutput> StoreBoxing(int numbers, Func<ItemLevel> ranLev, Func<ItemLevel, BaseEquip> box)
        {
            List<StoreEquipBoxOutput> _result = [];
            for (int i = 1; i <= numbers; i++)
            {
                var lev = ranLev.Invoke();
                _result.Add(new() { Equip = box.Invoke(lev), Level = lev });
            }
            _result.RemoveAll(e => e.TypeName == nameof(BaseEquip) && e.Equip.Rank <= 0);
            return _result;
        }
        /// <summary>  </summary>
        /// <param name="result">out put object</param>
        /// <param name="boxInfo">decorate info</param>
        /// <param name="box">create or find equip func</param>
        /// <param name="ranLev">itemLevel taker</param>
        private void StoreDecorateBoxing(ref List<StoreDecEquipBoxOutput> result, DecoratorEquipBoxInfo boxInfo, Func<ItemLevel> ranLev, Func<ItemLevel, BaseEquip> box)
        {
            DecoratorBuilder builder;
            for (int i = 1; i <= boxInfo.Numbers; i++)
            {
                var lev = ranLev.Invoke();
                StoreDecEquipBoxOutput _equip = new() { Equip = box.Invoke(lev), Level = lev };
                builder = CreateDecorateBuilder(_equip.Equip, boxInfo);
                _equip.Equip = (BaseEquip)builder.BuildEquip();
                _equip.UnhealthyStatuses = builder.GetUnhealthyStatuses();
                _equip.Elements = builder.GetElements();
                _equip.GreatElements = builder.GetGreatElements();
                _equip.PhysicTypes = builder.GetPhysics();
                result.Add(_equip);
            }
        }
        /// <summary>  </summary>
        private static void LimitDecorateCount(StoreDecEquipBoxInput input)
        {
            foreach (var _boxInfo in input.DecorateBox)
                ServiceExtension.LimitDecorateCount(_boxInfo);
        }

        private static Dictionary<ItemLevel, double> BuildRateDic(List<StoreBoxInfo> input)
        {
            Dictionary<ItemLevel, double> presentStage;
            if (input.Count > 1)
            {
                var levelBox = input.GroupBy(x => x.Level).Select(x => new StoreBoxInfo() { Level = x.Key, Precent = x.Sum(y => y.Precent) }).OrderBy(x => x.Level);
                double totalPersent = levelBox.Sum(b => b.Precent);
                presentStage = [];
                for (int i = 0; i < levelBox.Count(); i++)
                {
                    var item = levelBox.ElementAt(i);
                    presentStage.Add(item.Level, i == 0 ? item.Precent : presentStage.ElementAt(i - 1).Value + item.Precent);
                }
            }
            else presentStage = new Dictionary<ItemLevel, double>() { { input[0].Level, 1 } };
            return presentStage;
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

        bool CanRemove(DecoratorEquipOutput e) => e.TypeName == nameof(BaseEquip) && e.Equip.Rank <= 0;
        #endregion
    }
}
