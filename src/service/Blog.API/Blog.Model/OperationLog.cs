	//----------OperationLog开始----------
    
using System;
namespace Blog.Model
{	
	/// <summary>
	/// OperationLog
	/// </summary>	
	public class OperationLog//可以在这里加上基类等
	{
	//将该表下的字段都遍历出来，可以自定义获取数据描述等信息

	  public int  Id { get; set; }

	  public bool  ? IsDeleted { get; set; }

	  public string  Area { get; set; }

	  public string  Controller { get; set; }

	  public string  Action { get; set; }

	  public string  IPAddress { get; set; }

	  public string  Description { get; set; }

	  public DateTime  ? LogTime { get; set; }

	  public string  LoginName { get; set; }

	  public int  UserId { get; set; }

	  public int  ? User_uID { get; set; }
 

    }
}

	//----------OperationLog结束----------
	