#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Entity;
using Blog.Repository;
using SmartSql;

#endregion

namespace Blog.Service
{
    public class CategoryService
    {
        public CategoryService(ICategoryRepository repository, IArticleCategoriesRepository articleCategoriesRepository)
        {
            Repository = repository;
            ArticleCategoriesRepository = articleCategoriesRepository;
        }

        public ICategoryRepository Repository { get; }
        public IArticleCategoriesRepository ArticleCategoriesRepository { get; }

        //private async Task<bool> ResetArticleCategoriesAsync(int articleId, IEnumerable<ArticleCategories> resetWith)
        //{
        //    try
        //    {
        //        _mapper.BeginTransaction();

        //        await ArticleCategoriesRepository.DeleteByArticleAsync(articleId);
        //        await ArticleCategoriesRepository.InsertBatchAsync(resetWith);

        //        _mapper.CommitTransaction();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        _mapper.RollbackTransaction();
        //        throw;
        //    }
        //}

        //private async Task<int> InsertCategoriesNotExistsByName(params string[] names)
        //{
        //    var exists = await Repository.QueryByNamesAsync(names);
        //    if (exists.Count() == names.Length) return true;
        //    var existNames = exists.Select(x => x.Name);
        //    var notExists = names.Where(x => !existNames.Contains(x));
        //    return await Repository.InsertBatchByNamesAsync(notExists) > 0;
        //}

        //public async Task<bool> SetCategoriesForArticle(int aritlceId, string[] categories)
        //{
        //    try
        //    {
        //        _mapper.BeginTransaction();

        //        await InsertCategoriesNotExistsByName(categories);
        //        await ResetArticleCategoriesAsync()

        //        _mapper.CommitTransaction();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        _mapper.RollbackTransaction();
        //        throw;
        //    }
        //}
    }
}