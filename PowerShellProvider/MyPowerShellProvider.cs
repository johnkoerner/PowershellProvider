using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
