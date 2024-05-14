﻿using System;
using System.Collections.Generic;
using System.Text;
using AnodyneSharp.Archipelago;
using AnodyneSharp.Registry;

namespace AnodyneSharp.Resources.Loading
{
    public class LocationListLoader : ContentLoader
    {
        protected internal LocationListLoader(string filePath)
            : base(filePath)
        {
        }

        public Dictionary<long, Guid> LoadLocations()
        {
            Dictionary<long, Guid> locations = new();

            long id = GameConstants.ARCHIPELAGO_START_ID;

            while (!EndOfStream)
            {
                string[] line = SplitNextLine();

                if (line.Length == 1 &&
                    Guid.TryParse(line[0], out Guid treasureID))
                {
                    locations.Add(id, treasureID);

                    id++;
                }
            }

            return locations;
        }
    }
}