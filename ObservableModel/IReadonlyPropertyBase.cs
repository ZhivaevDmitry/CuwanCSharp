/*
 * File: IReadonlyPropertyBase.cs
 * Description: IReadonlyPropertyBase
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

namespace Cuwan.ObservableDataModel
{
    public interface IReadonlyPropertyBase
    {
        public Event<Action>.ReadOnly ChangedAsEvent { get; }

        public ModelContext.ReadOnly ModelContext { get; }
    }
}
