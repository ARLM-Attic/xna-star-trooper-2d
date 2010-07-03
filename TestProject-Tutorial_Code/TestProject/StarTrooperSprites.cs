#region Using directives

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion



namespace TestProject
{
  public class Trooper: Sprite
  {
    public Trooper(Texture2D Texture, int Frames, bool Loop)
          : base(Texture, Frames, Loop)
    { }

    public override void Update()
    {
        Vector2 vel = Vector2.Zero;
        switch (Input.InputMappings.AltMoveMethod)
        {
            case MovementMethod.Analogue:
                vel = Input.TrooperMoveStick();
                break;
            default:
                if (Input.MoveUp())
                    vel.Y = -2; // if trooper is under y=50 then go upward
                if (Input.MoveDown())
                    vel.Y = 2; // if trooper is over y=450 then go upward
                if (Input.MoveLeft())
                {
                    vel.X = -2; // go to the left
                }
                if (Input.MoveRight())
                {
                    vel.X = 2; // go to the right
                }
                break;
        }

        if (isWithinScreenBounds(Position + (vel * Speed))) Velocity = vel * Speed; else Velocity = Vector2.Zero;// set new velocity for Trooper

        if (Velocity.X > 0)
            SpriteEffect = SpriteEffects.None; // right flip trooper
        else
            SpriteEffect = SpriteEffects.FlipHorizontally; // Left flip trooper

        // if space bar is triggered
        if (Input.TrooperFired())
            TrooperFire();

        
    }
    

    void TrooperFire()
    {
        // dynamically create a new sprite
        Fire fire = (Fire)StarTrooperGame.Fire.Clone();
        fire.Position = new Vector2(Position.X, Position.Y - 35);
        fire.Velocity = new Vector2(0, -4);
        StarTrooperGame.Add(fire); // set the fire sprite active
        FireballLaunch(new Vector2(Position.X, Position.Y - 35), new Vector2(0, -40), new Vector2(0, -0.5f));


    }
    void FireballLaunch(Vector2 position, Vector2 velocity, Vector2 accel)
    {

        FireballSmokeParticleEmitter smokeemitter = new FireballSmokeParticleEmitter();
        smokeemitter.Initialize("smoke", 10);
        smokeemitter.EmitterPosition = position;
        smokeemitter.EmitterVelocity = velocity;
        smokeemitter.EmitterAcceleration = accel;
        smokeemitter.ParticleCycleTime = 0f;
        StarTrooperGame.ParticleManager.Add(smokeemitter);
		
        FireballParticleEmitter fireballemitter = new FireballParticleEmitter();
        fireballemitter.Initialize("explosion", 10);
        fireballemitter.EmitterPosition = position;
        fireballemitter.EmitterVelocity = velocity;
        fireballemitter.EmitterAcceleration = accel;
        fireballemitter.ParticleCycleTime = 0f;
        StarTrooperGame.ParticleManager.Add(fireballemitter);

    }
  }

  public class Condor : Sprite
  {
    public Condor()
    {
    }

    protected Condor(Condor condor): base(condor)
    {
    }

    public override Object Clone()
    {
      return new Condor(this);
    }

    public override void Update()
    {
        Trooper b = StarTrooperGame.Trooper;

        Vector2 v = new Vector2(b.Position.X - Position.X, b.Position.Y - Position.Y);
        v.Normalize();

        Velocity = v;

        if (v.X >= 0)
          SpriteEffect = SpriteEffects.None;
        else
          SpriteEffect = SpriteEffects.FlipHorizontally;
    }
  }
  public class Fire : Sprite
  {
    public Fire()
    {
    }

    protected Fire(Fire Fire)
        : base(Fire)
    {
    }
    public Fire(Texture2D Texture, int Frames, bool Loop)
          : base(Texture, Frames, Loop)
    { }

    public override Object Clone()
    {
        return new Fire(this);
    }

    public override void Update()
    {

    }
  }
}
