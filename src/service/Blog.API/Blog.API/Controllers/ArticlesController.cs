#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.API.Helpers;
using Blog.Common.Extensions;
using Blog.IService;
using Blog.Model;
using Blog.Model.Mapping;
using Blog.Model.ParameterModel;
using Blog.Model.ParameterModel.Base;
using Blog.Model.Resources;
using Blog.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

#endregion

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _service;
        private readonly IUrlHelper _urlHelper;
        private readonly IPropertyMappingContainer _mappingContainer;
        private readonly ITypeHelperService _typeHelper;
        private readonly IMapper _mapper;
        public ArticlesController(IArticleService service,
            IUrlHelper urlHelper,
            IPropertyMappingContainer mappingContainer,
            ITypeHelperService typeHelper,
            IMapper mapper
            )
        {
            _service = service;
            _urlHelper = urlHelper;
            _mappingContainer = mappingContainer;
            _typeHelper = typeHelper;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]ArticleParameters parameters)
        {
            if (!_mappingContainer.ValidateMappingExistsFor<ArticleViewModel, Article>(parameters.OrderBy))
                return BadRequest("Can't finds fields for sorting.");
            if (!_typeHelper.TypeHasProperties<ArticleViewModel>(parameters.Fields))
                return BadRequest("Fields not exist.");

            var list = await _service.GetPagedArticles(parameters);

            var viewModels = _mapper.Map<IEnumerable<Article>, IEnumerable<ArticleViewModel>>(list);

            var previousPageLink = list.HasPrevious ? CreatePostUri(parameters, PaginationResourceUriType.PreviousPage) : null;

            var nextPageLink = list.HasNext ? CreatePostUri(parameters, PaginationResourceUriType.NextPage) : null;

            var meta = new
            {
                list.TotalItemsCount,
                list.PageSize,
                list.PageIndex,
                list.PageCount,
                previousPageLink,
                nextPageLink
            };

            var result = new
            {
                succeed = true,
                data = viewModels.ToDynamicIEnumerable(parameters.Fields),
                pagination = meta
            };

            return Ok(result);
        }

        // GET: api/Blog/5
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) return BadRequest("Parameter invalid.");

            var model = await _service.GetArticle(id);
            if (model == null) return NotFound(id);

            var viewModel = _mapper.Map<Article, ArticleViewModel>(model);

            return Ok(new { succeed = true, data = viewModel });
        }
        
        [HttpPost(Name = "Add")]
        public async Task<IActionResult> Add([FromBody] ArticleParameters parameters)
        {
            if (parameters == null) return BadRequest();
            if (!ModelState.IsValid) return new MyUnprocessableEntityObjectResult(ModelState);

            var model = _mapper.Map<ArticleParameters, Article>(parameters);
            model.Author = 1;
            model.UpdatedAt = DateTime.Now;
            model.CreatedAt = DateTime.Now;

            model.Id = await _service.Add(model);
            var viewModel = _mapper.Map<Article, ArticleViewModel>(model);
            var links = CreateLinksForPost(viewModel.Id);

            var result = new
            {
                succeed = true,
                data = viewModel
            };
            return CreatedAtRoute("Get", new { id = viewModel.Id }, result);
        }

        #region helper methods

        private string CreatePostUri(ArticleParameters parameters, PaginationResourceUriType uriType)
        {
            switch (uriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    var previousParameters = new
                    {
                        pageIndex = parameters.PageIndex - 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        title = parameters.Title
                    };
                    return _urlHelper.Link("GetPosts", previousParameters);
                case PaginationResourceUriType.NextPage:
                    var nextParameters = new
                    {
                        pageIndex = parameters.PageIndex + 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        title = parameters.Title
                    };
                    return _urlHelper.Link("GetPosts", nextParameters);
                default:
                    var currentParameters = new
                    {
                        pageIndex = parameters.PageIndex,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        title = parameters.Title
                    };
                    return _urlHelper.Link("GetPosts", currentParameters);
            }
        }

        private IEnumerable<LinkResource> CreateLinksForPost(int id, string fields = null)
        {
            var links = new List<LinkResource>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkResource(
                        _urlHelper.Link("GetPost", new { id }), "self", "GET"));
            }
            else
            {
                links.Add(
                    new LinkResource(
                        _urlHelper.Link("GetPost", new { id, fields }), "self", "GET"));
            }

            links.Add(
                new LinkResource(
                    _urlHelper.Link("DeletePost", new { id }), "delete_post", "DELETE"));

            return links;
        }

        private IEnumerable<LinkResource> CreateLinksForPosts(ArticleParameters parameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<LinkResource>
            {
                new LinkResource(
                    CreatePostUri(parameters, PaginationResourceUriType.CurrentPage),
                    "self", "GET")
            };

            if (hasPrevious)
            {
                links.Add(
                    new LinkResource(
                        CreatePostUri(parameters, PaginationResourceUriType.PreviousPage),
                        "previous_page", "GET"));
            }

            if (hasNext)
            {
                links.Add(
                    new LinkResource(
                        CreatePostUri(parameters, PaginationResourceUriType.NextPage),
                        "next_page", "GET"));
            }

            return links;
        }

        #endregion
    }
}