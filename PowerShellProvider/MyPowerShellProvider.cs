using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Text;
using System.Threading.Tasks;

namespace PowerShellProvider
{
    [CmdletProvider("MyPowerShellProvider", ProviderCapabilities.None)]
    public class MyPowerShellProvider : NavigationCmdletProvider
    {

        protected override bool IsValidPath(string path)
        {
            return true;
        }

        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            PSDriveInfo drive = new PSDriveInfo("MyDrive", this.ProviderInfo, "", "", null);
            Collection<PSDriveInfo> drives = new Collection<PSDriveInfo>() {drive};
            return drives;
        }

        protected override bool ItemExists(string path)
        {
            return true;
        }

        protected override bool IsItemContainer(string path)
        {
            return true;
        }

        protected override void GetChildItems(string path, bool recurse)
        {

            var stackOverflow = new StackOverflowAPI.StackOverflow();
            var tags = stackOverflow.GetTags().Result;

            foreach (var tag in tags)
            {
                WriteItemObject(tag, tag.name,true);
            }
        }
    }
}
