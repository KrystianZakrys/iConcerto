using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iConcerto.Models;

namespace iConcerto.Repository
{
    public interface IUserDatasRepository
    {
        UserData GetUserData(int id);
        List<UserData> GetUserDatas();
        bool UpdateUserData();
        bool InsertUserData(UserData userData);
        bool InsertMultipleUserDatas(List<UserData> userDatas);
    }

    public class UserDatasRepository : IUserDatasRepository
    {
        public UserData GetUserData(int id)
        {
            UserData userData = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var query = db.Users.Where(l => l.UserDataId == id);
                if (query.Any())
                    userData = query.First();
            }
            return userData;
        }

        public List<UserData> GetUserDatas()
        {
            using (ApplicationDbContext db = new ApplicationDbContext()) { return db.Users.ToList(); }
        }

       

        public bool InsertUserData(UserData userData)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    db.Users.Add(userData);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }

       
        bool InsertMultipleUserDatas(List<UserData> userDatas)
        {
            throw new NotImplementedException();
        }

        bool IUserDatasRepository.InsertMultipleUserDatas(List<UserData> userDatas)
        {
            throw new NotImplementedException();
        }

        bool UpdateUserData()
        {
            throw new NotImplementedException();
        }

        bool IUserDatasRepository.UpdateUserData()
        {
            throw new NotImplementedException();
        }
    }
}