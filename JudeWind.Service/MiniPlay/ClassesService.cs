using DataAcxess.ProjectContext;
using DataAcxess.Repository;
using JudeWind.Model.MiniPlay;

namespace JudeWind.Service.MiniPlay
{
    public class ClassesService(ProjectContext projectContext, ClassesRepository classes) : BaseService(projectContext)
    {
        private ClassesRepository _classesRepository = classes;

        /// <summary> 職業樹 </summary>
        public ClassesTreeOutput GetTree(ClassesTreeInput input)
        {
            ArgumentNullException.ThrowIfNull(input);
            ClassesTreeOutput _result = new();
            _classesRepository.SetTreeType(input.ClsType);
            switch (input.ClsType)
            {
                case ClassesRepository.ClsTree.Rookie:
                case ClassesRepository.ClsTree.Yamato:
                    _result.BaseTree = _classesRepository.GetBaseTree();
                    break;
                case ClassesRepository.ClsTree.Warriors:
                case ClassesRepository.ClsTree.Mages:
                case ClassesRepository.ClsTree.MagicianWarriors:
                    _result.JobTree = _classesRepository.GetJobTree();
                    break;
            }
            return _result;
        }
    }
}
