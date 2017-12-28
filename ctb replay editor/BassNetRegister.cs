using Un4seen.Bass;

namespace ctb_replay_editor {
    public class BassNetRegister {
        private static bool registered;

        public static void Register() {
            if (registered)
                return;
            
            BassNet.Registration("poo@poo.com", "2X25242411252422");
            registered = true;
        }
    }
}