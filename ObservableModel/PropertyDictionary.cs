/*
 * File: PropertyDictionary.cs
 * Description: PropertyDictionary
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
//    public sealed class PDictionary<K, V, VReadonly> : PropertyCollectionBase<Dictionary<K, V>>, IPropertyBase<PDictionary<K, V, VReadonly>.Readonly>
//        where V             : class, IWithReadonly<VReadonly>
//        where VReadonly     : class
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

//            public bool Take(Dictionary<K, VReadonly> OUT_val_)
//            {
//                if (o.Get() == null) return false;

//                OUT_val_.Clear();

//                foreach (var kvp in o.Get())
//                {
//                    OUT_val_[kvp.Key] = kvp.Value != null ? kvp.Value.GetReadonly() : null;
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

//            public Readonly(PDictionary<K, V, VReadonly> o_) { o = o_; }

//            private readonly PDictionary<K, V, VReadonly> o;
//        }

//        public PDictionary
//        (
//            ModelContext                                    modelContext_,
//            Dictionary<K, V>                                initVal_,
//            Func<Dictionary<K, V>, Dictionary<K, V>, bool>  funcCompare_ = null
//        )
//            : 
//            base
//            (
//                modelContext_,
//                new Dictionary<K, V>(),
//                initVal_,
//                CollectionAssignMethods.AssignDict,  
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

//    public sealed class PDictionary<K, V> : PropertyCollectionBase<Dictionary<K, V>>, IPropertyBase<PDictionary<K, V>.Readonly>
//        where V : struct
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

//            public bool Take(Dictionary<K, V> OUT_val_)
//            {
//                return CollectionAssignMethods.AssignDict(OUT_val_, o.Get());
//            }

//            public ModelContext.Readonly GetModelContext()
//            {
//                return o.GetModelContext().GetReadonly();
//            }

//            public string ToStringValue()
//            {
//                return o.Get() != null ? o.Get().ToString() : null;
//            }

//            public Readonly(PDictionary<K, V> o_) { o = o_; }

//            private readonly PDictionary<K, V> o;
//        }

//        public PDictionary
//        (
//            ModelContext                                    modelContext_,
//            Dictionary<K, V>                                initVal_,
//            Func<Dictionary<K, V>, Dictionary<K, V>, bool>  funcCompare_ = null
//        )
//            : 
//            base
//            (
//                modelContext_, 
//                new Dictionary<K, V>(),
//                initVal_,
//                CollectionAssignMethods.AssignDict,  
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


