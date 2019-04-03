#region

using Blog.Entity;
using SmartSql.DyRepository;
using System;

#endregion

namespace Blog.Repository
{
    public interface IOperationLogRepository : IRepository<OperationLog, Guid>
    {
    }
}