	//----------Advertisement开始----------
    

using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Service.Base;

namespace Blog.Service
{	
	/// <summary>
	/// AdvertisementService
	/// </summary>	
	public class AdvertisementService : BaseService<Advertisement>, IAdvertisementService
    {
	
        IAdvertisementRepository dal;
        public AdvertisementService(IAdvertisementRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       
    }
}

	//----------Advertisement结束----------
	