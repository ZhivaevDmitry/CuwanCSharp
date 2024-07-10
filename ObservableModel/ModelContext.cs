/*
 * File: ModelContext.cs
 * Description: ModelContext
 * Author: Dmitry Zhivaev
 * Date: 2024-Jul-09
 * License: Apache License 2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 */


using Cuwan.Utils;
using System;
using System.Collections.Generic;


namespace Cuwan.ObservableDataModel
{
    public class ModelContext : IWithReadonly<ModelContext.ReadOnly>
    {
        public class ReadOnly
        {
            public event Action ModelChanged
            {
                add     { o.eventModelChanged.Bind(value); }
                remove  { o.eventModelChanged.Unbind(value); }                
            }

            public Event<Action>.ReadOnly ModelChangedAsEvent
            {
                get { return o.eventModelChanged.Readonly; }
            }          

            public ReadOnly(ModelContext o_) { o = o_; }

            private readonly ModelContext o;
        }

        public class EditTransaction : IDisposable
        {
            public void Dispose()
            {
                o.editLocksCounter.Unlock();
            }

            public EditTransaction(ModelContext o_) { o = o_; }

            private readonly ModelContext o;
        }    

        public interface IPropertyInternalControl
        {
            public bool CommitIfChanged();
            public void InvokeChanged(Action<Action> callbackWraper_);
        }

        public ModelContext(Func<Exception, bool> funcHandleCallbackException_)
        {
            funcHandleCallbackException = funcHandleCallbackException_;

            editLocksCounter = new LocksCounter(HandleEditFinished);

            editTransaction = new EditTransaction(this);

            readOnly = new ReadOnly(this);            
        }

        public ReadOnly Readonly => readOnly;

        public void AddPotentionallyChangedProperty(IPropertyInternalControl propertyInternalControl_)
        {            
            VerifyIsInEdit();
            potentionallyChangedProperties.Add(propertyInternalControl_);
        }

        public EditTransaction NewEdit()
        {
            BeginEdit();
            return editTransaction;
        }

        public void BeginEdit()
        {
            VerifyNotInRecursion();
            editLocksCounter.Lock();
        }        

        public void EndEdit()
        {
            editLocksCounter.Unlock();
        }

        private void HandleEditFinished()
        {
            if (potentionallyChangedProperties.Count == 0) return;

            recursionLock = true;

            try
            {
                foreach (var potentionallyChanged in potentionallyChangedProperties)
                {
                    if (!potentionallyChanged.CommitIfChanged()) continue;

                    propertiesToInvokeChanged.Add(potentionallyChanged);
                }

                foreach (var propertyToInvokeChanged in propertiesToInvokeChanged)
                {
                    propertyToInvokeChanged.InvokeChanged(InvokeCallback);
                }

                bool hasChanges = (0 < propertiesToInvokeChanged.Count);

                if (hasChanges)
                {
                    eventModelChanged.Invoke(InvokeCallback);
                }
                else
                {
                    // Do nothing.
                }                
            }
            finally
            {
                potentionallyChangedProperties.Clear();
                propertiesToInvokeChanged.Clear();
                recursionLock = false;
            }
        }

        private void InvokeCallback(Action callback_)
        {
            try
            {
                callback_.Invoke();
            }
            catch (Exception e_)
            {
                if (!funcHandleCallbackException(e_)) throw;
            }
        }

        private void VerifyIsInEdit()
        {
            if (editLocksCounter.IsLocked) return;
            
            throw new Exception("Edit operation without EditTransaction!");
        }

        private void VerifyNotInRecursion()
        {
            if (!recursionLock) return;
            
            throw new Exception("Recursion detected: trying to edit model, while reacting to it\'s changes.");
        }

        private readonly LocksCounter editLocksCounter;

        private bool recursionLock = false;

        private readonly HashSet<IPropertyInternalControl>  potentionallyChangedProperties  = new HashSet<IPropertyInternalControl>();
        private readonly List<IPropertyInternalControl>     propertiesToInvokeChanged       = new List<IPropertyInternalControl>();

        private Event<Action> eventModelChanged = new Event<Action>();

        private readonly EditTransaction editTransaction;

        private readonly Func<Exception, bool> funcHandleCallbackException;

        private readonly ReadOnly readOnly;
    }
}
