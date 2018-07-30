/*
 * Created by Ranorex
 * User: astan
 * Date: 29.01.2018
 * Time: 13:59
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
    /// Description of TranslationUtility.
    /// </summary>
    [TestModule("66F35D13-3DC5-406B-A7AD-DB4FADDD1830", ModuleType.UserCode, 1)]
    public class ViewsUtility
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ViewsUtility()
        {
            // Do not delete - a parameterless constructor is required!
        }
		
        private static Studio2017Repository repo = Studio2017Repository.Instance;
        
        public void goToFilesView() {
        	repo.StudioWindowForm.FilesView.Click();
        }
        
        public void includeSubfolders(string state) {
        	if (state == "check") {
        		if (repo.StudioWindowForm.IncludeSubfolders.Element.GetAttributeValue("checked").Equals(true)) {
        		//Checkbox already checked
        		}
        		
        		else {
        			repo.StudioWindowForm.IncludeSubfolders.Click();
        		}
        	}
        	
        	else if (state == "uncheck") {
        		if (repo.StudioWindowForm.IncludeSubfolders.Element.GetAttributeValue("checked").Equals(false)) {
        		//Checkbox already unchecked
        		}
        		else {
        			repo.StudioWindowForm.IncludeSubfolders.Click();
        		}
        	}
        }
        
        public void selectProject(string projectName) {
        	repo.StudioWindowForm.ProjectView.Click();
        	repo.StudioWindowForm.FileViewGrid.Click();
        	repo.StudioWindowForm.FileViewGrid.Click(new Location(1, 1));
        	var projectDetailsName = repo.StudioWindowForm.ProjectName.Element.GetAttributeValueText("uiautomationvaluevalue");
        	System.DateTime start = System.DateTime.Now;
        	while (!projectDetailsName.Contains(projectName) && System.DateTime.Now.Subtract(start).Seconds < 10) {
        	Keyboard.Press("{Down}");
        	projectDetailsName = repo.StudioWindowForm.ProjectName.Element.GetAttributeValueText("uiautomationvaluevalue");
        	}
        	if (!projectDetailsName.Contains(projectName)) {
        		Report.Failure("Fail", "Project" + projectName + "was not found");
        	}
        	Keyboard.Press("{Return}");
        }
        
        public void openFileForTranslation(string fileName) {
        	repo.StudioWindowForm.FilesView.Click();
            repo.StudioWindowForm.FileViewGrid.Click();
        	repo.StudioWindowForm.FileViewGrid.Click(new Location(1, 1));
        	var fileDetailsName = repo.StudioWindowForm.FileName.Element.GetAttributeValueText("uiautomationvaluevalue");
        	while (!fileDetailsName.Contains(fileName)) {
        		Keyboard.Press("{Down}");
        		fileDetailsName = repo.StudioWindowForm.FileName.Element.GetAttributeValueText("uiautomationvaluevalue");
        	}
        	Keyboard.Press("{Return}");
        }
    }
}
