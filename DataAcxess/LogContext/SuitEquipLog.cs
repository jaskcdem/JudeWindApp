using DataAcxess.DataEnums;
using GreenUtility;
using GreenUtility.Equip;
using GreenUtility.Interface;

namespace DataAcxess.LogContext
{
    public class SuitEquipLog : IRankItem, IParameters
    {
        #region Constructor
        public SuitEquipLog() { }
        public SuitEquipLog(BaseEquip baseEquip)
        {
            Utility.Cast(baseEquip, this);
        }
        public SuitEquipLog(params SuitEquipLog[] logs)
        {
            Rank = logs[0].Rank;
            Mhp = logs.Sum(x => x.Mhp);
            Mmp = logs.Sum(x => x.Mmp);
            Atk = logs.Sum(x => x.Atk);
            Def = logs.Sum(x => x.Def);
            Mat = logs.Sum(x => x.Mat);
            Mdf = logs.Sum(x => x.Mdf);
            Agi = logs.Sum(x => x.Agi);
            Level = logs.GroupBy(x => x.Level).Count() > 1 ? ItemLevel.UR : logs[0].Level;
            TotalPoint = logs.Sum(x => x.TotalPoint);
        }
        #endregion

        #region Properties
        /// <inheritdoc/>
        public long Id { get; set; }
        /// <inheritdoc/>
        public string Name { get; set; } = string.Empty;
        /// <inheritdoc/>
        public short Rank { get; set; }
        /// <inheritdoc/>
        public int Mhp { get; set; }
        /// <inheritdoc/>
        public int Mmp { get; set; }
        /// <inheritdoc/>
        public int Atk { get; set; }
        /// <inheritdoc/>
        public int Def { get; set; }
        /// <inheritdoc/>
        public int Mat { get; set; }
        /// <inheritdoc/>
        public int Mdf { get; set; }
        /// <inheritdoc/>
        public int Agi { get; set; }
        /// <inheritdoc/>
        public long Price { get; set; }
        /// <inheritdoc/>
        public string? Note { get; set; }
        /// <summary> 稀有度 </summary>
        public ItemLevel Level { get; set; }
        /// <summary> 總點數 </summary>
        public int TotalPoint { get; set; }
        /// <summary> 比較值 </summary>
        /// <remarks><list type="bullet">
        /// <item><term>0 </term><description> 總點數等於系統點數</description></item>
        /// <item><term>負數 </term><description> 總點數大於系統點數</description></item>
        /// <item><term>正數 </term><description> 總點數小於系統點數</description></item>
        /// </list></remarks>
        public int Compared
        {
            get
            {
                int _sysPoint = RarePoint[Level];
                return _sysPoint.CompareTo(TotalPoint);
            }
        }
        #endregion

        #region Field
        readonly Dictionary<ItemLevel, int> RarePoint = new()
        {
            { ItemLevel.N, 5 }, { ItemLevel.R, 7 }, { ItemLevel.SR, 9 }, { ItemLevel.ER, 11 }, { ItemLevel.LR, 13 }, { ItemLevel.UR, int.MaxValue }
        };
        #endregion

        #region inherit method
        /// <inheritdoc/>
        public void OnThrow(Action action) => throw new NotImplementedException();
        /// <inheritdoc/>
        public void OnUse(Action action) => throw new NotImplementedException();
        /// <inheritdoc/>
        public void OnUse(IActor player) => throw new NotImplementedException();
        /// <inheritdoc/>
        public void OnUse(params IActor[] players) => throw new NotImplementedException();
        /// <inheritdoc/>
        public void OnUse(IEnemy enemy) => throw new NotImplementedException();
        /// <inheritdoc/>
        public void OnUse(params IEnemy[] enemys) => throw new NotImplementedException();
        #endregion
    }
}
