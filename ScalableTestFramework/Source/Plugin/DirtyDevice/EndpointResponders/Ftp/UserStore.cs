using System;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class UserStore
    {
        private List<FtpUser> _userList = new List<FtpUser>();

        public void Add(string username, string password, string homeDirectory)
        {
            if (null == username)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (null == password)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (null == homeDirectory)
            {
                throw new ArgumentNullException(nameof(homeDirectory));
            }

            if (_userList.Any(u => u.UserName.ToLower() == username?.ToLower()))
            {
                throw new ArgumentException($"Cannot add duplicate username. ({username})");
            }

            _userList.Add(new FtpUser(username, password, homeDirectory));
        }

        public FtpUser Validate(string username, string password)
        {
            var user = _userList.SingleOrDefault(u => u.UserName == username && u.Password == password);

            return user;
        }
    }
}
