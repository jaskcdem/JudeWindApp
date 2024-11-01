using DataAcxess.Extension;
using GreenUtility;
using GreenUtility.ClassTree;

namespace DataAcxess.Repository
{
    public class ClassesRepository : ISampleRepository
    {
        public enum ClsTree { Rookie, Yamato, Warriors, Mages, MagicianWarriors }
        public ClsTree TreeType { get; private set; }

        public GreenTree<string> GetJobTree() => TreeType switch
        {
            ClsTree.Warriors => ClassesTree.Warriors(),
            ClsTree.Mages => ClassesTree.Mages(),
            ClsTree.MagicianWarriors => ClassesTree.MagicianWarriors(),
            _ => throw new NotImplementedException("undifind ClsTree")
        };

        public List<string> GetBaseTree() => TreeType switch
        {
            ClsTree.Rookie => ClassesTree.Rookie(),
            ClsTree.Yamato => ClassesTree.Yamato(),
            _ => throw new NotImplementedException("undifind ClsTree")
        };

        public ClassesRepository SetTreeType(ClsTree ct)
        {
            TreeType = ct;
            return this;
        }
    }
}
