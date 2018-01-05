using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using osu.Shared;
using osu_database_reader.Components.HitObjects;
using ReplayAPI;
using Un4seen.Bass;
using Mods = ReplayAPI.Mods;

namespace ctb_replay_editor {
    internal static class Utils {
        public static string GetOsuPath() {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\osu!\DefaultIcon");
            string path = key?.GetValue(null).ToString();
            return path?.Substring(1, path.Length - 13);
        }

        public static void JumpToTime(int ms) {
            if (Program.CurrentAudioStream != 0) {
                if (ms < 0)
                    ms = 0;
                Bass.BASS_ChannelSetPosition(Program.CurrentAudioStream, ms / (double)1000);
            }
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

        //Azuki note: I need to implement this for slider ends and slider ticks later too, but at the moment sliders aren't working and will not be for some time
        public static void InitializeHyperDash(List<HitObject> HitObjects, float catcherWidth) {
            int lastDirection = 0;
            float catcherWidthHalf = catcherWidth / 2;
            float lastExcess = catcherWidthHalf;

            for (int i = 0; i < HitObjects.Count - 1; i++) {
                HitObject currentObject = HitObjects[i];
                HitObject nextObject = HitObjects[i + 1];

                int thisDirection = nextObject.X > currentObject.X ? 1 : -1;
                //Azuki note: nextObject.Time is actually StartTime and currentObject.time is actually EndTime (for sliders later)
                float timeToNext = nextObject.Time - currentObject.Time - (float)((double)1000 / 60 / 4);
                float distanceToNext = Math.Abs(nextObject.X - currentObject.X) - (lastDirection == thisDirection ? lastExcess : catcherWidthHalf);

                if (timeToNext < distanceToNext) {
                    //Azuki note: Manipulate the Y position of the HitObject because it's useless for CtB anyways
                    //so I'm using it to indiciate if a HitObject is a HyperDash
                    HitObjects[i].Y = Int16.MaxValue;
                    lastExcess = catcherWidthHalf;
                }

                lastDirection = thisDirection;
            }
        }

        public static bool[] InitializeHits(List<ReplayFrame> ReplayFrames, List<HitObject> HitObjects, float catcherWidth) {
            bool[] isHitArray = new bool[HitObjects.Count];
            float catcherWidthHalf = catcherWidth / 2;
            float catchMargin = catcherWidth * 0.1f;

            for (var i = 0; i < HitObjects.Count; i++) {
                HitObject currObject = HitObjects[i];
                if (!currObject.Type.HasFlag(HitObjectType.Spinner)) {
                    ReplayFrame nearestFrame = ReplayFrames.First(a => a.Time >= currObject.Time);

                    isHitArray[i] = nearestFrame.X - catcherWidthHalf + catchMargin < currObject.X 
                        && nearestFrame.X + catcherWidthHalf - catchMargin > currObject.X;
                } else
                    isHitArray[i] = true;
            }
            return isHitArray;
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
        //Azuki note: no clue but osu! had it
        private static double AdjustCSToDifficulty(float cs, Mods m) => (ApplyModsToDifficulty(cs, 1.3, m) - 5) / 5;
    }

    public enum EditorState {
        Watching = 0,
        EditingFrame
    }
}
