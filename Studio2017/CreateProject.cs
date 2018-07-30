/*
 * Created by Ranorex
 * User: astan
 * Date: 19.01.2018
 * Time: 15:55
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
    /// Description of CreateProject.
    /// </summary>
    [TestModule("1F59DAA3-2A41-4297-AD17-BE0F7757BD47", ModuleType.UserCode, 1)]
    public class CreateProject : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CreateProject()
        {
            // Do not delete - a parameterless constructor is required!
        }

        private static Studio2017Repository repo = Studio2017Repository.Instance;
        UtilityMethods utilityMethods = new UtilityMethods();
        ProjectCreationUtility projectCreationUtility = new ProjectCreationUtility();
        
        public bool publishOnGroupShare = false;
        List<string> targetLanguages = new List<string>(new string[] {"German (Germany)"});
        public bool autoFillState = true;
        

        void ITestModule.Run()
        {
        	//SetUp Ranorex settings
        	utilityMethods.setTestRunSettings();
        	
        	//Give a name to the project
        	string projectName = utilityMethods.projectNameRandom();
        	string projectFolder = Constants.ProjectCreationPath + projectName;
        	string projectOnDisk = projectFolder + @"\" + projectName + ".sdlproj";

            //Start Studio
            utilityMethods.startStudio(Constants.NotFirstStart);
            
            
            //Activate if necessary
			if (repo.LicenseManagerForm.ButtonActivateButtonInfo.Exists(Constants.CasualWait)) {
				utilityMethods.studioActivation(Constants.LicenseServer);
			}
            
			//First setup
			if (repo.StudioInstallation.SetUpStudioFirstTimeInfo.Exists(Constants.CasualWait)){
				utilityMethods.firstSetup("@");
			}
			
			
			//Turn off Automatic Updates
			utilityMethods.turnOffAutomaticUpdates();
			
			//Start Studio
			utilityMethods.startStudio(Constants.NotFirstStart);
			
			//Create project
			projectCreationUtility.goToNewProjectWizard();
			projectCreationUtility.createProject(projectName, autoFillState, projectFolder, publishOnGroupShare, "", "", Constants.InputFilesLocation, Constants.InputTmtbLocation, Constants.EnglishGermanTM, Constants.PrinterTB, Constants.EnglishUS, targetLanguages);
			
			//Validate that the project is created
			bool projectIsCreated = projectCreationUtility.projectIsCreated();
			if (projectIsCreated) {
				Report.Success("Project was successfully created");
			}
			else {
				Report.Failure("Project was not successfully created");
			}
			
			projectCreationUtility.pressFinish();
			
			if (utilityMethods.waitFileToExist(projectOnDisk)) {
				Report.Success("Success", "File " + projectOnDisk + " is present");
			}
			else {
				Report.Failure("Fail", "File " + projectOnDisk + " is not present");
			}
			
			utilityMethods.closeStudio();
        }
    }
}
