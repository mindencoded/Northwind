﻿using System;
using System.Collections.Generic;
using Unity.Builder;
using Unity.Events;
using Unity.Extension;

namespace S3K.RealTimeOnline.CommonUtils
{
    public class Decorator : UnityContainerExtension
    {
        private Dictionary<Type, List<Type>> _typeStacks;

        protected override void Initialize()
        {
            _typeStacks = new Dictionary<Type, List<Type>>();
            Context.Registering += AddRegistration;

            Context.Strategies.Add(
                new DecoratorBuildStrategy(_typeStacks),
                UnityBuildStage.PreCreation
            );
        }

        private void AddRegistration(object sender, RegisterEventArgs e)
        {
            if (!e.TypeFrom.IsInterface)
            {
                return;
            }

            List<Type> stack;
            if (!_typeStacks.ContainsKey(e.TypeFrom))
            {
                stack = new List<Type>();
                _typeStacks.Add(e.TypeFrom, stack);
            }
            else
            {
                stack = _typeStacks[e.TypeFrom];
            }

            stack.Add(e.TypeTo);
        }
    }
}