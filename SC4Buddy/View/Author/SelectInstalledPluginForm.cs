﻿namespace NIHEI.SC4Buddy.View.Author
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    using NIHEI.SC4Buddy.Control.Plugins;
    using NIHEI.SC4Buddy.Control.UserFolders;
    using NIHEI.SC4Buddy.Entities;
    using NIHEI.SC4Buddy.View.Elements;

    public partial class SelectInstalledPluginForm : Form
    {
        private readonly UserFolderController userFolderController;

        private readonly PluginController pluginController;

        public SelectInstalledPluginForm(UserFolderController userFolderController, PluginController pluginController)
        {
            this.userFolderController = userFolderController;

            this.pluginController = pluginController;

            InitializeComponent();
        }

        public Plugin SelectedPlugin { get; set; }

        public bool IncludeInformation { get; set; }

        private void SelectInstalledPluginFormLoad(object sender, EventArgs e)
        {
            var userFolders = userFolderController.UserFolders.Where(x => x.Plugins.Any()).ToList();

            userFolderComboBox.Enabled = userFolders.Any();

            userFolderComboBox.BeginUpdate();
            foreach (var userFolder in userFolders)
            {
                userFolderComboBox.Items.Add(new ComboBoxItem<UserFolder>(userFolder.Alias, userFolder));
            }

            pluginComboBox.Enabled = false;
            userFolderComboBox.EndUpdate();
        }

        private void UserFolderComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (userFolderComboBox.SelectedItem != null)
            {
                pluginComboBox.BeginUpdate();
                pluginComboBox.Items.Clear();
                pluginComboBox.Enabled = true;

                var userFolder = ((ComboBoxItem<UserFolder>)userFolderComboBox.SelectedItem).Value;
                var plugins = pluginController.Plugins.Where(x => x.UserFolderId == userFolder.Id);

                foreach (var plugin in plugins)
                {
                    pluginComboBox.Items.Add(new ComboBoxItem<Plugin>(plugin.Name, plugin));
                }

                pluginComboBox.EndUpdate();
            }
            else
            {
                pluginComboBox.Enabled = false;
            }
        }

        private void PluginComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            var plugin = ((ComboBoxItem<Plugin>)pluginComboBox.SelectedItem).Value;

            okButton.Enabled = plugin != null;
            includeInformationCheckBox.Checked = plugin != null;
        }

        private void IncludeInformationCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            IncludeInformation = includeInformationCheckBox.Checked;
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            var plugin = ((ComboBoxItem<Plugin>)pluginComboBox.SelectedItem).Value;

            SelectedPlugin = plugin;

            Close();
        }
    }
}
