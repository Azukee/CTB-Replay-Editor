using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using osu_database_reader.BinaryFiles;
using osu_database_reader.Components.Beatmaps;
using osu_database_reader.Components.HitObjects;
using osu_database_reader.TextFiles;
using ReplayAPI;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using Timer = System.Windows.Forms.Timer;

namespace ctb_replay_editor {
    public partial class MainForm : Form {
        public int CurrentFrame = 0;
        public int CurrentObject = 0;
        public List<HitObject> Objects;
        public int OsuTime = 0;
        public Replay Replay;
        public bool IsPaused;

        public PlayField PlayField = null;

        private readonly string osuPath = Utils.GetOsuPath();
        private readonly Timer timerGui;

        public MainForm() {
            InitializeComponent();
            BassNetRegister.Register();
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);

            timerGui = new Timer {Interval = 100};
            timerGui.Tick += (o, e) => {
                charPosLabel.Text = $"Char Pos: {PlayField.CharPos:F2}";
                timeLabel.Text = $"Time: {OsuTime/1000f:F3}s";
            };
            timerGui.Start();
        }

        private void loadReplayButton_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog {Filter = "osu! replays (*.osr)|*.osr|All Files (*.*)|*.*"};
            if (ofd.ShowDialog() != DialogResult.OK) return;

            Replay = new Replay(ofd.FileName, true);
            Replay.ReplayFrames.Add(new ReplayFrame {Time = int.MaxValue - 1});

            //re-reading osu!.db every time to save some memory
            //it only takes a few ms anyway
            BeatmapEntry beatmapEntry = OsuDb.Read(osuPath + @"\osu!.db").Beatmaps.Find(a => a.BeatmapChecksum == Replay.MapHash);

            string beatmapFolder = Path.Combine(osuPath, "Songs", beatmapEntry.FolderName);
            string beatmapFilePath = Path.Combine(beatmapFolder, beatmapEntry.BeatmapFileName);
            string audioFilePath = Path.Combine(beatmapFolder, beatmapEntry.AudioFileName);

            PlayField.ApproachRateInMS = Utils.ARtoMS(beatmapEntry.ApproachRate);

            BeatmapFile beatmapFile = BeatmapFile.Read(beatmapFilePath);
            Objects = beatmapFile.HitObjects;
            PlayField.Width = Utils.GetCatcherWidth(beatmapEntry.CircleSize, Replay.Mods);
            PlayField.OsuPixelCircleSize = Utils.GetHitobjectSize(beatmapEntry.CircleSize);
            PlayField.HitArray = Utils.InitializeHits(Replay.ReplayFrames, Objects, PlayField.Width);

            Utils.InitializeHyperDash(Objects, PlayField.Width);

            Program.CurrentAudioStream = Bass.BASS_StreamCreateFile(audioFilePath, 0, 0, BASSFlag.BASS_STREAM_DECODE);
            Program.CurrentAudioStream = BassFx.BASS_FX_TempoCreate(Program.CurrentAudioStream, BASSFlag.BASS_FX_TEMPO_ALGO_LINEAR);

            if (Replay.Mods.HasFlag(Mods.DoubleTime) || Replay.Mods.HasFlag(Mods.NightCore))
                if (!Bass.BASS_ChannelSetAttribute(Program.CurrentAudioStream, BASSAttribute.BASS_ATTRIB_TEMPO, 25)) { 
                    BASSError lBassError = Bass.BASS_ErrorGetCode();
                    throw new Exception(lBassError.ToString());
                }

            if (Replay.Mods.HasFlag(Mods.HalfTime))
                if (!Bass.BASS_ChannelSetAttribute(Program.CurrentAudioStream, BASSAttribute.BASS_ATTRIB_TEMPO, -25)) {
                    BASSError lBassError = Bass.BASS_ErrorGetCode();
                    throw new Exception(lBassError.ToString());
                }

            timeTrackBar.Maximum = Replay.ReplayFrames.Count;
            Bass.BASS_ChannelPlay(Program.CurrentAudioStream, false);
        }

        public void ChangeTrackBarPosition(int replayFrameNumber) {
            timeTrackBar.Invoke(new MethodInvoker(() => timeTrackBar.Value = replayFrameNumber));
        }

        private void MainForm_Load(object sender, EventArgs e) { }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            timerGui?.Stop();
            Bass.BASS_Stop();
            PlayField.Exit();
        }

        private void togglePauseButton_Click(object sender, EventArgs e) {
            if (!IsPaused) {
                Bass.BASS_ChannelPause(Program.CurrentAudioStream);
                IsPaused = true;
            } else {
                Bass.BASS_ChannelPlay(Program.CurrentAudioStream, false);
                IsPaused = false;
            }
        }

        private void stopPlayButton_Click(object sender, EventArgs e) {
            Bass.BASS_ChannelStop(Program.CurrentAudioStream);
        }

        private void editModeCheckBox_CheckedChanged(object sender, EventArgs e) {
            PlayField.IsEditMode = editModeCheckBox.Checked;
        }

        private void timeTrackBar_Scroll(object sender, EventArgs e) {
            Utils.JumpToTime(Replay.ReplayFrames[timeTrackBar.Value].Time);
        }
    }
}