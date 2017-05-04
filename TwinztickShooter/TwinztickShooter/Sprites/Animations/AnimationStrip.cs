using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramGame.Sprites.Animations
{
    public class AnimationStrip
    {
        #region Declarations
        private Texture2D texture;
        private int frameWidth;
        private int frameHeight;

        private float frameTimer = 0f;
        private float frameDelay = 0.05f;

        private int currentFrame;

        private bool loopAnimation = true;
        private bool finishedPlaying = false;

        private string name;
        private string nextAnimation;
        #endregion

        #region Properties
        /// <summary>
        /// Sets / Returns the width of each animation frame
        /// </summary>
        public int FrameWidth
        {
            get { return frameWidth; }
            set { frameWidth = value; }
        }

        /// <summary>
        /// Sets / Returns the height of each animation frame
        /// </summary>
        public int FrameHeight
        {
            get { return frameHeight; }
            set { frameHeight = value; }
        }

        /// <summary>
        /// Sets / Returns the texture sheet of the animation
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        /// <summary>
        /// Sets / Returns the name of the animation
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Sets / Returns what the next animation to play is
        /// </summary>
        public string NextAnimation
        {
            get { return nextAnimation; }
            set { nextAnimation = value; }
        }

        /// <summary>
        /// Sets / Returns if the animation should loop
        /// </summary>
        public bool LoopAnimation
        {
            get { return loopAnimation; }
            set { loopAnimation = value; }
        }

        /// <summary>
        /// Returns if the animation is finished playing
        /// </summary>
        public bool FinishedPlaying
        {
            get { return finishedPlaying; }
        }

        /// <summary>
        /// Returns how many frames there are in the animation
        /// </summary>
        public int FrameCount
        {
            get { return texture.Width / frameWidth; }
        }

        /// <summary>
        /// Sets / Returns how long each frame should be on the screen for
        /// </summary>
        public float FrameLength
        {
            get { return frameDelay; }
            set { frameDelay = value; }
        }

        /// <summary>
        /// Returns the rectangle of each frame
        /// </summary>
        public Rectangle FrameRectangle
        {
            get
            {
                return new Rectangle(currentFrame * frameWidth, 0, frameWidth, FrameHeight);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates an animation with a given texture, frameWidth and name.
        /// </summary>
        /// <param name="texture">The texture sheet of the animation</param>
        /// <param name="frameWidth">The width of the animation frame</param>
        /// <param name="name">The name of the animation</param>
        public AnimationStrip(Texture2D texture, int frameWidth, string name)
        {
            this.texture = texture;
            this.FrameWidth = frameWidth;
            this.frameHeight = texture.Height;
            this.name = name;
        }
        #endregion

        #region Public Methods
        //Sets the current animation to start playing
        public void Play()
        {
            currentFrame = 0;
            finishedPlaying = false;
        }

        //Updates the current animation to change the image
        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            frameTimer += elapsed;

            if (frameTimer >= FrameCount)
            {
                currentFrame++;
                if (currentFrame >= frameDelay)
                {
                    if (loopAnimation)
                    {
                        currentFrame = 0;
                    }
                    else
                    {
                        currentFrame = FrameCount - 1;
                        finishedPlaying = true;
                    }
                }
                frameTimer = 0f;
            }
        }
        #endregion
    }
}