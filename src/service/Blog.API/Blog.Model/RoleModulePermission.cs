	//----------RoleModulePermission开始----------
    
using System;
using Blog.Model.Base;
namespace Blog.Model
{	
	/// <summary>
	/// RoleModulePermission
	/// </summary>	
	public class RoleModulePermission : BaseModel  //可以在这里加上基类等
	{
	//将该表下的字段都遍历出来，可以自定义获取数据描述等信息
   

	  public int  RoleId { get; set; }

	  public int  ModuleId { get; set; }

	  public int  ? PermissionId { get; set; }
 

    }
}

	//----------RoleModulePermission结束----------
	