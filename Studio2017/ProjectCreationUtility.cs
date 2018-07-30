/*
 * Created by Ranorex
 * User: astan
 * Date: 20.01.2018
 * Time: 22:04
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
    /// Description of ProjectCreation.
    /// </summary>
    [TestModule("E3FFFCDD-5644-41D5-96F8-4D05D19565A0", ModuleType.UserCode, 1)]
    public class ProjectCreationUtility
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProjectCreationUtility()
        {
            // Do not delete - a parameterless constructor is required!
        }

        /// <summary>
        /// Performs the playback of actions in this module.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
		
        private static Studio2017Repository repo = Studio2017Repository.Instance;
        
        public void goToNewProjectWizard() {
        
        	repo.StudioWindowForm.ApplicationMenuButton.Click();
        	repo.StudioWindowForm.IGNew.Click();
        	repo.StudioWindowForm.IGNewProject.Click();
        }
        
        public void saveTargetAs(string pathToSaveFile) {
        	repo.StudioWindowForm.ApplicationMenuButton.Click();
        	repo.StudioWindowForm.IGSaveTargetAs.Click();
        	repo.SaveTargetAs.PathToSaveFileAsTarget.TextValue = pathToSaveFile;
        	repo.SaveTargetAs.ButtonSave.Click();
        }
        
        
        public void createProject(string projectName, bool autoFillState, string projectCreationPath, bool publishGS, string GSServer, string serverLocation, string filesFolderPath, string tmtbLocation, string tmName, string termbaseName, string sourceLanguage, List<string> targetLanguages) {
        	
        	//Turn off Autofill
        	autoFill(autoFillState);
        	
        	//Add source and target languages
        	addSourceLanguage(sourceLanguage);
        	addTargetLanguage(targetLanguages.ToArray());
        	
        	//Add translatable files
        	addFilesFolder(filesFolderPath);
        	
        	
        	//Adds project name
        	addProjectName(projectName);
        	
        	//Add project creation path
        	addProjectPath(projectCreationPath);
        	
        	
        	//Leave one step -> General Page
        	pressNext();
        	
        	//Leave General Page -> Translation Resources
        	pressNext();
        	
        	//Add TMs
        	addTM(tmtbLocation, tmName);
        	
        	//Leave Translation Resources Page -> Termbases Page
        	pressNext();
        	
        	//Add termbases
        	addTermbase(tmtbLocation, termbaseName);
        	
        	
        	//Leave Termbases Page -> Trados GroupShare Page
        	pressNext();
        	
        	
        	//Publish on GroupShare
        	publishOnGroupShare(publishGS, GSServer, serverLocation);
        	
        	//Leave GroupShare Page -> Perfect Match Page
        	pressNext();
        	
        	
        	//Leave Perfect Match Page -> Batch Tasks Page
        	pressNext();
        	
        	
        	//Leave Batch Tasks Page -> Summary Page
        	pressNext();
        	
        	
        	//Going to Preparation
        	pressFinish(); 	
        }
        
        
        public void pressNext() {
        	repo.ProjectWizard.NextButtonInfo.WaitForExists(Constants.MinWait);
        	repo.ProjectWizard.NextButtonInfo.WaitForAttributeEqual(Constants.MinWait, "enabled", true);
        	repo.ProjectWizard.NextButton.Click();
        }
        
        public void pressFinish() {
        	repo.ProjectWizard.FinishProjectInfo.WaitForExists(Constants.MinWait);
        	repo.ProjectWizard.FinishProjectInfo.WaitForAttributeEqual(Constants.MediumWait, "enabled", true);
        	repo.ProjectWizard.FinishProject.Click();
        }
        
        public void addProjectTemplate(){
            //to be developed
        }
        
        public void addProjectPath(string projectPath) {
        	//repo.ProjectWizard.AddFolder.Click();
        	//repo.AddFolder.AddFolderName.TextValue = projectPath;
        	//repo.AddFolder.SelectFolder.Click();
        	repo.ProjectWizard.LocationPathInfo.WaitForExists(Constants.MinWait);
        	repo.ProjectWizard.LocationPath.TextValue = projectPath;
        	repo.ProjectWizard.LocationPath.Click();
        }
        
        
        public void addProjectName(string projectName) {
        	repo.ProjectWizard.ProjectName.TextValue = projectName;
        }
        
        public void addSourceLanguage(string sourceLanguage) {
            repo.ProjectWizard.SourceLanguage.Click();
        	repo.SDLTradosStudio.SelectLanguageSourceInfo.Path = "//contextmenu[@processname='SDLTradosStudio']//text[@caption='" + sourceLanguage + "']";
        	repo.SDLTradosStudio.SelectLanguageSource.Click();
        }
        
        public void addTargetLanguage(string[] targetLanguages) {
            foreach (string language in targetLanguages) {
        		repo.ProjectWizard.TargetLanguage.Click();
            	Keyboard.Press(language);
            	Keyboard.Press("{Return}");
            }	
        }
        
        public void addFilesFolder(string folderPath) {
            repo.ProjectWizard.AddFolder.Click();
        	repo.AddFolder.AddFolderName.TextValue = folderPath;
        	repo.AddFolder.SelectFolder.Click();
        	repo.JobProgressDialog.AddingFilesProgressInfo.WaitForAttributeEqual(Constants.MinWait, "visible", false);
        	repo.ProjectWizard.NextButtonInfo.WaitForAttributeEqual(2000, "enabled", true);   
        }
        
        public void addTM(string tmtbLocation, string tmName) {
            repo.ProjectWizard.Use.Click();
        	repo.SDLTradosStudio.FileBasedTMTB.Click();
        	repo.OpenFileBasedTMTB.AddTMTBName.TextValue = tmtbLocation + tmName;
        	repo.OpenFileBasedTMTB.OpenTMTB.Click();
        	repo.ProjectWizard.DisplayedTMsInfo.Path = "//form[@automationid='Window_1']//container[@automationid='DockPanel_1']//container[@controlname='SettingsUIControl']//container[@controlname='_translationMemoriesControl']//text[@automationid='[Editor] Edit Area' and @uiautomationvaluevalue~'" + tmName + "']";
        	repo.ProjectWizard.DisplayedTMsInfo.WaitForExists(Constants.MinWait);
        }
        
        public void addServerBasedTM(string tmName) {
        
        //to be completed
        
        }
        
        public void addTermbase(string tmtbLocation, string termbaseName) {
            repo.ProjectWizard.Use.Click();
        	repo.SDLTradosStudio.FileBasedTMTB.Click();
        	repo.OpenFileBasedTMTB.AddTMTBName.TextValue = tmtbLocation + termbaseName;
        	repo.OpenFileBasedTMTB.OpenTMTB.Click();
        	repo.ProjectWizard.DisplayedTermbasesInfo.Path = "//form[@automationid='Window_1']//container[@automationid='DockPanel_1']//container[@controlname='ProjectTermbasesWizardPageControl']//container[@controlname='_termbasesGrid']//text[@automationid='[Editor] Edit Area' and @uiautomationvaluevalue~'" + termbaseName + "']";
        	repo.ProjectWizard.DisplayedTermbasesInfo.WaitForExists(Constants.MinWait);
        }
        
        public void publishOnGroupShare(bool publishOnGS, string gsServer, string serverLocation) {
        	repo.ProjectWizard.PublishGroupShareInfo.WaitForExists(Constants.CasualWait);
        	if (publishOnGS) {
        		if (repo.ProjectWizard.PublishGroupShare.Element.GetAttributeValue("checked").Equals(true)) {
        			addServerAndLocation(gsServer, serverLocation);
        		}
        		else {
        			repo.ProjectWizard.PublishGroupShare.Click();
					addServerAndLocation(gsServer, serverLocation);
        		}
        	}
        	else {
        		if (repo.ProjectWizard.PublishGroupShare.Element.GetAttributeValue("checked").Equals(true)) {
        			repo.ProjectWizard.PublishGroupShare.Click();
        		}
        		else {
        		//Publish on GS server checkbox is disabled
        		}
        	}
        }
        
        public void addServerAndLocation(string gsServer, string serverLocation) {
        	repo.ServerList.ServerSelectionInfo.Path = "//list[@controlid='1000']//listitem[@text~'" + gsServer + "']";
        	repo.BrowserDialog.ServerLocationInfo.Path = "//form[@controlname='BrowserDialog']//treeitem[@automationid='0']//text[@automationid='0']//text[@uiautomationvaluevalue='" + serverLocation + "']";
            repo.ProjectWizard.ServerCombobox.Click();
            repo.ServerList.ServerSelection.Click();
        	repo.ProjectWizard.BrowseServerLocation.Click();
        	repo.BrowserDialog.ServerLocation.Click();
        	repo.BrowserDialog.OkButton.Click();
        }
        
        
        public bool projectIsCreated() {
        	repo.ProjectWizard.ProjectIsCreatedInfo.WaitForExists(Constants.MaxWait);
        	if (repo.ProjectWizard.FinishProjectInfo.Exists() && repo.ProjectWizard.RestartPreparationInfo.Exists()) {
        	return true;
        	} 
        	else {
        	return false;
        	}
        }
        
        
        public void autoFill(bool autoFillState) {
        	try {
        	repo.ProjectWizard.AutoFillInfo.WaitForExists(Constants.MinWait);
        	}
        	catch (ElementNotFoundException e) {
        		Console.WriteLine("{0} was found, the project wasn't created", e);
        	}
        	if (autoFillState) {
        		if (repo.ProjectWizard.AutoFill.Element.GetAttributeValue("checked").Equals(true)) {
        			//Autofill is already enabled
        		}
        		else {
        			repo.ProjectWizard.AutoFill.Click();
        		}
        	}
        	else {
        		if (repo.ProjectWizard.AutoFill.Element.GetAttributeValue("checked").Equals(true)) {
        			repo.ProjectWizard.AutoFill.Click();
        		}
        		else {
        			//Autofill is already disabled
        		}
        	}
        }
        
    }
}
