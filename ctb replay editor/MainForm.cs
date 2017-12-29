using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using osu_database_reader.BinaryFiles;
using osu_database_reader.Components.Beatmaps;
using osu_database_reader.Components.HitObjects;
using osu_database_reader.TextFiles;
using ReplayAPI;
using Un4seen.Bass;
using Timer = System.Windows.Forms.Timer;

namespace ctb_replay_editor {
    public partial class MainForm : Form {
        public int CurrentFrame = 0;
        public int CurrentObject = 0;
        public List<HitObject> Objects;
        public int OsuTime = 0;
        public Replay Replay;

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
            PlayField.Width = Utils.GetCatcherWidth(beatmapEntry.CircleSize);
            PlayField.OsuPixelCircleSize = Utils.GetHitobjectSize(beatmapEntry.CircleSize);

            Utils.InitializeHyperDash(Objects, PlayField.Width);

            Program.CurrentAudioStream = Bass.BASS_StreamCreateFile(audioFilePath, 0, 0, BASSFlag.BASS_DEFAULT);
            Bass.BASS_ChannelPlay(Program.CurrentAudioStream, false);
        }

        private void MainForm_Load(object sender, EventArgs e) {
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            timerGui?.Stop();
            Bass.BASS_Stop();
            PlayField.Exit();
        }
    }
}