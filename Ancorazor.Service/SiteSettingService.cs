using Ancorazor.API.Common.Constants;
using Ancorazor.API.Messages.Exceptions;
using Ancorazor.API.Messages.Settings;
using Ancorazor.Entity;
using EasyCaching.Core.Interceptor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancorazor.Service
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
            var setting = _context.SiteSetting.FirstOrDefault();
            if (setting == null) throw new EntityNotFoundException<SiteSetting>();
            return setting;
        }

        [EasyCachingPut]
        public virtual async Task<bool> UpdateSetting(SiteSetting entity)
        {
            _context.SiteSetting.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
