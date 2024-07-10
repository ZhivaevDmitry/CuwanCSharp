/*
 * File: IteratorOfStates.cs
 * Description: State Machine Iterator
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
using System.Collections.Generic;

namespace Cuwan.StateMachine
{
    public class IteratorOfStates
    {
        //////////////////////////////////////////
        // Public
        //////////////////////////////////////////

        public bool HasNext => (_currState != null || _pendingTransitionToState != null);

        public Exception? OptGraphException => _uncaughtException;        

        public bool IsInState => (_postLeftHandler != null);

        public bool IsGraphStackDamaged => isGraphStackDamaged;

        public IteratorOfStates()
        {
            _stateMachineCallback = new StateMachineCallback(this);
        }

        public void Revalidate
        (
            StateBeginGraph stateBeginGraph_
        )
        {
            Invalidate();
            _pendingTransitionToState = stateBeginGraph_;
        }

        public void Invalidate()
        {
            if (IsInState)
            {
                throw new Exception("Can't reset while in state!");
            }

            while (0 < graphStack.Count)
            {
                StateBeginGraph stateBeginGraph = graphStack.Pop().Item1;

                if (stateBeginGraph.graphInfo.optGraphInterruptHandler == null) continue;

                try
                {
                    stateBeginGraph.graphInfo.optGraphInterruptHandler.Invoke();
                }
                catch (Exception e_)
                {
                    // TODO: log error
                }
            }

            _uncaughtException           = null;
            _currState                   = null;
            _pendingTransitionToState    = null;
            isGraphStackDamaged         = false;
        }

        public void EnterNext(Action postLeftHandler_)
        {
            if (!HasNext || IsGraphStackDamaged) throw new Exception("Invalid call!");

            _postLeftHandler = postLeftHandler_;

            _currState = _pendingTransitionToState;
            _pendingTransitionToState = null;

            bool isSimpleStateFound = false;
            bool isExecutionFinished = false;

            while (!isSimpleStateFound && !isExecutionFinished)
            {
                if (_currState == null)
                {
                    isGraphStackDamaged = true;
                    throw new NullReferenceException();
                }

                if (_currState is StateBeginGraph)
                {
                    StateBeginGraph stateBeginGraph = (StateBeginGraph)_currState;
                    
                    graphStack.Push(Tuple.Create(stateBeginGraph, new Stack<StateBeginTry>()));

                    _currState = stateBeginGraph.graphInfo.stateFirst;

                    continue;
                }

                if (_currState is StateBeginTry)
                {
                    StateBeginTry stateBeginTry = (StateBeginTry)_currState;

                    graphStack.Peek().Item2.Push(stateBeginTry);

                    _currState = stateBeginTry.stateStart;

                    continue;
                }

                if (_currState is StateEndTry)
                {
                    if (graphStack.Peek().Item2.Count == 0)
                    {
                        isGraphStackDamaged = true;
                        throw new Exception("Graph stack damaged!");
                    }

                    StateBeginTry stateBeginTry = graphStack.Peek().Item2.Peek();

                    _currState = stateBeginTry.stateAfter;

                    graphStack.Peek().Item2.Pop();

                    continue;
                }

                if (_currState is StateEndGraph)
                {
                    if (graphStack.Count == 0)
                    {
                        isGraphStackDamaged = true;
                        throw new Exception("Graph stack damaged!");
                    }

                    Stack<StateBeginTry> tryStack = graphStack.Peek().Item2;

                    if (0 < tryStack.Count)
                    {
                        isGraphStackDamaged = true;
                        throw new Exception("Graph stack damaged!");
                    }

                    StateBeginGraph stateBeginGraph = graphStack.Peek().Item1;

                    _currState = stateBeginGraph.stateAfter;

                    graphStack.Pop();

                    isExecutionFinished = graphStack.Count == 0;

                    continue;
                }

                isSimpleStateFound = true;
            }

            if (isExecutionFinished)
            {
                _currState = null;

                Action? postHandler = _postLeftHandler;
                _postLeftHandler = null;
                postHandler.Invoke();

                return;
            }

            _currState.Enter(_stateMachineCallback);
        }

        public void LeaveState(bool immediately_)
        {
            if (!IsInState) return;

            _currState.Leave(immediately_);

            if (immediately_ && IsInState)
            {
                throw new Exception("Failed to leave state immediately!");
            }
        }

        //////////////////////////////////////////////
        // Private
        //////////////////////////////////////////////

        private void HandleLeft(IState nextState_)
        {
            _caughtException = null;
            _pendingTransitionToState = nextState_;

            Action? postHandler = _postLeftHandler;
            _postLeftHandler = null;
            postHandler.Invoke();
        }

        private void HandleLeftWithException(Exception e_)
        {
            //StateBeginTry? exceptionCatcher = null;

            //while (exceptionCatcher == null && 0 < graphStack.Count)
            //{
            //    Stack<StateBeginTry> tryStack = graphStack.Peek().Item2;

            //    if (tryStack.Count == 0)
            //    {
            //        graphStack.Pop();
            //        continue;
            //    }

            //    exceptionCatcher = tryStack.Pop();
            //}

            //if (exceptionCatcher == null)
            //{
            //    _uncaughtException = e_;
            //    _currState = null;
            //    _pendingTransitionToState = null;
            //}
            //else
            //{
            //    _caughtException = e_;
            //    _pendingTransitionToState = exceptionCatcher.stateAfter;
            //}


            _uncaughtException = e_;
            _currState  = null;
            _pendingTransitionToState = null;

            Action? postHandler = _postLeftHandler;
            _postLeftHandler = null;
            postHandler.Invoke();
        }

        //////////////////////////////////////////////
        // Private
        //////////////////////////////////////////////

        private class StateMachineCallback : IStateMachineCallback
        {           
            public void HandleLeft(IState sender_, IState nextState_)
            {
                if (_o._currState == null || _o._currState != sender_)
                {
                    throw new Exception("Invalid call!");
                }

                _o.HandleLeft(nextState_);
            }

            public void HandleLeftWithException(IState sender_, Exception e_)
            {
                if (_o._currState == null || _o._currState != sender_)
                {
                    throw new Exception("Invalid call!");
                }

                _o.HandleLeftWithException(e_);
            }

            public StateMachineCallback(IteratorOfStates o_) { _o = o_; }

            private readonly IteratorOfStates _o;
        }

        ////////////////////////////////////////////
        // Private
        ////////////////////////////////////////////

        private IState? _currState                = null;
        private IState? _pendingTransitionToState = null;

        private Action? _postLeftHandler = null;

        private Exception? _caughtException      = null;
        private Exception? _uncaughtException    = null;

        private readonly StateMachineCallback _stateMachineCallback;

        private readonly Stack<Tuple<StateBeginGraph, Stack<StateBeginTry>>> graphStack = new ();

        private bool isGraphStackDamaged = false;
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#nullable restore