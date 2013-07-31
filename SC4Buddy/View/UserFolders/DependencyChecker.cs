﻿namespace NIHEI.SC4Buddy.View.UserFolders
{
    using System.Collections.Generic;
    using System.Linq;

    using NIHEI.SC4Buddy.Control.UserFolders;
    using NIHEI.SC4Buddy.DataAccess;
    using NIHEI.SC4Buddy.DataAccess.Remote;
    using NIHEI.SC4Buddy.Entities;
    using NIHEI.SC4Buddy.Entities.Remote;

    public class DependencyChecker
    {
        public List<RemotePlugin> CheckDependencies(UserFolder userFolder)
        {
            var missingDependencies = GetMissingDependenciesForFolder(userFolder);

            var controller = new UserFolderController(RegistryFactory.UserFolderRegistry);

            if (!controller.IsMainFolder(userFolder))
            {
                var mainMissingDependencies = GetMissingDependenciesForFolder(controller.GetMainUserFolder());

                var extraMissing = mainMissingDependencies.Where(mainMissing => !missingDependencies.Any()).ToList();

                missingDependencies.AddRange(extraMissing);
            }

            return missingDependencies;
        }

        private static List<RemotePlugin> GetMissingDependenciesForFolder(UserFolder userFolder)
        {
            var knownPlugins = userFolder.Plugins.Where(x => x.RemotePluginId > 0).ToList();

            var remotePluginRegistry = RemoteRegistryFactory.RemotePluginRegistry;

            var remotePluginsWithDependencies = new List<RemotePlugin>(knownPlugins.Count());
            remotePluginsWithDependencies.AddRange(
                knownPlugins.Select(
                    knownPlugin =>
                    remotePluginRegistry.RemotePlugins.FirstOrDefault(
                        x => x.Id == knownPlugin.RemotePluginId && x.Dependencies.Any())));

            var missingDependencies =
                (remotePluginsWithDependencies.SelectMany(
                    remotePlugin => remotePlugin.Dependencies, (remotePlugin, dependency) => new { remotePlugin, dependency })
                                              .Where(@t => knownPlugins.All(x => x.RemotePluginId != @t.dependency.Id))
                                              .Select(@t => @t.dependency)).ToList();
            return missingDependencies;
        }
    }
}