﻿namespace NIHEI.SC4Buddy.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.Policy;

    using log4net;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NIHEI.SC4Buddy.Model;

    using RemotePlugin = Irradiated.Sc4Buddy.ApiClient.Model.Plugin;
    using RemotePluginFile = Irradiated.Sc4Buddy.ApiClient.Model.PluginFile;

    public class Entities : IEntities
    {
        private const string PluginsFilename = "Plugins.json";

        private const string PluginFilesFilename = "PluginFiles.json";

        private const string PluginGroupsFilename = "PluginGroups.json";

        private const string UserFoldersFilename = "UserFolders.json";

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Entities(string storageLocation)
        {
            StorageLocation = storageLocation;
        }

        public ICollection<Plugin> Plugins { get; private set; }

        public ICollection<PluginFile> Files { get; private set; }

        public ICollection<UserFolder> UserFolders { get; private set; }

        public ICollection<PluginGroup> Groups { get; private set; }

        private string StorageLocation { get; set; }

        private string PluginsLocation
        {
            get
            {
                return Path.Combine(StorageLocation, PluginsFilename);
            }
        }

        private string PluginFilesLocation
        {
            get
            {
                return Path.Combine(StorageLocation, PluginFilesFilename);
            }
        }

        private string PluginGroupsLocation
        {
            get
            {
                return Path.Combine(StorageLocation, PluginGroupsFilename);
            }
        }

        private string UserFoldersLocation
        {
            get
            {
                return Path.Combine(StorageLocation, UserFoldersFilename);
            }
        }

        public void SaveChanges()
        {
            StoreDataInFile(Plugins, PluginsLocation);
            StoreDataInFile(Files, PluginFilesLocation);
            StoreDataInFile(UserFolders, UserFoldersLocation);
            StoreDataInFile(Groups, PluginGroupsLocation);
        }

        public void RevertChanges(ModelBase entityObject)
        {
        }

        public void RevertChanges(IEnumerable<ModelBase> entityCollection)
        {
        }

        public void Dispose()
        {
        }

        public void LoadAllEntitiesFromDisc()
        {
            UserFolders = new Collection<UserFolder>();
            Groups = new Collection<PluginGroup>();
            Plugins = new Collection<Plugin>();
            Files = new Collection<PluginFile>();

            LoadUserFolders(UserFoldersLocation);
            LoadPluginGroups(PluginGroupsLocation);
            LoadPlugins(PluginsLocation);
            LoadPluginFiles(PluginFilesLocation);
        }

        private static void StoreDataInFile(IEnumerable<ModelBase> data, string dataLocation)
        {
            if (!data.Any())
            {
                Log.Info(string.Format("Empty collection for {0}, skipping storage.", dataLocation));
                return;
            }

            var fileInfo = new FileInfo(dataLocation);
            if (fileInfo.DirectoryName == null)
            {
                throw new DirectoryNotFoundException(string.Format("The location string {0} does not contain a directory name.", dataLocation));
            }

            Directory.CreateDirectory(fileInfo.DirectoryName);

            using (var writer = new StreamWriter(dataLocation))
            {
                var json = JsonConvert.SerializeObject(data);
                writer.Write(json);
            }
        }

        private void LoadPluginFiles(string fileLocation)
        {
            if (!File.Exists(fileLocation))
            {
                return;
            }

            using (var reader = new StreamReader(fileLocation))
            {
                var json = reader.ReadToEnd();

                dynamic dynamicPluginFiles = JArray.Parse(json);

                foreach (var dynamicPluginFile in dynamicPluginFiles)
                {
                    var file = new PluginFile
                                   {
                                       Id = dynamicPluginFile.Id,
                                       Checksum = dynamicPluginFile.Checksum,
                                       Path = dynamicPluginFile.Path,
                                       Plugin = Plugins.First(x => x.Id == (Guid)dynamicPluginFile.PluginId)
                                   };

                    if (dynamicPluginFile.QuarantinedFile != null)
                    {
                        var quarantinedFile = new QuarantinedFile
                                                  {
                                                      Id = dynamicPluginFile.QuarantinedFile.Id,
                                                      PluginFile = file,
                                                      QuarantinedPath = dynamicPluginFile.QuarantinedFile.QuarantinedPath
                                                  };
                        file.QuarantinedFile = quarantinedFile;
                    }

                    file.Plugin.PluginFiles.Remove(file);
                    file.Plugin.PluginFiles.Add(file);

                    Files.Add(file);
                }
            }
        }

        private void LoadPlugins(string fileLocation)
        {
            if (!File.Exists(fileLocation))
            {
                return;
            }

            using (var reader = new StreamReader(fileLocation))
            {
                var json = reader.ReadToEnd();

                dynamic dynamicPlugins = JArray.Parse(json);

                foreach (var dynamicPlugin in dynamicPlugins)
                {
                    var plugin = new Plugin
                                     {
                                         Id = dynamicPlugin.Id,
                                         Author = dynamicPlugin.Author,
                                         Description = dynamicPlugin.Description,
                                         Name = dynamicPlugin.Name,
                                         UserFolder = UserFolders.First(x => x.Id == (Guid)dynamicPlugin.UserFolderId)
                                     };

                    if (dynamicPlugin.Link != null)
                    {
                        string link = dynamicPlugin.Link.Value;
                        plugin.Link = new Url(link);
                    }

                    if (dynamicPlugin.PluginGroupId != Guid.Empty)
                    {
                        plugin.PluginGroup = Groups.First(x => x.Id == (Guid)dynamicPlugin.PluginGroupId);
                    }

                    if (dynamicPlugin.RemotePluginId > 0)
                    {
                        plugin.RemotePlugin = new RemotePlugin { Id = dynamicPlugin.RemotePluginId };
                    }

                    var files = new Collection<PluginFile>();

                    foreach (var pluginFileId in dynamicPlugin.PluginFileIds)
                    {
                        files.Add(new PluginFile { Id = pluginFileId });
                    }

                    plugin.PluginFiles = files;

                    UserFolders.First(x => x.Id == plugin.UserFolderId).Plugins.Remove(plugin);
                    UserFolders.First(x => x.Id == plugin.UserFolderId).Plugins.Add(plugin);

                    Plugins.Add(plugin);

                    if (plugin.PluginGroupId != Guid.Empty)
                    {
                        Groups.First(x => x.Id == plugin.PluginGroupId).Plugins.Remove(plugin);
                        Groups.First(x => x.Id == plugin.PluginGroupId).Plugins.Add(plugin);
                    }
                }
            }
        }

        private void LoadPluginGroups(string fileLocation)
        {
            if (!File.Exists(fileLocation))
            {
                return;
            }

            using (var reader = new StreamReader(fileLocation))
            {
                var json = reader.ReadToEnd();

                dynamic dynamicGroups = JArray.Parse(json);

                foreach (var dynamicGroup in dynamicGroups)
                {
                    var pluginGroup = new PluginGroup { Id = dynamicGroup.Id, Name = dynamicGroup.Name };
                    var groupPlugins = new Collection<Plugin>();

                    foreach (var pluginId in dynamicGroup.PluginIds)
                    {
                        groupPlugins.Add(new Plugin { Id = pluginId });
                    }

                    pluginGroup.Plugins = groupPlugins;
                    Groups.Add(pluginGroup);
                }
            }
        }

        private void LoadUserFolders(string fileLocation)
        {
            if (!File.Exists(fileLocation))
            {
                return;
            }

            using (var reader = new StreamReader(fileLocation))
            {
                var json = reader.ReadToEnd();

                dynamic dynamicUserFolders = JArray.Parse(json);

                foreach (var dynamicUserFolder in dynamicUserFolders)
                {
                    var userFolder = new UserFolder
                                         {
                                             Id = dynamicUserFolder.Id,
                                             Alias = dynamicUserFolder.Alias,
                                             FolderPath = dynamicUserFolder.FolderPath,
                                             IsMainFolder = dynamicUserFolder.IsMainFolder,
                                             IsStartupFolder = dynamicUserFolder.IsStartupFolder
                                         };

                    var pluginIds = new Collection<Plugin>();

                    foreach (var pluginId in dynamicUserFolder.PluginIds)
                    {
                        pluginIds.Add(new Plugin { Id = pluginId });
                    }

                    userFolder.Plugins = pluginIds;

                    UserFolders.Add(userFolder);
                }
            }
        }
    }
}
