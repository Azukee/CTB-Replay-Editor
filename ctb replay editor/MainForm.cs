using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using osu_database_reader.BinaryFiles;
using osu_database_reader.Components.Beatmaps;
using osu_database_reader.Components.HitObjects;
using osu_database_reader.TextFiles;
using ReplayAPI;
using Un4seen.Bass;

namespace ctb_replay_editor {
    public partial class MainForm : Form {
        public BeatmapEntry BeatmapEntry;
        public BeatmapFile BeatmapFile;
        public int currentFrame = 0;
        public int currentObject = 0;
        public List<HitObject> Objects;
        public int osuTime = 0;
        public PlayField PlayField = null;
        public OsuDb Reader;
        public Replay Replay;
        public Thread StuffThread;

        public MainForm() {
            InitializeComponent();
            BassNetRegister.Register();
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            Bass.BASS_SetVolume(0.45f);
        }

        public void LabelKeeper() {
            while (true) {
                charPosLabel.Invoke(new MethodInvoker(() => charPosLabel.Text = $"Char Pos: {PlayField.CharPos}"));
                timeLabel.Invoke(new MethodInvoker(() => timeLabel.Text = $"Time: {osuTime}"));
                Thread.Sleep(5);
            }
        }

        public IntPtr GetControlHandle(Control control) {
            return control.Handle;
        }

        private void loadReplayButton_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog {Filter = "osu! replays (*.osr)|*.osr|All Files (*.*)|*.*"};
            if (ofd.ShowDialog() == DialogResult.OK) {
                Replay = new Replay(ofd.FileName, true);
                Replay.ReplayFrames.Add(new ReplayFrame {Time = int.MaxValue - 1});
                BeatmapEntry = Reader.Beatmaps.Find(a => a.BeatmapChecksum == Replay.MapHash);
                BeatmapFile = BeatmapFile.Read(@"E:\osu!\Songs\" + BeatmapEntry.FolderName + @"\" + BeatmapEntry.BeatmapFileName);
                PlayField.ApproachRateInMS = ARtoMS(BeatmapEntry.ApproachRate);
                PlayField.CircleSize = BeatmapEntry.CircleSize;
                Objects = BeatmapFile.HitObjects;
                PlayField.WIDTH = PlayField.getCatcherWidth();
                PlayField.osuPixelCircleSize = 109 - 9 * PlayField.CircleSize;
                Program.CurrentAudioStream = Bass.BASS_StreamCreateFile(@"E:\osu!\Songs\" + BeatmapEntry.FolderName + @"\" + BeatmapEntry.AudioFileName, 0, 0, BASSFlag.BASS_DEFAULT);
                Bass.BASS_ChannelPlay(Program.CurrentAudioStream, false);
            }
        }

        public float ARtoMS(float AR) {
            //time on screen
            if (AR > 5)
                return 1200 + (450 - 1200) * (AR - 5) / 5;
            if (AR < 5)
                return 1200 - (1200 - 1800) * (5 - AR) / 5;
            return 1200;
        }

        private void MainForm_Load(object sender, EventArgs e) {
            StuffThread = new Thread(LabelKeeper) {IsBackground = true};
            StuffThread.Start();
            Reader = OsuDb.Read(@"E:\osu!\osu!.db");
        }
    }
}