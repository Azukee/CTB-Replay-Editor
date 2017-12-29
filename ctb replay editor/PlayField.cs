using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using osu_database_reader.Components.HitObjects;
using Un4seen.Bass;
using Keys = ReplayAPI.Keys;

namespace ctb_replay_editor {
    public class PlayField : Game {
        public float ApproachRateInMS = 300f;

        public float CharPos = 256f;

        public bool GoingLeft;
        public bool IsDash;

        public float OsuPixelCircleSize = 0.0f;
        private SpriteBatch spriteBatch;
        public float Width = 0.0f;

        public PlayField(IntPtr Object) {
            //change target to our control
            var graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 512,
                PreferredBackBufferHeight = 384
            };
            graphics.PreparingDeviceSettings += (sender, e) => e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = Object;
            Mouse.WindowHandle = Object;

            //hide the XNAForm
            Control.FromHandle(Window.Handle).VisibleChanged += (sender, args) => ((Control) sender).Visible = false;
        }

        private Texture2D TextureFromFile(string path, int width, int height) {
            try {
                return Texture2D.FromStream(GraphicsDevice, new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read), width, height, true);
            } catch (Exception) {
                return TextureFromColor(Color.White, width, height);
            }
        }

        private Texture2D TextureFromColor(Color color, int w = 1, int h = 1) {
            var texture = new Texture2D(GraphicsDevice, w, h);
            var data = new Color[w*h];
            for (int i = 0; i < w*h; i++)
                data[i] = color;
            texture.SetData(data);
            return texture;
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            if (Program.Form.Replay != null) {
                Program.Form.OsuTime = (int)Math.Round(Bass.BASS_ChannelBytes2Seconds(Program.CurrentAudioStream, Bass.BASS_ChannelGetPosition(Program.CurrentAudioStream))*1000, 0, MidpointRounding.AwayFromZero);
                var currFrame = Program.Form.Replay.ReplayFrames.First(a => a.Time >= Program.Form.OsuTime);
                CharPos = currFrame.X;
                IsDash = currFrame.Keys.HasFlag(Keys.M1);
                //if (CurrFrame.frameIndex != 0)
                //    GoingLeft = Program.form.Replay.ReplayFrames[CurrFrame.frameIndex - 1].X < CurrFrame.X;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (Program.Form.Replay != null && Program.Form.Objects != null) {
                int catcherWidth = (int) Math.Round(Width, 0, MidpointRounding.AwayFromZero);
                int catcherHeight = (int) Math.Round(Width + 7, 0, MidpointRounding.AwayFromZero);
                foreach (HitObject obj in Program.Form.Objects.Where(a => Program.Form.OsuTime > a.Time - ApproachRateInMS && Program.Form.OsuTime < a.Time)) {
                    var posY = 384f - catcherHeight - (obj.Time - Program.Form.OsuTime) / ApproachRateInMS * 384f;
                    int calculatedCS = (int) Math.Round(OsuPixelCircleSize, 0, MidpointRounding.AwayFromZero);
                    spriteBatch.Draw(TextureFromFile("images\\fruit-apple.png", calculatedCS, calculatedCS), new Vector2(obj.X - calculatedCS/2, posY), Color.White);
                }
                spriteBatch.Draw(TextureFromFile("images\\catcher.png", catcherWidth, catcherHeight),
                    new Vector2(CharPos - catcherWidth / 2f, 384f - catcherHeight), null,
                    !IsDash ? Color.White : Color.Red, 0, Vector2.Zero, 1,
                    !GoingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}