using Microsoft.Win32;

namespace ctb_replay_editor {
    internal static class Utils {
        public static string GetOsuPath() {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\osu!\DefaultIcon");
            string path = key?.GetValue(null).ToString();
            return path?.Substring(1, path.Length - 13);
        }
    }
}
