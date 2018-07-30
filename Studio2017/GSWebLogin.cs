/*
 * Created by Ranorex
 * User: astan
 * Date: 24.05.2018
 * Time: 15:09
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
    /// Description of GSWebLogin.
    /// </summary>
    [TestModule("6D2E34BB-DE59-4F97-B747-9EA47A1E4F78", ModuleType.UserCode, 1)]
    public class GSWebLogin : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public GSWebLogin()
        {
            // Do not delete - a parameterless constructor is required!
        }
        
        private static Studio2017Repository repo = Studio2017Repository.Instance;
        UtilityMethods utilityMethods = new UtilityMethods();
        GSUtility gsUtility = new GSUtility();
        
        void ITestModule.Run()
        {
			utilityMethods.setTestRunSettings();
			
			utilityMethods.openBrowser("http:\\cljvmgs2017", "chrome", "");

			gsUtility.WebGSLogin("thaiteam", "Sa1");
        }
    }
}
