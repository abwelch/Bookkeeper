using System;
using System.Collections.Generic;
using System.Linq;
using Bookkeeper.Data;
using System.Threading.Tasks;


namespace Bookkeeper.Models
{
    public interface IUserInfoUtils
    {
        public UserInfo GetMostRecentUser();
    }

    public class UserInfoUtils : IUserInfoUtils
    {
        private readonly BookkeeperContext dbContext;
        public UserInfoUtils(BookkeeperContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public UserInfo GetMostRecentUser() =>
            dbContext.UserInfos.OrderByDescending(p => p.UserID).FirstOrDefault();
    }
}
