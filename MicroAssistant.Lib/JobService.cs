using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroAssistant.Data;

namespace MicroAssistant.Lib
{
    public class JobService
    {
        MicroAssistantEntities objcontext = new MicroAssistantEntities();
        private static volatile JobService instance;
        private static object sync = new object();

        private JobService()
        {
        }
        /// <summary>
        /// Gets the Single Instance of JobPortalService
        /// </summary>
        public static JobService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new JobService();
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
        public void adJob(long Id, string strTitle, string strdescs, string strblurb, byte[] strphoto, string strvideo, Decimal price, string strphone, string strCity, long StateId, long CategoryId, int IsBestOffer, int IsFree, DateTime DateCreated, DateTime DateUpdated, string strCreatedBy, string strRemark)
        {
            objcontext.SP_Job_Post(Id, strTitle, strdescs, strblurb, strphoto, strvideo, price, strphone, strCity, CategoryId, StateId, IsBestOffer, IsFree, DateCreated, DateUpdated, strCreatedBy, strRemark);
        }

    }
}
