/*
 * File: StateBeginGraph.cs
 * Description: Special State to start new Graph
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
    public class StateBeginGraph : IState
    {
        public readonly GraphOfStates   graphInfo;
        public readonly IState          stateAfter;

        public StateBeginGraph(GraphOfStates graphInfo_, IState stateAfter_)
        {
            if (graphInfo_ == null)     throw new NullReferenceException();
            if (stateAfter_ == null)    throw new NullReferenceException();

            graphInfo   = graphInfo_;
            stateAfter  = stateAfter_;
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