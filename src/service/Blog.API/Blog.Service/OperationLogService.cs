	//----------OperationLog开始----------
    

using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Service.Base;

namespace Blog.Service
{	
	/// <summary>
	/// OperationLogService
	/// </summary>	
	public class OperationLogService : BaseService<OperationLog>, IOperationLogService
    {
	
        IOperationLogRepository dal;
        public OperationLogService(IOperationLogRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       
    }
}

	//----------OperationLog结束----------
	