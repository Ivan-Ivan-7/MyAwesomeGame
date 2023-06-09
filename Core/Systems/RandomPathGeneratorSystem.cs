﻿using Godot;
using My_awesome_character.Core.Constatns;
using My_awesome_character.Core.Game;
using My_awesome_character.Core.Game.Movement;
using My_awesome_character.Core.Game.Unknown;
using My_awesome_character.Core.Ui;
using System;
using System.Linq;

namespace My_awesome_character.Core.Systems
{
    internal class RandomPathGeneratorSystem: ISystem
    {
        private readonly IPathBuilder _pathBuilder;
        private readonly ISceneAccessor _sceneAccessor;
        private readonly IStorage _storage;

        public RandomPathGeneratorSystem(IPathBuilder pathBuilder, ISceneAccessor sceneAccessor, IStorage storage)
        {
            _pathBuilder = pathBuilder;
            _sceneAccessor = sceneAccessor;
            _storage = storage;
        }

        public void Process(double gameTime)
        {
            var map = _sceneAccessor.FindFirst<Map>(SceneNames.Map);
            var game = _sceneAccessor.FindFirst<Node2D>(SceneNames.Game);
            var lazyCharacters = _sceneAccessor.FindAll<character>();

            foreach (var character in lazyCharacters)
            {
                if (_storage.Exists<CharacterMovement>(m => m.CharacterId == character.Id && m.Actual))
                    continue;

                var randomPoint = map.GetCells().Where(p => p != character.MapPosition).OrderBy(g => Guid.NewGuid()).First();
                var pathToRandomPoint = _pathBuilder.FindPath(character.MapPosition, randomPoint, SelectSelector(character.MapPosition, randomPoint, map));

                _storage.Add(new CharacterMovement
                {
                    CharacterId = character.Id,
                    Actual = true,
                    Path = pathToRandomPoint,
                    StartCell = character.MapPosition
                });
            }
        }

        private INeighboursSelector SelectSelector(MapCell currentPosition, MapCell targetPosition, Map map)
        {
            if (currentPosition.Tags.Contains(MapCellTags.Road) && targetPosition.Tags.Contains(MapCellTags.Road))
                return new OnlyRoadNeighboursSelector(map);
            else
                return new AllNeighboursSelector(map);
        }
    }
}