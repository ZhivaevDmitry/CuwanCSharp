/*
 * File: StateSolid.cs
 * Description: State that consist of only one solid function call
 * Author: Dmitry Zhivaev
 * Date: 2024-Jul-08
 * License: Apache License 2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 */

#nullable enable

using System;

namespace Cuwan.StateMachine
{
    public class StateSolid : IState
    {
        ////////////////////////////////////////////
        // Public
        ////////////////////////////////////////////

        public interface IStateHandlers
        {
            IState Enter(); // throws Exception
        };

        ////////////////////////////////////////////
        // Public
        ////////////////////////////////////////////

        public StateSolid(IStateHandlers handlers_)
        {
            _handlers = handlers_;
        }

        public void Enter(IStateMachineCallback stateMachineCallback_)
        {
            _stateMachineCallback = stateMachineCallback_;

            IState nextState;

            try
            {
                nextState = _handlers.Enter();
            }
            catch (Exception e_)
            {
                stateMachineCallback_.HandleLeftWithException(this, e_);
                return;
            }

            _stateMachineCallback.HandleLeft(this, nextState);

            _stateMachineCallback = null;
        }

        public void Leave(bool immediately_)
        {
            throw new InvalidOperationException();
        }

        private readonly IStateHandlers _handlers;

        private IStateMachineCallback? _stateMachineCallback = null;
    }

    public class StateSolid<T> : StateSolid where T : StateSolid.IStateHandlers
    {
        public StateSolid(T handlers_) : base(handlers_) { }        
    }
}


#nullable restore