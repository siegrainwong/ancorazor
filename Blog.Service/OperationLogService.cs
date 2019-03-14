#region

using System;
using Blog.Entity;
using Blog.Repository;

#endregion

namespace Blog.Service
{
    public class OperationLogService
    {
        public OperationLogService(IOperationLogRepository operationLogRepository)
        {
            OperationLogRepository = operationLogRepository;
        }

        public IOperationLogRepository OperationLogRepository { get; }

        public int Insert(OperationLog operationLog)
        {
            return OperationLogRepository.Insert(operationLog);
        }

        public int DeleteById(Guid id)
        {
            return OperationLogRepository.DeleteById(id);
        }

        public int Update(OperationLog operationLog)
        {
            return OperationLogRepository.Update(operationLog);
        }
    }
}