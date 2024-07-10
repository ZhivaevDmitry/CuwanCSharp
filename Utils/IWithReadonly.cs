/*
 * File: IWithReadonly.cs
 * Description: IWithReadonly
 * Author: Dmitry Zhivaev
 * Date: 2024-Jul-10
 * License: Apache License 2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 */


namespace Cuwan.Utils
{
    public interface IWithReadonly<out T> where T : class
    {
        public T Readonly { get; } 
    }
}

