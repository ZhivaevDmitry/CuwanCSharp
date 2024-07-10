/*
 * File: GraphOfStates.cs
 * Description: Graph Of States
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
    public class GraphOfStates
    {
        public readonly IState  stateFirst;
        public readonly Action? optGraphInterruptHandler;

        public GraphOfStates(IState stateFirst_, Action? optGraphInterruptHandler_ = null)
        {
            if (stateFirst_ == null)
            {
                throw new NullReferenceException();
            }

            bool isSpecialState =
                stateFirst_ is StateBeginGraph  ||
                stateFirst_ is StateBeginTry    ||
                stateFirst_ is StateEndGraph    ||
                stateFirst_ is StateEndTry;

            if (isSpecialState)
            {
                throw new ArgumentException("First state can't be special state.");
            }

            stateFirst                  = stateFirst_;
            optGraphInterruptHandler    = optGraphInterruptHandler_;
        }
    }
}

#nullable restore