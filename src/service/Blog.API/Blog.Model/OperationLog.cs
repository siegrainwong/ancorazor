#region

using System;
using Blog.Model.Base;

#endregion

namespace Blog.Model
{	
	/// <summary>
	/// OperationLog
	/// </summary>	
	public class OperationLog : BaseModel  //可以在这里加上基类等
	{
        //将该表下的字段都遍历出来，可以自定义获取数据描述等信息

        public Guid? Guid { get; set; }
        public int  UserId { get; set; }

        public string  LoginName { get; set; }

        public string  Area { get; set; }

        public string  Controller { get; set; }

        public string  Action { get; set; }

        public string  IPAddress { get; set; }

        public DateTime  LogTime { get; set; }
 

    }
}

	//----------OperationLog结束----------
	