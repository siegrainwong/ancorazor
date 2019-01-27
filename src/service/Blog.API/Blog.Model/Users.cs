	//----------Users开始----------
    
using System;
using Blog.Model.Base;
namespace Blog.Model
{	
	/// <summary>
	/// Users
	/// </summary>	
	public class Users : BaseModel  //可以在这里加上基类等
	{
	//将该表下的字段都遍历出来，可以自定义获取数据描述等信息
    

	  public string  LoginName { get; set; }

	  public string  Password { get; set; }

	  public string  RealName { get; set; }

	  public int  Status { get; set; }
 

    }
}

	//----------Users结束----------
	