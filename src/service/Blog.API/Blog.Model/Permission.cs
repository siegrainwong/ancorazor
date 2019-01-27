	//----------Permission开始----------
    
using System;
using Blog.Model.Base;
namespace Blog.Model
{	
	/// <summary>
	/// Permission
	/// </summary>	
	public class Permission : BaseModel  //可以在这里加上基类等
	{
	//将该表下的字段都遍历出来，可以自定义获取数据描述等信息

	  public int  Id { get; set; }

	  public string  Code { get; set; }

	  public string  Name { get; set; }

	  public int  OrderSort { get; set; }

	  public string  Icon { get; set; }

	  public bool  Enabled { get; set; }

	  public int  ? Creator { get; set; }

	  public DateTime  CreatedAt { get; set; }

	  public int  ? Updator { get; set; }

	  public DateTime  ? UpdatedAt { get; set; }

	  public bool  ? IsDeleted { get; set; }

	  public string  Remark { get; set; }
 

    }
}

	//----------Permission结束----------
	