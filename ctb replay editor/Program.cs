using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ctb_replay_editor {
    static class Program {
        public static int CurrentAudioStream;
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetProcessDPIAware();
        public static MainForm form;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new MainForm();
            form.Show();
            form.PlayField = new PlayField(form.GetControlHandle(form.playfieldPictureBox), form);
            form.PlayField.Run();
        }
    }
}
