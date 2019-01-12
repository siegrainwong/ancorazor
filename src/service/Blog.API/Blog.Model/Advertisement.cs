	//----------Advertisement开始----------
    
using System;
namespace Blog.Model
{	
	/// <summary>
	/// Advertisement
	/// </summary>	
	public class Advertisement//可以在这里加上基类等
	{
	//将该表下的字段都遍历出来，可以自定义获取数据描述等信息

	  public int  Id { get; set; }

	  public DateTime  Createdate { get; set; }

	  public string  ImgUrl { get; set; }

	  public string  Title { get; set; }

	  public string  Url { get; set; }

	  public string  Remark { get; set; }
 

    }
}

	//----------Advertisement结束----------
	