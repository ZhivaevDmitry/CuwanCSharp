/*
 * File: StateEndGraph.cs
 * Description: Special State to end graph
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
    public class StateEndGraph : IState
    {
        public static readonly StateEndGraph instance = new StateEndGraph();

        private StateEndGraph() {}

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