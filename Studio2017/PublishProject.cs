/*
 * Created by Ranorex
 * User: astan
 * Date: 22.02.2018
 * Time: 15:44
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
    /// Description of PublishProject.
    /// </summary>
    [TestModule("C1C0ADF0-85B7-4189-BF81-0E6F2F2FEAA2", ModuleType.UserCode, 1)]
    public class PublishProject : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public PublishProject()
        {
            // Do not delete - a parameterless constructor is required!
        }
        
		private static Studio2017Repository repo = Studio2017Repository.Instance;
        UtilityMethods utilityMethods = new UtilityMethods();
        ProjectCreationUtility projectCreationUtility = new ProjectCreationUtility();
        ViewsUtility views = new ViewsUtility();
        EditorUtility editor = new EditorUtility();
        BatchTaskUtility batchTask = new BatchTaskUtility();
        List<string> projectFilesList = new List<string>(new string[] { Constants.SamplePhotoPrinter, Constants.SamplePresentation, Constants.SampleXML, Constants.SampleSecondDoc, Constants.SamplePerfectMatch });
		List<string> targetLanguages = new List<string>(new string[] {"German (Germany)"});        
		public string groupShareServer = "cljvmgs2017";	
		public string GSUsername = "thaiteam";
		public string GSPassword = "Sa1";
		public bool publishOnGroupShare = true;
		public bool autoFillState = false;
		
		

        
        void ITestModule.Run()
        {
        	utilityMethods.deleteStudioProjects(Constants.ProjectCreationPath);
        	utilityMethods.setTestRunSettings();
            
        	
        	//Give a name to the project
            string projectName = utilityMethods.projectNameRandom();
            string projectFilesFolder = Constants.ProjectCreationPath + projectName + @"\de-DE\" + Constants.TranslatableFiles + @"\";

            
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
			
			//Add GS server
			utilityMethods.addGSServer(groupShareServer, GSUsername, GSPassword);
		
			
			//Create project
			projectCreationUtility.goToNewProjectWizard();
			projectCreationUtility.createProject(projectName, autoFillState, Constants.ProjectCreationPath, publishOnGroupShare, groupShareServer, Constants.RootOrganization, Constants.InputFilesLocation, Constants.InputTmtbLocation, Constants.EnglishGermanTM, Constants.PrinterTB, Constants.EnglishUS, targetLanguages);
			utilityMethods.closeStudio();
        }
    }
}
