/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.Engine.Adapters.Karamba3D;
using BH.oM.Adapter;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.Karamba3D
{
    public partial class Karamba3DAdapter
    {
        /***************************************************/
        /**** Methods                                  *****/
        /***************************************************/

        public override bool SetupPushType(PushType pushType, out PushType pt)
        {
            pt = pushType;

            if (pushType == PushType.AdapterDefault)
                pt = m_AdapterSettings.DefaultPushType;

            if (pushType == PushType.FullPush)
            {
                BH.Engine.Base.Compute.RecordError($"The specified {nameof(PushType)} {nameof(PushType.FullPush)} is not supported.");
                return false;
            }

            return true;
        }

        public override bool SetupPushConfig(ActionConfig actionConfig, out ActionConfig pushConfig)
        {
            pushConfig = new ActionConfig();
            return true;
        }

        public override List<object> Push(IEnumerable<object> objects, string tag = "", PushType pushType = PushType.AdapterDefault, ActionConfig actionConfig = null)
        {
            List<object> convertedObjects = new List<object>();


            foreach (var karambaMoDEL in objects.OfType<Karamba.Models.Model>())
            {
                convertedObjects.Add(karambaMoDEL.ToBHoM());
            }

            return convertedObjects;
        }

        /***************************************************/

    }
}


