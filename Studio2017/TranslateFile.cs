/*
 * Created by Ranorex
 * User: astan
 * Date: 29.01.2018
 * Time: 13:55
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
    /// Description of TranslateFile.
    /// </summary>
    [TestModule("6A39A0E4-3F5F-4F23-B5DA-A19481F2E6CF", ModuleType.UserCode, 1)]
    public class TranslateFile : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public TranslateFile()
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
        
        public bool publishOnGroupShare = false;
       	private string batchTaskName = "Finalize";
       	public bool autoFillState = true;
        
        void ITestModule.Run()
        {
        	utilityMethods.setTestRunSettings();
            
        	
        	//Give a name to the project
            string projectName = utilityMethods.projectNameRandom();
            string projectFilesFolder = Constants.ProjectCreationPath + projectName + @"\de-DE\" + Constants.TranslatableFiles + @"\";
            string projectFolder = Constants.ProjectCreationPath + projectName;

            
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
			
			//Verify that the project was successfully created
			bool projectIsCreated = projectCreationUtility.projectIsCreated();
			
			if (projectIsCreated) {
				Report.Success("Project was successfully created");
			}
			else {
				Report.Failure("Project was not successfully created");
			}
			
			projectCreationUtility.pressFinish();
			
			//Select translation file
			views.selectProject(projectName);
			views.goToFilesView();
			views.includeSubfolders("check");
			views.openFileForTranslation(Constants.SamplePhotoPrinter);	
			
			
			//Get translation of first segment
			string firstTranslation = editor.getSegmentTranslation(1);
			Validate.AreEqual(Constants.FirstSegmentExpectedTranslation, firstTranslation);
			
			//Apply translation from translation memory
			string secondTranslation = editor.getSegmentTranslation(2);
			Validate.AreEqual(Constants.SecondSegmentExpectedTranslation, secondTranslation);
			
			//Get terms from Term Recognition
			editor.goToSpecificView("Term Recognition");
			var existingTerms = new List<string>();
			existingTerms = editor.getTerms();
			if (existingTerms.Contains(Constants.ExpectedTermSecondSegment)) {
				Report.Success("Success", "Terms is found for the second segment");
			}
			else {
				Report.Failure("Fail", "Term is not found for the second segment");
			}
			
			//Translate a segment and confirm
			editor.goToHomeTab();
			editor.translateSegment(5, Constants.RanorexTranslation);
			editor.cofirmTranslation();
			
			string thirdTranslation = editor.getSegmentTranslation(5);
			Validate.AreEqual(Constants.RanorexTranslation, thirdTranslation);
			
			
			//Verify TM works delete and then come back to segment to see if the interaction with the TM works
			editor.deleteSegment(5);
			editor.saveFile();
			
			//Move to third segment to change segments and validate
			string fourthTranslation = editor.getSegmentTranslation(3);
			Validate.AreEqual(Constants.ThirdSegmentExpectedTranslation, fourthTranslation);
			
			//Go back to the deleted segment
			string deletedSegment = editor.getSegmentTranslation(5);
			if (deletedSegment == "\r\n" || deletedSegment == "") {
				editor.applyTranslation();
				deletedSegment = editor.getSegmentTranslation(5);
				Validate.AreEqual(Constants.RanorexTranslation, deletedSegment);
			}
			else {
			Validate.AreEqual(Constants.RanorexTranslation, deletedSegment);
			}
			
			
			//Add term from termbase
			editor.addTermbase(5);
			string termbaseAddedSegment = editor.getSegmentTranslation(5);
			if (Validate.Equals(termbaseAddedSegment, "USB-Kabel") || Validate.Equals(termbaseAddedSegment, "Fotodrucker")) {
			    Report.Success("Success", "Term was added into target segment");    
			}
			else {
			    Report.Failure("Fail", "Term was not added into target segment");
			}
			
			string englishTerm = editor.addTermInTermbase(Constants.CustomTerm, "Default layout");
            //Get terms from Term Recognition
			var addedTerms = new List<string>();
			addedTerms = editor.getTerms();
			if (addedTerms.Contains(Constants.CustomTerm) && addedTerms.Contains(englishTerm)) {
				Report.Success("Success", "Terms is found for the second segment");
			}
			else {
				Report.Failure("Fail", "Term is not found for the second segment");
			}
			
			//Search and edit term
			if (editor.checkIfTermExists(englishTerm, Constants.CustomTerm)) {
				Report.Success("Term " + englishTerm + " was found in termbase search");
			}
			else {
				Report.Failure("Term" + englishTerm + " was not found in termbase search");
			}
			
			editor.viewTermDetails(Constants.CustomTerm);
			editor.editTerm(Constants.EditedTerm);
			editor.goToSpecificView("Term Recognition");
			var termsEdited = new List<string>();
			termsEdited = editor.getTerms();
			if (termsEdited.Contains(englishTerm) && termsEdited.Contains(Constants.EditedTerm)) {
				Report.Success("Success", "Edited term is found");
			}
			else {
				Report.Failure("Fail", "Edited term is not found");
			}
			
			
			//Delete term
			editor.goToSpecificView("Termbase Search");
			editor.viewTermDetails(Constants.CustomTerm);
			editor.deleteTermEntry();
			
			//Verify that term is deleted
			if (editor.checkIfTermExists(englishTerm, Constants.CustomTerm)) {
				Report.Failure("Fail", "Term was not deleted");
			}
			else {
				Report.Success("Success", "Term was deleted");
			}
			
			
			//Save Target As
			projectCreationUtility.saveTargetAs(projectFilesFolder+Constants.SamplePhotoPrinter);
			System.DateTime start = System.DateTime.Now;
			while (!System.IO.File.Exists(projectFilesFolder+Constants.SamplePhotoPrinter) && System.DateTime.Now.Subtract(start).Seconds < 20) {
				Console.WriteLine("Target file " + Constants.SamplePhotoPrinter + " is not present yet");
			}
			bool fileIsPresent = System.IO.File.Exists(projectFilesFolder+Constants.SamplePhotoPrinter);
			if (fileIsPresent) {
				Report.Success("Success", "File " + Constants.SamplePhotoPrinter + " is saved as target");
			}
			else {
				Report.Failure("Fail", "File " + Constants.SamplePhotoPrinter + " is not saved as target");
			}			
			
			
			//Finalize project
			batchTask.runBatchTask(batchTaskName, projectName);
			editor.questionSaveChanges("Yes");
			projectCreationUtility.pressNext();
			projectCreationUtility.pressFinish();
			
			
			//Verify Finalize batch task run is successful
			bool batchTaskIsSuccessfullyDone = batchTask.batchTaskIsDone();
			
			if (batchTaskIsSuccessfullyDone) {
				Report.Success("Success", batchTask + " batch task was successfully run");
			}
			else {
				Report.Failure("Fail", "An error occured running the " + batchTask + " batch task");
			}
			projectCreationUtility.pressFinish();
			
			
			//Verify that project was finalised (files are saved as target)
			foreach (string file in projectFilesList) {
				System.DateTime finalizeStartTime = System.DateTime.Now;
				while (!System.IO.File.Exists(projectFilesFolder+file) && System.DateTime.Now.Subtract(finalizeStartTime).Seconds < 20) {
					Console.WriteLine("Target file " + file + " is not present yet");
				}
				bool fileIsAvailable = System.IO.File.Exists(projectFilesFolder+file);
				if (fileIsAvailable) {
					Report.Success("Success", "File " + file + " is saved as target");
				}
				else {
					Report.Failure("Fail", "File " + file + " is not saved as target");
				}
        	}
			
			//Close Studio
			utilityMethods.closeStudio();
			editor.questionSaveChanges("No");
			
			//CompareSdlFilesLib.CompareSdlFiles.FileCompareXliff("armand", "Studio");
        }
        
    }
}
