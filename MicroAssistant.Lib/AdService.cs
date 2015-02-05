using MicroAssistant.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroAssistant.Lib
{
    public class AdServices
    {
          MicroAssistantEntities objcontext = new MicroAssistantEntities();
          private static volatile AdServices instance;
        private static object sync = new object();

        private AdServices()
        {
        }
        /// <summary>
        /// Gets the Single Instance of JobPortalService
        /// </summary>
        public static AdServices Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new AdServices();
                        }
                    }
                }
                return instance;
            }
        }
        /// <summary>
        /// Get Parent Category of Specialization
        /// </summary>
        /// <returns></returns>
        /// 
        public IList<Category> GetCategories()
        {
            return DataHelper.GetList<Category>().Where(obj => obj.IsDeleted == false).OrderBy(obj => obj.Name).ToList();
        }
        public IList<State> GetStates()
        {
            return DataHelper.GetList<State>().Where(obj => obj.IsActive == 0).OrderBy(obj => obj.Name).ToList();
        }
        public void adService(long Id, string strTitle, string strdescs, string strblurb, byte[] strphoto, long CategoryId, long StateId, string strCity, string strCreatedBy, string strRemark)
        {
            objcontext.SP_MicroAssistant_Services_Post(Id, strTitle, strdescs, strblurb, strphoto, CategoryId, strCity, StateId, 0, strCreatedBy,strRemark);
        }

    }
}
