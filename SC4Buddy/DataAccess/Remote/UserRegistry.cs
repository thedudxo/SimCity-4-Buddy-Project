﻿namespace NIHEI.SC4Buddy.DataAccess.Remote
{
    using System.Collections.Generic;

    using NIHEI.SC4Buddy.Entities.Remote;

    public class UserRegistry
    {
        private readonly RemoteEntities entities;

        public UserRegistry(RemoteEntities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<User> Users
        {
            get
            {
                return entities.Users;
            }
        }

        public void Add(User user)
        {
            entities.Users.AddObject(user);
        }
    }
}
