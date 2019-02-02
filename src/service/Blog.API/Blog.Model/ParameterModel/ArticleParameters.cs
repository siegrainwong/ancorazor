using System;
using System.Collections.Generic;
using System.Text;
using Blog.Model.ParameterModel.Base;

namespace Blog.Model.ParameterModel
{
    public class ArticleParameters : QueryParameters
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
