﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Integration;
using Assets.Scripts.LabyrinthGeneration;

namespace Assets.Scripts
{
    public static class Game
    {
        private static Matrix _labyrinthMatrix;

        public static int TotalGroups { get; private set; }

        public static int TotalDevices { get; private set; }

        public static void GenerateLabyrinth(IPublicApiGateway publicApiGateway)
        {
            if (publicApiGateway == null)
                throw new ArgumentNullException("publicApiGateway");

            var deviceGroups = publicApiGateway.GetDeviceGroups();
            var devices = publicApiGateway.GetDevices();

            TotalGroups = deviceGroups.Length;
            TotalDevices = devices.Length;

            _labyrinthMatrix = MapGenerator.GenerateMap(deviceGroups, devices);
        }

        public static Matrix LabyrinthMatrix
        {
            get
            {
                if (_labyrinthMatrix == null)
                    GenerateLabyrinth(new PublicApiGatewayMock());

                return _labyrinthMatrix;
            }
        }
    }
}
