using System.Collections.Generic;
using Ostium11.Extensions;
using UnityEngine;

#pragma warning disable IDE1006 // public properties start with lowercase for compatibility with unity API

namespace Ostium11
{
    // floats only
    public enum ParticleSystemProperty
    {
        [NestedEnumSubMenu("Main")]
        Duration = 0,
        StartDelay,
        StartLifetime,
        StartSpeed,
        StartSize,
        StartRotation,
        FlipRotation,
        GravityModifier,
        SimulationSpeed,

        [NestedEnumSubMenu("Emission")]
        RateOverTime = 50,
        RateOverDistance,
        FirstBurstCount,

        [NestedEnumSubMenu("Shape")]
        Angle = 100,
        Radius,
        DonutRadius,
        RadiusThickness,
        Length,
        PositionX,
        PositionY,
        PositionZ,
        RotationX,
        RotationY,
        RotationZ,
        ScaleX,
        ScaleY,
        ScaleZ,
        RandomizeDirection,
        SpherizeDirection,
        RandomizePosition,

        [NestedEnumSubMenu("Velocity Over Lifetime")]
        LinearX = 150,
        LinearY,
        LinearZ,
        OrbitalX,
        OrbitalY,
        OrbitalZ,
        OffsetX,
        OffsetY,
        OffsetZ,
        Radial,
        SpeedModifier,

        [NestedEnumSubMenu("Limit Velocity Over Lifetime")]
        Speed = 200,
        LimitX,
        LimitY,
        LimitZ,
        Dampen,
        Drag,

        [NestedEnumSubMenu("Inherit Velocity")]
        InheritVelocity = 250,

        [NestedEnumSubMenu("Lifetime By Emitter Speed")]
        LifetimeByEmitterSpeed = 300,
        RangeMin,
        RangeMax,

        [NestedEnumSubMenu("Force Over Lifetime")]
        ForceX = 350,
        ForceY,
        ForceZ,

        [NestedEnumSubMenu("Size Over Lifetime")]
        SizeCurveMultiplier = 500,
    }

    /// <summary>
    /// Allows to set ParticleSystem properties in one line and access them by property type enum.
    /// </summary>
    public class ParticleSystemWrapper
    {
        #region Modules

        public class Main
        {
            ParticleSystem.MainModule module;

            public Main(ParticleSystem.MainModule module) => this.module = module;

            public float duration { get => module.duration; set => module.duration = value; }
            public ParticleSystem.MinMaxCurve startDelay { get => module.startDelay; set => module.startDelay = value; }
            public ParticleSystem.MinMaxCurve startLifetime { get => module.startLifetime; set => module.startLifetime = value; }
            public ParticleSystem.MinMaxCurve startSpeed { get => module.startSpeed; set => module.startSpeed = value; }
            public ParticleSystem.MinMaxCurve startSize { get => module.startSize; set => module.startSize = value; }
            public ParticleSystem.MinMaxCurve startRotation { get => module.startRotation; set => module.startRotation = value; }
            public float flipRotation { get => module.flipRotation; set => module.flipRotation = value; }
            public ParticleSystem.MinMaxCurve gravityModifier { get => module.gravityModifier; set => module.gravityModifier = value; }
            public float simulationSpeed { get => module.simulationSpeed; set => module.simulationSpeed = value; }
        }

        public class Emission
        {
            ParticleSystem.EmissionModule module;

            public Emission(ParticleSystem.EmissionModule module) => this.module = module;
            public ParticleSystem.MinMaxCurve rateOverTime { get => module.rateOverTime; set => module.rateOverTime = value; }
            public ParticleSystem.MinMaxCurve rateOverDistance { get => module.rateOverDistance; set => module.rateOverDistance = value; }
            public ParticleSystem.MinMaxCurve firstBirstCount { get => module.GetBurst(0).count; set => SetBurstCount(0, value); }

            void SetBurstCount(int id, ParticleSystem.MinMaxCurve value)
            {
                var b = module.GetBurst(id);
                b.count = value;
                module.SetBurst(id, b);
            }
        }

        public class Shape
        {
            ParticleSystem.ShapeModule module;

            public Shape(ParticleSystem.ShapeModule module) => this.module = module;

