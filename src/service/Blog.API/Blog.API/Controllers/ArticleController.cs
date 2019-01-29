﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Common.Extensions;
using Blog.Common.Redis;
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
    [AllowAnonymous]
    //[Authorize(Policy = "Admin")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _service;
        private readonly IRedisCacheManager _cache;
        private readonly IUrlHelper _urlHelper;
        private readonly IPropertyMappingContainer _mappingContainer;
        private readonly ITypeHelperService _typeHelper;
        private readonly IMapper _mapper;
        public ArticleController(IArticleService service,
            IRedisCacheManager cache,
            IUrlHelper urlHelper,
            IPropertyMappingContainer mappingContainer,
            ITypeHelperService typeHelper,
            IMapper mapper
            )
        {
            _service = service;
            _cache = cache;
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

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            return Ok(viewModels.ToDynamicIEnumerable(parameters.Fields));
        }

        // GET: api/Blog/5
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public async Task<object> Get(int id)
        {
            var model = await _service.GetArticle(id);
            return new { succeed = true, data = model };
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