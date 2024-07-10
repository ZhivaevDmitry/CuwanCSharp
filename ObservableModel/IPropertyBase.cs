/*
 * File: IPropertyBase.cs
 * Description: IPropertyBase
 * Author: Dmitry Zhivaev
 * Date: 2024-Jul-09
 * License: Apache License 2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 */


using Cuwan.Utils;
using System.Collections;
using System.Collections.Generic;

namespace Cuwan.ObservableDataModel
{
    public interface IPropertyBase<out TReadonly> : IWithReadonly<TReadonly>
        where TReadonly : class, IReadonlyPropertyBase
    {
        public ModelContext ModelContext { get; }
    }
}