            public float angle { get => module.angle; set => module.angle = value; }
            public float radius { get => module.radius; set => module.radius = value; }
            public float donutRadius { get => module.donutRadius; set => module.donutRadius = value; }
            public float radiusThickness { get => module.radiusThickness; set => module.radiusThickness = value; }
            public float length { get => module.length; set => module.length = value; }
            public float randomDirectionAmount { get => module.randomDirectionAmount; set => module.randomDirectionAmount = value; }
            public Vector3 position { get => module.position; set => module.position = value; }
            public Vector3 rotation { get => module.rotation; set => module.rotation = value; }
            public Vector3 scale { get => module.scale; set => module.scale = value; }
            public float sphericalDirectionAmount { get => module.sphericalDirectionAmount; set => module.sphericalDirectionAmount = value; }
            public float randomPositionAmount { get => module.randomPositionAmount; set => module.randomPositionAmount = value; }
        }

        public class VelocityOverLifetime
        {
            ParticleSystem.VelocityOverLifetimeModule module;

            public VelocityOverLifetime(ParticleSystem.VelocityOverLifetimeModule module) => this.module = module;

            public ParticleSystem.MinMaxCurve x { get => module.x; set => module.x = value; }
            public ParticleSystem.MinMaxCurve y { get => module.y; set => module.y = value; }
            public ParticleSystem.MinMaxCurve z { get => module.z; set => module.z = value; }
            public ParticleSystem.MinMaxCurve orbitalX { get => module.orbitalX; set => module.orbitalX = value; }
            public ParticleSystem.MinMaxCurve orbitalY { get => module.orbitalY; set => module.orbitalY = value; }
            public ParticleSystem.MinMaxCurve orbitalZ { get => module.orbitalZ; set => module.orbitalZ = value; }
            public ParticleSystem.MinMaxCurve orbitalOffsetX { get => module.orbitalOffsetX; set => module.orbitalOffsetX = value; }
            public ParticleSystem.MinMaxCurve orbitalOffsetY { get => module.orbitalOffsetY; set => module.orbitalOffsetY = value; }
            public ParticleSystem.MinMaxCurve orbitalOffsetZ { get => module.orbitalOffsetZ; set => module.orbitalOffsetZ = value; }
            public ParticleSystem.MinMaxCurve radial { get => module.radial; set => module.radial = value; }
            public ParticleSystem.MinMaxCurve speedModifier { get => module.speedModifier; set => module.speedModifier = value; }
        }

        public class LimitVelocityOverLifetime
        {
            ParticleSystem.LimitVelocityOverLifetimeModule module;

            public LimitVelocityOverLifetime(ParticleSystem.LimitVelocityOverLifetimeModule module) => this.module = module;

            public ParticleSystem.MinMaxCurve limit { get => module.limit; set => module.limit = value; }
            public ParticleSystem.MinMaxCurve limitX { get => module.limitX; set => module.limitX = value; }
            public ParticleSystem.MinMaxCurve limitY { get => module.limitY; set => module.limitY = value; }
            public ParticleSystem.MinMaxCurve limitZ { get => module.limitZ; set => module.limitZ = value; }
            public float dampen { get => module.dampen; set => module.dampen = value; }
            public ParticleSystem.MinMaxCurve drag { get => module.drag; set => module.drag = value; }
        }

        public class InheritVelocity
        {
            ParticleSystem.InheritVelocityModule module;

            public InheritVelocity(ParticleSystem.InheritVelocityModule module) => this.module = module;

            public ParticleSystem.MinMaxCurve curve { get => module.curve; set => module.curve = value; }
        }

        public class LifetimeByEmitterSpeed
        {
            ParticleSystem.LifetimeByEmitterSpeedModule module;

            public LifetimeByEmitterSpeed(ParticleSystem.LifetimeByEmitterSpeedModule module) => this.module = module;

            public ParticleSystem.MinMaxCurve curve { get => module.curve; set => module.curve = value; }
            public Vector2 range { get => module.range; set => module.range = value; }
        }

        public class ForceOverLifetime
        {
            ParticleSystem.ForceOverLifetimeModule module;

            public ForceOverLifetime(ParticleSystem.ForceOverLifetimeModule module) => this.module = module;
            public ParticleSystem.MinMaxCurve x { get => module.x; set => module.x = value; }
            public ParticleSystem.MinMaxCurve y { get => module.y; set => module.y = value; }
            public ParticleSystem.MinMaxCurve z { get => module.z; set => module.z = value; }
        }

        public class ColorOverLifetime
        {
            ParticleSystem.ColorOverLifetimeModule module;

            public ColorOverLifetime(ParticleSystem.ColorOverLifetimeModule module) => this.module = module;
        }

