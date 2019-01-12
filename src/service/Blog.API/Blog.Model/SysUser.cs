	//----------SysUser开始----------
    
using System;
namespace Blog.Model
{	
	/// <summary>
	/// SysUser
	/// </summary>	
	public class SysUser//可以在这里加上基类等
	{
	//将该表下的字段都遍历出来，可以自定义获取数据描述等信息

	  public int  Id { get; set; }

	  public string  LoginName { get; set; }

	  public string  Password { get; set; }

	  public string  RealName { get; set; }

	  public int  Status { get; set; }

	  public string  Remark { get; set; }

	  public DateTime  CreateTime { get; set; }

	  public DateTime  UpdateTime { get; set; }

	  public DateTime  LastErrorTime { get; set; }

	  public int  ErrorCount { get; set; }
 

    }
}

	//----------SysUser结束----------
	