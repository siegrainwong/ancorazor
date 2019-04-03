#region

using Blog.Entity;
using Blog.Repository;
using System;

#endregion

namespace Blog.Service
{
    public class OperationLogService
    {
        public IOperationLogRepository OperationLogRepository { get; }

        public OperationLogService(IOperationLogRepository operationLogRepository)
        {
            OperationLogRepository = operationLogRepository;
        }

        public int DeleteById(Guid id)
        {
            return OperationLogRepository.DeleteById(id);
        }

        public int Insert(OperationLog operationLog)
        {
            return OperationLogRepository.Insert(operationLog);
        }

        public int Update(OperationLog operationLog)
        {
            return OperationLogRepository.Update(operationLog);
        }
    }
}