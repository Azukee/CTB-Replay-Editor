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
        public float OsuPixelCircleSize = 0.0f;
        public float Width = 0.0f;

        public bool GoingLeft;
        public bool IsDash;
        public bool IsEditMode;

        public bool[] HitArray;

        private SpriteBatch spriteBatch;
        
        public MouseState MouseState;
        public KeyboardState CurrentKeyboardState;
        public KeyboardState PreviousKeyboardState;

        public EditorState EditorState;

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

        //Azuki note: having PreviousKeyboardState prevents the bot doing multiple unwated actions: i.e
        //Hopping multiple notes rather than just one.
        protected override void Update(GameTime gameTime) {
            if (Program.Form.Replay != null) {
                MouseState = Mouse.GetState();
                CurrentKeyboardState = Keyboard.GetState();

                Program.Form.OsuTime = (int)Math.Round(Bass.BASS_ChannelBytes2Seconds(Program.CurrentAudioStream, 
                    Bass.BASS_ChannelGetPosition(Program.CurrentAudioStream))*1000, 0, MidpointRounding.AwayFromZero);

                var currFrame = Program.Form.Replay.ReplayFrames.First(a => a.Time >= Program.Form.OsuTime);
                CharPos = currFrame.X;
                IsDash = currFrame.Keys.HasFlag(Keys.M1);

                int currFrameIndex = Program.Form.Replay.ReplayFrames.FindIndex(a => a.Time == currFrame.Time);

                Program.Form.ChangeTrackBarPosition(currFrameIndex);
                
                if (Program.Form.Replay.ReplayFrames.IndexOf(currFrame) != 0) 
                    GoingLeft = Program.Form.Replay.ReplayFrames[currFrameIndex - 1].X < currFrame.X;

                if (IsEditMode) {
                    if (CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) &&
                        CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.E))
                        EditorState = EditorState.EditingFrame;

                    if (CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                        EditorState = EditorState.Watching;

                    if (CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                        Utils.JumpToTime(CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) ? Program.Form.OsuTime - 10 : Program.Form.OsuTime - 1);

                    if (CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                        Utils.JumpToTime(CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) ? Program.Form.OsuTime + 10 : Program.Form.OsuTime + 1);

                    //Azuki note: Here only check if previouskeyboardstate is not key down V because if you check for LeftControl aswell
                    //holding LeftControl won't work and you have to hit the keys at the EXACT same tick which is gay
                    if (CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) &&
                        CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.V) &&
                        !PreviousKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.V))
                        Utils.JumpToTime(Program.Form.Objects.First(a => a.Time > Program.Form.OsuTime).Time);

                    if (EditorState == EditorState.EditingFrame) { 
                        if (CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                            currFrame.X += CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) ? 2.0f : 0.1f;

                        if (CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                            currFrame.X -= CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) ? 2.0f : 0.1f;
                    }
                    Program.Form.Replay.ReplayFrames[Program.Form.Replay.ReplayFrames.FindIndex(a => a.Time == currFrame.Time)] = currFrame;
                }
                PreviousKeyboardState = CurrentKeyboardState;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(EditorState.HasFlag(EditorState.EditingFrame) ? Color.DarkGreen : Color.Black);
            spriteBatch.Begin();
            if (Program.Form.Replay != null && Program.Form.Objects != null) {
                int catcherWidth = (int) Math.Round(Width, 0, MidpointRounding.AwayFromZero);
                int catcherHeight = (int) Math.Round(Width + 7, 0, MidpointRounding.AwayFromZero);
                foreach (HitObject obj in Program.Form.Objects.Where(a => Program.Form.OsuTime > a.Time - ApproachRateInMS && Program.Form.OsuTime < a.Time)) {
                    bool isHit = HitArray[Program.Form.Objects.IndexOf(obj)];
                    var posY = 384f - catcherHeight - (obj.Time - Program.Form.OsuTime) / ApproachRateInMS * 384f;
                    int calculatedCS = (int) Math.Round(OsuPixelCircleSize, 0, MidpointRounding.AwayFromZero);
                    spriteBatch.Draw(TextureFromFile(isHit ? "images\\fruit-apple.png" : "images\\fruit-apple-miss.png",
                        calculatedCS, calculatedCS),
                        new Vector2(obj.X - calculatedCS/2, posY),
                        obj.Y != Int16.MaxValue ? Color.White : Color.Red);
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