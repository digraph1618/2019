/*
 * Created by Ranorex
 * User: astan
 * Date: 24.05.2018
 * Time: 08:24
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace Studio2017
{
    /// <summary>
    /// Description of CloseAppsAndBrowsers.
    /// </summary>
    [TestModule("192D3FD4-BBF0-46C3-B152-C273FEA11F0A", ModuleType.UserCode, 1)]
    public class CloseAppDeleteProjects : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CloseAppDeleteProjects()
        {
            // Do not delete - a parameterless constructor is required!
        }
        
        UtilityMethods utilityMethods = new UtilityMethods();

        void ITestModule.Run()
        {
        	utilityMethods.closeAppsAndCleanProjects(Constants.ProjectCreationPath);
        }
    }
}
