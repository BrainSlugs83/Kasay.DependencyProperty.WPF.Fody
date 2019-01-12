﻿using System;
using Xunit;
using Fody;
using System.Windows;
#pragma warning disable 618

public class AutoDependencyProperty_Test
{
    Type type;
    dynamic instance;

    public AutoDependencyProperty_Test()
    {
        var weavingTask = new ModuleWeaver(true);
        var testResult = weavingTask.ExecuteTestRun("AssemblyToProcess.dll");
        type = testResult.Assembly.GetType("AssemblyToProcess.SomeControl");
        instance = (dynamic)Activator.CreateInstance(type);
    }

    [Theory]
    [InlineData("SomeName", "Lalo")]
    [InlineData("SomeNumber", 14)]
    [InlineData("SomeCondition", true)]
    void AutoProperty_Test(String propertyName, Object value)
    {
        var dependencyProperty = (DependencyProperty)type.GetField($"{propertyName}Property").GetValue(null);
        instance.GetType().GetProperty(propertyName).SetValue(instance, value);

        Assert.Equal(value, instance.GetType().GetProperty(propertyName).GetValue(instance));
        Assert.Equal(value, instance.GetValue(dependencyProperty));
    }
}
