/*
 * Created by Ranorex
 * User: astan
 * Date: 31.01.2018
 * Time: 13:28
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
    /// Description of EditorUtility.
    /// </summary>
    [TestModule("34EE392D-950D-4208-9C9F-3C2139EE25C8", ModuleType.UserCode, 1)]
    public class EditorUtility
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public EditorUtility()
        {
            // Do not delete - a parameterless constructor is required!
        }

        private static Studio2017Repository repo = Studio2017Repository.Instance;
                
        
        public void goToSegment(int nrOfSegment) {
        	repo.StudioWindowForm.GoToSegment.Click();
        	repo.GoToDialog.SegmentNumber.TextValue = nrOfSegment.ToString();
        	repo.GoToDialog.GoToButton.Click();
        	repo.GoToDialog.CloseDialogInfo.WaitForAttributeEqual(Constants.MinWait, "enabled", true);
        	repo.GoToDialog.CloseDialog.Click();
        	repo.GoToDialog.CloseDialogInfo.WaitForNotExists(Constants.CasualWait);
        	waitForTargetPanelToBeEnabled();
        }
        
        public void translateSegment(int nrOfSegment, string translation) {
        	string segmentTranslation = getSegmentTranslation(nrOfSegment);
        	if (segmentTranslation.Equals("\r\n") || segmentTranslation.Equals("")) {
        		addTranslationAsText(translation);
        	}
        	else {
        		pressHomeToResetCursor();
        		selectAllFromSegment();
        		deleteSelectionFromSegment();
        		addTranslationAsText(translation);
        	}
        }
        
        public void cofirmTranslation() {
        	repo.StudioWindowForm.ConfirmButtonInfo.WaitForAttributeEqual(Constants.CasualWait, "enabled", true);
        	repo.StudioWindowForm.ConfirmButton.Click();
        }
        
        public string getSegmentTranslation(int nrOfSegment) {
        	System.Windows.Forms.Clipboard.Clear();
        	goToSegment(nrOfSegment);
        	pressHomeToResetCursor();
        	waitForTargetPanelToBeEnabled();
        	selectAllFromSegment();
        	waitForTargetPanelToBeEnabled();
        	copyToClipboard();
        	waitForTargetPanelToBeEnabled();
        	string segmentText = System.Windows.Forms.Clipboard.GetText();
        	if (segmentText.Equals("\r\n") || segmentText.Equals("")) {
        		Keyboard.Press("{Up}");
        	}
        	pressHomeToResetCursor();
        	return segmentText;
        }
        
        
        public void deleteSegment(int nrOfSegment) {
        	string segmentTranslation = getSegmentTranslation(nrOfSegment);
        	if (segmentTranslation.Equals("\r\n") || segmentTranslation.Equals("")) {
        		//Segment is empty
        	}
        	else {
        		pressHomeToResetCursor();
        		waitForTargetPanelToBeEnabled();
        		selectAllFromSegment();
        		waitForTargetPanelToBeEnabled();
        		deleteSelectionFromSegment();
        		waitForTargetPanelToBeEnabled();
        	}    	
        }

        
        public List<string> getTerms() {
        	IList<Ranorex.TreeItem> listedTerms = repo.StudioWindowForm.ContainerMainPanel.TermRecognitionPanel.FindDescendants<Ranorex.TreeItem>();
        	var extractedTerms = new List<string>();	
        	foreach (Ranorex.TreeItem terms in listedTerms) {
        		extractedTerms.Add(terms.Text);
        	}
        	return extractedTerms;
        }
        
        
        public void addTermbase(int nrOfSegment) {
        	string segmentTranslation = getSegmentTranslation(nrOfSegment);
        	if (segmentTranslation.Equals("\r\n") || segmentTranslation.Equals("")) {
        		pressCtrlShiftL();
        		Keyboard.Press("{Return}");
        	}
        	else {
        		pressHomeToResetCursor();
        		waitForTargetPanelToBeEnabled();
        		selectAllFromSegment();
        		waitForTargetPanelToBeEnabled();
        		deleteSelectionFromSegment();
        		waitForTargetPanelToBeEnabled();
        	    pressCtrlShiftL();
        	    Keyboard.Press("{Return}");
        	}
        }
        
        public string addTermInTermbase(string term, string termbaseLayout) {
        	
        	//Select Default Layout for termbase
        	goToSpecificView("Termbase Viewer");
        	changeTermbaseLayout(termbaseLayout);
        	clickOnTargetPanel();
        	goToHomeTab();
        	
        	//Focus the cursor on the Editor
        	repo.StudioWindowForm.ContainerMainPanel.SourceControlPanel.Click(new Location(1, 1));
        	repo.StudioWindowForm.ContainerMainPanel.SourceControlPanel.Click(new Location(50, 1));
        	pressHomeToResetCursor();
        	waitForSourcePanelToBeEnabled();
        	pressCtrlShiftRight();
        	waitForSourcePanelToBeEnabled();
        	copyToClipboard();
        	waitForSourcePanelToBeEnabled();
        	string sourceSelection = System.Windows.Forms.Clipboard.GetText();
        	
        	//Add term into termbase
        	repo.StudioWindowForm.AddNewTermInfo.WaitForAttributeEqual(Constants.CasualWait, "enabled", true);
        	repo.StudioWindowForm.AddNewTerm.Click();
        	termAlreadyExists("Yes");
        	repo.AddTerm.AddLanguageDropdownButton.MoveTo();
        	repo.AddTerm.AddTermInfo.WaitForAttributeEqual(Constants.MinWait, "visible", true);
        	repo.AddTerm.AddTerm.Click();
        	repo.AddTerm.AddTermNameInfo.WaitForAttributeEqual(Constants.CasualWait, "enabled", true);
        	repo.AddTerm.AddTermName.InnerText = term;
        	Keyboard.Press("{Return}");
        	return sourceSelection.Replace(" ", string.Empty);
        }
        
        public void editTerm(string editedTerm) {
        	repo.AddTerm.AddLanguageDropdownButton.MoveTo();
        	repo.AddTerm.EditInfo.WaitForExists(Constants.CasualWait);
        	repo.AddTerm.Edit.Click();
        	repo.AddTerm.AddTermNameInfo.WaitForAttributeEqual(Constants.CasualWait, "enabled", true);
        	repo.AddTerm.AddTermName.InnerText = editedTerm;
        	Keyboard.Press("{Return}");
        }
        
        public bool checkIfTermExists(string term, string languageTerm) {
        	goToSpecificView("Termbase Search");
        	repo.StudioWindowForm.ContainerMainPanel.TermSearch.Click();
        	repo.StudioWindowForm.ContainerMainPanel.TermSearch.TextValue = term;
        	Keyboard.Press("{Return}");
        	repo.StudioWindowForm.ContainerMainPanel.TermNameInfo.Path = "//container[@controlname='_mainSplitContainer']//container[@controltypename='ViewPane']//container[@controltypename='DockableWindow' and @instance='1']//treeitem[@accessiblename='" + languageTerm + "']";
        	if (repo.StudioWindowForm.ContainerMainPanel.TermNameInfo.Exists(Constants.MaxWait)) {
				return true;
        	}
        	else {
        		return false;
        	}
        }
        	
        public void viewTermDetails(string languageTerm) {
        	repo.StudioWindowForm.ContainerMainPanel.TermNameInfo.Path = "//container[@controlname='_mainSplitContainer']//container[@controltypename='ViewPane']//container[@controltypename='DockableWindow' and @instance='1']//treeitem[@accessiblename='" + languageTerm + "']";
        	repo.StudioWindowForm.ContainerMainPanel.TermName.Click(System.Windows.Forms.MouseButtons.Right);
        	repo.ViewTerms.ViewTermDetails.Click();
        }	
        
        public void goToSpecificView(string viewName) {
        	repo.StudioWindowForm.ViewRibbon.Click();
        	repo.StudioWindowForm.GoToSpecificViewTabInfo.Path = "//form[@controlname='StudioWindowForm']//container[@automationid='Lower Ribbon']//container[@automationid='Group : informationRibbonGroup']//button[@name='" + viewName + "']";
        	repo.StudioWindowForm.GoToSpecificViewTab.Click();
        }
        
        public void waitForTargetPanelToBeEnabled() {
        	repo.StudioWindowForm.ContainerMainPanel.TargetControlPanelInfo.WaitForAttributeEqual(Constants.CasualWait, "enabled", true);
        }
        
        public void waitForSourcePanelToBeEnabled() {
        	repo.StudioWindowForm.ContainerMainPanel.SourceControlPanelInfo.WaitForAttributeEqual(Constants.CasualWait, "enabled", true);
        }
        
        public void pressHomeToResetCursor() {
        	Keyboard.Press("{Home}");
        }
        
        public void selectAllFromSegment() {
        	Keyboard.Press("{LControlKey down}{LShiftKey down}{Down}{LControlKey up}{LShiftKey up}");
        }
        
        public void copyToClipboard() {
        	Keyboard.Press("{LControlKey down}{Ckey}{LControlKey up}");
        }
        
        public void deleteSelectionFromSegment() {
       		Keyboard.Press("{LMenu down}{Delete}{LMenu up}");
        }
        
        public void addTranslationAsText(string translation) {
        	Keyboard.Press(translation);
        }
        
        //Add term
        public void pressCtrlShiftL() {
        	Keyboard.Press("{LControlKey down}{LShiftKey down}{Lkey}{LShiftKey up}{LControlKey up}");
        }
        
        public void applyTranslation() {
        	Keyboard.Press("{LControlKey down}{TKey}{LControlKey up}");
        }
        
        public void saveFile() {
        	Keyboard.Press("{LControlKey down}{Skey}{LControlKey up}");
        }
        
        public void pressCtrlShiftRight() {
        	Keyboard.Press("{LControlKey down}{LShiftKey down}{Right}{LShiftKey up}{LControlKey up}");
        }
        
        public void deleteTermEntry() {
        	repo.AddTerm.AddLanguageDropdownButton.MoveTo();
        	repo.StudioWindowForm.ContainerMainPanel.DeleteEntryTerm.Click();
        	repo.Question.ButtonYes.Click();
        }
        
        public void goToHomeTab() {
        	repo.StudioWindowForm.HomeRibbon.Click();
        }
        
        public void questionSaveChanges(string option) {
        	if (repo.WarningSaveDocument.ButtonYesInfo.Exists(Constants.CasualWait) && repo.WarningSaveDocument.ButtonNoInfo.Exists(Constants.CasualWait)) {
            	if (option.Equals("Yes")) {
                repo.WarningSaveDocument.ButtonYes.Click();
            	}
            	else {
                repo.WarningSaveDocument.ButtonNo.Click();
            	}
        	}
        
        }
        
        public void changeTermbaseLayout(string layout) {
        	hoverOverTermbaseViewer();
        	repo.StudioInstallation.TermbaseLayoutComboboxInfo.WaitForExists(Constants.MinWait);
        	repo.StudioInstallation.TermbaseLayoutCombobox.Click();
        	repo.ServerList.TermbaseLayoutSelectionInfo.Path = "//list[@controlid='1000']//listitem[@text='" + layout + "']";
        	repo.ServerList.TermbaseLayoutSelection.Click();
        }
        
        public void clickOnTargetPanel() {
        	repo.StudioWindowForm.ContainerMainPanel.TargetControlPanel.Click();
        }
        
        public void hoverOverTermbaseViewer() {
            repo.SDLMultiTerm.TermbaseViewerInfo.WaitForExists(Constants.MinWait);
        	repo.SDLMultiTerm.TermbaseViewer.MoveTo();
        }
        
        public void termAlreadyExists(string option) {
        	if (repo.SelectedTermAlreadyExists.SelectOptionInfo.Exists(500)) {
        		repo.SelectedTermAlreadyExists.SelectOptionInfo.Path = "//form[@title~'Selected Term Already Exists']//button[@text~'" + option + "']";
        		repo.SelectedTermAlreadyExists.SelectOption.Click();
        	}
        }
    }
}
