	//----------RoleModulePermission开始----------
    
using System;
namespace Blog.Model
{	
	/// <summary>
	/// RoleModulePermission
	/// </summary>	
	public class RoleModulePermission//可以在这里加上基类等
	{
	//将该表下的字段都遍历出来，可以自定义获取数据描述等信息

	  public int  Id { get; set; }

	  public bool  ? IsDeleted { get; set; }

	  public int  RoleId { get; set; }

	  public int  ModuleId { get; set; }

	  public int  ? PermissionId { get; set; }

	  public int  ? CreateId { get; set; }

	  public string  CreateBy { get; set; }

	  public DateTime  ? CreateTime { get; set; }

	  public int  ? ModifyId { get; set; }

	  public string  ModifyBy { get; set; }

	  public DateTime  ? ModifyTime { get; set; }
 

    }
}

	//----------RoleModulePermission结束----------
	