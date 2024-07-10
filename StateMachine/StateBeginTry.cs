/*
 * File: StateBeginTry.cs
 * Description: Special State to open some kind of try catch subgraph
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
    public class StateBeginTry : IState
    {
        public readonly IState      stateStart;
        public readonly IStateCatch stateCatch;
        public readonly IState      stateAfter;

        public StateBeginTry(IState stateStart_, IStateCatch stateCatch_, IState stateAfter_)
        {
            if (stateStart_ == null) throw new NullReferenceException();
            if (stateCatch_ == null) throw new NullReferenceException();
            if (stateAfter_ == null) throw new NullReferenceException();

            stateStart = stateStart_;
            stateCatch = stateCatch_;
            stateAfter = stateAfter_;
        }

        public void Enter(IStateMachineCallback stateMachineCallback_)
        {
            throw new InvalidOperationException();
        }

        public void Leave(bool immediately_)
        {
            throw new InvalidOperationException();
        }
    }
}

#nullable restore