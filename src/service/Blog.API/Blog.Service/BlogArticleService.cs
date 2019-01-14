//----------BlogArticle开始----------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Common.Attributes;
using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Model.ViewModel;
using Blog.Service.Base;

namespace Blog.Service
{
    /// <summary>
    /// BlogArticleService
    /// </summary>	
    public class BlogArticleService : BaseService<BlogArticle>, IBlogArticleService
    {
        IBlogArticleRepository _dal;
        IMapper _mapper;
        public BlogArticleService(IBlogArticleRepository dal, IMapper mapper)
        {
            _dal = dal;
            _mapper = mapper;
            baseDal = dal;
        }

        public async Task<List<BlogArticle>> GetArticles()
        {
            var bloglist = await _dal.Query(a => a.Id > 0, a => a.Id);
            return bloglist;
        }

        public async Task<BlogViewModel> GetArticle(int id)
        {
            var list = await GetArticles();
            var article = (await _dal.Query(a => a.Id == id)).FirstOrDefault();
            if (article == null) return new BlogViewModel();

            var models = _mapper.Map<BlogViewModel>(article);

            var index = list.FindIndex(item => item.Id == id);
            if (index >= 0)
            {
                // 上一篇
                var prev = index > 0 ? list[index - 1] : null;
                if (prev != null)
                {
                    models.Previous = prev.Title;
                    models.PreviousId = prev.Id;
                }
                // 下一篇
                var next = index + 1 < list.Count ? list[index + 1] : null;
                if (next != null)
                {
                    models.Next = next.Title;
                    models.NextId = next.Id;
                }
            }
            article.Traffic += 1;
            await _dal.Update(article, new List<string> { "Traffic" });

            return models;
        }
    }
}

//----------BlogArticle结束----------
