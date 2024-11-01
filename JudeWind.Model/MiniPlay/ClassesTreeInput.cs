using DataAcxess.Repository;
using GreenUtility;

namespace JudeWind.Model.MiniPlay
{
    /// <summary> Class Tree </summary>
    public class ClassesTreeInput
    {
        public ClassesRepository.ClsTree ClsType { get; set; }
    }

    /// <summary> Class Tree </summary>
    public class ClassesTreeOutput
    {
        public GreenTree<string>? JobTree { get; set; } = null;
        public List<string> BaseTree { get; set; } = [];
    }
}
