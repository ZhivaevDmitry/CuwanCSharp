/*
 * File: Event.cs
 * Description: Event
 * Author: Dmitry Zhivaev
 * Date: 2024-Jul-10
 * License: Apache License 2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 */


using System;
using System.Collections.Generic;

namespace Cuwan.Utils
{
    public class Event<D> : IWithReadonly<Event<D>.ReadOnly>
        where D : Delegate
    {
        public class ReadOnly
        {
            public void Bind(D d_)      { o.Bind(d_);   }
            public void Unbind(D d_)    { o.Unbind(d_); }

            public ReadOnly(Event<D> o_) { o = o_; }

            private readonly Event<D> o;
        }

        public void Bind(D d_)
        {
            if (d_ == null) throw new NullReferenceException();

            lock (readOnlyInvocationList)
            {
                invocationList.Add(d_);
            }
        }

        public void Unbind(D d_)
        {
            if (d_ == null) throw new NullReferenceException();

            lock (readOnlyInvocationList)
            {
                int index = invocationList.LastIndexOf(d_);

                if (index < 0) return;

                invocationList.RemoveAt(index);
            }
        }

        public Event()
        {
            invocationList              = new List<D>();
            inProgressInvocationList    = new List<D>();

            readOnlyInvocationList = invocationList.AsReadOnly();

            readOnly = new ReadOnly(this);
        }

        public void Invoke(Action<D> actionInvoke_)
        {
            lock (inProgressInvocationList)
            {
                inProgressInvocationList.Clear();

                lock (readOnlyInvocationList)
                {
                    inProgressInvocationList.AddRange(invocationList);
                }

                foreach (var callback in inProgressInvocationList)
                {
                    actionInvoke_(callback);
                }
  
                inProgressInvocationList.Clear();       
            }
        }

        public IReadOnlyCollection<D> InvocationList => readOnlyInvocationList;


        public ReadOnly Readonly => readOnly;


        private readonly List<D>                invocationList;
        private readonly IReadOnlyCollection<D> readOnlyInvocationList;
        private readonly List<D>                inProgressInvocationList;

        private readonly ReadOnly readOnly;
    }
}
