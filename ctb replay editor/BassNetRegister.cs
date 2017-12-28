using System.Text;
using Un4seen.Bass;

namespace ctb_replay_editor {
    public class BassNetRegister {
        private static bool registered;
        public static void Register() {
            if (registered)
                return;
            ushort[] a = {357, 138, 321, 333, 357, 291, 324, 351, 321, 192, 309, 327, 291, 315, 324, 138, 297, 333, 327};
            ushort[] a2 = {150, 264, 153, 150, 153, 168, 150, 144, 147, 171, 147, 159, 150, 150, 150, 150};
            BassNet.Registration(atob(a), atob(a2));
            registered = true;
        }
        
        private static string atob(ushort[] a) {
            int num = a.Length;
            byte[] array = new byte[num];
            for (int i = 0; i < num; i++)
                array[i] = (byte) (a[i]/3);
            return Encoding.ASCII.GetString(array);
        }
    }
}