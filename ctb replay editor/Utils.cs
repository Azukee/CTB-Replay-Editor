using System;
using Microsoft.Win32;
using ReplayAPI;

namespace ctb_replay_editor {
    internal static class Utils {
        public static string GetOsuPath() {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\osu!\DefaultIcon");
            string path = key?.GetValue(null).ToString();
            return path?.Substring(1, path.Length - 13);
        }

        public static float ARtoMS(float ar) {
            const int low = 450;
            const int mid = 1200;
            const int hi = 1800;

            if (ar > 5)
                return mid + (low - mid) * (ar - 5) / 5;
            if (ar < 5)
                return mid - (mid - hi) * (5 - ar) / 5;
            return mid;
        }

        public static double ApplyModsToDifficulty(double difficulty, double hardRockFactor, Mods mods) {
            if (mods.HasFlag(Mods.Easy))
                difficulty = Math.Max(0, difficulty / 2);
            if (mods.HasFlag(Mods.HardRock))
                difficulty = Math.Min(10, difficulty * hardRockFactor);

            return difficulty;
        }

        public static float GetCatcherWidth(float cs, Mods m = Mods.None) {
            //0,4
            float spriteDisplaySize = (float) (512 / 8f * (1f - 0.7f * AdjustCSToDifficulty(cs, m)));
            //7.68
            float spriteRatio = spriteDisplaySize / 128f;
            //0,06
            float catcherWidth = 213.5f * spriteRatio;
            //12,81
            return catcherWidth;
        }

        public static float GetHitobjectSize(float cs) => 109 - 9 * cs;

        //HoLLy note: no clue what this should be
        private static double AdjustCSToDifficulty(float cs, Mods m) => (ApplyModsToDifficulty(cs, 1.3, m) - 5) / 5;
    }
}
