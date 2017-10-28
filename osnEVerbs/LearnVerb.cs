using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using osnE.Interop;
using System.Configuration;
using osnE.WindowsHooks;
using osnE.StateMachine;
using System.Windows;
using Common.Logging;
using System.Runtime.InteropServices;
using WindowsInput;
using System.IO;
using wsh = IWshRuntimeLibrary;

namespace osnE.Verbs
{
    public class LearnVerb: Verb
    {
        private static int callCount=0;
        public static readonly ILog logger = LogManager.GetCurrentClassLogger();
        public LearnVerb():
            base("learn","Type learn followed by a name to create a new 'open' command",SubjectType.Arbitrary,
            "Type learn followed by a name to create a new 'open' command",PredicateType.None)
        {

        }
        public void STAThreadExecute(object junk)
        {
            callCount = callCount + 1;
            #region Debugging Code
            /*
            Console.WriteLine("CurrentClipboard:" + StateManager.Instance.KbdManager.CurrentClipboard.ToString() + " callcount=" + callCount.ToString());
            Console.WriteLine("CapturedClipboard:" + Clipboard.GetText() + " callcount=" + callCount.ToString());
            Console.WriteLine("Bitmap:" + Clipboard.ContainsData("Bitmap").ToString());
            Console.WriteLine("CSV:" + Clipboard.ContainsData("CSV").ToString());
            Console.WriteLine("DeviceIndependentBitmap:" + Clipboard.ContainsData("DeviceIndependentBitmap").ToString());
            Console.WriteLine("DataInterchangeFormat:" + Clipboard.ContainsData("DataInterchangeFormat").ToString());
            Console.WriteLine("EnhancedMetafile:" + Clipboard.ContainsData("EnhancedMetafile").ToString());
            Console.WriteLine("FileDrop:" + Clipboard.ContainsData("FileDrop").ToString());
            Console.WriteLine("HTML Format:" + Clipboard.ContainsData("HTML Format").ToString());
            Console.WriteLine("Locale:" + Clipboard.ContainsData("Locale").ToString());
            Console.WriteLine("MetaFilePict:" + Clipboard.ContainsData("MetaFilePict").ToString());
            Console.WriteLine("OEMText:" + Clipboard.ContainsData("OEMText").ToString());
            Console.WriteLine("Palette:" + Clipboard.ContainsData("Palette").ToString());
            Console.WriteLine("PenData:" + Clipboard.ContainsData("PenData").ToString());
            Console.WriteLine("RiffAudio:" + Clipboard.ContainsData("RiffAudio").ToString());
            Console.WriteLine("Rich Text Format:" + Clipboard.ContainsData("Rich Text Format").ToString());
            Console.WriteLine("PersistentObject:" + Clipboard.ContainsData("PersistentObject").ToString());
            Console.WriteLine("System.String:" + Clipboard.ContainsData("System.String").ToString());
            Console.WriteLine("SymbolicLink:" + Clipboard.ContainsData("SymbolicLink").ToString());
            Console.WriteLine("Text:" + Clipboard.ContainsData("Text").ToString());
            Console.WriteLine("TaggedImageFileFormat:" + Clipboard.ContainsData("TaggedImageFileFormat").ToString());
            Console.WriteLine("UnicodeText:" + Clipboard.ContainsData("UnicodeText").ToString());
            Console.WriteLine("WaveAudio:" + Clipboard.ContainsData("WaveAudio").ToString());
            Console.WriteLine("Xaml:" + Clipboard.ContainsData("Xaml").ToString());
            Console.WriteLine("XamlPackage:" + Clipboard.ContainsData("XamlPackage").ToString());
             * */
            #endregion
            
            string url = "";
            string path= ConfigurationManager.AppSettings["CommandDirectory"];
            
            if (StateManager.Instance.KbdManager.CurrentClipboard != null)
            {
                string[] formats = StateManager.Instance.KbdManager.CurrentClipboard.GetFormats();
                if(formats.Contains("FileName"))
                {
                    object i = StateManager.Instance.KbdManager.CurrentClipboard.GetData("FileName");
                    string fileName = ((string[])i)[0];
                    Console.WriteLine("filename=" + fileName);
                    
                    try
                    {
                        if (File.Exists(fileName))
                        {
                            wsh.WshShellClass shell = new wsh.WshShellClass();
                            IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut(path+"/" + this.subject + ".lnk") as IWshRuntimeLibrary.IWshShortcut;
                            shortcut.TargetPath = fileName;
                            // not sure about what this is for
                            shortcut.WindowStyle = 1;
                            shortcut.Description = "Learned Verb " + this.subject;
                            shortcut.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            shortcut.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("exception creating shortcut ex=" + ex.Message);
                    }
                    return;
                }
                
                if (formats.Contains("UniformResourceLocatorW"))
                {
                    url = (string)StateManager.Instance.KbdManager.CurrentClipboard.GetData("System.String");
                    
                }
                else if (formats.Contains("System.String"))
                {
                    string test = (string)StateManager.Instance.KbdManager.CurrentClipboard.GetData("System.String");
                    if(test.StartsWith("http://") || test.StartsWith("https://"))
                    {
                        url = test;
                    }
                }
                if(url!="")
                {
                    Console.WriteLine("url=" + url);
                    string urlshortcut = "[InternetShortcut]\r\nURL={0}";
                    using (StreamWriter writer = new StreamWriter(path + "\\" + this.subject + ".URL"))
                    {
                        writer.WriteLine(string.Format(urlshortcut, url));
                        writer.Flush();
                    }
                }
            }
        }
        public override void Execute()
        {
            ParameterizedThreadStart pts = new ParameterizedThreadStart(this.STAThreadExecute);
            Thread t = new Thread(pts);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
        public override List<string> GetPredicateOptions(string search)
        {
            return new List<string>();
        }
        public override List<string> GetSubjectsOptions(string search)
        {
            return new List<string>();
        }
            
    }
}
