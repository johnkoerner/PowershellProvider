using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StackExchange.StacMan;

namespace PowerShellProvider
{
    [CmdletProvider("MyPowerShellProvider", ProviderCapabilities.ExpandWildcards)]
    public class MyPowerShellProvider : NavigationCmdletProvider
    {

        protected override bool IsValidPath(string path)
        {
            return true;
        }

        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            PSDriveInfo drive = new PSDriveInfo("SO", this.ProviderInfo, "", "", null);
            Collection<PSDriveInfo> drives = new Collection<PSDriveInfo>() {drive};
            return drives;
        }

        protected override bool ItemExists(string path)
        {
            if (tags == null)
                return true;

            if (String.IsNullOrEmpty(path))
                return true;

            var itemFromTag = from tag in tags.Data.Items
                              where tag.Name.Equals(path, StringComparison.CurrentCultureIgnoreCase)
                              select tag;

            return itemFromTag.Any();
            
        }

        private string[] TagsFromPath(string path)
        {
            if (tags == null)
                return null;



            var regexString = Regex.Escape(path).Replace("\\*", ".*");
            regexString = "^" + regexString + "$";
            var regex = new Regex(regexString);

            var itemFromTag = from tag in tags.Data.Items
                              where regex.IsMatch(tag.Name)
                              select tag.Name;

            if (itemFromTag.Any())
                return itemFromTag.ToArray();

            return null;
        }


        protected override string[] ExpandPath(string path)
        {
            return TagsFromPath(path);
        }



        private static StacManResponse<Tag> tags;
        private void LoadTags(bool forceReload=false)
        {
            if (tags == null || forceReload)
            {
                var stackOverflow = new StacManClient();
                tags = stackOverflow.Tags.GetAll("stackoverflow.com").Result;
            }
        }

        protected override bool IsItemContainer(string path)
        {
            // The path is empty, so we are at the root.  The root is always valid for us.
            if (String.IsNullOrEmpty(path))
                return true;

            LoadTags();


            if (tags.Data == null)
                return false;

            if (tags.Data.Items == null)
                return false;

            var itemFromTag = from tag in tags.Data.Items
                where tag.Name.Equals(path, StringComparison.CurrentCultureIgnoreCase)
                select tag;

            return itemFromTag.Any();
            
        }


        protected override void GetChildItems(string path, bool recurse)
        {
            if (string.IsNullOrEmpty(path))
            {
                // Write the tags
                LoadTags();

                foreach (var tag in tags.Data.Items)
                {
                    WriteItemObject(tag, tag.Name, false);
                }
            }
            else
            {
                // Write the questions for this tag
                var stackOverflow = new StacManClient();
                var questions = stackOverflow.Questions.GetAll("stackoverflow.com", tagged:path);

                foreach (var q in questions.Result.Data.Items)
                {
                    Debug.WriteLine(q.Owner.Reputation);
                    
                    WriteItemObject(q, q.Title, false);
                }
            }
        }
    }
}
