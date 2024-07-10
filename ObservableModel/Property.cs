/*
 * File: Property.cs
 * Description: Property
 * Author: Dmitry Zhivaev
 * Date: 2024-Jul-09
 * License: Apache License 2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 */

using System;
using Cuwan.Utils;

namespace Cuwan.ObservableDataModel
{
    public sealed class P<T> : PropertyStructBase<T>, IPropertyBase<P<T>.ReadOnly>
        where T : struct
    {
        public class ReadOnly : IReadonlyPropertyBase
        {
            public event Action Changed
            {
                add     { o.Changed += value; }
                remove  { o.Changed -= value; }
            }

            public Event<Action>.ReadOnly ChangedAsEvent => o.ChangedAsEvent;            

            public T Val => o.Val;

            public ModelContext.ReadOnly ModelContext => o.ModelContext.Readonly;

            public ReadOnly(P<T> o_){ o = o_; }

            private readonly P<T> o;
        }

        public static P<T> Revalidate
        (
            P<T>                o_,
            ModelContext        modelContext_,
            T                   initVal_,
            Func<T, T, bool>    comparator_ = null
        )
        {
            o_ = (P<T>)PropertyStructBase<T>.Revalidate(o_, modelContext_, initVal_, comparator_);

            return o_;
        }

        public P()
        {
            readOnly = new ReadOnly(this);            
        }

        public ReadOnly Readonly => readOnly;

        private readonly ReadOnly readOnly;
    }


    public abstract class PropertyStructBase<T> where T : struct
    {
        public event Action Changed
        {
            add     { eventChanged.Bind(value);     }
            remove  { eventChanged.Unbind(value);   }
        }

        public Event<Action>.ReadOnly ChangedAsEvent
        {
            get { return eventChanged.Readonly; }
        }

        public static PropertyStructBase<T> Revalidate
        (
            PropertyStructBase<T>   o_, 
            ModelContext            modelContext_,
            T                       initVal_,
            Func<T, T, bool>        funcCompare_ = null
        )
        {
            if (o_.modelContext != null) throw new Exception("Already valid");

            o_.modelContext = modelContext_;
            o_.funcCompare  = funcCompare_;

            o_.val      = initVal_;
            o_.dirtyVal = initVal_;

            return o_;
        }

        public PropertyStructBase()
        {
            control = new Control(this);            
        }

        public ModelContext ModelContext => modelContext;

        public T DirtyVal
        {
            get => dirtyVal;

            set
            {
                modelContext.AddPotentionallyChangedProperty(control);
                dirtyVal = value;
            }
        }

        protected T Val => val;      

        private class Control : ModelContext.IPropertyInternalControl
        {
            public bool CommitIfChanged()
            {
                if (o.funcCompare != null && o.funcCompare(o.val, o.dirtyVal)) return false;

                o.val = o.dirtyVal;

                return true;
            }

            public void InvokeChanged(Action<Action> callbackWrapper_)
            {
                o.eventChanged.Invoke(callbackWrapper_);
            }

            public Control(PropertyStructBase<T> o_){ o = o_;}

            private readonly PropertyStructBase<T> o;
        }

        private T val;
        private T dirtyVal;

        private readonly Control control;

        private Event<Action> eventChanged = new Event<Action>();

        private ModelContext modelContext = null;

        private Func<T, T, bool> funcCompare = null;
    }
}
