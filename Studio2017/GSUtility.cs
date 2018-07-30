/*
 * Created by Ranorex
 * User: astan
 * Date: 24.05.2018
 * Time: 16:13
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
    /// Description of GSUtility.
    /// </summary>
    [TestModule("06C521A1-C545-402E-9794-592160D4D0F4", ModuleType.UserCode, 1)]
    public class GSUtility
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public GSUtility()
        {
            // Do not delete - a parameterless constructor is required!
        }

        private static Studio2017Repository repo = Studio2017Repository.Instance;

        public void WebGSLogin(string username, string password)
        {
			repo.GroupShare.LoginView.Username.Value = username;
			repo.GroupShare.LoginView.Password.Value = password;
			repo.GroupShare.LoginView.LogIn.Click();
			repo.GroupShare.LoadingWheelInfo.WaitForNotExists(Constants.MediumWait);
        }
    }
}
