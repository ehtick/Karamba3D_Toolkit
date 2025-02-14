﻿/*
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Engine.Adapters.Karamba3D
{
    internal static class K3dLogger
    {
        private static List<string> m_allMessages = new List<string>();
        private static List<string> m_reportedErrors = new List<string>();
        private static List<string> m_reportedWarnings = new List<string>();
        private static List<string> m_reportedNotes = new List<string>();

        internal static void RecordError(string error, bool doNotRepeat = false, bool appendToPrevious = false)
        {
            RecordMessage(MessageLevel.ERROR, error, doNotRepeat, appendToPrevious);
        }

        internal static void RecordWarning(string warning, bool doNotRepeat = false, bool appendToPrevious = false)
        {
            RecordMessage(MessageLevel.Warning, warning, doNotRepeat, appendToPrevious);
        }

        internal static void RecordNote(string note, bool doNotRepeat = false, bool appendToPrevious = false)
        {
            RecordMessage(MessageLevel.Note, note, doNotRepeat, appendToPrevious);
        }

        internal static void RecordMessage(MessageLevel messageLevel, string message, bool doNotRepeat = false, bool appendToPrevious = false)
        {
            List<string> m_relevantMessages = GetMessages(messageLevel);

            if (doNotRepeat && m_relevantMessages.Contains(message))
                return;

            Console.WriteLine(message);
            ReportToUI(messageLevel, message);

            if (appendToPrevious && m_relevantMessages.Any())
                m_relevantMessages.AppendToLast(message);
            else
                m_relevantMessages.Add(message);

            m_allMessages.Add($"{DateTime.Now}\t{messageLevel}: {message}");
        }

        internal static List<string> GetMessages(MessageLevel messageLevel)
        {
            if (messageLevel == MessageLevel.ERROR)
                return m_reportedErrors;

            if (messageLevel == MessageLevel.Warning)
                return m_reportedWarnings;

            if (messageLevel == MessageLevel.Note)
                return m_reportedNotes;

            return new List<string>();
        }

        internal static void ReportToUI(MessageLevel messageLevel, string message)
        {
            if (messageLevel == MessageLevel.ERROR)
                BH.Engine.Base.Compute.RecordError(message);

            if (messageLevel == MessageLevel.Warning)
                BH.Engine.Base.Compute.RecordWarning(message);

            if (messageLevel == MessageLevel.Note)
                BH.Engine.Base.Compute.RecordNote(message);
        }

        internal static List<string> GetErrors()
        {
            return new List<string>(m_reportedErrors);
        }

        internal static List<string> GetWarnings()
        {
            return new List<string>(m_reportedWarnings);
        }

        internal static List<string> GetNotes()
        {
            return new List<string>(m_reportedNotes);
        }

        internal static List<string> GetAllMessages()
        {
            return new List<string>(m_allMessages);
        }

        internal static void SaveLogToDisk(string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllLines(filePath, m_allMessages);
        }

        internal static void Clean()
        {
            m_allMessages = new List<string>();
            m_reportedErrors = new List<string>();
            m_reportedWarnings = new List<string>();
            m_reportedNotes = new List<string>();
        }

        internal static void AppendToLast(this IList<string> set, string toAppend)
        {
            string lastMessage = set.Last();
            set.Remove(lastMessage);
            lastMessage += Environment.NewLine + toAppend;
            set.Add(lastMessage);
        }
    }
}