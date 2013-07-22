﻿namespace NIHEI.SC4Buddy.DataAccess.Remote
{
    using System.Collections.Generic;
    using System.Data.Objects;

    using NIHEI.SC4Buddy.Entities.Remote;

    public class RemotePluginRegistry
    {
        private readonly RemoteEntities entities;

        public RemotePluginRegistry(RemoteEntities entities)
        {
            this.entities = entities;
        }

        public ObjectSet<RemotePlugin> RemotePlugins
        {
            get
            {
                return entities.RemotePlugins;
            }
        }

        public void Add(RemotePlugin remotePlugin)
        {
            entities.RemotePlugins.AddObject(remotePlugin);
            entities.SaveChanges();
        }

        public void Update(RemotePlugin dependency)
        {
            entities.SaveChanges();
        }
    }
}
