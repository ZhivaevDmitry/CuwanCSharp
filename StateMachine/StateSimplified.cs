/*
 * File: StateSimplified.cs
 * Description: Simplified version of basic IState
 * Author: Dmitry Zhivaev
 * Date: 2024-Jul-08
 * License: Apache License 2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 */



#nullable enable
#pragma warning disable CS8602 // Dereference of a possibly null reference.

using System;

namespace Cuwan.StateMachine
{
    public class StateSimplified : IState
    {
        ///////////////////////////////////////////
        // Public
        ////////////////////////////////////////////

        public interface IStateCallback
        {
            void InvokeOnReadyToLeave();
        }

        public interface IStateHandlers
        {
            void    Enter(IStateCallback stateCallback_);   // throws Exception
            void    Leave(bool immediately_);               // throws Exception
            IState  HandleOnReadyToLeave();                 // throws Exception
        }

        ///////////////////////////////////////////
        // Public
        ////////////////////////////////////////////

        public StateSimplified(IStateHandlers handlers_)
        {
            _handlers = handlers_;

            _stateCallback = new StateCallback(this);
        }

        public void Enter(IStateMachineCallback stateMachineCallback_)
        {
            _stateMachineCallback = stateMachineCallback_;

            try
            {
                _handlers.Enter(_stateCallback);
            }
            catch (NeedToRethrowException e_)
            {
                throw e_.e;
            }
            catch (Exception e_)
            {
                _stateMachineCallback.HandleLeftWithException(this, e_);
                return;
            }
        }

        public void Leave(bool immediately_)
        {
            try
            {
                _handlers.Leave(immediately_);
            }
            catch (NeedToRethrowException e_)
            {
                throw e_.e;
            }
            catch (Exception e_)
            {
                _stateMachineCallback.HandleLeftWithException(this, e_);
                return;
            }
        }

        ///////////////////////////////////////////
        // Private
        ////////////////////////////////////////////

        private class NeedToRethrowException : Exception
        {
            public readonly Exception e;
            public NeedToRethrowException(Exception e_) { e = e_; }
        }

        ///////////////////////////////////////////
        // Private
        ////////////////////////////////////////////

        private void Leave()
        {
            IState nextState;

            try
            {
                try
                {
                    nextState = _handlers.HandleOnReadyToLeave();
                }
                catch (Exception e_)
                {
                    _stateMachineCallback.HandleLeftWithException(this, e_);
                    return;
                }

                _stateMachineCallback.HandleLeft(this, nextState);
                return;
            }
            catch (Exception e_)
            {
                throw new NeedToRethrowException(e_);
            }
        }

        ///////////////////////////////////////////
        // Private
        ////////////////////////////////////////////

        private class StateCallback : IStateCallback
        {
            public void InvokeOnReadyToLeave()
            {
                _o.Leave();
            }

            public StateCallback(StateSimplified o_) { _o = o_; }

            private readonly StateSimplified _o;
        };

        private readonly StateCallback _stateCallback;

        private readonly IStateHandlers _handlers;

        private IStateMachineCallback? _stateMachineCallback = null;
    }

    public class StateSimplified<T> : StateSimplified where T : StateSimplified.IStateHandlers
    {
        public StateSimplified(T handlers_) : base(handlers_) { }
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#nullable restore