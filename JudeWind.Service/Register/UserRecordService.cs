using DataAcxess.LogContext;
using DataAcxess.ProjectContext;
using Microsoft.EntityFrameworkCore;

namespace JudeWind.Service.Register
{
    public class UserRecordService(ProjectContext projectContext, LogContext logContext) : BaseService(projectContext)
    {
        protected readonly LogContext _dblog = logContext;
        public async Task AddUserRecord(string userid, string actionName, string inputData, string ipAddress)
        {
            if (!string.IsNullOrEmpty(userid))
            {
                try
                {
                    var userData = _db.UserInfo.AsNoTracking().Where(c => c.ID == userid).FirstOrDefault();
                    _dblog.SYS_UserRecord.Add(new Sys_UserRecord()
                    {
                        UserId = userid,
                        UR_RecordDateTime = DateTime.Now,
                        RecordEvent = actionName,
                        InputData = inputData,
                        IP = ipAddress
                    });
                    await _dblog.SaveChangesAsync();
                }
                catch (Exception) { /*no db-connect, probably save fail*/ }
            }
        }

    }
}
