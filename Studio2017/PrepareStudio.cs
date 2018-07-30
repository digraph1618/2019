/*
 * Created by Ranorex
 * User: astan
 * Date: 09.02.2018
 * Time: 10:33
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
using System.Linq;
using System.IO;
using System.IO.Compression;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace Studio2017
{
    /// <summary>
    /// Description of CopyStudioBuild.
    /// </summary>
    [TestModule("BC48316B-4BF1-4786-AAFE-76F828C6EA3E", ModuleType.UserCode, 1)]
    public class PrepareStudio : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public PrepareStudio()
        {
            // Do not delete - a parameterless constructor is required!
        }
		private static Studio2017Repository repo = Studio2017Repository.Instance;
		UtilityMethods utilityMethods = new UtilityMethods();
        
        void ITestModule.Run()
        {
        	utilityMethods.setTestRunSettings();
            
            //Copy Studio build
            if (!Directory.Exists(Constants.LocalBuildFolder)) {
            	Directory.CreateDirectory(Constants.LocalBuildFolder);
            }
            
            var directory = new DirectoryInfo(Constants.BuildsServer).GetDirectories(Constants.StudioFolderPattern).OrderBy(folder => folder.LastWriteTime).ToList();
            int lastBuild = directory.Count;
            string buildFolder = directory[lastBuild-1].FullName;
            
            var studioExecutable = new DirectoryInfo(@buildFolder).GetFiles(Constants.StudioExePattern);
            
            var sourceFolder = @buildFolder + @"\" + studioExecutable[0];
            var localDestinationFolder = @Constants.LocalBuildFolder + studioExecutable[0];
            

            File.Copy(sourceFolder, localDestinationFolder, true);
            
            
            var localStudioExecutable = new DirectoryInfo(Constants.LocalBuildFolder).GetFiles(Constants.StudioExePattern).OrderBy(file => file.LastWriteTime).ToList();
            int fileNr = localStudioExecutable.Count;
            
            //Install Studio
            utilityMethods.installStudio(localStudioExecutable[fileNr - 1].FullName);  
        }
    }
}
