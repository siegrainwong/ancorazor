#region

using System;
using Blog.Entity;
using SmartSql.DyRepository;

#endregion

namespace Blog.Repository
{
    public interface IOperationLogRepository : IRepository<OperationLog, Guid>
    {
    }
}