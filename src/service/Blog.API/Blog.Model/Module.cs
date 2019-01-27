	//----------Module开始----------
    
using System;
using Blog.Model.Base;
namespace Blog.Model
{	
	/// <summary>
	/// Module
	/// </summary>	
	public class Module : BaseModel  //可以在这里加上基类等
	{
	//将该表下的字段都遍历出来，可以自定义获取数据描述等信息
    

	  public int  ? ParentId { get; set; }

	  public string  Name { get; set; }

	  public string  Url { get; set; }

	  public string  Area { get; set; }

	  public string  Controller { get; set; }

	  public string  Action { get; set; }

	  public string  Icon { get; set; }

	  public string  Code { get; set; }

	  public int  OrderSort { get; set; }

	  public bool  IsMenu { get; set; }

	  public bool  IsEnabled { get; set; }

	  public int  ? Creator { get; set; }

	  public int  ? Updator { get; set; }

    }
}

	//----------Module结束----------
	