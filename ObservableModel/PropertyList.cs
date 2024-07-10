/*
 * File: PropertyList.cs
 * Description: PropertyList
 * Author: Dmitry Zhivaev
 * Date: 2024-Jul-10
 * License: Apache License 2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 */


//using Cuwan.Utils;
//using System;
//using System.Collections.Generic;

//namespace Cuwan.ObservableDataModel
//{
//    public sealed class PList<T, TReadonly> : PropertyCollectionBase<List<T>>, IPropertyBase<PList<T, TReadonly>.Readonly>
//        where T : class, IWithReadonly<TReadonly>
//        where TReadonly : class
//    {
//        public class Readonly : IReadonlyPropertyBase
//        {
//            public event Action Changed
//            {
//                add     { o.Changed += value; }
//                remove  { o.Changed -= value; }
//            }

//            public Event<Action>.Readonly ChangedAsEvent
//            {
//                get { return o.ChangedAsEvent; }
//            }

//            public bool Take(List<TReadonly> OUT_val_)
//            {
//                if (o.Get() == null) return false;

//                OUT_val_.Clear();

//                foreach (var v in o.Get())
//                {
//                    OUT_val_.Add(v != null ? v.GetReadonly() : null);
//                }

//                return true;
//            }

//            public ModelContext.Readonly GetModelContext()
//            {
//                return o.GetModelContext().GetReadonly();
//            }

//            public string ToStringValue()
//            {
//                return o.Get() != null ? o.Get().ToString() : null;
//            }

//            public Readonly(PList<T, TReadonly> o_) { o = o_; }

//            private readonly PList<T, TReadonly> o;
//        }

//        public PList
//        (
//            ModelContext                    modelContext_,
//            List<T>                         initVal_,
//            Func<List<T>, List<T>, bool>    funcCompare_ = null
//        )
//            : 
//            base
//            (
//                modelContext_,
//                new List<T>(),
//                initVal_,
//                CollectionAssignMethods.AssignList,
//                funcCompare_
//            )
//        {
//            readOnly = new Readonly(this);

//            modelContext_.RegisterPropertyInternal(this);
//        }

//        public Readonly GetReadonly()
//        {
//            return readOnly;
//        }

//        public readonly Readonly readOnly;
//    }

//    public sealed class PList<T> : PropertyCollectionBase<List<T>>, IPropertyBase<PList<T>.Readonly>
//        where T : struct
//    {
//        public class Readonly : IReadonlyPropertyBase
//        {
//            public event Action Changed
//            {
//                add     { o.Changed += value; }
//                remove  { o.Changed -= value; }
//            }

//            public Event<Action>.Readonly ChangedAsEvent
//            {
//                get { return o.ChangedAsEvent; }
//            }

//            public bool Take(List<T> OUT_val_)
//            {
//                return CollectionAssignMethods.AssignList(OUT_val_, o.Get());
//            }

//            public ModelContext.Readonly GetModelContext()
//            {
//                return o.GetModelContext().GetReadonly();
//            }

//            public string ToStringValue()
//            {
//                return o.Get() != null ? o.Get().ToString() : null;
//            }

//            public Readonly(PList<T> o_) { o = o_; }

//            private readonly PList<T> o;
//        }

//        public PList
//        (
//            ModelContext                    modelContext_,
//            List<T>                         initVal_,
//            Func<List<T>, List<T>, bool>    funcCompare_ = null
//        )
//            : 
//            base
//            (
//                modelContext_, 
//                new List<T>(),
//                initVal_,
//                CollectionAssignMethods.AssignList,
//                funcCompare_
//            )
//        {
//            readOnly = new Readonly(this);

//            modelContext_.RegisterPropertyInternal(this);
//        }

//        public Readonly GetReadonly()
//        {
//            return readOnly;
//        }

//        public readonly Readonly readOnly;
//    }
//}

 
