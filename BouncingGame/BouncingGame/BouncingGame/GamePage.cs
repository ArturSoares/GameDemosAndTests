﻿using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using CocosSharp;


namespace BouncingGame
{
    public class GamePage : ContentPage
    {
        CocosSharpView gameView;

        public GamePage()
        {
            gameView = new CocosSharpView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                // Set the game world dimensions
                DesignResolution = new Size(1024, 768),
                // Set the method to call once the view has been initialised
                ViewCreated = LoadGame
            };

            Content = gameView;
        }

        protected override void OnDisappearing()
        {
            if (gameView != null)
            {
                gameView.Paused = true;
            }

            base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (gameView != null)
                gameView.Paused = false;
        }

        void LoadGame(object sender, EventArgs e)
        {
            var nativeGameView = sender as CCGameView;

            if (nativeGameView != null)
            {
                var contentSearchPaths = new List<string>() { "Fonts", "Sounds" };
                CCSizeI viewSize = nativeGameView.ViewSize;
                CCSizeI designResolution = nativeGameView.DesignResolution;

                viewSize.Width = 768;
                viewSize.Height = 1027;

                nativeGameView.ContentManager.SearchPaths = contentSearchPaths;

                CCScene gameScene = new CCScene(nativeGameView);
                gameScene.AddLayer(new GameLayer());
                nativeGameView.RunWithScene(gameScene);
            }
        }
    }
}
