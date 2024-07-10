/*
 * File: LocksCounter.cs
 * Description: LocksCounter
 * Author: Dmitry Zhivaev
 * Date: 2024-Jul-10
 * License: Apache License 2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 */

using System;

namespace Cuwan.Utils
{
    public class LocksCounter
    {
        public LocksCounter(Action actionCompletelyUnlocked_)
        {
            _actionCompletelyUnlocked = actionCompletelyUnlocked_;
            _locksCount = 0;
        }

        public void Lock()
        {
            ++_locksCount;
        }

        public void Unlock()
        {
            --_locksCount;

            if (0 < _locksCount) return;

            if (_locksCount < 0) throw new Exception("LocksCounter damaged!");

            _actionCompletelyUnlocked?.Invoke();
        }


        public bool IsLocked => 0 < _locksCount;

        public int LocksCount => _locksCount;   
        

        private readonly Action _actionCompletelyUnlocked;

        private int _locksCount;
    }
}