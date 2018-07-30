/*
 * Created by Ranorex
 * User: astan
 * Date: 19.01.2018
 * Time: 16:08
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;
using System.Linq;
using System.IO;
using System.IO.Compression;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace Studio2017
{
    /// <summary>
    /// Description of ActivateStudio.
    /// </summary>
    [TestModule("79E43D7F-E8C5-42AD-9057-7C27567FA48A", ModuleType.UserCode, 1)]
    public class UtilityMethods
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public UtilityMethods()
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
        
        public void setTestRunSettings() {
            Mouse.DefaultMoveTime = 0;
            Keyboard.DefaultKeyPressTime = 0;
            Mouse.DefaultClickTime = 0;
            Delay.SpeedFactor = 0.0;
        }
        
        
        public void studioActivation(string activationServer) {
        	repo.LicenseManagerForm.ButtonActivateButton.Click();
			repo.LicenseManagerForm.AlternativeActivationOptions.Click();
			repo.LicenseManagerForm.ButtonLicenseServerButton.Click();
			repo.LicenseManagerForm.Text.TextValue = activationServer;
			repo.LicenseManagerForm.ButtonConnectButton.Click();
			repo.LicenseManagerForm.ButtonOkCancelButton.Click();
        }
        
        public void firstSetup(string email) {
			repo.ProjectWizard.NextButton.Click();
			repo.StudioInstallation.EmailAddress.TextValue = email;
			repo.ProjectWizard.NextButton.Click();
			repo.ProjectWizard.NextButton.Click();
			repo.StudioInstallation.NoCustomerExperience.Click();
			repo.ProjectWizard.NextButton.Click();
			repo.ProjectWizard.FinishProject.Click();
        }
        
        public void closeWalkMe() {
        	repo.ProjectWizard.SeeWhatsNewInfo.WaitForExists(Constants.MaxWait);
        	Keyboard.Press("{Escape}");
        }
        
        
        public void turnOffAutomaticUpdates() {
        	repo.StudioWindowForm.ApplicationMenuButton.Click();
			repo.StudioWindowForm.IGOptions.Click();
			repo.SettingsDialogForm.AutomaticUpdates.Click();
			repo.SettingsDialogForm.RadioButtonManuallyCheckForUpdatesOption.Click();
			repo.SettingsDialogForm.OkButton.Click();
			repo.StudioWindowForm.Close.Click();
        }
        
        public void startStudio(bool firstTime) {
        	
        	if (firstTime) {
				Host.Local.RunApplication(Constants.StudioAppPath);
				//repo.LicenseManagerForm.ButtonActivateButtonInfo.WaitForExists(Constants.MaxWait);
        	}
        	else {
			ProcessStartInfo startInfo = new ProcessStartInfo(Constants.StudioAppPath);
			startInfo.WindowStyle = ProcessWindowStyle.Maximized;
			Process.Start(startInfo);        	
        	}
		}
        
        public void goToProductActivation() {
        	repo.StudioWindowForm.HelpRibbon.Click();
			repo.StudioWindowForm.ProductActivation.Click();
       }
        
        public void deactivateButton() {
        	repo.LicenseManagerForm.ButtonDeactivateButton.Click();
        }
        
        public void closeStudio() {
        	repo.StudioWindowForm.Close.Click();
        }
        
        public void deleteRegistry(string registryPath, string entry) {
           using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(registryPath, true))
			{
			if (key == null || key.SubKeyCount == 0) {}
			
			else {
				key.DeleteSubKey(entry);
			  }
			}
        }
        
        public void deleteFiles(List<string> filesToDelete, string path) {
        	foreach (string file in filesToDelete) {
        		if (File.Exists(path+file)) {
        			File.Delete(path+file);
        		}
        }
        
        
        }
        
        public string projectNameRandom() {
        	Random random = new Random();
        	int projectNr = random.Next(1, Constants.MinWait);
        	string projectName = "AutomationProject" + projectNr.ToString();
        	return projectName;
        }
        
        public void goToServers() {
        	repo.StudioWindowForm.ApplicationMenuButton.Click();
        	repo.StudioWindowForm.IGSetup.Click();
        	repo.StudioWindowForm.IGServers.Click();
        }
        
        public void addServerButton() {
        	repo.ServersDialog.AddServer.Click();
        }
        
        public void closeServersDialog() {
        	repo.ServersDialog.CloseButton.Click();
        }
        
        public void provideGSDetails(string serverAddress, string username, string password) {
        
        	repo.AddEditServerDialog.ServerAddress.TextValue = serverAddress;
        	repo.AddEditServerDialog.UseSDLAuthentication.Click();
        	repo.AddEditServerDialog.UserName.TextValue = username;
        	repo.AddEditServerDialog.Password.TextValue = password;
        }
        
        public void saveGSServer() {
        	repo.AddEditServerDialog.OkButton.Click();
        	repo.ServersDialog.GSNameInfo.WaitForExists(Constants.MediumWait);
        }
        
        public void deleteGSServer(string serverAddress) {
            IList<Ranorex.TreeItem> listedServers = repo.ServersDialog.Self.FindDescendants<Ranorex.TreeItem>();
        	//var extractedServers = new List<string>();	
        	foreach (Ranorex.TreeItem servers in listedServers) {
        		if (servers.Text.Contains(serverAddress)) {
        			servers.Click();
        			break;
        		}
        		else {
        		//Server is not in the list
        		}
        	}
        	repo.ServersDialog.DeleteGSServer.Click();
        	repo.Question.ButtonYes.Click();
        }
        
        public void addGSServer(string serverAddress, string username, string password) {
        	goToServers();
        	if (gsServerExists(serverAddress) == false) {
        	addServerButton();
        	provideGSDetails(serverAddress, username, password);
        	saveGSServer();
        	   if (gsServerExists(serverAddress) == true) {
        		   Report.Success("Success", "Server " + serverAddress + " was added");
        	   }
        	   else {
        		   Report.Failure("Fail", "Server " + serverAddress + " was not added");
        	   }
        	   closeServersDialog();
        	}
        	else {
        		deleteGSServer(serverAddress);
        		addServerButton();
        		provideGSDetails(serverAddress, username, password);
        		saveGSServer();
      			if (gsServerExists(serverAddress) == true) {
        		   	Report.Success("Success", "Server " + serverAddress + " was added");
        	   	}
        	   	else {
        		   	Report.Failure("Fail", "Server " + serverAddress + " was not added");
        	   	}
        		closeServersDialog();
        	}
        }
        
        public bool gsServerExists(string gsServer) {
        	repo.ServersDialog.AddServerInfo.WaitForExists(Constants.MinWait);
        	bool serverExists = false;
            IList<Ranorex.TreeItem> listedServers = repo.ServersDialog.Self.FindDescendants<Ranorex.TreeItem>();
        	//var extractedServers = new List<string>();	
        	foreach (Ranorex.TreeItem servers in listedServers) {
        		if (servers.Text.Contains(gsServer)) {
        		serverExists = true;
        		break;
        		}
        		else {
        		serverExists = false;
        		}
        	}
			return serverExists;       	
        }
        
        
        public void installStudio(string studioExe) {
        	Host.Local.RunApplication(studioExe);
            
            repo.StudioInstallation.Accept.Click();
            
            repo.StudioInstallation.BackPanel.CheckBoxAcceptInfo.WaitForExists(60000);
            repo.StudioInstallation.BackPanel.CheckBoxAcceptInfo.WaitForAttributeEqual(Constants.CasualWait, "enabled", true);
            repo.StudioInstallation.BackPanel.CheckBoxAccept.Click();
            
            repo.StudioInstallation.BackPanel.ButtonNextInfo.WaitForAttributeEqual(Constants.CasualWait, "enabled", true);
            repo.StudioInstallation.BackPanel.ButtonNext.Click();
			
            
            repo.StudioInstallation.SetupCompletedInfo.WaitForExists(Constants.MaxWait);
            
            repo.StudioInstallation.ButtonOK.Click();
        }
        
        
        public void deleteStudioProjects(string projectsFolder) {    
        var directory = new DirectoryInfo(projectsFolder).GetDirectories(Constants.AutomationProjects);
        foreach (DirectoryInfo dir in directory) {
        	
        	dir.Delete(true);
          }
        }
        
        public void openBrowser(string linkToOpen, string browser, string browserArgs) {
        	int processId = Host.Local.OpenBrowser(linkToOpen, browser, browserArgs, true, true);
        }
        
        public void closeBrowser(string browserName) {
        	Host.Local.KillBrowser(browserName);
        }
        
        public void closeAppsAndCleanProjects(string projectsFolder) {
        	foreach (Process proc in Process.GetProcessesByName("SDLTradosStudio")) {
        		proc.Kill();
        	}
        	deleteStudioProjects(projectsFolder);
        }
        
        public bool waitFileToExist(string pathToFile) {
        	System.DateTime finalizeStartTime = System.DateTime.Now;
			while (!System.IO.File.Exists(pathToFile) && System.DateTime.Now.Subtract(finalizeStartTime).Seconds < 20) {
				Console.WriteLine("Target file " + pathToFile + " is not present yet");
			}
			bool fileIsAvailable = System.IO.File.Exists(pathToFile);
			if (fileIsAvailable) {
				return true;
			}
			else {
				return false;
			}
        }
     }
}
