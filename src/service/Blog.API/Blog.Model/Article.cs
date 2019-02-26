	//----------Article开始----------
    
using System;
using Blog.Model.Base;
namespace Blog.Model
{	
	/// <summary>
	/// Article
	/// </summary>	
	public class Article : BaseModel  //可以在这里加上基类等
	{
	//将该表下的字段都遍历出来，可以自定义获取数据描述等信息

	  public int  ? Author { get; set; }

      public string Cover { get; set; }

      public string Digest { get; set; }

	  public string  Title { get; set; }

	  public string  Category { get; set; }

	  public string  Content { get; set; }

	  public int  ViewCount { get; set; }

	  public int  CommentCount { get; set; }
 

    }
}

	//----------Article结束----------
	