        public class ColorBySpeed
        {
            ParticleSystem.ColorBySpeedModule module;

            public ColorBySpeed(ParticleSystem.ColorBySpeedModule module) => this.module = module;
        }

        public class SizeOverLifetime
        {
            ParticleSystem.SizeOverLifetimeModule module;

            public SizeOverLifetime(ParticleSystem.SizeOverLifetimeModule module) => this.module = module;

            public float sizeMultiplier { get => module.sizeMultiplier; set => module.sizeMultiplier = value; }
        }

        public class SizeBySpeed
        {
            ParticleSystem.SizeBySpeedModule module;

            public SizeBySpeed(ParticleSystem.SizeBySpeedModule module) => this.module = module;
        }

        public class RotationOverLifetime
        {
            ParticleSystem.RotationOverLifetimeModule module;

            public RotationOverLifetime(ParticleSystem.RotationOverLifetimeModule module) => this.module = module;
        }

        public class RotationBySpeed
        {
            ParticleSystem.RotationBySpeedModule module;

            public RotationBySpeed(ParticleSystem.RotationBySpeedModule module) => this.module = module;
        }

        public class ExternalForces
        {
            ParticleSystem.ExternalForcesModule module;

            public ExternalForces(ParticleSystem.ExternalForcesModule module) => this.module = module;
        }

        public class Noise
        {
            ParticleSystem.NoiseModule module;

            public Noise(ParticleSystem.NoiseModule module) => this.module = module;
        }

        public class Collision
        {
            ParticleSystem.CollisionModule module;

            public Collision(ParticleSystem.CollisionModule module) => this.module = module;
        }

        public class Trigger
        {
            ParticleSystem.TriggerModule module;

            public Trigger(ParticleSystem.TriggerModule module) => this.module = module;
        }

        public class SubEmitters
        {
            ParticleSystem.SubEmittersModule module;

            public SubEmitters(ParticleSystem.SubEmittersModule module) => this.module = module;
        }

        public class TextureSheetAnimation
        {
            ParticleSystem.TextureSheetAnimationModule module;

            public TextureSheetAnimation(ParticleSystem.TextureSheetAnimationModule module) => this.module = module;
        }

        public class Lights
        {
            ParticleSystem.LightsModule module;

            public Lights(ParticleSystem.LightsModule module) => this.module = module;
        }

        public class Trails
        {
            ParticleSystem.TrailModule module;

            public Trails(ParticleSystem.TrailModule module) => this.module = module;
        }

        public class CustomData
        {
            ParticleSystem.CustomDataModule module;

            public CustomData(ParticleSystem.CustomDataModule module) => this.module = module;
        }

        #endregion

        #region Properties

        Main _main;
        public Main main => _main ??= new Main(ps.main);

        Emission _emission;
        public Emission emission => _emission ??= new Emission(ps.emission);

        Shape _shape;
        public Shape shape => _shape ??= new Shape(ps.shape);

        VelocityOverLifetime _velocityOverLifetime;
        public VelocityOverLifetime velocityOverLifetime => _velocityOverLifetime ??= new VelocityOverLifetime(ps.velocityOverLifetime);

        LimitVelocityOverLifetime _limitVelocityOverLifetime;
        public LimitVelocityOverLifetime limitVelocityOverLifetime => _limitVelocityOverLifetime ??= new LimitVelocityOverLifetime(ps.limitVelocityOverLifetime);

        InheritVelocity _inheritVelocity;
        public InheritVelocity inheritVelocity => _inheritVelocity ??= new InheritVelocity(ps.inheritVelocity);

        LifetimeByEmitterSpeed _lifetimeByEmitterSpeed;
        public LifetimeByEmitterSpeed lifetimeByEmitterSpeed => _lifetimeByEmitterSpeed ??= new LifetimeByEmitterSpeed(ps.lifetimeByEmitterSpeed);

        ForceOverLifetime _forceOverLifetime;
        public ForceOverLifetime forceOverLifetime => _forceOverLifetime ??= new ForceOverLifetime(ps.forceOverLifetime);

        ColorOverLifetime _colorOverLifetime;
        public ColorOverLifetime colorOverLifetime => _colorOverLifetime ??= new ColorOverLifetime(ps.colorOverLifetime);

        ColorBySpeed _colorBySpeed;
        public ColorBySpeed colorBySpeed => _colorBySpeed ??= new ColorBySpeed(ps.colorBySpeed);

