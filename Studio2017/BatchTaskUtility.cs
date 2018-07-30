/*
 * Created by Ranorex
 * User: astan
 * Date: 06.02.2018
 * Time: 16:57
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
    /// Description of BatchTaskUtility.
    /// </summary>
    [TestModule("6631FB35-D7BB-4017-94DA-2C5066FEE2E7", ModuleType.UserCode, 1)]
    public class BatchTaskUtility
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public BatchTaskUtility()
        {
            // Do not delete - a parameterless constructor is required!
        }

		private static Studio2017Repository repo = Studio2017Repository.Instance;
		ViewsUtility views = new ViewsUtility();
		
		
		public void runBatchTask(string batchTask, string projectName) {
			views.selectProject(projectName);
			repo.StudioWindowForm.FileViewGrid.Click(System.Windows.Forms.MouseButtons.Right, new Location(1, 1));
			repo.RightClickMenu.BatchTasks.Click();
			repo.BatchTaskMenu.RunTaskInfo.Path = "//container[@automationid='&Batch Tasks']//menuitem[@text='" + batchTask + "']";
			repo.BatchTaskMenu.RunTask.Click();
		}
		
		public bool batchTaskIsDone() {
			repo.ProjectWizard.BatchTaskIsFinishedInfo.WaitForExists(Constants.MaxWait);
			if (repo.ProjectWizard.FinishProject.Element.GetAttributeValue("enabled").Equals(true) && repo.ProjectWizard.BatchTasksRestartPreparationInfo.Exists()) {
			return true;
			}
			else {
			return false;	
			}
		}
		
		

    }
}
