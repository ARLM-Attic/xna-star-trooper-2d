using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Starter3DGame
{
    static class GameConstants
    {
        //camera constants
        public const float CameraHeight = 250.0f;
        public const float PlayfieldSizeX = 70f;
        public const float PlayfieldSizeY = 115f;
		//asteroid constants
        public const int NumAsteroids = 5;
        public const float AsteroidMinSpeed = 10f;
        public const float AsteroidMaxSpeed = 30.0f;
        public const float AsteroidSpeedAdjustment = 0.5f;
        //collision constants
        public const float AsteroidBoundingSphereScale = 0.95f;  //95% size
        public const float ShipBoundingSphereScale = 0.5f;  //50% size
        //bullet constants
        public const int NumBullets = 30;
        public const float BulletSpeedAdjustment = 100.0f;
        //scoring constants
        public const int ShotPenalty = 1;
        public const int DeathPenalty = 100;
        public const int WarpPenalty = 50;
        public const int KillBonus = 25;
    }

}