        SizeOverLifetime _sizeOverLifetime;
        public SizeOverLifetime sizeOverLifetime => _sizeOverLifetime ??= new SizeOverLifetime(ps.sizeOverLifetime);

        SizeBySpeed _sizeBySpeed;
        public SizeBySpeed sizeBySpeed => _sizeBySpeed ??= new SizeBySpeed(ps.sizeBySpeed);

        RotationOverLifetime _rotationOverLifetime;
        public RotationOverLifetime rotationOverLifetime => _rotationOverLifetime ??= new RotationOverLifetime(ps.rotationOverLifetime);

        RotationBySpeed _rotationBySpeed;
        public RotationBySpeed rotationBySpeed => _rotationBySpeed ??= new RotationBySpeed(ps.rotationBySpeed);

        ExternalForces _externalForces;
        public ExternalForces externalForces => _externalForces ??= new ExternalForces(ps.externalForces);

        Noise _noise;
        public Noise noise => _noise ??= new Noise(ps.noise);

        Collision _collision;
        public Collision collision => _collision ??= new Collision(ps.collision);

        Trigger _trigger;
        public Trigger trigger => _trigger ??= new Trigger(ps.trigger);

        SubEmitters _subEmitters;
        public SubEmitters subEmitters => _subEmitters ??= new SubEmitters(ps.subEmitters);

        TextureSheetAnimation _textureSheetAnimation;
        public TextureSheetAnimation textureSheetAnimation => _textureSheetAnimation ??= new TextureSheetAnimation(ps.textureSheetAnimation);

        Lights _lights;
        public Lights lights => _lights ??= new Lights(ps.lights);

        Trails _trails;
        public Trails trails => _trails ??= new Trails(ps.trails);

        CustomData _customData;
        public CustomData customData => _customData ??= new CustomData(ps.customData);

        #endregion

        #region Static

        readonly static Dictionary<ParticleSystemProperty, PropertySetter> settersCache = new Dictionary<ParticleSystemProperty, PropertySetter>();

