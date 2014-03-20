namespace NIHEI.SC4Buddy.View.Elements
{
    using System.Windows.Forms;

    using NIHEI.SC4Buddy.Model;

    public class UserFolderListViewItem : ListViewItem
    {
        public UserFolderListViewItem(UserFolder userFolder)
        {
            Text = userFolder.Alias;
            Name = userFolder.Id.ToString();
            UserFolder = userFolder;
        }

        public UserFolder UserFolder { get; set; }
    }
}
