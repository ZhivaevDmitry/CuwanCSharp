/*
 * File: PropertyCollectionBase.cs
 * Description: PropertyCollectionBase
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
//    public static class CollectionAssignMethods
//    {
//        public static bool AssignList<T>(List<T> to_, List<T> from_)
//        {
//            if (to_ == null) return false;

//            to_.Clear();

//            if (from_ == null) return false;

//            to_.AddRange(from_);

//            return true;
//        }

//        public static bool AssignDict<K, V>(Dictionary<K, V> to_, Dictionary<K, V> from_)
//        {
//            if (to_ == null) return false;

//            to_.Clear();

//            if (from_ == null) return false;

//            foreach (var kvp in from_)
//            {
//                to_[kvp.Key] = kvp.Value;
//            }

//            return true;
//        }

//        public static bool AssignHashSet<T>(HashSet<T> to_, HashSet<T> from_)
//        {
//            if (to_ == null) return false;

//            to_.Clear();

//            if (from_ == null) return false;

//            foreach (var v in from_)
//            {
//                to_.Add(v);
//            }

//            return true;
//        }
//    }

//    public abstract class PropertyCollectionBase<T> where T : class
//    {
//        public event Action Changed
//        {
//            add     { eventChanged += value; }
//            remove  { eventChanged -= value; }
//        }

//        public Event<Action>.Readonly ChangedAsEvent
//        {
//            get { return eventChanged.GetReadonly(); }
//        }

//        public PropertyCollectionBase
//        (
//            ModelContext        modelContext_,
//            T                   collectionForVal_,
//            T                   initVal_,
//            Func<T, T, bool>    funcAssign_,
//            Func<T, T, bool>    funcCompare_
//        )
//        {
//            modelContext = modelContext_;

//            funcAssign = funcAssign_;
//            funcCompare = funcCompare_;

//            control = new Control(this);

//            collectionForVal = collectionForVal_;

//            dirtyVal = initVal_;

//            val = funcAssign(val, dirtyVal) ? val : null;
//        }

//        public ModelContext GetModelContext()
//        {
//            return modelContext;
//        }

//        public T GetDirtyVal(bool willPotentionallyChange = true)
//        {
//            if (willPotentionallyChange)
//            {
//                modelContext.AddPotentionallyChangedProperty(control);
//            }
//            else
//            {
//                // Do nothing.
//            }

//            return dirtyVal;
//        }

//        public void SetDirty(T dirtyVal_)
//        {
//            modelContext.AddPotentionallyChangedProperty(control);
//            dirtyVal = dirtyVal_;
//        }

//        protected T Get()
//        {
//            return val;
//        }

//        private class Control : ModelContext.IPropertyInternalControl
//        {
//            public bool CommitIfChanged()
//            {
//                if (o.funcCompare != null && o.funcCompare(o.val, o.dirtyVal)) return false;

//                if (o.val == null && o.dirtyVal != null)
//                {
//                    o.val = o.collectionForVal;
//                }
//                else
//                {
//                    // Do nothing.
//                }

//                o.val = o.funcAssign(o.val, o.dirtyVal) ? o.val : null;

//                return true;
//            }

//            public void InvokeChanged(Func<Exception, bool> funcHandleInvokeException_)
//            {
//                o.eventChanged.Invoke(Event<Action>.INVOKE_ACTION, funcHandleInvokeException_);
//            }

//            public Control(PropertyCollectionBase<T> o_) { o = o_; }

//            private readonly PropertyCollectionBase<T> o;
//        }

//        private T val;
//        private T dirtyVal;

//        private T collectionForVal;

//        private readonly Func<T, T, bool> funcAssign;
//        private readonly Func<T, T, bool> funcCompare;

//        private readonly Control control;

//        private Event<Action> eventChanged = new Event<Action>();

//        private readonly ModelContext modelContext;
//    }
//}


