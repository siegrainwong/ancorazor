	//----------BlogArticle开始----------
    
using System;
namespace Blog.Model
{	
	/// <summary>
	/// BlogArticle
	/// </summary>	
	public class BlogArticle//可以在这里加上基类等
	{
	//将该表下的字段都遍历出来，可以自定义获取数据描述等信息

	  public int  Id { get; set; }

	  public string  Author { get; set; }

	  public string  Title { get; set; }

	  public string  Category { get; set; }

	  public string  Content { get; set; }

	  public int  Traffic { get; set; }

	  public int  CommentCount { get; set; }

	  public DateTime  UpdateTime { get; set; }

	  public DateTime  CreateTime { get; set; }

	  public string  Remark { get; set; }
 

    }
}

	//----------BlogArticle结束----------
	