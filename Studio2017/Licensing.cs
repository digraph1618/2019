/*
 * Created by Ranorex
 * User: astan
 * Date: 18.01.2018
 * Time: 14:20
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
	/// Description of Licensing.
	/// </summary>
	[TestModule("887A96F6-73ED-4777-86F6-486EBE7ED380", ModuleType.UserCode, 1)]
	public class Licensing : ITestModule
	{

		public Licensing()
		{
			// Do not delete - a parameterless constructor is required!
		}

		private static Studio2017Repository repo = Studio2017Repository.Instance;
		UtilityMethods utilityMethods = new UtilityMethods();
		List<string> filesToDelete = new List<string>(new string[] { "BaseSettings.xml", "Settings.xml", "UserSettings.xml" });
		
		
		void ITestModule.Run()
		{
			
			utilityMethods.setTestRunSettings();
		
			//Reset first step wizard
			utilityMethods.deleteFiles(filesToDelete, Constants.FirstStepWizardPath);
			
			//Delete registry for First step
			utilityMethods.deleteRegistry(Constants.RegistryPath, Constants.RegistryEntry);

			
			//Start Studio
			utilityMethods.startStudio(Constants.FirstStart);
			
			//Activate Studio
			if (repo.LicenseManagerForm.ButtonActivateButtonInfo.Exists(Constants.CasualWait)) {
				utilityMethods.studioActivation(Constants.LicenseServer);
			}
			
			//First setup
			utilityMethods.firstSetup("@");
			
			
			//Close WalkMe window
			utilityMethods.closeWalkMe();
			
			
			//Turn off Automatic Updates
			utilityMethods.turnOffAutomaticUpdates();
			
			
			//Start Studio
			utilityMethods.startStudio(Constants.NotFirstStart);
			
			
			//Go to Product Activation
			utilityMethods.goToProductActivation();
			
			
			if (repo.LicenseManagerForm.ButtonDeactivateButtonInfo.Exists(Constants.CasualWait)) {
				utilityMethods.deactivateButton();
				utilityMethods.studioActivation(Constants.LicenseServer);
			}
			else {
				utilityMethods.studioActivation(Constants.LicenseServer);
			}
			
			utilityMethods.closeStudio();
		}	
	}
}
