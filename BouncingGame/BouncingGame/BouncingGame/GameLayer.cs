using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace BouncingGame
{
    public class GameLayer : CCLayerColor
    {
        private CCSprite paddleSprite;
        private CCSprite ballSprite;
        private CCLabel scoreLabel;

        private float ballXVelocity;
        private float ballYVelocity;

        int score;

        // How much to modify the ball's y velocity per second:
        private const float gravity = 140;

        public GameLayer() : base(CCColor4B.Black)
        {
            paddleSprite = new CCSprite("paddle");
            paddleSprite.PositionX = 100;
            paddleSprite.PositionY = 100;
            AddChild(paddleSprite);
            ballSprite = new CCSprite("ball");
            ballSprite.PositionX = 320;
            ballSprite.PositionY = 600;
            AddChild(ballSprite);
            scoreLabel = new CCLabel("Score: 0", "Arial", 20, CCLabelFormat.SystemFont);
            scoreLabel.Color = CCColor3B.White;
            scoreLabel.PositionX = 10;
            scoreLabel.PositionY = 700;
            scoreLabel.AnchorPoint = CCPoint.AnchorUpperLeft;
            AddChild(scoreLabel);

            Schedule(RunGameLogic);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            CCRect bounds = VisibleBoundsWorldspace;

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();

            touchListener.OnTouchesMoved = HandleTouchesMoved;
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);
        }

        void HandleTouchesMoved(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
        {
            // we only care about the first touch:
            var locationOnScreen = touches[0].Location;
            paddleSprite.PositionX = locationOnScreen.X;
        }

        private void RunGameLogic(float frameTimeInSeconds)
        {
            // This is a linear approximation, so not 100% accurate

            ballYVelocity += frameTimeInSeconds * -gravity;
            ballSprite.PositionX += ballXVelocity * frameTimeInSeconds;
            ballSprite.PositionY += ballYVelocity * frameTimeInSeconds;

            // Check if the two CCSprites overlap...
            bool doesBallOverlapPaddle = ballSprite.BoundingBoxTransformedToParent.IntersectsRect(
                paddleSprite.BoundingBoxTransformedToParent);

            // Artur Code
            var halfOfRectangle = paddleSprite.BoundingBoxTransformedToParent.Size.Width / 2;
            var rectangleLeftSide = new CCRect(paddleSprite.BoundingBoxTransformedToParent.MinX, paddleSprite.BoundingBoxTransformedToParent.MinY, halfOfRectangle, paddleSprite.BoundingBoxTransformedToParent.MaxY);
            var rectangleRightSide = new CCRect(paddleSprite.BoundingBoxTransformedToParent.MidX, paddleSprite.BoundingBoxTransformedToParent.MinY, halfOfRectangle, paddleSprite.BoundingBoxTransformedToParent.MaxY);
            bool ballOverlapPaddleAtLeftSide = ballSprite.BoundingBoxTransformedToParent.IntersectsRect(rectangleLeftSide);
            bool ballOverlapPaddleAtRightSide = ballSprite.BoundingBoxTransformedToParent.IntersectsRect(rectangleRightSide);
            // End Artur Code

            // ... and if the ball is moving downward.
            bool isMovingDownward = ballYVelocity < 0;

            if (doesBallOverlapPaddle && isMovingDownward)
            {
                // First let's invert the velocity:
                ballYVelocity *= -1;
                // Then let's assign a random value to the ball's x velocity:
                const float minXVelocity = -300;
                const float maxXVelocity = 300;
                /*
                ballXVelocity = CCRandom.GetRandomFloat(minXVelocity, maxXVelocity);
                */

                // Artur Code
                if (ballOverlapPaddleAtLeftSide)
                {
                    ballXVelocity = CCRandom.GetRandomFloat(-1, minXVelocity);
                }
                if (ballOverlapPaddleAtRightSide)
                {
                    ballXVelocity = CCRandom.GetRandomFloat(1, maxXVelocity);
                }
                // End Artur Code


                score++;
                scoreLabel.Text = "Score: " + score;
            }

            // First let’s get the ball position:   
            float ballRight = ballSprite.BoundingBoxTransformedToParent.MaxX;
            float ballLeft = ballSprite.BoundingBoxTransformedToParent.MinX;
            // Then let’s get the screen edges
            float screenRight = VisibleBoundsWorldspace.MaxX;
            float screenLeft = VisibleBoundsWorldspace.MinX;
            // Check if the ball is either too far to the right or left:    
            bool shouldReflectXVelocity = (ballRight > screenRight && ballXVelocity > 0) || (ballLeft < screenLeft && ballXVelocity < 0);

            if (shouldReflectXVelocity)
            {
                ballXVelocity *= -1;
            }
        }

        private void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
            }
        }
    }
}