        static PropertySetter GetSetter(ParticleSystemProperty prop) => prop switch
        {
            ParticleSystemProperty.Duration => (psw, value) => psw.main.duration = value,
            ParticleSystemProperty.StartDelay => (psw, value) => psw.main.startDelay = value,
            ParticleSystemProperty.StartLifetime => (psw, value) => psw.main.startLifetime = value,
            ParticleSystemProperty.StartSpeed => (psw, value) => psw.main.startSpeed = value,
            ParticleSystemProperty.StartSize => (psw, value) => psw.main.startSize = value,
            ParticleSystemProperty.StartRotation => (psw, value) => psw.main.startRotation = value,
            ParticleSystemProperty.FlipRotation => (psw, value) => psw.main.flipRotation = value,
            ParticleSystemProperty.GravityModifier => (psw, value) => psw.main.gravityModifier = value,
            ParticleSystemProperty.SimulationSpeed => (psw, value) => psw.main.simulationSpeed = value,

            ParticleSystemProperty.RateOverTime => (psw, value) => psw.emission.rateOverTime = value,
            ParticleSystemProperty.RateOverDistance => (psw, value) => psw.emission.rateOverDistance = value,
            ParticleSystemProperty.FirstBurstCount => (psw, value) => psw.emission.firstBirstCount = value,

            ParticleSystemProperty.Angle => (psw, value) => psw.shape.angle = value,
            ParticleSystemProperty.Radius => (psw, value) => psw.shape.radius = value,
            ParticleSystemProperty.DonutRadius => (psw, value) => psw.shape.donutRadius = value,
            ParticleSystemProperty.RadiusThickness => (psw, value) => psw.shape.radiusThickness = value,
            ParticleSystemProperty.Length => (psw, value) => psw.shape.length = value,
            ParticleSystemProperty.PositionX => (psw, value) => psw.shape.position = psw.shape.position.SetX(value),
            ParticleSystemProperty.PositionY => (psw, value) => psw.shape.position = psw.shape.position.SetY(value),
            ParticleSystemProperty.PositionZ => (psw, value) => psw.shape.position = psw.shape.position.SetZ(value),
            ParticleSystemProperty.RotationX => (psw, value) => psw.shape.rotation = psw.shape.rotation.SetX(value),
            ParticleSystemProperty.RotationY => (psw, value) => psw.shape.rotation = psw.shape.rotation.SetY(value),
            ParticleSystemProperty.RotationZ => (psw, value) => psw.shape.rotation = psw.shape.rotation.SetZ(value),
            ParticleSystemProperty.ScaleX => (psw, value) => psw.shape.scale = psw.shape.scale.SetX(value),
            ParticleSystemProperty.ScaleY => (psw, value) => psw.shape.scale = psw.shape.scale.SetY(value),
            ParticleSystemProperty.ScaleZ => (psw, value) => psw.shape.scale = psw.shape.scale.SetZ(value),
            ParticleSystemProperty.RandomizeDirection => (psw, value) => psw.shape.randomDirectionAmount = value,
            ParticleSystemProperty.SpherizeDirection => (psw, value) => psw.shape.sphericalDirectionAmount = value,
            ParticleSystemProperty.RandomizePosition => (psw, value) => psw.shape.randomPositionAmount = value,

            ParticleSystemProperty.LinearX => (psw, value) => psw.velocityOverLifetime.x = value,
            ParticleSystemProperty.LinearY => (psw, value) => psw.velocityOverLifetime.y = value,
            ParticleSystemProperty.LinearZ => (psw, value) => psw.velocityOverLifetime.z = value,
            ParticleSystemProperty.OrbitalX => (psw, value) => psw.velocityOverLifetime.orbitalX = value,
            ParticleSystemProperty.OrbitalY => (psw, value) => psw.velocityOverLifetime.orbitalY = value,
            ParticleSystemProperty.OrbitalZ => (psw, value) => psw.velocityOverLifetime.orbitalZ = value,
            ParticleSystemProperty.OffsetX => (psw, value) => psw.velocityOverLifetime.orbitalOffsetX = value,
            ParticleSystemProperty.OffsetY => (psw, value) => psw.velocityOverLifetime.orbitalOffsetY = value,
            ParticleSystemProperty.OffsetZ => (psw, value) => psw.velocityOverLifetime.orbitalOffsetZ = value,
            ParticleSystemProperty.Radial => (psw, value) => psw.velocityOverLifetime.radial = value,
            ParticleSystemProperty.SpeedModifier => (psw, value) => psw.velocityOverLifetime.speedModifier = value,

            ParticleSystemProperty.Speed => (psw, value) => psw.limitVelocityOverLifetime.limit = value,
            ParticleSystemProperty.LimitX => (psw, value) => psw.limitVelocityOverLifetime.limitX = value,
            ParticleSystemProperty.LimitY => (psw, value) => psw.limitVelocityOverLifetime.limitY = value,
            ParticleSystemProperty.LimitZ => (psw, value) => psw.limitVelocityOverLifetime.limitZ = value,
            ParticleSystemProperty.Dampen => (psw, value) => psw.limitVelocityOverLifetime.dampen = value,
            ParticleSystemProperty.Drag => (psw, value) => psw.limitVelocityOverLifetime.drag = value,

            ParticleSystemProperty.InheritVelocity => (psw, value) => psw.inheritVelocity.curve = value,

            ParticleSystemProperty.LifetimeByEmitterSpeed => (psw, value) => psw.lifetimeByEmitterSpeed.curve = value,
            ParticleSystemProperty.RangeMin => (psw, value) => psw.lifetimeByEmitterSpeed.range = psw.lifetimeByEmitterSpeed.range.SetX(value),
            ParticleSystemProperty.RangeMax => (psw, value) => psw.lifetimeByEmitterSpeed.range = psw.lifetimeByEmitterSpeed.range.SetY(value),

            ParticleSystemProperty.ForceX => (psw, value) => psw.forceOverLifetime.x = value,
            ParticleSystemProperty.ForceY => (psw, value) => psw.forceOverLifetime.y = value,
            ParticleSystemProperty.ForceZ => (psw, value) => psw.forceOverLifetime.z = value,

            ParticleSystemProperty.SizeCurveMultiplier => (psw, value) => psw.sizeOverLifetime.sizeMultiplier = value,

            _ => throw new System.Exception($"Wrong {nameof(ParticleSystemProperty)}!"),
        };

        #endregion

        delegate void PropertySetter(ParticleSystemWrapper psw, float value);

        readonly ParticleSystem ps;

        public ParticleSystemWrapper(ParticleSystem ps)
        {
            if (ps != null)
                this.ps = ps;
            else throw new System.ArgumentException("Particle System cannot be null!");
        }

        public void Set(ParticleSystemProperty param, float value) => settersCache.GetOrCreate(param, GetSetter)(this, value);
    }
}