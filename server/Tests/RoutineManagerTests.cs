using System;
using System.Collections.Generic;
using Konek.Server.Core.Bridges;
using Konek.Server.Core.Models;
using NUnit.Framework;
using Konek.Server.Core.Scheduling;
using Moq;

namespace Konek.Server.Tests;

public class RoutineManagerTests
{
    private Mock<IBridge> _bridgeMock = null!;
    private Mock<ITimeProvider> _timeProviderMock = new();
    private FakeScheduler _fakeScheduler = null!;
    private RoutineManager _routineManager = null!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _timeProviderMock.SetupGet(x => x.Now).Returns(new DateTime(2022, 01, 01, 12, 01, 01));
    }

    [SetUp]
    public void Setup()
    {
        _bridgeMock = new();
        _timeProviderMock = new();
        _fakeScheduler = new();

        _routineManager = new RoutineManager(
            new Lamp("Lamp1", "LampId1"),
            _bridgeMock.Object,
            _fakeScheduler,
            _timeProviderMock.Object
        );
    }

    [Test]
    public void Schedule_SingleEffect()
    {
        var routineDefinition = new RoutineDefinition(new List<Effect>
        {
            new(new TimeOnly(2, 10), new TimeOnly(3, 15), 100, 150) { EffectId = 0 },
        });
        var routine = new Routine(routineDefinition, null);

        _routineManager.Schedule(routine);
        _fakeScheduler.Simulate<RoutineSchedulingArgs>();

        _bridgeMock.Verify(mock => mock.SetBrightness(It.IsAny<IEffectBearer>(), 100), Times.Once);
        _bridgeMock.Verify(mock => mock.SetTemperature(It.IsAny<IEffectBearer>(), 150), Times.Once);
        _bridgeMock.Verify(mock => mock.SetBrightness(It.IsAny<IEffectBearer>(), 0), Times.Once);
        _bridgeMock.VerifyNoOtherCalls();
    }

    [Test]
    public void Schedule_TwoEffectsWithGap()
    {
        var routineDefinition = new RoutineDefinition(new List<Effect>
        {
            new(new TimeOnly(2, 10), new TimeOnly(3, 15), 100, 150) { EffectId = 0 },
            new(new TimeOnly(4, 10), new TimeOnly(5, 15), 200, 250) { EffectId = 1 },
        });
        var routine = new Routine(routineDefinition, null);

        var brightnessArgs = new List<byte>();
        var temperatureArgs = new List<byte>();
        _bridgeMock.Setup(mock =>
            mock.SetBrightness(It.IsAny<IEffectBearer>(), Capture.In(brightnessArgs)));
        _bridgeMock.Setup(mock =>
            mock.SetTemperature(It.IsAny<IEffectBearer>(), Capture.In(temperatureArgs)));

        _routineManager.Schedule(routine);
        _fakeScheduler.Simulate<RoutineSchedulingArgs>();

        Assert.That(brightnessArgs, Is.EqualTo(new[] { 100, 0, 200, 0 }));
        Assert.That(temperatureArgs, Is.EqualTo(new[] { 150, 250 }));
    }

    [Test]
    public void Schedule_SingleRoutineMultipleEffects()
    {
        var routineDefinition = new RoutineDefinition(new List<Effect>
        {
            new(new TimeOnly(2, 10), new TimeOnly(3, 15), 25, 30) { EffectId = 0 },
            new(new TimeOnly(3, 15), new TimeOnly(5, 0), 30, 35) { EffectId = 1 },
        });
        var routine = new Routine(routineDefinition, null);

        var brightnessArgs = new List<byte>();
        var temperatureArgs = new List<byte>();
        _bridgeMock.Setup(mock =>
            mock.SetBrightness(It.IsAny<IEffectBearer>(), Capture.In(brightnessArgs)));
        _bridgeMock.Setup(mock =>
            mock.SetTemperature(It.IsAny<IEffectBearer>(), Capture.In(temperatureArgs)));

        _routineManager.Schedule(routine);
        _fakeScheduler.Simulate<RoutineSchedulingArgs>();

        Assert.That(brightnessArgs, Is.EqualTo(new[] { 25, 30, 0 }));
        Assert.That(temperatureArgs, Is.EqualTo(new[] { 30, 35 }));
    }

    [Test]
    public void Schedule_MultipleRoutines()
    {
        //   |###|###| (routine2)
        // |###|###|   (routine1)
        var routineDefinition1 = new RoutineDefinition(new List<Effect>
        {
            new(new TimeOnly(2, 10), new TimeOnly(3, 15), 25, 30) { EffectId = 0 },
            new(new TimeOnly(3, 15), new TimeOnly(5, 0), 30, 35) { EffectId = 1 },
        });
        var routineDefinition2 = new RoutineDefinition(new List<Effect>
        {
            new(new TimeOnly(2, 30), new TimeOnly(3, 35), 36, 41) { EffectId = 2 },
            new(new TimeOnly(3, 35), new TimeOnly(5, 20), 46, 51) { EffectId = 3 },
        });
        var routine1 = new Routine(routineDefinition1, null);
        var routine2 = new Routine(routineDefinition2, null);

        var brightnessArgs = new List<byte>();
        var temperatureArgs = new List<byte>();
        _bridgeMock.Setup(mock =>
            mock.SetBrightness(It.IsAny<IEffectBearer>(), Capture.In(brightnessArgs)));
        _bridgeMock.Setup(mock =>
            mock.SetTemperature(It.IsAny<IEffectBearer>(), Capture.In(temperatureArgs)));

        _routineManager.Schedule(routine1);
        _routineManager.Schedule(routine2);
        _fakeScheduler.Simulate<RoutineSchedulingArgs>();

        Assert.That(brightnessArgs, Is.EqualTo(new[] { 25, 36, 46, 0 }));
        Assert.That(temperatureArgs, Is.EqualTo(new[] { 30, 41, 51 }));
    }

    [Test]
    public void Schedule_EffectsBetweenTwoDays()
    {
        var routineDefinition = new RoutineDefinition(new List<Effect>
        {
            new(new TimeOnly(21, 50), new TimeOnly(23, 50), 20, 25) { EffectId = 0 },
            new(new TimeOnly(23, 50), new TimeOnly(01, 15), 30, 35) { EffectId = 0 },
            new(new TimeOnly(01, 15), new TimeOnly(3, 50), 40, 45) { EffectId = 0 },
        });
        var routine = new Routine(routineDefinition, null);

        var brightnessArgs = new List<byte>();
        var temperatureArgs = new List<byte>();
        _bridgeMock.Setup(mock =>
            mock.SetBrightness(It.IsAny<IEffectBearer>(), Capture.In(brightnessArgs)));
        _bridgeMock.Setup(mock =>
            mock.SetTemperature(It.IsAny<IEffectBearer>(), Capture.In(temperatureArgs)));

        _routineManager.Schedule(routine);
        _fakeScheduler.Simulate<RoutineSchedulingArgs>();

        Assert.That(brightnessArgs, Is.EqualTo(new[] { 20, 30, 40, 0 }));
        Assert.That(temperatureArgs, Is.EqualTo(new[] { 25, 35, 45 }));
    }
}