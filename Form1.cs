using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using CefSharp;
using CefSharp.WinForms;
using System.Threading;

namespace InstaRefollow
{
    public partial class Form1 : Form
    {

        public ChromiumWebBrowser chromeBrowser;

        Thread thread;

        // Form reference for Anti-minimize.
        static Form1 myForm;

        // WindowState for Anti-minimize.
        static FormWindowState preWindowState;

        // Delegae for Anti-minimize.
        public delegate void UpWindowDelegate();

        // Anti-minimize. (Because CefSharp doesn't work in minimize-window.)
        private void UpWindow()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpWindowDelegate(this.UpWindow));
                return;
            }

            if (this.WindowState == FormWindowState.Minimized)
            {
                myForm.WindowState = preWindowState;

                int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

                this.SetBounds(0, height - 10, 0, 0, BoundsSpecified.Y);
            }

        }



        // Delegate for setText()
        public delegate void setTextDelegate();

        // Message for setText()
        string message = "";

        // Set text.
        public void setText()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new setTextDelegate(this.setText));
                return;
            }

            this.Text = "InstaRefollow - " + message;

        }




        public Form1()
        {
            InitializeComponent();

            // Start the browser after initialize global component
            InitializeChromium();

            // Remember current WindowState.
            myForm = this;
            preWindowState = this.WindowState;


            try
            {
                
                thread = new Thread(new ThreadStart(() =>
                {

                    // Number of @megafloat4's followings.
                    int limitter = loadText("limitter.txt");

                    // Limit number of refollowing at this time.
                    int daymax = loadText("daymax.txt");

                    // Offsset number of refollowing at this time.
                    int offset = loadText("offset.txt");

                    if( (offset + daymax) > limitter)
                    {
                        daymax = limitter - offset;
                    }

                    message = "Please login in 1 minute. Then minimize this window for few hours. " + offset + "～" + (offset + daymax) + "/" + limitter;
                    setText();




                    // Wait until login & Load @megafloat4
                    Thread.Sleep(60000);
                    UpWindow();
                    chromeBrowser.Load("https://instagram.com/megafloat4");

                    message = "Loading accounts to unfollow. Then minimize this window for few hours. " + offset + "～" + (offset + daymax) + "/" + limitter;
                    setText();


                    // Click "following members".
                    Thread.Sleep(10000);
                    UpWindow();
                    string jsScript = "var inputs = document.getElementsByClassName('-nal3 '); " +
                                      "inputs[2].click(); ";
                    chromeBrowser.ExecuteScriptAsync(jsScript);


                    // For safe, reload @megafloat4
                    Thread.Sleep(10000);
                    UpWindow();
                    chromeBrowser.Load("https://instagram.com/megafloat4");

                    // Click "following members".
                    Thread.Sleep(10000);
                    UpWindow();
                    jsScript = "var inputs = document.getElementsByClassName('-nal3 '); " +
                                      "inputs[2].click(); ";
                    chromeBrowser.ExecuteScriptAsync(jsScript);


                    // Load all following members.
                    for(int count = 0; count < limitter/5; count++)
                    {
                        // Scroll down the members list.
                        Thread.Sleep(10000);
                        UpWindow();
                        jsScript = "var inputs = document.getElementsByClassName('isgrP'); " +
                                   "inputs[0].scrollTo(0, 10000); ";
                        chromeBrowser.ExecuteScriptAsync(jsScript);
                    }


                    //--------------------------------
                    // Unfollow each accounts.
                    //--------------------------------

                    message = "Unfollowing the accounts. Leave this window for few hours. " + offset + "～" + (offset + daymax) + "/" + limitter;
                    setText();

                    for (int counter = offset; counter < (offset + daymax); counter++)
                    {
                        // Click "Unfollow" button of offset.
                        Thread.Sleep(10000);
                        UpWindow();
                        jsScript = "var buttons = document.getElementsByClassName('sqdOP  L3NKy    _8A5w5    '); " +
                                   "var firstElem = buttons[" + offset + "];" +
                                   "var message = firstElem.innerText;" +
                                   "if(message == 'フォロー中'){buttons[" + offset + "].click();}" +
                                   "else{buttons[" + (offset+1) + "].click();} ";
                        chromeBrowser.ExecuteScriptAsync(jsScript);

                        // Click "OK".
                        Thread.Sleep(10000);
                        UpWindow();
                        jsScript = "var inputs = document.getElementsByClassName('aOOlW -Cab_   '); " +
                                                      "inputs[0].click(); ";
                        chromeBrowser.ExecuteScriptAsync(jsScript);

                        // Scroll down the members list.
                        Thread.Sleep(10000);
                        UpWindow();
                        jsScript = "var inputs = document.getElementsByClassName('isgrP'); " +
                                   "inputs[0].scrollTo(0, 10000); ";
                        chromeBrowser.ExecuteScriptAsync(jsScript);
                    }


                    message = "Loading accounts to follow. Then minimize this window for few hours. " + offset + "～" + (offset + daymax) + "/" + limitter;
                    setText();


                    // Reload @megafloat4
                    Thread.Sleep(10000);
                    UpWindow();
                    chromeBrowser.Load("https://instagram.com/megafloat4");

                    // Click "FOLLOWING members".
                    Thread.Sleep(10000);
                    UpWindow();
                    jsScript = "var inputs = document.getElementsByClassName('-nal3 '); " +
                                      "inputs[2].click(); ";
                    chromeBrowser.ExecuteScriptAsync(jsScript);

                    // Load all following members.
                    for (int count = 0; count < limitter/5; count++)
                    {
                        // Scroll down the members list.
                        Thread.Sleep(10000);
                        UpWindow();
                        jsScript = "var inputs = document.getElementsByClassName('isgrP'); " +
                                   "inputs[0].scrollTo(0, 10000); ";
                        chromeBrowser.ExecuteScriptAsync(jsScript);
                    }


                    //-------------------------------
                    // FOLLOW each accounts.
                    //-------------------------------
                    message = "Following the accounts. Leave this window for few hours. " + offset + "～" + (offset + daymax) + "/" + limitter;
                    setText();

                    // FOLLOW buttons begin from [0]!
                    for (int counter = 0; counter < limitter; counter++)
                    {
                        // Click the first "follow" button.
                        Thread.Sleep(20000);
                        UpWindow();
                        jsScript = "var buttons = document.getElementsByClassName('sqdOP  L3NKy   y3zKF     '); " +
                                   "var firstElem = buttons[0];" +
                                   "var message = firstElem.innerText;" +
                                   "if(message == 'フォローする'){buttons[0].click();}" +
                                   "else{buttons[1].click();} ";
                        chromeBrowser.ExecuteScriptAsync(jsScript);


                        // Scroll down the members list.
                        Thread.Sleep(10000);
                        UpWindow();
                        jsScript = "var inputs = document.getElementsByClassName('isgrP'); " +
                                   "inputs[0].scrollTo(0, 10000); ";
                        chromeBrowser.ExecuteScriptAsync(jsScript);
                    }
                    



                    // Save next offset.
                    if(offset + daymax >= limitter)
                    {
                        writeText("offset.txt", 0);
                    }
                    else
                    {
                        int new_offset = offset + daymax;
                        writeText("offset.txt", new_offset);
                    }

                    message = "FINISHED refollowing accounts. " + offset + "～" + (offset + daymax) + "/" + limitter;
                    setText();
                }));

                // Run the thread above.
                thread.Start();               


            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => { MessageBox.Show(this, ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error); }));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chromeBrowser.ShowDevTools();
        }

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            settings.Locale = "ja";
            settings.AcceptLanguageList = "ja-JP";
            settings.CachePath = "cache";
            settings.PersistSessionCookies = true;

            // Initialize cef with the provided settings
            Cef.Initialize(settings);

            //The Global CookieManager has been initialized, you can now set cookies
            var cookieManager = Cef.GetGlobalCookieManager();
            //cookieManager.SetStoragePath("cookies", true);

            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser("https://instagram.com");

            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            // Allow the use of local resources in the browser
            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            chromeBrowser.BrowserSettings = browserSettings;
            
        }
        



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();

            thread.Abort();
        }


        // Load integer from a text-file.
        private int loadText(String fileName)
        {
            StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding("Shift_JIS"));

            string text = sr.ReadToEnd();

            sr.Close();

            return int.Parse(text);
        }

        // Write integer into a text-file.
        private void writeText(string fileName, int value)
        {
            string message = value + "";

            StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding("Shift_JIS"));
            writer.WriteLine(message);
            writer.Close();
        }
    }
}
