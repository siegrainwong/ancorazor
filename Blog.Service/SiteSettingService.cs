using Blog.API.Common.Constants;
using Blog.Entity;
using EasyCaching.Core.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service
{
    public class SiteSettingService
    {
        private readonly BlogContext _context;

        public SiteSettingService(BlogContext context)
        {
            _context = context;
        }

        [EasyCachingAble(Expiration = Constants.SiteSetting.Expiration)]
        public virtual SiteSetting GetSetting()
        {
            return _context.SiteSetting.FirstOrDefault();
        }

        [EasyCachingPut]
        public virtual async Task<bool> UpdateSetting(SiteSetting entity)
        {
            _context.SiteSetting.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
