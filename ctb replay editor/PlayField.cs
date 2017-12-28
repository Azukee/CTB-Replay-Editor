using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using osu_database_reader.Components.HitObjects;
using ReplayAPI;
using Un4seen.Bass;
using Keys = ReplayAPI.Keys;

namespace ctb_replay_editor {
    public class PlayField : Game {
        public float ApproachRateInMS = 300f;

        public ReplayFrame cFrame;

        public float CharPos = 256f;
        public float CircleSize = 0.5f;

        public bool GoingLeft;
        private readonly GraphicsDeviceManager graphics;
        public bool IsDash;

        public float osuPixelCircleSize = 0.0f;
        private SpriteBatch spriteBatch;
        public float WIDTH = 0.0f;

        public PlayField(IntPtr Object, Form OwnerForm) {
            DrawingSurface = Object;
            Size = new Vector2(512, 512);
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 512;
            graphics.PreferredBackBufferHeight = 384;
            Size = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
            XNAForm = Control.FromHandle(Window.Handle);
            Mouse.WindowHandle = Object;
            XNAForm.VisibleChanged += XNAForm_VisibleChanged;
        }

        public Vector2 Size { get; set; }
        public Form ParentForm { get; set; }
        public Control XNAForm { get; set; }
        public IntPtr DrawingSurface { get; set; }

        private void XNAForm_VisibleChanged(object sender, EventArgs e) {
            if (XNAForm.Visible)
                XNAForm.Visible = false;
        }

        public static double ApplyModsToDifficulty(double difficulty, double hardRockFactor, Mods mods) {
            if (mods.HasFlag(Mods.Easy))
                difficulty = Math.Max(0, difficulty/2);
            if (mods.HasFlag(Mods.HardRock))
                difficulty = Math.Min(10, difficulty*hardRockFactor);

            return difficulty;
        }


        public double AdjustDifficulty() {
            return (ApplyModsToDifficulty(CircleSize, 1.3, Mods.None) - 5)/5;
        }

        public float getCatcherWidth() {
            //0,4
            float SpriteDisplaySize = (float) (512/8f*(1f - 0.7f*AdjustDifficulty())); //== 7,68
            //
            float SpriteRatio = SpriteDisplaySize/128;
            //0,06
            float catcherWidth = 305/1*SpriteRatio*0.7f;
            //12,81
            return catcherWidth;
        }

        private Texture2D TextureFromFile(string path, int width, int height) {
            try {
                return Texture2D.FromStream(GraphicsDevice, new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read), width, height, true);
            } catch (Exception) {
                return TextureFromColor(Color.White, 20, 20);
            }
        }

        private Texture2D TextureFromColor(Color color, int w = 1, int h = 1) {
            Texture2D texture = new Texture2D(GraphicsDevice, w, h);
            Color[] data = new Color[w*h];
            for (int i = 0; i < w*h; i++)
                data[i] = color;
            texture.SetData(data);
            return texture;
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e) {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = DrawingSurface;
        }

        protected override void Update(GameTime gameTime) {
            if (Program.form.Replay != null) {
                Program.form.osuTime = (int)Math.Round(Bass.BASS_ChannelBytes2Seconds(Program.CurrentAudioStream, Bass.BASS_ChannelGetPosition(Program.CurrentAudioStream))*1000, 0, MidpointRounding.AwayFromZero);
                cFrame = Program.form.Replay.ReplayFrames.First(a => a.Time >= Program.form.osuTime);
                CharPos = cFrame.X;
                IsDash = cFrame.Keys.HasFlag(Keys.M1);
                //if (cFrame.frameIndex != 0)
                //    GoingLeft = Program.form.Replay.ReplayFrames[cFrame.frameIndex - 1].X < cFrame.X;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (Program.form.Replay != null && Program.form.Objects != null) {
                int catcherWidth = (int) Math.Round(WIDTH, 0, MidpointRounding.AwayFromZero);
                int catcherHeight = (int) Math.Round(WIDTH + 7, 0, MidpointRounding.AwayFromZero);
                foreach (HitObject obj in Program.form.Objects.Where(a => Program.form.osuTime > a.Time - ApproachRateInMS && Program.form.osuTime < a.Time)) {
                    var posY = 384f - catcherHeight - (obj.Time - Program.form.osuTime) / ApproachRateInMS * 384f;
                    int CalculatedCircleSize = (int) Math.Round(osuPixelCircleSize, 0, MidpointRounding.AwayFromZero);
                    spriteBatch.Draw(TextureFromFile("images\\fruit-apple.png", CalculatedCircleSize, CalculatedCircleSize), new Vector2(obj.X - CalculatedCircleSize/2, posY), Color.White);
                }
                if (!IsDash)
                    spriteBatch.Draw(TextureFromFile("images\\catcher.png", catcherWidth, catcherHeight), new Vector2(CharPos - catcherWidth/2, 384f - catcherHeight), null, Color.White, 0, Vector2.Zero, 1, !GoingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                else
                    spriteBatch.Draw(TextureFromFile("images\\catcher.png", catcherWidth, catcherHeight), new Vector2(CharPos - catcherWidth/2, 384f - catcherHeight), null, Color.Red, 0, Vector2.Zero, 1, !GoingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}