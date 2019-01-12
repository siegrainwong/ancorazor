	//----------Module开始----------
    
using System;
namespace Blog.Model
{	
	/// <summary>
	/// Module
	/// </summary>	
	public class Module//可以在这里加上基类等
	{
	//将该表下的字段都遍历出来，可以自定义获取数据描述等信息

	  public int  Id { get; set; }

	  public bool  ? IsDeleted { get; set; }

	  public int  ? ParentId { get; set; }

	  public string  Name { get; set; }

	  public string  LinkUrl { get; set; }

	  public string  Area { get; set; }

	  public string  Controller { get; set; }

	  public string  Action { get; set; }

	  public string  Icon { get; set; }

	  public string  Code { get; set; }

	  public int  OrderSort { get; set; }

	  public string  Description { get; set; }

	  public bool  IsMenu { get; set; }

	  public bool  Enabled { get; set; }

	  public int  ? CreateId { get; set; }

	  public string  CreateBy { get; set; }

	  public DateTime  ? CreateTime { get; set; }

	  public int  ? ModifyId { get; set; }

	  public string  ModifyBy { get; set; }

	  public DateTime  ? ModifyTime { get; set; }
 

    }
}

	//----------Module结束----------
	