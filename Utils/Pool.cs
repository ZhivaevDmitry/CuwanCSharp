/*
 * File: Pool.cs
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
using System.Collections;
using System.Collections.Generic;

namespace Cuwan.Utils
{
    public class Pool<T> where T : class
    {
        public Pool(){}

        public void Init
        (
            Func<T> factoryMethod_
        )
        {
            _factoryMethod = factoryMethod_;
        }

        public T Aquire()
        {
            T obj =
                (0 < _storage.Count) ? _storage.Dequeue() : ObtainNew();

            return obj;
        }

        public void Release(T obj_)
        {
            if (obj_ == null) throw new ArgumentException("Released object can not be null!");

            _storage.Enqueue(obj_);
        }

        private T ObtainNew()
        {
            T newObj = _factoryMethod.Invoke();

            if (newObj == null)
            {
                throw new Exception("Factory produced null!");
            }

            return newObj;
        }

        private Func<T> _factoryMethod;

        private readonly Queue<T> _storage = new Queue<T>();
    }
}
