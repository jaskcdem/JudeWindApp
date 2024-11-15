using JudeWind.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudeWind.Model.Equips
{
    /// <summary> Store box </summary>
    public class StoreEquipBoxInput
    {
        /// <summary> box info </summary>
        public List<StoreBoxInfo> BoxInfos { get; set; } = [];
    }
}
