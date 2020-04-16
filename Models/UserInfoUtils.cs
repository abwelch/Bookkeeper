using System;
using System.Collections.Generic;
using System.Linq;
using Bookkeeper.Data;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace Bookkeeper.Models
{
    public interface IUserInfoUtils
    {
        public UserInfo GetMostRecentUser();
        public void IncrementTotalTransactions(int UserID);
        public bool UserAtMaxTransactions(int userID);
    }

    public class UserInfoUtils : IUserInfoUtils
    {
        private readonly BookkeeperContext dbContext;
        private readonly IDbConnection dbConnection;
        public UserInfoUtils(BookkeeperContext _dbContext, IDbConnection _dbConnection)
        {
            dbContext = _dbContext;
            dbConnection = _dbConnection;
        }
        public UserInfo GetMostRecentUser() =>
            dbContext.UserInfos.OrderByDescending(p => p.UserID).FirstOrDefault();

        public void IncrementTotalTransactions(int UserID)
        {
            string retrieveUser = $"SELECT * FROM [dbo].[UserInfos] WHERE UserID = {UserID}";
            var user = dbConnection.QueryFirstOrDefault<UserInfo>(retrieveUser);
            if (user != null)
            {
                user.TotalCurrentTransactions++;
            }
            dbContext.UserInfos.Update(user);
            dbContext.SaveChanges();
        }

        public bool UserAtMaxTransactions(int userID)
        {
            string retrieveUser = $"SELECT * FROM [dbo].[UserInfos] WHERE UserID = {userID}";
            var user = dbConnection.QueryFirstOrDefault<UserInfo>(retrieveUser);
            if (user.TotalCurrentTransactions >= 150)
            {
                return true;
            }
            return false;
        }
    }
}
