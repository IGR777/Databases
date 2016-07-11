using App13.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App13
{
    public class DbHelper
    {
        object _locker = new object();

        SQLiteConnection _conn;
        DbHelper()
        {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            _conn = new SQLiteConnection(Path.Combine(path,"myDB"));
            _conn.CreateTable<User>();
        }


        static Lazy<DbHelper> instance = new Lazy<DbHelper>(() => new DbHelper());

        public static DbHelper Instance {
            get
            {
                return instance.Value;
            }
        }

        public IList<User> GetUsers()
        {
            return _conn.Table<User>().ToList();
        }

        public void AddOrUpdateUser(User u)
        {
            lock (_locker)
            {
                var existed = _conn.Table<User>().FirstOrDefault(item => item.FirstName.Equals(u.FirstName) && item.LastName.Equals(u.LastName));
                if (existed == null)
                {
                    _conn.Insert(u);
                }
                else
                {
                    existed.Age = u.Age;
                    existed.Occupation = u.Occupation;
                    _conn.Update(u);
                }
            }
        }


        public User GetUser(int id)
        {
            return _conn.Get<User>(id);
        }

        public void RemoveUser(int id)
        {
            lock (_locker)
            {
                _conn.Delete<User>(id);
            }
        }
    }
}

