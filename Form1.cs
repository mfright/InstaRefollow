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

        public Form1()
        {
            InitializeComponent();

            // Start the browser after initialize global component
            InitializeChromium();

            try
            {
                
                thread = new Thread(new ThreadStart(() =>
                {
                    // Wait until Login
                    Thread.Sleep(60000);

                    // Load @megafloat4
                    chromeBrowser.Load("https://instagram.com/megafloat4");

                    // Click "following members".
                    Thread.Sleep(3000);
                    string jsScript = "var inputs = document.getElementsByClassName('-nal3 '); " +
                                      "inputs[2].click(); ";
                    chromeBrowser.ExecuteScriptAsync(jsScript);


                    for(int counter = 0; counter < 100; counter++)
                    {
                        // Click first "Unfollow" button.
                        Thread.Sleep(30000);
                        jsScript = "var buttons = document.getElementsByClassName('sqdOP  L3NKy    _8A5w5    '); " +
                                   "var firstElem = buttons[0];" +
                                   "var message = firstElem.innerText;" +
                                   "if(message == 'フォロー中'){buttons[0].click();}" +
                                   "else{buttons[1].click();} ";
                        chromeBrowser.ExecuteScriptAsync(jsScript);

                        // Click "OK".
                        Thread.Sleep(3000);
                        jsScript = "var inputs = document.getElementsByClassName('aOOlW -Cab_   '); " +
                                                      "inputs[0].click(); ";
                        chromeBrowser.ExecuteScriptAsync(jsScript);

                        // Scroll down the members list.
                        Thread.Sleep(3000);
                        jsScript = "var inputs = document.getElementsByClassName('isgrP'); " +
                                   "inputs[0].scrollTo(0, 10000); ";
                        chromeBrowser.ExecuteScriptAsync(jsScript);
                    }


                    // Reload @megafloat4
                    chromeBrowser.Load("https://instagram.com/megafloat4");

                    // Click "following members".
                    Thread.Sleep(3000);
                    jsScript = "var inputs = document.getElementsByClassName('-nal3 '); " +
                                      "inputs[2].click(); ";
                    chromeBrowser.ExecuteScriptAsync(jsScript);

                    for (int counter = 0; counter < 100; counter++)
                    {
                        // Click "follow".
                        Thread.Sleep(30000);
                        jsScript = "var buttons = document.getElementsByClassName('sqdOP  L3NKy   y3zKF     '); " +
                                   "var firstElem = buttons[0];" +
                                   "var message = firstElem.innerText;" +
                                   "if(message == 'フォローする'){buttons[0].click();}" +
                                   "else{buttons[1].click();} ";
                        chromeBrowser.ExecuteScriptAsync(jsScript);


                        // Scroll down the members list.
                        Thread.Sleep(3000);
                        jsScript = "var inputs = document.getElementsByClassName('isgrP'); " +
                                   "inputs[0].scrollTo(0, 10000); ";
                        chromeBrowser.ExecuteScriptAsync(jsScript);
                    }
                    

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
    }
}
